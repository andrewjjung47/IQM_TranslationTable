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

using System.Collections.Generic;
using System.Net;
using ClearCanvas.Common;

namespace ClearCanvas.Dicom.Network.Scp
{
    /// <summary>
    /// Base class implementing a DICOM SCP.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class uses the ClearCanvas.Dicom assembly to implement a DICOM SCP.  
    /// It handles most of the basic interactions with the DICOM library.
    /// </para>
    /// <para>
    /// The class depends on an <see cref="ExtensionPoint"/> for handling action DICOM 
    /// services.  The class will load plugins that implement the <see cref="IDicomScp{TContext}"/> interface.
    /// It will query these plugins to determine what DICOM Servies they support, and then 
    /// construct a list of transfer syntaxes and DICOM services supported based on the plugins.
    /// </para>
    /// <para>
    /// When a request message arrives, the appropriate plugin will be called to process the
    /// incoming message.  Note that different plugins can support the same DICOM service, but 
    /// different transfer syntaxes.
    /// </para>
    /// </remarks>
    public class DicomScp<TContext>
    {
        /// <summary>
        /// Delegate called to verify if an association should be accepted or rejected.
        /// </summary>
        /// <remarks>
        /// If assigned in the constructor to <see cref="DicomScp{TContext}"/>, this delegate is called by <see cref="DicomScp{TContext}"/>
        /// to check if an association should be rejected or accepted.  
        /// </remarks>
        /// <param name="context">User parameters passed to the constructor to <see cref="DicomScp{TContext}"/></param>
        /// <param name="assocParms">Parameters for the association.</param>
        /// <param name="result">If the delegate returns false, the DICOM reject result is returned here.</param>
        /// <param name="reason">If the delegate returns false, the DICOM reject reason is returned here.</param>
        /// <returns>true if the association should be accepted, false if rejected.</returns>
        public delegate bool AssociationVerifyCallback(TContext context, ServerAssociationParameters assocParms, out DicomRejectResult result, out DicomRejectReason reason);

        #region Constructors
        /// <summary>
        /// Constructor for the DICOM SCP.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The constructor allows the user to pass an object to plugins that implement the 
        /// <see cref="IDicomScp{TContext}"/> interface. 
        /// </para>
        /// </remarks>
        /// <param name="context">An object to be passed to plugins implementing the <see cref="IDicomScp{TContext}"/> interface.</param>
        /// <param name="verifier">Delegate called when a new association arrives to verify if it should be accepted or rejected.  Can be set to null.</param>
        public DicomScp(TContext context, AssociationVerifyCallback verifier)
        {
            _context = context;
            _verifier = verifier;
        }
        #endregion

        #region Private Members

        private string _aeTitle;
        private int _listenPort;
        private ServerAssociationParameters _assocParameters;
        private readonly TContext _context;
        private readonly AssociationVerifyCallback _verifier;
        #endregion

        #region Properties
        /// <summary>
        /// The local Application Entity Title of the DICOM SCP.
        /// </summary>
        public string AeTitle
        {
            get { return _aeTitle; }
            set { _aeTitle = value; }
        }

        /// <summary>
        /// The listen port of the DICOM SCP. 
        /// </summary>
        public int ListenPort
        {
            get { return _listenPort; }
            set { _listenPort = value; }
        }

        /// <summary>
        /// The Association parameters used to negotiate the association.
        /// </summary>
        public ServerAssociationParameters AssociationParameters
        {
            get { return _assocParameters; }
        }

		/// <summary>
		/// The context associated with component.
		/// </summary>
    	public TContext Context
    	{
			get { return _context; }
    	}
        #endregion

        #region Private Methods
        /// <summary>
        /// Create the list of presentation contexts for the DICOM SCP.
        /// </summary>
        /// <remarks>
        /// The method loads the DICOM Scp plugins, and then queries them
        /// to construct a list of presentation contexts that are supported.
        /// </remarks>
        private void CreatePresentationContexts()
        {
            DicomScpExtensionPoint<TContext> ep = new DicomScpExtensionPoint<TContext>();
            object[] scps = ep.CreateExtensions();
            foreach (object obj in scps)
            {
                IDicomScp<TContext> scp = obj as IDicomScp<TContext>;
                scp.SetContext(_context);

                IList<SupportedSop> sops = scp.GetSupportedSopClasses();
                foreach (SupportedSop sop in sops)
                {
                    byte pcid = _assocParameters.FindAbstractSyntax(sop.SopClass);
                    if (pcid == 0)
                        pcid = _assocParameters.AddPresentationContext(sop.SopClass);

                    // Now add all the transfer syntaxes, if necessary
                    foreach (TransferSyntax syntax in sop.SyntaxList)
                    {
                        // Check if the syntax is registered already
                        if (0 == _assocParameters.FindAbstractSyntaxWithTransferSyntax(sop.SopClass, syntax))
                        {
                            _assocParameters.AddTransferSyntax(pcid, syntax);
                        }
                    }
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Delegate for use with <see cref="DicomServer"/> to create a handler
        /// that implements the <see cref="IDicomServerHandler"/> interface for a new incoming association.
        /// </summary>
        /// <param name="assoc">The association parameters for the negotiated association.</param>
        /// <returns>A new <see cref="DicomScpHandler{TContext}"/> instance.</returns>
        public IDicomServerHandler StartAssociation(DicomServer server, ServerAssociationParameters assoc)
        {
            return new DicomScpHandler<TContext>(server, assoc, _context, _verifier);
        }

        /// <summary>
        /// Start listening for associations.
        /// </summary>
        /// <returns>true on success, false on failure.</returns>
        public bool Start(IPAddress addr)
        {
            try
            {
                _assocParameters = new ServerAssociationParameters(AeTitle, new IPEndPoint(addr, ListenPort));

                // Load our presentation contexts from all the extensions
                CreatePresentationContexts();

                if (_assocParameters.GetPresentationContextIDs().Count == 0)
                {
                    Platform.Log(LogLevel.Fatal, "No configured presentation contexts for AE: {0}", AeTitle);
                    return false;
                }

                DicomServer.StartListening(_assocParameters, StartAssociation);
            }
            catch (DicomException ex)
            {
                Platform.Log(LogLevel.Fatal, ex, "Unexpected exception when starting listener on port {0)", ListenPort);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Stop the association listener.
        /// </summary>
        public void Stop()
        {
            try
            {
                DicomServer.StopListening(_assocParameters);
            }
            catch (DicomException e)
            {
                Platform.Log(LogLevel.Error, e, "Unexpected exception when stopping listening on port {0}", ListenPort);
            }
        }
        #endregion
    }
}
