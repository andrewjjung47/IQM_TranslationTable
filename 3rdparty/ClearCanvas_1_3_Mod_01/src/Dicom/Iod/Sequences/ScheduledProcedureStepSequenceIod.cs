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

namespace ClearCanvas.Dicom.Iod.Sequences
{
    /// <summary>
    /// Scheduled Procedure Step Sequence (0040,0100)
    /// </summary>
    /// <remarks>As per Dicom Doc 3, C.4-10 (pg 249)</remarks>
    public class ScheduledProcedureStepSequenceIod : SequenceIodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledProcedureStepSequenceIod"/> class.
        /// </summary>
        public ScheduledProcedureStepSequenceIod()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledProcedureStepSequenceIod"/> class.
        /// </summary>
        /// <param name="dicomSequenceItem">The dicom sequence item.</param>
        public ScheduledProcedureStepSequenceIod(DicomSequenceItem dicomSequenceItem)
            : base(dicomSequenceItem)
        {
        }
        #endregion

        #region Public Properties
        public string ScheduledStationAeTitle
        {
            get { return base.DicomAttributeCollection[DicomTags.ScheduledStationAeTitle].GetString(0, String.Empty); }
            set { base.DicomAttributeCollection[DicomTags.ScheduledStationAeTitle].SetString(0, value); }
        }

        public string ScheduledStationName
        {
            get { return base.DicomAttributeCollection[DicomTags.ScheduledStationName].GetString(0, String.Empty); }
            set { base.DicomAttributeCollection[DicomTags.ScheduledStationName].SetString(0, value); }
        }

        public string ScheduledProcedureStepLocation
        {
            get { return base.DicomAttributeCollection[DicomTags.ScheduledProcedureStepLocation].GetString(0, String.Empty); }
            set { base.DicomAttributeCollection[DicomTags.ScheduledProcedureStepLocation].SetString(0, value); }
        }

        public DateTime ScheduledProcedureStepStartDate
        {
            get { return base.DicomAttributeCollection[DicomTags.ScheduledProcedureStepStartDate].GetDateTime(0, DateTime.MinValue); }
            set { base.DicomAttributeCollection[DicomTags.ScheduledProcedureStepStartDate].SetDateTime(0, value); }
        }

        public DateTime ScheduledProcedureStepEndDate
        {
            get { return base.DicomAttributeCollection[DicomTags.ScheduledProcedureStepEndDate].GetDateTime(0, DateTime.MinValue); }
            set { base.DicomAttributeCollection[DicomTags.ScheduledProcedureStepEndDate].SetDateTime(0, value); }
        }

        public PersonName ScheduledPerformingPhysiciansName
        {
            get { return new PersonName(base.DicomAttributeCollection[DicomTags.ScheduledPerformingPhysiciansName].GetString(0, String.Empty)); }
            set { base.DicomAttributeCollection[DicomTags.ScheduledPerformingPhysiciansName].SetString(0, value.ToString()); }
        }

        public string ScheduledProcedureStepDescription
        {
            get { return base.DicomAttributeCollection[DicomTags.ScheduledProcedureStepDescription].GetString(0, String.Empty); }
            set { base.DicomAttributeCollection[DicomTags.ScheduledProcedureStepDescription].SetString(0, value); }
        }

        public string ScheduledProcedureStepId
        {
            get { return base.DicomAttributeCollection[DicomTags.ScheduledProcedureStepId].GetString(0, String.Empty); }
            set { base.DicomAttributeCollection[DicomTags.ScheduledProcedureStepId].SetString(0, value); }
        }

        public ScheduledProcedureStepStatus ScheduledProcedureStepStatus
        {
            get { return IodBase.ParseEnum<ScheduledProcedureStepStatus>(base.DicomAttributeCollection[DicomTags.ScheduledProcedureStepStatus].GetString(0, String.Empty), ScheduledProcedureStepStatus.None); }
            set { IodBase.SetAttributeFromEnum(base.DicomAttributeCollection[DicomTags.ScheduledProcedureStepStatus], value, false); }
        }



        public string CommentsOnTheScheduledProcedureStep
        {
            get { return base.DicomAttributeCollection[DicomTags.CommentsOnTheScheduledProcedureStep].GetString(0, String.Empty); }
            set { base.DicomAttributeCollection[DicomTags.CommentsOnTheScheduledProcedureStep].SetString(0, value); }
        }

        public string Modality
        {
            get { return base.DicomAttributeCollection[DicomTags.Modality].GetString(0, String.Empty); }
            set { base.DicomAttributeCollection[DicomTags.Modality].SetString(0, value); }
        }

        public string RequestedContrastAgent
        {
            get { return base.DicomAttributeCollection[DicomTags.RequestedContrastAgent].GetString(0, String.Empty); }
            set { base.DicomAttributeCollection[DicomTags.RequestedContrastAgent].SetString(0, value); }
        }

        public string PreMedication
        {
            get { return base.DicomAttributeCollection[DicomTags.PreMedication].GetString(0, String.Empty); }
            set { base.DicomAttributeCollection[DicomTags.PreMedication].SetString(0, value); }
        }
        
       #endregion

        #region Public Methods
        /// <summary>
        /// Sets the common tags for a typical Modality Worklist Request.
        /// </summary>
        public void SetCommonTags()
        {
            SetCommonTags(this);
        }
        #endregion

        #region Public Static Methods


        /// <summary>
        /// Sets the common tags for a typical Modality Worklist Request.
        /// </summary>
		/// <param name="scheduledProcedureStepSequenceIod">The scheduled step attributes sequence iod.</param>
        public static void SetCommonTags(ScheduledProcedureStepSequenceIod scheduledProcedureStepSequenceIod)
        {
            scheduledProcedureStepSequenceIod.SetAttributeNull(DicomTags.Modality);
            scheduledProcedureStepSequenceIod.SetAttributeNull(DicomTags.ScheduledProcedureStepId);
            scheduledProcedureStepSequenceIod.SetAttributeNull(DicomTags.ScheduledProcedureStepDescription);
            scheduledProcedureStepSequenceIod.SetAttributeNull(DicomTags.ScheduledStationAeTitle);
            scheduledProcedureStepSequenceIod.SetAttributeNull(DicomTags.ScheduledProcedureStepStartDate);
            scheduledProcedureStepSequenceIod.SetAttributeNull(DicomTags.ScheduledProcedureStepStartTime);
            scheduledProcedureStepSequenceIod.SetAttributeNull(DicomTags.ScheduledPerformingPhysiciansName);
            scheduledProcedureStepSequenceIod.SetAttributeNull(DicomTags.ScheduledProcedureStepLocation);
            scheduledProcedureStepSequenceIod.SetAttributeNull(DicomTags.ScheduledProcedureStepStatus);
            scheduledProcedureStepSequenceIod.SetAttributeNull(DicomTags.CommentsOnTheScheduledProcedureStep);
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ScheduledProcedureStepStatus
    {
        /// <summary>
        /// No status, or empty value
        /// </summary>
        None,
        /// <summary>
        /// Procedure Step scheduled
        /// </summary>
        Scheduled,
        /// <summary>
        /// patient is available for the Scheduled Procedure Step
        /// </summary>
        Arrived,
        /// <summary>
        /// all patient and other necessary preparation for this step has been completed
        /// </summary>
        Ready,
        /// <summary>
        /// at least one Performed Procedure Step has been created that references this Scheduled Procedure Step
        /// </summary>
        Started
    }
}
