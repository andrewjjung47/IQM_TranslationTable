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

using ClearCanvas.Dicom.Iod.Modules;
using ClearCanvas.Dicom.Iod.Sequences;

namespace ClearCanvas.Dicom.Iod.Iods
{
    /// <summary>
    /// Modality worklist IOD
    /// </summary>
    /// <remarks>As per Dicom Doc 4, K6.1.2 (pg 193-194)</remarks>
    public class ModalityWorklistIod : IodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ModalityWorklistIod"/> class.
        /// </summary>
        public ModalityWorklistIod()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModalityWorklistIod"/> class.
        /// </summary>
        /// <param name="dicomAttributeCollection">The dicom attribute collection.</param>
        public ModalityWorklistIod(DicomAttributeCollection dicomAttributeCollection)
            :base(dicomAttributeCollection)
        {
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the patient module.
        /// </summary>
        /// <value>The patient module.</value>
        public PatientIdentificationModuleIod PatientIdentificationModule
        {
            get { return base.GetModuleIod<PatientIdentificationModuleIod>(); }
        }

        /// <summary>
        /// Gets the requested procedure module.
        /// </summary>
        /// <value>The requested procedure module.</value>
        public RequestedProcedureModuleIod RequestedProcedureModule
        {
            get { return base.GetModuleIod<RequestedProcedureModuleIod>(); }
        }

        /// <summary>
        /// Gets the scheduled procedure step module.
        /// </summary>
        /// <value>The scheduled procedure step module.</value>
        public ScheduledProcedureStepModuleIod ScheduledProcedureStepModule
        {
            get { return base.GetModuleIod<ScheduledProcedureStepModuleIod>(); }
        }

        /// <summary>
        /// Gets the imaging service request module.
        /// </summary>
        /// <value>The imaging service request module.</value>
        public ImagingServiceRequestModule ImagingServiceRequestModule
        {
            get { return base.GetModuleIod<ImagingServiceRequestModule>(); }
        }
       #endregion

        #region Public Methods
        /// <summary>
        /// Sets the common tags for a typical Modality Worklist Request.
        /// </summary>
        public void SetCommonTags()
        {
            SetCommonTags(base.DicomAttributeCollection);
        }
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Sets the common tags for a typical Modality Worklist Request.
        /// </summary>
        /// <param name="dicomAttributeCollection">The dicom attribute collection.</param>
        public static void SetCommonTags(DicomAttributeCollection dicomAttributeCollection)
        {
            ModalityWorklistIod iod = new ModalityWorklistIod(dicomAttributeCollection);
            //iod.PatientIdentificationModule.PatientsName.FirstName = "*";
            iod.DicomAttributeCollection[DicomTags.PatientsName].SetStringValue("*");
            iod.SetAttributeNull(DicomTags.PatientId);
            iod.SetAttributeNull(DicomTags.PatientsBirthDate);
            iod.SetAttributeNull(DicomTags.PatientsBirthTime);
            iod.SetAttributeNull(DicomTags.PatientsWeight);
            iod.SetAttributeNull(DicomTags.RequestedProcedureId);
            iod.SetAttributeNull(DicomTags.RequestedProcedureDescription);
            iod.SetAttributeNull(DicomTags.StudyInstanceUid);
            iod.SetAttributeNull(DicomTags.ReasonForTheRequestedProcedure);
            iod.SetAttributeNull(DicomTags.RequestedProcedureComments);
            iod.SetAttributeNull(DicomTags.RequestedProcedurePriority);
            iod.SetAttributeNull(DicomTags.ImagingServiceRequestComments);
            iod.SetAttributeNull(DicomTags.RequestingPhysician);
            iod.SetAttributeNull(DicomTags.ReferringPhysiciansName);
            iod.SetAttributeNull(DicomTags.RequestedProcedureLocation);
            iod.SetAttributeNull(DicomTags.AccessionNumber);

            ScheduledProcedureStepSequenceIod scheduledProcedureStepSequenceIod = new ScheduledProcedureStepSequenceIod();
            scheduledProcedureStepSequenceIod.SetCommonTags();
            iod.ScheduledProcedureStepModule.ScheduledProcedureStepSequenceList.Add(scheduledProcedureStepSequenceIod);

            //// TODO: this better and easier...
            //DicomAttributeSQ dicomAttributeSQ = dicomAttributeCollection[DicomTags.ScheduledProcedureStepSequence] as DicomAttributeSQ;
            //DicomSequenceItem dicomSequenceItem = new DicomSequenceItem();
            //dicomAttributeSQ.Values = dicomSequenceItem;

            //dicomSequenceItem[DicomTags.Modality].SetNullValue();
            //dicomSequenceItem[DicomTags.ScheduledProcedureStepId].SetNullValue();
            //dicomSequenceItem[DicomTags.ScheduledProcedureStepDescription].SetNullValue();
            //dicomSequenceItem[DicomTags.ScheduledStationAeTitle].SetNullValue();
            //dicomSequenceItem[DicomTags.ScheduledProcedureStepStartDate].SetNullValue();
            //dicomSequenceItem[DicomTags.ScheduledProcedureStepStartTime].SetNullValue();
            //dicomSequenceItem[DicomTags.ScheduledPerformingPhysiciansName].SetNullValue();
            //dicomSequenceItem[DicomTags.ScheduledProcedureStepLocation].SetNullValue();
            //dicomSequenceItem[DicomTags.ScheduledProcedureStepStatus].SetNullValue();
            //dicomSequenceItem[DicomTags.CommentsOnTheScheduledProcedureStep].SetNullValue();

        }
        #endregion
    }
}
