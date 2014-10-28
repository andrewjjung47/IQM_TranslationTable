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
using System.IO;
using ClearCanvas.Dicom.Iod.Modules;

namespace ClearCanvas.Dicom.Iod.Sequences
{
    /// <summary>
    /// Scheduled Procedure Step Sequence
    /// </summary>
    /// <remarks>As per Dicom Doc 3, Table C.13-5 (pg 871)</remarks>
    public class BasicGrayscaleImageSequenceIod : SequenceIodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicGrayscaleImageSequenceIod"/> class.
        /// </summary>
        public BasicGrayscaleImageSequenceIod()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicGrayscaleImageSequenceIod"/> class.
        /// </summary>
        /// <param name="dicomSequenceItem">The dicom sequence item.</param>
        public BasicGrayscaleImageSequenceIod(DicomSequenceItem dicomSequenceItem)
            : base(dicomSequenceItem)
        {
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the samples per pixel.  Number of samples (planes) in this image.
        /// <para>Possible values for Basic Grayscale Sequence Iod is 1.</para>
        /// </summary>
        /// <value>The samples per pixel.</value>
        /// <remarks>See Part 3, C.7.6.3.1.1 for more info.</remarks>
        public ushort SamplesPerPixel
        {
            get { return base.DicomAttributeCollection[DicomTags.SamplesPerPixel].GetUInt16(0, 0); }
            set { base.DicomAttributeCollection[DicomTags.SamplesPerPixel].SetUInt16(0, value); }
        }

        /// <summary>
        /// Gets or sets the photometric interpretation.
        /// <para>Possible values for Basic Grayscale SequenceIod are MONOCHOME1 or MONOCHROME2.</para>
        /// </summary>
        /// <value>The photometric interpretation.</value>
        public PhotometricInterpretation PhotometricInterpretation
        {
            get { return PhotometricInterpretationHelper.FromString(base.DicomAttributeCollection[DicomTags.PhotometricInterpretation].GetString(0, String.Empty)); }
            set { base.DicomAttributeCollection[DicomTags.PhotometricInterpretation].SetString(0, value == PhotometricInterpretation.Unknown ? null : PhotometricInterpretationHelper.GetString(value)); }
        }

        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>The rows.</value>
        public ushort Rows
        {
            get { return base.DicomAttributeCollection[DicomTags.Rows].GetUInt16(0, 0); }
            set { base.DicomAttributeCollection[DicomTags.Rows].SetUInt16(0, value); }
        }

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>The columns.</value>
        public ushort Columns
        {
            get { return base.DicomAttributeCollection[DicomTags.Columns].GetUInt16(0, 0); }
            set { base.DicomAttributeCollection[DicomTags.Columns].SetUInt16(0, value); }
        }

        /// <summary>
        /// Gets or sets the pixel aspect ratio.
        /// </summary>
        /// <value>The pixel aspect ratio.</value>
        public PixelAspectRatio PixelAspectRatio
        {
            get { return PixelAspectRatio.FromString(base.DicomAttributeCollection[DicomTags.PixelAspectRatio].ToString()); }
			set
			{
				if (value == null || value.IsNull)
					base.DicomAttributeCollection[DicomTags.PixelAspectRatio].SetNullValue();
				else 
					base.DicomAttributeCollection[DicomTags.PixelAspectRatio].SetStringValue(value.ToString());
			}
        }

        /// <summary>
        /// Gets or sets the bits allocated.
        /// <para>Possible values for Bits Allocated are 8 (if Bits Stored = 8) or 16 (if Bits Stored = 12).</para>
        /// </summary>
        /// <value>The bits allocated.</value>
        public ushort BitsAllocated
        {
            get { return base.DicomAttributeCollection[DicomTags.BitsAllocated].GetUInt16(0, 0); }
            set { base.DicomAttributeCollection[DicomTags.BitsAllocated].SetUInt16(0, value); }
        }

        /// <summary>
        /// Gets or sets the bits stored.
        /// <para>Possible values for Bits Stored are 8 or 12.</para>
        /// </summary>
        /// <value>The bits stored.</value>
        public ushort BitsStored
        {
            get { return base.DicomAttributeCollection[DicomTags.BitsStored].GetUInt16(0, 0); }
            set { base.DicomAttributeCollection[DicomTags.BitsStored].SetUInt16(0, value); }
        }

        /// <summary>
        /// Gets or sets the high bit.
        /// <para>Possible values for High Bit are 7 (if Bits Stored = 8) or 11 (if Bits Stored = 12).</para>
        /// </summary>
        /// <value>The high bit.</value>
        public ushort HighBit
        {
            get { return base.DicomAttributeCollection[DicomTags.HighBit].GetUInt16(0, 0); }
            set { base.DicomAttributeCollection[DicomTags.HighBit].SetUInt16(0, value); }
        }

        /// <summary>
        /// Gets or sets the pixel representation.Data representation of the pixel samples. 
        /// Each sample shall have the same pixel representation. 
        /// <para>Possible values for Basic Grayscale Sequence Iod is 0 (000H).</para>
        /// </summary>
        /// <value>The pixel representation.</value>
        public ushort PixelRepresentation
        {
            get { return base.DicomAttributeCollection[DicomTags.PixelRepresentation].GetUInt16(0, 0); }
            set { base.DicomAttributeCollection[DicomTags.PixelRepresentation].SetUInt16(0, value); }
        }

        /// <summary>
        /// Gets or sets the pixel data.
        /// </summary>
        /// <value>The pixel data.</value>
        public byte[] PixelData
        {
            get
            {
                if (base.DicomAttributeCollection.Contains(DicomTags.PixelData) && !base.DicomAttributeCollection[DicomTags.PixelData].IsEmpty)
                    return (byte[])base.DicomAttributeCollection[DicomTags.PixelData].Values;
                else
                    return null;
            }
            set { base.DicomAttributeCollection[DicomTags.PixelData].Values = value; }
        }

        #endregion

        /// <summary>
        /// Adds the dicom file values.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <exception cref="FileNotFoundException"/>
        public void AddDicomFileValues(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            DicomFile dicomFile = new DicomFile(filePath);
            dicomFile.Load();
            AddDicomFileValues(dicomFile);
        }

        /// <summary>
        /// Adds the dicom file values.
        /// </summary>
        /// <param name="dicomFile">The dicom file.</param>
        public void AddDicomFileValues(DicomFile dicomFile)
        {
            try
            {
                ImagePixelMacroIod imagePixelMacroIod = new ImagePixelMacroIod(dicomFile.DataSet);

                this.SamplesPerPixel = 1; // only possible value for grayscale as per dicom standard

                if (imagePixelMacroIod.PhotometricInterpretation != PhotometricInterpretation.Monochrome1 || imagePixelMacroIod.PhotometricInterpretation != PhotometricInterpretation.Monochrome2)
                {
                    // Dicom File doesn't have Monochrome1 or MonoChrome2 - what to do?  throw exception or pick one?  let's try picking one...
                    this.PhotometricInterpretation = PhotometricInterpretation.Monochrome1;
                }
                else
                {
                    this.PhotometricInterpretation = imagePixelMacroIod.PhotometricInterpretation;
                }

                this.Rows = imagePixelMacroIod.Rows;
                this.Columns = imagePixelMacroIod.Columns;

                PixelAspectRatio ratio = imagePixelMacroIod.PixelAspectRatio;
				if (ratio.IsNull)
					this.PixelAspectRatio = new PixelAspectRatio(1, 1);

                //TODO: figure out when to make it 12... possible values are only 8 or 12...
                this.BitsStored = 8;

                // Bits allocated is 8 if BitsStored = 8, 12 if BitsStored = 12...
                this.BitsAllocated = (this.BitsStored == (ushort)8) ? (ushort)8 : (ushort)16;

                // High bit is 7 if Bits Stored = 8, 11 if Bits Stored = 12..
                this.HighBit = (this.BitsStored == (ushort)8) ? (ushort)7 : (ushort)11;

                // Always 0 as per DICOM standard
                this.PixelRepresentation = 0;

                // Sets the pixel data from the Dicom File
                this.PixelData = imagePixelMacroIod.PixelData;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }


}
