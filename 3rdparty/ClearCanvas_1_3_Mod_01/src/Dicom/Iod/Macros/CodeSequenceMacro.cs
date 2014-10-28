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
using ClearCanvas.Dicom.Utilities;

namespace ClearCanvas.Dicom.Iod.Macros
{
    /// <summary>
    /// Code Sequence Attributes Macro
    /// </summary>
    /// <remarks>As per Dicom Doc 3, Table 8.8-1 (pg 74)</remarks>
    public class CodeSequenceMacro : SequenceIodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeSequenceMacro"/> class.
        /// </summary>
        public CodeSequenceMacro()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeSequenceMacro"/> class.
        /// </summary>
        /// <param name="dicomSequenceItem">The dicom sequence item.</param>
        public CodeSequenceMacro(DicomSequenceItem dicomSequenceItem)
            : base(dicomSequenceItem)
        {
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the code value.
        /// </summary>
        /// <value>The code value.</value>
        public string CodeValue
        {
            get { return base.DicomAttributeCollection[DicomTags.CodeValue].GetString(0, String.Empty); }
            set { base.DicomAttributeCollection[DicomTags.CodeValue].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the coding scheme designator.
        /// </summary>
        /// <value>The coding scheme designator.</value>
        public string CodingSchemeDesignator
        {
            get { return base.DicomAttributeCollection[DicomTags.CodingSchemeDesignator].GetString(0, String.Empty); }
            set { base.DicomAttributeCollection[DicomTags.CodingSchemeDesignator].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the coding scheme version.
        /// </summary>
        /// <value>The coding scheme version.</value>
        public string CodingSchemeVersion
        {
            get { return base.DicomAttributeCollection[DicomTags.CodingSchemeVersion].GetString(0, String.Empty); }
            set { base.DicomAttributeCollection[DicomTags.CodingSchemeVersion].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the code meaning.
        /// </summary>
        /// <value>The code meaning.</value>
        public string CodeMeaning
        {
            get { return base.DicomAttributeCollection[DicomTags.CodeMeaning].GetString(0, String.Empty); }
            set { base.DicomAttributeCollection[DicomTags.CodeMeaning].SetString(0, value); }
        }

        /// <summary>
        /// Enhanced Encoding Mode: Gets or sets the context identifier.
        /// </summary>
        /// <value>The context identifier.</value>
        public string ContextIdentifier
        {
            get { return base.DicomAttributeCollection[DicomTags.ContextIdentifier].GetString(0, String.Empty); }
            set { base.DicomAttributeCollection[DicomTags.ContextIdentifier].SetString(0, value); }
        }

        /// <summary>
        /// Enhanced Encoding Mode: Gets or sets the mapping resource.
        /// </summary>
        /// <value>The mapping resource.</value>
        public string MappingResource
        {
            get { return base.DicomAttributeCollection[DicomTags.MappingResource].GetString(0, String.Empty); }
            set { base.DicomAttributeCollection[DicomTags.MappingResource].SetString(0, value); }
        }

        /// <summary>
        /// Enhanced Encoding Mode: Gets or sets the context group version.
        /// </summary>
        /// <value>The context group version.</value>
        public DateTime? ContextGroupVersion
        {
	        get { return DateTimeParser.ParseDateAndTime(base.DicomAttributeCollection, DicomTags.ContextGroupVersion, 0, 0);  }

            set { DateTimeParser.SetDateTimeAttributeValues(value, base.DicomAttributeCollection, DicomTags.ContextGroupVersion, 0, 0); }
        }

        /// <summary>
        /// Enhanced Encoding Mode: Gets or sets the context group extension flag.  Y or N
        /// </summary>
        /// <value>The context group extension flag.</value>
        public string ContextGroupExtensionFlag
        {
            get { return base.DicomAttributeCollection[DicomTags.ContextGroupExtensionFlag].GetString(0, String.Empty); }
            set { base.DicomAttributeCollection[DicomTags.ContextGroupExtensionFlag].SetString(0, value); }
        }

        /// <summary>
        /// Enhanced Encoding Mode: Gets or sets the context group local version.
        /// </summary>
        /// <value>The context group local version.</value>
        public DateTime? ContextGroupLocalVersion
        {
            get { return DateTimeParser.ParseDateAndTime(base.DicomAttributeCollection, DicomTags.ContextGroupLocalVersion, 0, 0); }

            set { DateTimeParser.SetDateTimeAttributeValues(value, base.DicomAttributeCollection, DicomTags.ContextGroupLocalVersion, 0, 0); }
        }

        /// <summary>
        /// Enhanced Encoding Mode: Gets or sets the context group extension creator uid.
        /// </summary>
        /// <value>The context group extension creator uid.</value>
        public string ContextGroupExtensionCreatorUid
        {
            get { return base.DicomAttributeCollection[DicomTags.ContextGroupExtensionCreatorUid].GetString(0, String.Empty); }
            set { base.DicomAttributeCollection[DicomTags.ContextGroupExtensionCreatorUid].SetString(0, value); }
        }
        
        #endregion

    }
}
