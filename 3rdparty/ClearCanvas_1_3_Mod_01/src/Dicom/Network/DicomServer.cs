#region License

// Copyright (c) 2006-2008, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

#region mDCM License
// mDCM: A C# DICOM library
//
// Copyright (c) 2008  Colby Dillion
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Author:
//    Colby Dillion (colby.dillion@gmail.com)
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ClearCanvas.Common;

namespace ClearCanvas.Dicom.Network
{
    public delegate IDicomServerHandler StartAssociation(DicomServer server, ServerAssociationParameters assoc);
    /// <summary>
    /// Class used by DICOM server applications for network related activites.
    /// </summary>
    public sealed class DicomServer : NetworkBase, IDisposable
    {

        #region Static Public Methods
        /// <summary>
        /// Start listening for incoming associations.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Note that StartListening can be called multiple times with different association parameters.
        /// </para>
        /// </remarks>
        /// <param name="parameters">The parameters to use when listening for associations.</param>
        /// <param name="acceptor">A delegate to be called to return a class instance that implements
        /// the <see cref="IDicomServerHandler"/> interface to handle an incoming association.</param>
        public static void StartListening(ServerAssociationParameters parameters, StartAssociation acceptor)
        {
            Listener.Listen(parameters, acceptor);
        }

        /// <summary>
        /// Stop listening for incoming associations.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Note that <see cref="StartListening"/> can be called multiple times with different association
        /// parameters.
        /// </para>
        /// </remarks>
        /// <param name="parameters">The parameters to stop listening on.</param>
        public static void StopListening(ServerAssociationParameters parameters)
        {
            Listener.StopListening(parameters);
        }
        #endregion

        #region Private Members
        private string _host;
		private Socket _socket;
		private Stream _network;
		private ManualResetEvent _closedEvent;
		private bool _closedOnError = false;
        IDicomServerHandler _handler;
        private Dictionary<string, ListenerInfo> _appList;
        private bool _disposed = false;
		#endregion

        #region Public Properties
        /// <summary>
        /// Property that tells if an association was closed because of an error.
        /// </summary>
        public bool ClosedOnError
        {
            get { return _closedOnError; }
        }
        #endregion

        #region Constructors

        internal DicomServer(Socket socket, Dictionary<string,ListenerInfo> appList)
            : base()
        {
            IPEndPoint remote = (IPEndPoint)socket.RemoteEndPoint;

            _host = remote.Address.ToString();

            _socket = socket;
            _network = new NetworkStream(_socket);
            _closedEvent = null;
            _handler = null;
            _appList = appList;

            // Start background thread for incoming associations
            InitializeNetwork(_network, "DicomServer Handler: " + remote);
        }
		#endregion

        #region Private Methods
        private void SetSocketOptions(ServerAssociationParameters parameters)
        {
            _socket.ReceiveBufferSize = parameters.ReceiveBufferSize;
            _socket.SendBufferSize = parameters.SendBufferSize;
            _socket.ReceiveTimeout = parameters.ReadTimeout;
            _socket.SendTimeout = parameters.WriteTimeout;
            _socket.LingerState = new LingerOption(false, 0);
			// Nagle option
        	_socket.NoDelay = false;
        }

        private static bool NegotiateAssociation(AssociationParameters cp, ServerAssociationParameters sp)
        {
            foreach (DicomPresContext clientContext in cp.GetPresentationContexts())
            {
                TransferSyntax selectedSyntax = null;
                foreach (TransferSyntax ts in clientContext.GetTransfers())
                {
                        byte pcid = sp.FindAbstractSyntaxWithTransferSyntax(clientContext.AbstractSyntax, ts);
                        if (pcid != 0)
                        {
                            // TODO Role negotiation here, need to check if roles set, and if so, if they match
                            selectedSyntax = ts;
                            break;
                        
                    }
                }
                if (selectedSyntax != null)
                {
                    clientContext.ClearTransfers();
                    clientContext.AddTransfer(selectedSyntax);
                    clientContext.SetResult(DicomPresContextResult.Accept);
                }
                else
                {
                    // No contexts accepted, set if abstract or transfer syntax reject
                    if (0 == sp.FindAbstractSyntax(clientContext.AbstractSyntax))
                        clientContext.SetResult(DicomPresContextResult.RejectAbstractSyntaxNotSupported);
                    else
                        clientContext.SetResult(DicomPresContextResult.RejectTransferSyntaxesNotSupported);
                }
            }
            bool anyValidContexts = false;

            foreach (DicomPresContext clientContext in cp.GetPresentationContexts())
            {
                if (clientContext.Result == DicomPresContextResult.Accept)
                {
                    anyValidContexts = true;
                    break;
                }
            }
            if (anyValidContexts == false)
            {
                return false;
            }      

            return true;
        }

        #endregion

        #region Public Methods

        #endregion

        #region NetworkBase Overrides
        /// <summary>
        /// Close the association.
        /// </summary>
        protected override void CloseNetwork()
        {
			ShutdownNetworkThread();
			lock (this)
            {
                if (_network != null)
                {
                    _network.Close();
                    _network.Dispose();
                    _network = null;
                }
                if (_socket != null)
                {
                    if (_socket.Connected)
                        _socket.Close();
                    _socket = null;
                }
                if (_closedEvent != null)
                {
                    _closedEvent.Set();
                    _closedEvent = null;
                }
            }
        }

        /// <summary>
        /// Used internally to determine if the connection has network data available.
        /// </summary>
        /// <returns></returns>
        protected override bool NetworkHasData()
        {
            if (_socket == null)
                return false;

            // Tells the state of the connection as of the last activity on the socket
            if (!_socket.Connected)
            {
				OnNetworkError(null, true);
                return false;
            }

            // This is the recommended way to determine if a socket is still active, make a
            // zero byte send call, and see if an exception is thrown.  See the Socket.Connected
            // MSDN documentation  Only do the check when we know there's no data available
            try
            {
				List<Socket> readSockets = new List<Socket>();
				readSockets.Add(_socket);
            	Socket.Select(readSockets, null, null, 100000);
				if (readSockets.Count == 1)
				{
					if (_socket.Available > 0)
						return true;
					OnNetworkError(null, true);
					return false;
				}

				_socket.Send(new byte[1], 0, 0);
            }
            catch (SocketException e)
            {
                // 10035 == WSAEWOULDBLOCK
                if (!e.NativeErrorCode.Equals(10035))
					OnNetworkError(e, true);
            }

            return false;
        }

        /// <summary>
        /// Method called on a network error.
        /// </summary>
        /// <param name="e">The exception that caused the network error</param>
        /// <param name="closeConnection">Flag telling if the connection should be closed</param>
        protected override void OnNetworkError(Exception e, bool closeConnection)
        {
            try
            {
                if (_handler != null && base._state != DicomAssociationState.Sta13_AwaitingTransportConnectionClose)
                    _handler.OnNetworkError(this, this._assoc as ServerAssociationParameters, e);
            }
            catch (Exception x) 
            {
				Platform.Log(LogLevel.Error, x, "Unexpected exception when calling IDicomServerHandler.OnNetworkError");
            }

            _closedOnError = true;
            if (closeConnection)
                CloseNetwork();
        }

        /// <summary>
        /// Method called when receiving an association request.
        /// </summary>
        /// <param name="association"></param>
        protected override void OnReceiveAssociateRequest(ServerAssociationParameters association)
        {
            if (!_appList.ContainsKey(association.CalledAE))
            {
				Platform.Log(LogLevel.Error, "Rejecting association from {0}: Invalid Called AE Title ({1}).", association.CallingAE, association.CalledAE);
                SendAssociateReject(DicomRejectResult.Permanent, DicomRejectSource.ServiceProviderACSE, DicomRejectReason.CalledAENotRecognized);
                return;
            }

            ListenerInfo info = _appList[association.CalledAE];

            // Populate the AssociationParameters properly
            association.ReadTimeout = info.Parameters.ReadTimeout;
            association.ReceiveBufferSize = info.Parameters.ReceiveBufferSize;
            association.WriteTimeout = info.Parameters.WriteTimeout;
            association.SendBufferSize = info.Parameters.SendBufferSize;

            association.RemoteEndPoint = _socket.RemoteEndPoint as IPEndPoint;
            association.LocalEndPoint = _socket.LocalEndPoint as IPEndPoint;


            // Setup Socketoptions based on the user's settings
            SetSocketOptions(association);

            // Select the presentation contexts
            bool anyValidContexts = NegotiateAssociation(association, info.Parameters);
            if (!anyValidContexts)
            {
				Platform.Log(LogLevel.Error, "Rejecting association from {0}: No valid presentation contexts.", association.CallingAE);
                SendAssociateReject(DicomRejectResult.Permanent, DicomRejectSource.ServiceProviderACSE, DicomRejectReason.NoReasonGiven);
                return;
            }
            
            _appList = null;

            try
            {
                _handler = info.StartDelegate(this, association);
                _handler.OnReceiveAssociateRequest(this, association);
            }
            catch (Exception e)
            {
                OnUserException(e, "Unexpected exception on OnReceiveAssociateRequest or StartDelegate");
            }
        }

        protected override void OnDimseTimeout()
        {
            try
            {
                _handler.OnDimseTimeout(this, this._assoc as ServerAssociationParameters);
            }
            catch (Exception e)
            {
                OnUserException(e, "Unexpected exception on OnDimseTimeout");
            }
        }

        protected override void OnReceiveAbort(DicomAbortSource source, DicomAbortReason reason)
        {
            try
            {
                _handler.OnReceiveAbort(this, this._assoc as ServerAssociationParameters, source, reason);
            }
            catch (Exception e) 
            {
                OnUserException(e, "Unexpected exception OnReceiveAbort");
            }

            _closedOnError = true;
            CloseNetwork();
        }

        protected override void OnReceiveReleaseRequest()
        {
            try
            {
                _handler.OnReceiveReleaseRequest(this, this._assoc as ServerAssociationParameters);
                
            }
            catch (Exception e)
            {
                OnUserException(e, "Unexpected exception on OnReceiveReleaseRequest");
                return;
            }
            SendReleaseResponse();
        }

        protected override void OnReceiveDimseRequest(byte pcid, DicomMessage msg)
        {
            try
            {
                _handler.OnReceiveRequestMessage(this, this._assoc as ServerAssociationParameters, pcid, msg);
            }
            catch (Exception e)
            {
                OnUserException(e, "Unexpected exception on OnReceiveRequestMessage");
            }
            return ;
        }

        protected override void OnReceiveDimseResponse(byte pcid, DicomMessage msg)
        {

            try
            {
                _handler.OnReceiveResponseMessage(this, this._assoc as ServerAssociationParameters, pcid, msg);
            }
            catch (Exception e)
            {
                OnUserException(e, "Unexpected exception on OnReceiveResponseMessage");
            }
            return;

        }

        #endregion

        #region IDisposable Members
        ///
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// object is reclaimed by garbage collection.
        ///
        ~DicomServer()
        {
            Dispose(false);
        }

        ///
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ///
        /// Disposes the specified disposing.
        ///
        /// if set to true [disposing].
        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                // Dispose of other Managed objects, ie
                CloseNetwork();
            }
            // FREE UNMANAGED RESOURCES
            _disposed = true;
        }
        #endregion
    }
}
