﻿#region License

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
using System.Globalization;
using ClearCanvas.Common;
using ClearCanvas.Dicom.Iod;
using ClearCanvas.Dicom.Iod.Iods;

namespace ClearCanvas.Dicom.Network.Scu
{

    /// <summary>
    /// Abstract class for Move SCU.  Please see <see cref="PatientRootMoveScu"/> and <see cref="StudyRootMoveScu"/>.
    /// </summary>
    public abstract class MoveScuBase :ScuBase 
    {
       #region Public Events/Delegates

        /// Event called when an image has completed being moved.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        public event EventHandler<EventArgs> ImageMoveCompleted;

        /// <summary>
        /// Delegate for starting Move in ASynch mode with <see cref="BeginMove"/>.
        /// </summary>
        public delegate void MoveDelegate();

        #endregion

        #region Private Variables
        private int _warningSubOperations = 0;
        private int _failureSubOperations = 0;
        private int _successSubOperations = 0;
        private int _totalSubOperations = 0;
        private int _remainingSubOperations = 0;
        private readonly DicomAttributeCollection _dicomAttributeCollection = new DicomAttributeCollection();
        string _destinationAe;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for Move SCU Component.
        /// </summary>
        /// <param name="localAe">The local AE title.</param>
        /// <param name="remoteAe">The remote AE title being connected to.</param>
        /// <param name="remoteHost">The hostname or IP address of the remote AE.</param>
        /// <param name="remotePort">The listen port of the remote AE.</param>
        /// <param name="destinationAe">The destination AE.</param>
        public MoveScuBase(string localAe, string remoteAe, string remoteHost, int remotePort, string destinationAe)
        {
            ClientAETitle = localAe;
            RemoteAE = remoteAe;
            RemoteHost = remoteHost;
            RemotePort = remotePort;
            _destinationAe = destinationAe;
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// The number of tranferred SOP Instances which had a warning status.
        /// </summary>
        public int WarningSubOperations
        {
            get { return _warningSubOperations; }
        }

        /// <summary>
        /// The number of transferred SOP Instances that had a failure status.
        /// </summary>
        public int FailureSubOperations
        {
            get { return _failureSubOperations; }
        }

        /// <summary>
        /// The number of transferred SOP Instances that had a success status.
        /// </summary>
        public int SuccessSubOperations
        {
            get { return _successSubOperations; }
        }

        /// <summary>
        /// The total number of SOP Instances to transfer.
        /// </summary>
        public int TotalSubOperations
        {
            get { return _totalSubOperations; }
        }

        /// <summary>
        /// The number of remaining SOP Instances to transfer.
        /// </summary>
        public int RemainingSubOperations
        {
            get { return _remainingSubOperations; }
        }


        /// <summary>
        /// Gets or sets the destination ae.
        /// </summary>
        /// <value>The destination ae.</value>
        public string DestinationAe
        {
            get { return _destinationAe; }
        	set { _destinationAe = value; }
        }




        /// <summary>
        /// Specifies the find sop class.
        /// </summary>
        /// <value>The find sop class.</value>
        /// <remarks>Abstract so subclass can specify.</remarks>
        public abstract SopClass MoveSopClass
        { get ; }
        #endregion

        #region Public Methods

        /// <summary>
        /// Move the SOP Instances
        /// </summary>
        public void Move()
        {
             _totalSubOperations = 0;
            _remainingSubOperations = 0;
            _failureSubOperations = 0;
            _successSubOperations = 0;
            _warningSubOperations = 0;

            Platform.Log(LogLevel.Info, "Preparing to connect to AE {0} on host {1} on port {2} for move request to {3}.",
                         RemoteAE, RemoteHost, RemotePort, _destinationAe);

            try
            {
                // the connect launches the actual send in a background thread.
                Connect();
            }
            catch (Exception e)
            {
				Platform.Log(LogLevel.Error,e,"Unexpected exception attempting to connect to {0}",RemoteAE);
                throw;
            }
        }

        /// <summary>
        /// Begins the move request in asynchronous mode.  
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <returns></returns>
        public IAsyncResult BeginMove(AsyncCallback callback, object asyncState)
        {
            MoveDelegate moveDelegate = new MoveDelegate(this.Move);
            return moveDelegate.BeginInvoke(callback, asyncState);
        }

        /// <summary>
        /// Ends the move (asynchronous mode).  
        /// </summary>
        /// <param name="ar">The ar.</param>
        public void EndMove(IAsyncResult ar)
        {
            MoveDelegate moveDelegate = ((System.Runtime.Remoting.Messaging.AsyncResult)ar).AsyncDelegate as MoveDelegate;

            if (moveDelegate != null)
            {
                moveDelegate.EndInvoke(ar);
            }
            else
            {
                throw new InvalidOperationException("cannot end invoke, asynchresult is null");
            }
        }

        /// <summary>
        /// Adds a study instance uid to the move request.
        /// </summary>
        /// <param name="studyInstanceUid">The study instance uid.</param>
        /// <exception cref="InvalidOperationException">If adding an instance of a different Query Level</exception>
        public void AddStudyInstanceUid(string studyInstanceUid)
        {
            CheckForOtherLevel(QueryRetrieveLevel.Study);
            _dicomAttributeCollection[DicomTags.StudyInstanceUid].AppendString(studyInstanceUid);
        }

        /// <summary>
        /// Adds a patient id to the move request.
        /// </summary>
        /// <param name="patientId">The patient id.</param>
        /// <exception cref="InvalidOperationException">If adding an instance of a different Query Level</exception>
        public void AddPatientId(string patientId)
        {
            CheckForOtherLevel(QueryRetrieveLevel.Patient);
            _dicomAttributeCollection[DicomTags.PatientId].AppendString(patientId);

        }

        /// <summary>
        /// Adds a series instance uid to the move request.
        /// </summary>
        /// <param name="seriesInstanceUid">The series instance uid.</param>
        /// <exception cref="InvalidOperationException">If adding an instance of a different Query Level</exception>
        public void AddSeriesInstanceUid(string seriesInstanceUid)
        {
            CheckForOtherLevel(QueryRetrieveLevel.Series);
            _dicomAttributeCollection[DicomTags.SeriesInstanceUid].AppendString(seriesInstanceUid);
        }

        /// <summary>
        /// Adds a sop instance uid to the move request.
        /// </summary>
        /// <param name="sopInstanceUid">The sop instance uid.</param>
        /// <exception cref="InvalidOperationException">If adding an instance of a different Query Level</exception>
        public void AddSopInstanceUid(string sopInstanceUid)
        {
            CheckForOtherLevel(QueryRetrieveLevel.Image);
            _dicomAttributeCollection[DicomTags.SopInstanceUid].AppendString(sopInstanceUid);
        }
        #endregion

        #region Protected Virtual Methods
        /// <summary>
        /// Called when [image move completed].
        /// </summary>
        protected virtual void OnImageMoveCompleted()
        {
            EventHandler<EventArgs> tempHandler = ImageMoveCompleted;
            if (tempHandler != null)
                tempHandler(this, new EventArgs());
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Sends the move request (called after the association is accepted).
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="association">The association.</param>
        private void SendMoveRequest(DicomClient client, ClientAssociationParameters association)
        {
            byte pcid = association.FindAbstractSyntaxOrThrowException(MoveSopClass);

            DicomMessage dicomMessage = new DicomMessage();
            foreach (DicomAttribute dicomAttribute in _dicomAttributeCollection)
            {
                // Need to do it this way in case the attribute is blank
                DicomAttribute dicomAttribute2 = dicomMessage.DataSet[dicomAttribute.Tag];
                if (dicomAttribute.Values != null)
                    dicomAttribute2.Values = dicomAttribute.Values;
            }

            client.SendCMoveRequest(pcid, client.NextMessageID(), _destinationAe, dicomMessage);
        }

        /// <summary>
        /// Checks for other query retrieve levels already used, returns exception if trying to add a different level.
        /// Sets the QueryRetrieveLevel tag to the <paramref name="queryRetrieveLevel"/> if it's not invalid.
        /// </summary>
        /// <param name="queryRetrieveLevel">The query retrieve level.</param>
        /// <exception cref="InvalidOperationException">If adding an instance of a different Query Level</exception>
        private void CheckForOtherLevel(QueryRetrieveLevel queryRetrieveLevel)
        {
            QueryRetrieveLevel currentLevel = IodBase.ParseEnum<QueryRetrieveLevel>(_dicomAttributeCollection[DicomTags.QueryRetrieveLevel].GetString(0, String.Empty), QueryRetrieveLevel.None);

            if (currentLevel != QueryRetrieveLevel.None && queryRetrieveLevel != currentLevel)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "Cannot add a {0} instance to Move request, alrady has a different kind of instance", queryRetrieveLevel));
            }
            else
            {
                IodBase.SetAttributeFromEnum(_dicomAttributeCollection[DicomTags.QueryRetrieveLevel], queryRetrieveLevel);
            }

        }
        #endregion

        #region Protected Overridden Methods
        /// <summary>
        /// Scan the files to send, and create presentation contexts for each abstract syntax to send.
        /// </summary>
        protected override void SetPresentationContexts()
        {
            byte pcid = AssociationParameters.FindAbstractSyntax(MoveSopClass);
            if (pcid == 0)
            {
                pcid = AssociationParameters.AddPresentationContext(MoveSopClass);

                AssociationParameters.AddTransferSyntax(pcid, TransferSyntax.ExplicitVrLittleEndian);
                AssociationParameters.AddTransferSyntax(pcid, TransferSyntax.ImplicitVrLittleEndian);
            }        
        }

        /// <summary>
        /// Called when received associate accept.  
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="association">The association.</param>
        public override void OnReceiveAssociateAccept(DicomClient client, ClientAssociationParameters association)
        {
            base.OnReceiveAssociateAccept(client, association);

            SendMoveRequest(client, association);
        }

        /// <summary>
        /// Called when received response message. 
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="association">The association.</param>
        /// <param name="presentationID">The presentation ID.</param>
        /// <param name="message">The message.</param>
		public override void OnReceiveResponseMessage(DicomClient client, ClientAssociationParameters association, byte presentationID, DicomMessage message)
        {
        	_failureSubOperations = message.NumberOfFailedSubOperations;
        	_successSubOperations = message.NumberOfCompletedSubOperations;
        	_remainingSubOperations = message.NumberOfRemainingSubOperations;
        	_warningSubOperations = message.NumberOfWarningSubOperations;
        	_totalSubOperations = _failureSubOperations + _successSubOperations + _remainingSubOperations +
        	                      _warningSubOperations;

        	if (message.Status.Status == DicomState.Pending)
        	{
        		OnImageMoveCompleted();
        	}
        	else
        	{
				DicomState status = message.Status.Status;
				if (message.Status.Status != DicomState.Success)
				{
					if (status == DicomState.Cancel)
					{
						Platform.Log(LogLevel.Info, "Cancel status received in Move Scu: {0}", message.Status);
						Status = ScuOperationStatus.Canceled;
					}
					else if (status == DicomState.Failure)
					{
						string msg = String.Format("Failure status received in Move Scu: {0}", message.Status);
						Platform.Log(LogLevel.Error, msg);
						Status = ScuOperationStatus.Failed;
						FailureDescription = msg;
					}
					else if (status == DicomState.Warning)
					{
						Platform.Log(LogLevel.Warn, "Warning status received in Move Scu: {0}", message.Status);
					}
					else if (this.Status == ScuOperationStatus.Canceled)
					{
						Platform.Log(LogLevel.Info, "Client cancelled Move Scu operation.");
					}
				}
				else
				{
					Platform.Log(LogLevel.Info, "Success status received in Move Scu!");
				}

				client.SendReleaseRequest();
				StopRunningOperation();
			}
        }

    	#endregion

        #region IDisposable Members

        private bool _disposed = false;
        /// <summary>
        /// Disposes the specified disposing.
        /// </summary>
        /// <param name="disposing">if set to <c>true</c> [disposing].</param>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                // Dispose of other Managed objects, ie

            }
            // FREE UNMANAGED RESOURCES
            base.Dispose(true);
            _disposed = true;
        }
        #endregion

    }

    #region PatientRootFindScu Class
    /// <summary>
    /// Patient Root Move Scu
    /// <para>
    /// <example>
    /// MoveScuBase moveScu = new PatientRootMoveScu("myClientAeTitle", "myServerAe", "127.0.0.1", 5678, "destinationAE");
    /// moveScu.AddStudyInstanceUid("1.3.46.670589.5.2.10.2156913941.892665384.993397");
    /// moveScu.Move();
    /// </example>
    /// </para>
    /// </summary>
    public class PatientRootMoveScu : MoveScuBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientRootMoveScu"/> class.
        /// </summary>
        /// <param name="localAe">The local AE title.</param>
        /// <param name="remoteAe">The remote AE title being connected to.</param>
        /// <param name="remoteHost">The hostname or IP address of the remote AE.</param>
        /// <param name="remotePort">The listen port of the remote AE.</param>
        /// <param name="destinationAe">The destination AE.</param>
        public PatientRootMoveScu(string localAe, string remoteAe, string remoteHost, int remotePort, string destinationAe)
            : base(localAe, remoteAe, remoteHost, remotePort, destinationAe)
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Specifies the move sop class (PatientRootQueryRetrieveInformationModelMove)
        /// </summary>
        /// <value>The find sop class.</value>
        /// <remarks>Abstract so subclass can specify.</remarks>
        public override SopClass MoveSopClass
        {
            get { return SopClass.PatientRootQueryRetrieveInformationModelMove; }
        }
        #endregion
    }

    #endregion

    #region StudyRootMoveScu
    /// <summary>
    /// Patient Root Move Scu
    /// <para>
    /// <example>
    /// MoveScuBase moveScu = new StudyRootMoveScu("myClientAeTitle", "myServerAe", "127.0.0.1", 5678, "destinationAE");
    /// moveScu.AddStudyInstanceUid("1.3.46.670589.5.2.10.2156913941.892665384.993397");
    /// moveScu.Move();
    /// </example>
    /// </para>
    /// </summary>
    public class StudyRootMoveScu : MoveScuBase
    {

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="StudyRootMoveScu"/> class.
        /// </summary>
        /// <param name="localAe">The local AE title.</param>
        /// <param name="remoteAe">The remote AE title being connected to.</param>
        /// <param name="remoteHost">The hostname or IP address of the remote AE.</param>
        /// <param name="remotePort">The listen port of the remote AE.</param>
        /// <param name="destinationAe">The destination AE.</param>
        public StudyRootMoveScu(string localAe, string remoteAe, string remoteHost, int remotePort, string destinationAe)
            : base(localAe, remoteAe, remoteHost, remotePort, destinationAe)
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Specifies the move sop class (StudyRootQueryRetrieveInformationModelMove)
        /// </summary>
        /// <value>The find sop class.</value>
        /// <remarks>Abstract so subclass can specify.</remarks>
        public override SopClass MoveSopClass
        {
            get { return SopClass.StudyRootQueryRetrieveInformationModelMove; }
        }
        #endregion
    }

    #endregion

}
