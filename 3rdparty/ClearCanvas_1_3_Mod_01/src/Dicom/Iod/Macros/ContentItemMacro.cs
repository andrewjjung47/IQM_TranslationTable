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

namespace ClearCanvas.Dicom.Iod.Macros
{
    /// <summary>
    /// Code Sequence Attributes Macro
    /// </summary>
    /// <remarks>As per Dicom Doc 3, Table 10-2 (pg 76)</remarks>
    public class ContentItemMacro : SequenceIodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentItemMacro"/> class.
        /// </summary>
        public ContentItemMacro()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentItemMacro"/> class.
        /// </summary>
        /// <param name="dicomSequenceItem">The dicom sequence item.</param>
        public ContentItemMacro(DicomSequenceItem dicomSequenceItem)
            : base(dicomSequenceItem)
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the type of the value.
        /// </summary>
        /// <value>The type of the value.</value>
        public ContentItemValueType ValueType
        {
            get { return IodBase.ParseEnum<ContentItemValueType>(base.DicomAttributeCollection[DicomTags.ValueType].GetString(0, String.Empty), ContentItemValueType.None); }
            set { IodBase.SetAttributeFromEnum(base.DicomAttributeCollection[DicomTags.ValueType], value); }
        }

        public SequenceIodList<CodeSequenceMacro> ConceptNameCodeSequenceList
        {
            get
            {
                return new SequenceIodList<CodeSequenceMacro>(base.DicomAttributeCollection[DicomTags.ConceptNameCodeSequence] as DicomAttributeSQ);
            }
        }

        /// <summary>
        /// Datetime value for this name-value Item. Required if Value Type (0040,A040) is DATETIME.
        /// </summary>
        /// <value>The datetime.</value>
        public DateTime? Datetime
        {
        	get { return base.DicomAttributeCollection[DicomTags.Datetime].GetDateTime(0); }

            set 
            { 
                if (value.HasValue)
                    base.DicomAttributeCollection[DicomTags.Datetime].SetDateTime(0, value.Value); 
                else
                    base.DicomAttributeCollection[DicomTags.Datetime].SetNullValue();
            }
        }

        /// <summary>
        /// Date value for this name-value Item. Required if Value Type (0040,A040) is DATE.
        /// </summary>
        /// <value>The date.</value>
        public DateTime? Date
        {
            get { return base.DicomAttributeCollection[DicomTags.Date].GetDateTime(0); }

            set { base.DicomAttributeCollection[DicomTags.Date].SetDateTime(0, value); }
        }

        /// <summary>
        /// Time value for this name-value Item.  Required if Value Type (0040,A040) is TIME.
        /// </summary>
        /// <value>The time.</value>
        public DateTime? Time
        {
            get { return base.DicomAttributeCollection[DicomTags.Time].GetDateTime(0); }

            set { base.DicomAttributeCollection[DicomTags.Time].SetDateTime(0, value); }
        }

        /// <summary>
        /// Person name value for this name-value Item.  Required if Value Type (0040,A040) is PNAME.
        /// </summary>
        /// <value>The name of the person.</value>
        public PersonName PersonName
        {
            get { return new PersonName(base.DicomAttributeCollection[DicomTags.PersonName].GetString(0, String.Empty)); }
            set { base.DicomAttributeCollection[DicomTags.PersonName].SetString(0, value.ToString()); }
        }

        /// <summary>
        /// UID value for this name-value Item.  Required if Value Type (0040,A040) is UIDREF.
        /// </summary>
        /// <value>The uid.</value>
        public string Uid
        {
            get { return base.DicomAttributeCollection[DicomTags.Uid].GetString(0, String.Empty); }
            set { base.DicomAttributeCollection[DicomTags.Uid].SetString(0, value); }
        }

        /// <summary>
        /// Text value for this name-value Item.  Required if Value Type (0040,A040) is TEXT.
        /// </summary>
        /// <value>The text value.</value>
        public string TextValue
        {
            get { return base.DicomAttributeCollection[DicomTags.TextValue].GetString(0, String.Empty); }
            set { base.DicomAttributeCollection[DicomTags.TextValue].SetString(0, value); }
        }

        /// <summary>
        /// Coded concept value of this name-value Item.  Required if Value Type (0040,A040) is CODE.
        /// </summary>
        /// <value>The concept code sequence list.</value>
        public SequenceIodList<CodeSequenceMacro> ConceptCodeSequenceList
        {
            get
            {
                return new SequenceIodList<CodeSequenceMacro>(base.DicomAttributeCollection[DicomTags.ConceptCodeSequence] as DicomAttributeSQ);
            }
        }

        /// <summary>
        /// Numeric value for this name-value Item. Required if Value Type (0040,A040) is NUMERIC.
        /// </summary>
        /// <value>The numeric value.</value>
        public float NumericValue
        {
            get { return base.DicomAttributeCollection[DicomTags.NumericValue].GetFloat32(0, 0.0F); }
            set { base.DicomAttributeCollection[DicomTags.NumericValue].SetFloat32(0, value); }
        }

        /// <summary>
        /// Units of measurement for a numeric value in this namevalue Item.  Required if Value Type (0040,A040) is NUMERIC.
        /// </summary>
        /// <value>The measurement units code sequence list.</value>
        public SequenceIodList<CodeSequenceMacro> MeasurementUnitsCodeSequenceList
        {
            get
            {
                return new SequenceIodList<CodeSequenceMacro>(base.DicomAttributeCollection[DicomTags.MeasurementUnitsCodeSequence] as DicomAttributeSQ);
            }
        }
        
        #endregion

    }

    #region ContentItemValueType Enum
    /// <summary>
    /// 
    /// </summary>
    public enum ContentItemValueType
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 
        /// </summary>
        DateTime,
        /// <summary>
        /// 
        /// </summary>
        Date,
        /// <summary>
        /// 
        /// </summary>
        Time,
        /// <summary>
        /// 
        /// </summary>
        PName,
        /// <summary>
        /// 
        /// </summary>
        UidRef,
        /// <summary>
        /// 
        /// </summary>
        Text,
        /// <summary>
        /// 
        /// </summary>
        Code,
        /// <summary>
        /// 
        /// </summary>
        Numeric
    }
    #endregion
    
}
