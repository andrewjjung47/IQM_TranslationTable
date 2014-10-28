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

using System;
using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Dicom.Iod;
using ClearCanvas.Dicom.Utilities.Statistics;

namespace ClearCanvas.Dicom.Network.Scu
{
	/// <summary>
	/// This is the base class for SCU classes.
	/// </summary>
	/// <remarks>The three main methods that shouold be overwritten are <see cref="SetPresentationContexts"/>,
	/// <see cref="OnReceiveAssociateAccept"/>, and <see cref="OnReceiveAssociateAccept"/>.
	/// <para>Note, built into this class, so all Scu classes can use the <see cref="Timeout"/> property and
	/// the <see cref="Cancel"/> method.
	/// </para>
	/// </remarks>
	public class ScuBase : IDicomClientHandler, IDisposable
	{
		#region Private Variables...
		private string _clientAETitle;
		protected ClientAssociationParameters _associationParameters = null;
		protected DicomClient _dicomClient = null;
		private readonly System.Threading.AutoResetEvent _progressEvent = new System.Threading.AutoResetEvent(false);
		private string _remoteAE;
		private string _remoteHost;
		private int _remotePort;
		private ScuOperationStatus _status;
		private int _timeout = 60; // 60 second default timeout
		private AssociationStatisticsRecorder _statsRecorder;
		private string _failureDescription = "";
        private DicomState _resultStatus;
		#endregion

		#region Protected Properties...
		/// <summary>
		/// Gets the progress event.
		/// </summary>
		/// <value>The progress event.</value>
		protected System.Threading.AutoResetEvent ProgressEvent
		{
			get { return _progressEvent; }
		}

		#region AssociationParameters
		/// <summary>
		/// Gets or sets the association parameters.
		/// </summary>
		/// <value>The association parameters.</value>
		protected ClientAssociationParameters AssociationParameters
		{
			get { return _associationParameters; }
			set { _associationParameters = value; }
		}
		#endregion

		#region Client
		/// <summary>
		/// Gets or sets the Dicom Client.
		/// </summary>
		/// <value>The client.</value>
		protected DicomClient Client
		{
			get { return _dicomClient; }
			set { _dicomClient = value; }
		}
		#endregion

		#region ClientAETitle
		/// <summary>
		/// Gets or sets the client AE title.
		/// </summary>
		/// <value>The client AE title.</value>
		protected string ClientAETitle
		{
			get { return _clientAETitle; }
			set { _clientAETitle = value; }
		}
		#endregion

		#region RemoteAE
		/// <summary>
		/// Gets or sets the remote AE.
		/// </summary>
		/// <value>The remote AE.</value>
		protected string RemoteAE
		{
			get { return _remoteAE; }
			set { _remoteAE = value; }
		}
		#endregion

		#region RemoteHost
		/// <summary>
		/// Gets or sets the remote host.
		/// </summary>
		/// <value>The remote host.</value>
		protected string RemoteHost
		{
			get { return _remoteHost; }
			set { _remoteHost = value; }
		}
		#endregion

		#region RemotePort
		/// <summary>
		/// Gets or sets the remote port.
		/// </summary>
		/// <value>The remote port.</value>
		protected int RemotePort
		{
			get { return _remotePort; }
			set { _remotePort = value; }
		}
		#endregion

		#region FailureDescription
		public string FailureDescription
		{
			get { return _failureDescription; }
			set { _failureDescription = value; }
		}
		#endregion

		#region Status
		/// <summary>
		/// Gets or sets the operation status.
		/// </summary>
		/// <value>The status.</value>
		public ScuOperationStatus Status
		{
			get { return _status; }
			set { _status = value; }
		}
		#endregion

		#region Timeout
		/// <summary>
		/// Gets or sets the timeout.
		/// </summary>
		/// <value>The timeout in seconds.  (Default: 60)</value>
		public int Timeout
		{
			get { return _timeout; }
			set { _timeout = value; }
		}
		#endregion

		/// <summary>
		/// Gets or sets Canceled - ie, rerturns true if the <see cref="Status"/> Property equals Canceled.
		/// </summary>
		/// <value><c>true</c> if canceled; otherwise, <c>false</c>.</value>
		public bool Canceled
		{
			get { return Status == ScuOperationStatus.Canceled; }
		}

        /// <summary>
        /// Gets the result status.
        /// </summary>
        /// <value>The result status.</value>
        public DicomState ResultStatus
        {
            get { return _resultStatus; }
            protected set { _resultStatus = value; }
        }
		#endregion

		#region Public Methods...
		/// <summary>
		/// Cancels the operation.
		/// </summary>
		public void Cancel()
		{
			Platform.Log(LogLevel.Info, "Canceling...");
			Status = ScuOperationStatus.Canceled;
			ProgressEvent.Set();
		}

		/// <summary>
		/// Wait for the background thread for the client to close. 
		/// </summary>
		public void Join()
		{
			if (Client != null)
			{
				Client.Join();
				Client = null;
			}
		}

		/// <summary>
		/// Wait for the background thread for the client to close. 
		/// </summary>
		/// <returns>
		/// <value>true</value> if the thread as exited, <value>false</value> if timeout.
		/// </returns>
		public bool Join(TimeSpan timeout)
		{
			if (Client != null)
			{
				bool returnVal = Client.Join(timeout);
				Client = null;
				return returnVal;
			}
			return true;
		}

		/// <summary>
		/// Convert a collection of DICOM attribute collections into the specificed list of Iods.
		/// </summary>
		/// <typeparam name="T">An <see cref="IodBase"/> derived class.</typeparam>
		/// <param name="collectionList">The list of DICOM attribute collections to convert</param>
		/// <returns>The list of IODs.</returns>
		public static IList<T> GetResultsAsIod<T>(IList<DicomAttributeCollection> collectionList) where T:IodBase, new()
		{
			IList<T> iodList = new List<T>();

			foreach (DicomAttributeCollection collection in collectionList)
			{
				T iod = new T();
				iod.DicomAttributeCollection = collection;
				iodList.Add(iod);
			}
			return iodList;
		}
	
		#endregion

		#region Protected Methods...

		/// <summary>
		/// Connects to specified server with the specified information.
		/// </summary>
		/// <remarks>Note this calls <see cref="SetPresentationContexts"/> to get the list of presentation
		/// contexts for the association request, so this method should be overwritten in the subclass.</remarks>
		protected void Connect()
		{
			try
			{
				if (_dicomClient != null)
				{
					// TODO: Dispose of properly...
					CloseDicomClient();
				}

				if (String.IsNullOrEmpty(ClientAETitle) || String.IsNullOrEmpty(RemoteAE) ||
				    String.IsNullOrEmpty(RemoteHost) || RemotePort == 0)
					throw new InvalidOperationException("Client and/or Remote info not specified.");

				AssociationParameters =
					new ClientAssociationParameters(ClientAETitle, RemoteAE, RemoteHost, RemotePort);

				AssociationParameters.ReadTimeout = Timeout*1000;
				AssociationParameters.WriteTimeout = Timeout*1000;

				SetPresentationContexts();

				Status = ScuOperationStatus.Running;
				_dicomClient = DicomClient.Connect(AssociationParameters, this);
				ProgressEvent.WaitOne();
			}
			catch (Exception ex)
			{
				Status = ScuOperationStatus.ConnectFailed;
				FailureDescription = ex.Message;
				Platform.Log(LogLevel.Error, ex, "Exception attempting connection to RemoteHost {0} ({1}:{2})", RemoteAE, RemoteHost, RemotePort);
				throw;
			}

		}

        protected void Connect(string clientAETitle, string remoteAE, string remoteHost, int remotePort)
        {
            Platform.Log(LogLevel.Info, "Preparing to connect to AE {0} on host {1} on port {2} for printer status request.", remoteAE, remoteHost, remotePort);
            try
            {
                ClientAETitle = clientAETitle;
                RemoteAE = remoteAE;
                RemoteHost = remoteHost;
                RemotePort = remotePort;
                Connect();
            }
            catch (Exception e)
            {
                Platform.Log(LogLevel.Error, e, "Unexpected exception trying to connect to Remote AE {0} on host {1} on port {2}", remoteAE, remoteHost, remotePort);
                throw;
            }
        }

		/// <summary>
		/// Checks for canceled.  Throws a <see cref="OperationCanceledException"/> if the operation is canceled.
		/// </summary>
		/// <exception cref="OperationCanceledException"/>
		protected void CheckForCanceled()
		{
			if (Status == ScuOperationStatus.Canceled)
				throw new OperationCanceledException();
		}

		/// <summary>
		/// Checks for timeout expired.  Throws a <see cref="TimeoutException"/> if timeout has expired.
		/// </summary>
		/// <exception cref="TimeoutException"/>
		protected void CheckForTimeoutExpired()
		{
			if (Status == ScuOperationStatus.TimeoutExpired)
				throw new TimeoutException();
		}


		/// <summary>
		/// Stops the running operation, by setting the Status to NotRunning, stopping the timer, and setting
		/// the Progress Wait Event so execution can continue.
		/// </summary>
		protected void StopRunningOperation()
		{
			//If it's anything else, then the client code may want to inspect the value.
			if (Status == ScuOperationStatus.Running)
				Status = ScuOperationStatus.NotRunning;

			ProgressEvent.Set();
		}

		protected void StopRunningOperation(ScuOperationStatus status)
		{
			Status = status;
			ProgressEvent.Set();
		}

        /// <summary>
        /// Adds the sop class to presentation context for Explicit and Implicit Vr Little Endian
        /// </summary>
        /// <param name="sopClass">The sop class.</param>
        /// <exception cref="DicomException"/>
        /// <exception cref="ArgumentNullException"/>
        protected void AddSopClassToPresentationContext(SopClass sopClass)
        {
            if (sopClass == null)
                throw new ArgumentNullException("sopClass");

            byte pcid = AssociationParameters.FindAbstractSyntax(sopClass);
            if (pcid == 0)
            {
                pcid = AssociationParameters.AddPresentationContext(sopClass);

                AssociationParameters.AddTransferSyntax(pcid, TransferSyntax.ExplicitVrLittleEndian);
                AssociationParameters.AddTransferSyntax(pcid, TransferSyntax.ImplicitVrLittleEndian);
            }
            else
            {
                throw new DicomException("Cannot find SopClass in association parameters: " + sopClass.ToString());
            }
        }

        /// <summary>
        /// Releases the connection.
        /// </summary>
        /// <param name="client">The client.</param>
        protected void ReleaseConnection(DicomClient client)
        {
            ProgressEvent.Set();
            if (client != null)
                client.SendReleaseRequest();
            StopRunningOperation();
        }
		#endregion

		#region Protected Abstracts/Virtual Methods...
		/// <summary>
		/// Sets the presentation contexts that the association will attempt to connect on.
		/// Note, this must be implemented in the subclass.
		/// </summary>
		protected virtual void SetPresentationContexts()
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Private Methods...
		/// <summary>
		/// This (forcefully?) Closes the dicom client.
		/// </summary>
		private void CloseDicomClient()
		{
			try
			{
				_dicomClient.Dispose();
				_dicomClient = null;
			}
			catch (Exception)
			{ }

		}
		#endregion

		#region IDicomClientHandler Members

		/// <summary>
		/// Called when received associate accept.  In this event we should then send the specific request 
		/// we wish to do (ie, CStore, CEcho, etc.).
		/// </summary>
		/// <remarks>Note, this should be overridden in subclasses.  It is also recommended for the overridden
		/// method to first call this to log the associate accept information:
		/// <code>
		/// public override void OnReceiveAssociateAccept(DicomClient client, ClientAssociationParameters association)
		/// {
		///     base.OnReceiveAssociateAccept(client, association);
		///     // SEND Request like CStore or CEcho...
		/// }
		/// </code>
		/// </remarks>
		/// <param name="client">The client.</param>
		/// <param name="association">The association.</param>
		public virtual void OnReceiveAssociateAccept(DicomClient client, ClientAssociationParameters association)
		{
			_statsRecorder = new AssociationStatisticsRecorder(client);

			Platform.Log(LogLevel.Info, "Association Accepted when {0} connected to remote AE {1}", association.CallingAE, association.CalledAE);
		}

		/// <summary>
		/// Called when received response message.  This should be overridden by the subclass.
		/// </summary>
		/// <remarks>This is where we receive the message back in a CEcho or CFind, and for a CStore, we
		/// would send additional files if necessary.  Note, the subclass should *not* call this method.  Perhaps
		/// this is comfusing with <see cref="OnReceiveAssociateAccept"/> recommended to call the base class.?
		/// <para>Note, the overridden method in the subclass should call <see cref="StopRunningOperation"/> 
		/// when operation is completed.</para>
		/// </remarks>
		/// <param name="client">The client.</param>
		/// <param name="association">The association.</param>
		/// <param name="presentationID">The presentation ID.</param>
		/// <param name="message">The message.</param>
		public virtual void OnReceiveResponseMessage(DicomClient client, ClientAssociationParameters association, byte presentationID, DicomMessage message)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Called when [receive associate reject].
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="association">The association.</param>
		/// <param name="result">The result.</param>
		/// <param name="source">The source.</param>
		/// <param name="reason">The reason.</param>
		public void OnReceiveAssociateReject(DicomClient client, ClientAssociationParameters association, DicomRejectResult result, DicomRejectSource source, DicomRejectReason reason)
		{
			FailureDescription = String.Format("Association Rejection when {0} connected to remote AE {1}", association.CallingAE, association.CalledAE);
			Platform.Log(LogLevel.Info, FailureDescription);
			StopRunningOperation(ScuOperationStatus.Failed);
		}

		/// <summary>
		/// Called when [receive request message].
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="association">The association.</param>
		/// <param name="presentationID">The presentation ID.</param>
		/// <param name="message">The message.</param>
		public void OnReceiveRequestMessage(DicomClient client, ClientAssociationParameters association, byte presentationID, DicomMessage message)
		{
			Platform.Log(LogLevel.Error, "Unexpected OnReceiveRequestMessage callback on client.");
			try
			{
				client.SendAssociateAbort(DicomAbortSource.ServiceUser, DicomAbortReason.NotSpecified);
			}
			catch (Exception ex)
			{
				Platform.Log(LogLevel.Error, ex, "Error aborting association");
			}
			StopRunningOperation(ScuOperationStatus.Failed);
			throw new Exception("The method or operation is not implemented.");
		}

		/// <summary>
		/// Called when [receive release response].
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="association">The association.</param>
		public void OnReceiveReleaseResponse(DicomClient client, ClientAssociationParameters association)
		{
			Platform.Log(LogLevel.Info, "Association released from {0} to {1}", association.CallingAE, association.CalledAE);
			StopRunningOperation();
		}

		/// <summary>
		/// Called when [receive abort].
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="association">The association.</param>
		/// <param name="source">The source.</param>
		/// <param name="reason">The reason.</param>
		public void OnReceiveAbort(DicomClient client, ClientAssociationParameters association, DicomAbortSource source, DicomAbortReason reason)
		{
			FailureDescription = String.Format( "Unexpected association abort received from {0} to {1}", association.CallingAE, association.CalledAE);
			Platform.Log(LogLevel.Info, FailureDescription);
			StopRunningOperation(ScuOperationStatus.Failed);
		}

		/// <summary>
		/// Called when [network error].
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="association">The association.</param>
		/// <param name="e">The e.</param>
		public void OnNetworkError(DicomClient client, ClientAssociationParameters association, Exception e)
		{
			FailureDescription = String.Format("Unexpected network error");
			Platform.Log(LogLevel.Info, FailureDescription);
			StopRunningOperation(ScuOperationStatus.Failed);
		}

		/// <summary>
		/// Called when [dimse timeout].
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="association">The association.</param>
		public void OnDimseTimeout(DicomClient client, ClientAssociationParameters association)
		{
			Status = ScuOperationStatus.TimeoutExpired;
			FailureDescription = String.Format("Timeout Expired for remote host {0}, aborting connection", RemoteAE);
			Platform.Log(LogLevel.Info, FailureDescription );

			try
			{
				client.SendAssociateAbort(DicomAbortSource.ServiceUser, DicomAbortReason.NotSpecified);
			}
			catch (Exception ex)
			{
				Platform.Log(LogLevel.Error, ex, "Error aborting association");
			}

			Platform.Log(LogLevel.Info, "Completed aborting connection from {0} to {1}", association.CallingAE, association.CalledAE);
			ProgressEvent.Set();
		}

		#endregion

		#region IDisposable Members
		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="ScuBase"/> is reclaimed by garbage collection.
		/// </summary>
		~ScuBase()
		{
			Dispose(false);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private bool _disposed = false;
		/// <summary>
		/// Disposes the specified disposing.
		/// </summary>
		/// <param name="disposing">if set to <c>true</c> [disposing].</param>
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;
			if (disposing)
			{
				// Dispose of other Managed objects, ie
				CloseDicomClient();
			}
			// FREE UNMANAGED RESOURCES
			_disposed = true;
		}
		#endregion

	}

	/// <summary>
	/// 
	/// </summary>
	public enum ScuOperationStatus
	{
		/// <summary>
		/// 
		/// </summary>
		NotRunning = 0,
		/// <summary>
		/// 
		/// </summary>
		Running = 1,
		/// <summary>
		/// 
		/// </summary>
		TimeoutExpired = 2,
		/// <summary>
		/// 
		/// </summary>
		Canceled = 3,
		///<summary>
		/// A failure occured
		/// </summary>
		Failed = 4,
		///<summary>
		/// A connection failure occured
		/// </summary>
		ConnectFailed = 5,
	}
}
