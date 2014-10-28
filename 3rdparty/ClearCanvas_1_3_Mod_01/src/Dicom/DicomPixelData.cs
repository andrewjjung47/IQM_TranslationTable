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

// mDCM: A C# DICOM library
//
// Copyright (c) 2006-2008  Colby Dillion
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
using ClearCanvas.Common;
using ClearCanvas.Dicom.Codec;
using ClearCanvas.Dicom.IO;

namespace ClearCanvas.Dicom
{
    /// <summary>
    /// Base class representing pixel data.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class is used to represent pixel data within the DICOM library.  It contains
    /// all the basic tags required to work with pixel data.
    /// </para>
    /// </remarks>
    public abstract class DicomPixelData
    {
        #region Private Members

        private int _frames = 1;
        private ushort _width;
        private ushort _height;
        private ushort _highBit;
        private ushort _bitsStored;
        private ushort _bitsAllocated;
        private ushort _samplesPerPixel = 1;
        private ushort _pixelRepresentation;
        private ushort _planarConfiguration;
        private string _photometricInterpretation;
        private TransferSyntax _transferSyntax = TransferSyntax.ExplicitVrLittleEndian;
        private string _lossyImageCompression = "";
        private string _lossyImageCompressionMethod = "";
        private float _compressionRatio;
        private string _derivationDescription = "";

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Creates an instance of <see cref="DicomPixelData"/> from specified image path
        /// </summary>
        /// <param name="path"></param>
        /// <returns>
        /// </returns>
        public static DicomPixelData CreateFrom(string path)
        {
            DicomFile file = new DicomFile(path);
            file.Load(DicomReadOptions.StorePixelDataReferences);
            return CreateFrom(file);
        }


        /// <summary>
        /// Creates an instance of <see cref="DicomPixelData"/> from specified dicom message
        /// </summary>
        /// <param name="path"></param>
        /// <returns>
        /// </returns>
        public static DicomPixelData CreateFrom(DicomMessageBase message)
        {
            if (message.TransferSyntax.LosslessCompressed || message.TransferSyntax.LossyCompressed)
                return new DicomCompressedPixelData(message);
            else
                return new DicomUncompressedPixelData(message);
        }
        #endregion



        #region Constructors

        public DicomPixelData(DicomMessageBase message)
        {
            _transferSyntax = message.TransferSyntax;

            message.DataSet.LoadDicomFields(this);
            if (message.DataSet.Contains(DicomTags.NumberOfFrames))
                NumberOfFrames = message.DataSet[DicomTags.NumberOfFrames].GetInt32(0, 1);
            if (message.DataSet.Contains(DicomTags.PlanarConfiguration))
                PlanarConfiguration = message.DataSet[DicomTags.PlanarConfiguration].GetUInt16(0, 1);
            if (message.DataSet.Contains(DicomTags.LossyImageCompression))
                LossyImageCompression = message.DataSet[DicomTags.LossyImageCompression].GetString(0, "");
            if (message.DataSet.Contains(DicomTags.LossyImageCompressionRatio))
                LossyImageCompressionRatio = message.DataSet[DicomTags.LossyImageCompressionRatio].GetFloat32(0, 1.0f);
            if (message.DataSet.Contains(DicomTags.LossyImageCompressionMethod))
                LossyImageCompressionMethod = message.DataSet[DicomTags.LossyImageCompressionMethod].GetString(0, "");
            if (message.DataSet.Contains(DicomTags.DerivationDescription))
                DerivationDescription = message.DataSet[DicomTags.DerivationDescription].GetString(0, "");
        }


        public DicomPixelData(DicomAttributeCollection collection)
        {
            collection.LoadDicomFields(this);
            if (collection.Contains(DicomTags.NumberOfFrames))
                NumberOfFrames = collection[DicomTags.NumberOfFrames].GetInt32(0, 1);
            if (collection.Contains(DicomTags.PlanarConfiguration))
                PlanarConfiguration = collection[DicomTags.PlanarConfiguration].GetUInt16(0, 1);
            if (collection.Contains(DicomTags.LossyImageCompression))
                LossyImageCompression = collection[DicomTags.LossyImageCompression].GetString(0, "");
            if (collection.Contains(DicomTags.LossyImageCompressionRatio))
                LossyImageCompressionRatio = collection[DicomTags.LossyImageCompressionRatio].GetFloat32(0, 1.0f);
            if (collection.Contains(DicomTags.LossyImageCompressionMethod))
                LossyImageCompressionMethod = collection[DicomTags.LossyImageCompressionMethod].GetString(0, "");
            if (collection.Contains(DicomTags.DerivationDescription))
                DerivationDescription = collection[DicomTags.DerivationDescription].GetString(0, "");
        }

        internal DicomPixelData(DicomPixelData attrib)
        {
            this.NumberOfFrames = attrib.NumberOfFrames;
            this.ImageWidth = attrib.ImageWidth;
            this.ImageHeight = attrib.ImageHeight;
            this.HighBit = attrib.HighBit;
            this.BitsStored = attrib.BitsStored;
            this.BitsAllocated = attrib.BitsAllocated;
            this.SamplesPerPixel = attrib.SamplesPerPixel;
            this.PixelRepresentation = attrib.PixelRepresentation;
            this.PlanarConfiguration = attrib.PlanarConfiguration;
            this.PhotometricInterpretation = attrib.PhotometricInterpretation;
            this.LossyImageCompression = attrib.LossyImageCompression;
            this.DerivationDescription = attrib.DerivationDescription;
            this.LossyImageCompressionRatio = attrib.LossyImageCompressionRatio;
            this.LossyImageCompressionMethod = attrib.LossyImageCompressionMethod;
        }

        #endregion

        /// <summary>
        /// Get a specific frame's data in uncompressed format.
        /// </summary>
        /// <param name="frame">The zero offset frame to get.</param>
        /// <returns>A byte array containing the pixel data.</returns>
        public byte[] GetFrame(int frame)
        {
            string photometricInterpretation;

            return GetFrame(frame, out photometricInterpretation);
        }

        /// <summary>
        /// Get a specific uncompressed frame with the photometric interpretation.
        /// </summary>
        /// <param name="frame">A zero offset frame number.</param>
        /// <param name="photometricInterpretation">The photometric interpretation of the pixel data.</param>
        /// <returns>A byte array containing the uncompressed pixel data.</returns>
        public abstract byte[] GetFrame(int frame, out string photometricInterpretation);

        /// <summary>
        /// Update the tags in an attribute collection.
        /// </summary>
        /// <param name="dataset">The attribute collection to update.</param>
        public abstract void UpdateAttributeCollection(DicomAttributeCollection dataset);

        /// <summary>
        /// Updat ethe pixel data related tags in a DICOM message.
        /// </summary>
        /// <param name="message"></param>
        public abstract void UpdateMessage(DicomMessageBase message);

        #region Public Properties

        /// <summary>
        /// The number of frames in the pixel data.
        /// </summary>
        public int NumberOfFrames
        {
            get { return _frames; }
            set { _frames = value; }
        }

        [DicomField(DicomTags.Columns, DefaultValue = DicomFieldDefault.Default)]
        public ushort ImageWidth
        {
            get { return _width; }
            set { _width = value; }
        }

        [DicomField(DicomTags.Rows, DefaultValue = DicomFieldDefault.Default)]
        public ushort ImageHeight
        {
            get { return _height; }
            set { _height = value; }
        }

        [DicomField(DicomTags.HighBit, DefaultValue = DicomFieldDefault.Default)]
        public ushort HighBit
        {
            get { return _highBit; }
            set { _highBit = value; }
        }

        [DicomField(DicomTags.BitsStored, DefaultValue = DicomFieldDefault.Default)]
        public ushort BitsStored
        {
            get { return _bitsStored; }
            set { _bitsStored = value; }
        }

        [DicomField(DicomTags.BitsAllocated, DefaultValue = DicomFieldDefault.Default)]
        public ushort BitsAllocated
        {
            get { return _bitsAllocated; }
            set { _bitsAllocated = value; }
        }

        public int BytesAllocated
        {
            get
            {
                int bytes = BitsAllocated/8;
                if ((BitsAllocated%8) > 0)
                    bytes++;
                return bytes;
            }
        }

        [DicomField(DicomTags.SamplesPerPixel, DefaultValue = DicomFieldDefault.Default)]
        public ushort SamplesPerPixel
        {
            get { return _samplesPerPixel; }
            set { _samplesPerPixel = value; }
        }

        [DicomField(DicomTags.PixelRepresentation, DefaultValue = DicomFieldDefault.Default)]
        public ushort PixelRepresentation
        {
            get { return _pixelRepresentation; }
            set { _pixelRepresentation = value; }
        }

        public bool IsSigned
        {
            get { return _pixelRepresentation != 0; }
        }

        // Not always in images, so don't make it an attribute and manually update it if needed
        public ushort PlanarConfiguration
        {
            get { return _planarConfiguration; }
            set { _planarConfiguration = value; }
        }

        public bool IsPlanar
        {
            get { return _planarConfiguration != 0; }
        }

        /// <summary>
        /// Photometric Interpretation (0028,0004)
        /// </summary>
        [DicomField(DicomTags.PhotometricInterpretation, DefaultValue = DicomFieldDefault.Null)]
        public string PhotometricInterpretation
        {
            get { return _photometricInterpretation; }
            set { _photometricInterpretation = value; }
        }

        /// <summary>
        /// The frame size of an uncompressed frame.
        /// </summary>
        public int UncompressedFrameSize
        {
            get
            {
                // ybr full 422 only stores 2/3 of the pixels
                if (_photometricInterpretation!=null && _photometricInterpretation.Equals("YBR_FULL_422"))
                    return ImageWidth*ImageHeight*BytesAllocated*2;

                return ImageWidth*ImageHeight*BytesAllocated*SamplesPerPixel;
            }
        }

        public TransferSyntax TransferSyntax
        {
            get { return _transferSyntax; }
            set { _transferSyntax = value; }
        }

        // Not always in an image, do a manual update.        
        public string LossyImageCompression
        {
            get { return _lossyImageCompression; }
            set { _lossyImageCompression = value; }
        }

        public float LossyImageCompressionRatio
        {
            get { return _compressionRatio; }
            set { _compressionRatio = value; }
        }

        public string DerivationDescription
        {
            get { return _derivationDescription; }
            set { _derivationDescription = value; }
        }

        public string LossyImageCompressionMethod
        {
            get { return _lossyImageCompressionMethod; }
            set { _lossyImageCompressionMethod = value; }
        }

        #endregion
    }


    /// <summary>
    /// Class representing uncompressed pixel data.
    /// </summary>
    public class DicomUncompressedPixelData : DicomPixelData
    {
        #region Private Members

        private readonly DicomAttribute _pd;
        private MemoryStream _ms;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor from a <see cref="DicomMessageBase"/> instance.
        /// </summary>
        /// <param name="message"></param>
        public DicomUncompressedPixelData(DicomMessageBase message)
            : base(message)
        {
            _pd = message.DataSet[DicomTags.PixelData];
        }

        /// <summary>
        /// Constructor from an <see cref="DicomAttributeCollection"/> instance.
        /// </summary>
        /// <param name="collection"></param>
        public DicomUncompressedPixelData(DicomAttributeCollection collection)
            : base(collection)
        {
            _pd = collection[DicomTags.PixelData];
        }

        /// <summary>
        /// Contructor from a <see cref="DicomCompressedPixelData"/> instance.
        /// </summary>
        /// <param name="pd"></param>
        public DicomUncompressedPixelData(DicomCompressedPixelData pd) : base(pd)
        {
            if (this.BitsAllocated > 8)
                _pd = new DicomAttributeOW(DicomTags.PixelData);
            else
            {
                DicomTag pdTag = DicomTagDictionary.GetDicomTag(DicomTags.PixelData);

                _pd =
                    new DicomAttributeOB(
                        new DicomTag(DicomTags.PixelData, pdTag.Name, pdTag.VariableName, DicomVr.OBvr, pdTag.MultiVR,
                                     pdTag.VMLow, pdTag.VMHigh, pdTag.Retired));
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Internal optimization for getting pixel data for the GetFrame method.
        /// </summary>
        /// <returns></returns>
        internal byte[] GetData()
        {
            if (_ms == null) return null;
            return _ms.ToArray();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Update a <see cref="DicomAttributeCollection"/> with the pixel data contained
        /// within this object and also update pixel data related tags.
        /// </summary>
        /// <remarks>
        /// This method will replace the pixel data attribute in <paramref name="dataset"/>
        /// and update other pixel data related tags within the collection.
        /// </remarks>
        /// <param name="dataset">The collection to update.</param>
        public override void UpdateAttributeCollection(DicomAttributeCollection dataset)
        {
            dataset.SaveDicomFields(this);
            if (dataset.Contains(DicomTags.NumberOfFrames) || this.NumberOfFrames > 1)
                dataset[DicomTags.NumberOfFrames].SetInt32(0, NumberOfFrames);
            if (dataset.Contains(DicomTags.PlanarConfiguration))
                dataset[DicomTags.PlanarConfiguration].SetInt32(0, PlanarConfiguration);
            if (dataset.Contains(DicomTags.LossyImageCompressionRatio))
                dataset[DicomTags.LossyImageCompressionRatio].SetFloat32(0, this.LossyImageCompressionRatio);

            dataset[DicomTags.PixelData] = _pd;

            if (_ms != null)
            {
                // Add a padding character
                if ((_ms.Length & 1) == 1)
                    _ms.WriteByte(0);
                _pd.Values = _ms.ToArray();
                _ms.Close();
                _ms.Dispose();
                _ms = null;
            }
        }

        /// <summary>
        /// Update a <see cref="DicomMessageBase"/> with the pixel data contained
        /// within this object and also update pixel data related tags.
        /// </summary>
        /// <param name="message"></param>
        public override void UpdateMessage(DicomMessageBase message)
        {
            UpdateAttributeCollection(message.DataSet);
            DicomFile file = message as DicomFile;
            if (file != null)
                file.TransferSyntax = TransferSyntax;
        }

        /// <summary>
        /// Append a frame of data to the pixel data.
        /// </summary>
        /// <param name="frameData">The data to append.</param>
        public void AppendFrame(byte[] frameData)
        {
            if (_ms == null)
            {
                if (_pd.Count == 0)
                    _ms = new MemoryStream(frameData.Length);
                else
                    _ms = new MemoryStream((byte[]) _pd.Values);
            }

            _ms.Seek(0, SeekOrigin.End);
            _ms.Write(frameData, 0, frameData.Length);
        }

        /// <summary>
        /// Get a specific uncompressed frame.
        /// </summary>
        /// <param name="frame">The frame to retrieve</param>
        /// <returns>A byte array containing the frame data.</returns>
        /// <param name="photometricInterpretation">The photometric interpretation of the pixel data.</param>
        public override byte[] GetFrame(int frame, out string photometricInterpretation)
    
        {
            if (frame >= NumberOfFrames)
                throw new ArgumentOutOfRangeException("frame");

            photometricInterpretation = PhotometricInterpretation;

            if (_ms != null)
            {
                _ms.Seek(0, SeekOrigin.Begin);
                byte[] pixels = new byte[UncompressedFrameSize];

                _ms.Read(pixels, frame*UncompressedFrameSize, UncompressedFrameSize);

                return pixels;
            }

            DicomAttributeOB obAttrib = _pd as DicomAttributeOB;
            if (obAttrib != null)
            {
                if (obAttrib._reference != null)
                {
                    ByteBuffer bb;
					using (FileStream fs = File.OpenRead(obAttrib._reference.Filename))
                    {
                        long offset = obAttrib._reference.Offset + frame*UncompressedFrameSize;
                        fs.Seek(offset, SeekOrigin.Begin);

                        bb = new ByteBuffer();
                        bb.CopyFrom(fs, UncompressedFrameSize);
                        bb.Endian = obAttrib._reference.Endian;
						fs.Close();
                    }

                    return bb.ToBytes();
                }

                byte[] pixels = new byte[UncompressedFrameSize];

                Array.Copy((byte[])obAttrib.Values, frame * UncompressedFrameSize, pixels, 0, UncompressedFrameSize);

                return pixels;
            }

            DicomAttributeOW owAttrib = _pd as DicomAttributeOW;
            if (owAttrib != null)
            {
                if (owAttrib._reference != null)
                {
                    ByteBuffer bb;
                    if ((UncompressedFrameSize%2 == 1)
                        && (owAttrib._reference.Endian != ByteBuffer.LocalMachineEndian))
                    {
                        // When frames are odd size, and we need to byte swap, we need to get an extra byte.
                        // For odd number frames, we get a byte before the frame
                        // and for even frames we get a byte after the frame.

						using (FileStream fs = File.OpenRead(owAttrib._reference.Filename))
                        {
                            if (frame%2 == 1)
                                fs.Seek((owAttrib._reference.Offset + frame*UncompressedFrameSize) - 1, SeekOrigin.Begin);
                            else
                                fs.Seek(owAttrib._reference.Offset + frame*UncompressedFrameSize, SeekOrigin.Begin);

                            bb = new ByteBuffer();
                            bb.CopyFrom(fs, UncompressedFrameSize + 1);
                            bb.Endian = owAttrib._reference.Endian;
							fs.Close();
                        }

                        bb.Swap(owAttrib._reference.Vr.UnitSize);
                        bb.Endian = ByteBuffer.LocalMachineEndian;

                        if (frame%2 == 1)
                            return bb.ToBytes(1, UncompressedFrameSize);
                        return bb.ToBytes(0, UncompressedFrameSize);
                    }

					using (FileStream fs = File.OpenRead(owAttrib._reference.Filename))
                    {
                        fs.Seek(owAttrib._reference.Offset + frame*UncompressedFrameSize, SeekOrigin.Begin);

                        bb = new ByteBuffer();
                        bb.CopyFrom(fs, UncompressedFrameSize);
                        bb.Endian = owAttrib._reference.Endian;
						fs.Close();
                    }

                    if (owAttrib._reference.Endian != ByteBuffer.LocalMachineEndian)
                    {
                        bb.Swap(owAttrib._reference.Vr.UnitSize);
                        bb.Endian = ByteBuffer.LocalMachineEndian;
                    }

                    return bb.ToBytes();
                }
                byte[] pixels = new byte[UncompressedFrameSize];

                Array.Copy((byte[])owAttrib.Values, frame * UncompressedFrameSize, pixels, 0, UncompressedFrameSize);

                return pixels;
            }

            return null;
        }

        #endregion

        #region Static Methods
        public static void TogglePlanarConfiguration(byte[] pixelData, int numValues, int bitsAllocated,
        int samplesPerPixel, int oldPlanerConfiguration)
        {
            int bytesAllocated = bitsAllocated / 8;
            int numPixels = numValues / samplesPerPixel;
            if (bytesAllocated == 1)
            {
                byte[] buffer = new byte[pixelData.Length];
                if (oldPlanerConfiguration == 1)
                {
                    for (int n = 0; n < numPixels; n++)
                    {
                        for (int s = 0; s < samplesPerPixel; s++)
                        {
                            buffer[n * samplesPerPixel + s] = pixelData[n + numPixels * s];
                        }
                    }
                }
                else
                {
                    for (int n = 0; n < numPixels; n++)
                    {
                        for (int s = 0; s < samplesPerPixel; s++)
                        {
                            buffer[n + numPixels * s] = pixelData[n * samplesPerPixel + s];
                        }
                    }
                }
                Array.Copy(buffer, 0, pixelData, 0, numValues);
            }
            else if (bytesAllocated == 2)
            {
                throw new DicomCodecException(String.Format("BitsAllocated={0} is not supported!", bitsAllocated));
            }
            else
                throw new DicomCodecException(String.Format("BitsAllocated={0} is not supported!", bitsAllocated));
        }
        #endregion
    }


    /// <summary>
    /// Class representing compressed pixel data.
    /// </summary>
    public class DicomCompressedPixelData : DicomPixelData
    {
        #region Protected Members

        protected List<uint> _table;
        protected List<DicomFragment> _fragments = new List<DicomFragment>();
        private readonly DicomFragmentSequence _sq;

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor from a <see cref="DicomMessageBase"/> instance.
        /// </summary>
        /// <param name="msg">The message to initialize the object from.</param>
        public DicomCompressedPixelData(DicomMessageBase msg)
            : base(msg)
        {
            _sq = (DicomFragmentSequence) msg.DataSet[DicomTags.PixelData];
        }

        /// <summary>
        /// Constructor from a <see cref="DicomAttributeCollection"/> instance.
        /// </summary>
        /// <param name="collection">The collection to initialize the object from.</param>
        public DicomCompressedPixelData(DicomAttributeCollection collection) : base(collection)
        {
            _sq = (DicomFragmentSequence) collection[DicomTags.PixelData];
        }

		public DicomCompressedPixelData(DicomMessageBase msg, byte[] frameData) : base(msg)
		{
			_sq = new DicomFragmentSequence(DicomTags.PixelData);
			AddFrameFragment(frameData);
			//ByteBuffer buffer = new ByteBuffer(frameData);
			//DicomFragment fragment = new DicomFragment(buffer);
			//_sq.AddFragment(fragment);
			NumberOfFrames = 1;
		}

        /// <summary>
        /// Constructor from a <see cref="DicomUncompressedPixeldata"/> instance.
        /// </summary>
        /// <param name="pd">The uuncompressed pixel data attribute to initialize the object from.</param>
        public DicomCompressedPixelData(DicomUncompressedPixelData pd) : base(pd)
        {
            _sq = new DicomFragmentSequence(DicomTags.PixelData);
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Update an <see cref="DicomAttributeCollection"/> with pixel data related tags.
        /// </summary>
        /// <param name="dataset">The collection to update.</param>
        public override void UpdateAttributeCollection(DicomAttributeCollection dataset)
        {
            if (dataset.Contains(DicomTags.NumberOfFrames) || this.NumberOfFrames > 1)
                dataset[DicomTags.NumberOfFrames].SetInt32(0, NumberOfFrames);
            if (dataset.Contains(DicomTags.PlanarConfiguration))
                dataset[DicomTags.PlanarConfiguration].SetInt32(0, PlanarConfiguration);
            if (dataset.Contains(DicomTags.LossyImageCompression) || LossyImageCompression.Length > 0)
                dataset[DicomTags.LossyImageCompression].SetString(0, LossyImageCompression);
            if (dataset.Contains(DicomTags.LossyImageCompressionRatio) || (LossyImageCompressionRatio != 1.0f && LossyImageCompressionRatio != 0.0f)) 
                dataset[DicomTags.LossyImageCompressionRatio].SetFloat32(0, LossyImageCompressionRatio);
            if (dataset.Contains(DicomTags.LossyImageCompressionMethod) || LossyImageCompressionMethod.Length > 0)
                dataset[DicomTags.LossyImageCompressionMethod].SetString(0, LossyImageCompressionMethod);
            if (dataset.Contains(DicomTags.DerivationDescription) || DerivationDescription.Length > 0)
				dataset[DicomTags.DerivationDescription].SetStringValue(DerivationDescription);

            dataset.SaveDicomFields(this);
            dataset[DicomTags.PixelData] = _sq;
        }
        /// <summary>
        /// Update a <see cref="DicomMessageBase"/> with pixel data related tags.
        /// </summary>
        /// <param name="message">The message to update.</param>
        public override void UpdateMessage(DicomMessageBase message)
        {
            UpdateAttributeCollection(message.DataSet);
            DicomFile file = message as DicomFile;
            if (file != null)
                file.TransferSyntax = TransferSyntax;
        }

        /// <summary>
        /// Get a specific frame's data in uncompressed format.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If a DICOM file is loaded with the <see cref="DicomReadOptions.StorePixelDataReferences"/>
        /// option set, this method will only load the specific frame's data from the source file to
        /// do the decompress, thus reducing memory usage to only the frame being decompressed.
        /// </para>
        /// </remarks>
        /// <param name="frame">A zero offset frame to request.</param>
        /// <param name="photometricInterpretation">The photometric interpretation of the output data</param>
        /// <returns>A byte array containing the frame.</returns>
        public override byte[] GetFrame(int frame, out string photometricInterpretation)
        {
            DicomUncompressedPixelData pd = new DicomUncompressedPixelData(this);

            IDicomCodec codec = DicomCodecRegistry.GetCodec(TransferSyntax);
            if (codec == null)
            {
                Platform.Log(LogLevel.Error, "Unable to get registered codec for {0}", TransferSyntax);

                throw new DicomCodecException("No registered codec for: " + TransferSyntax.Name);
            }

            DicomCodecParameters parameters = DicomCodecRegistry.GetCodecParameters(TransferSyntax, null);

            codec.DecodeFrame(frame, this, pd, parameters);

            pd.TransferSyntax = TransferSyntax.ExplicitVrLittleEndian;

            photometricInterpretation = pd.PhotometricInterpretation;

            return pd.GetData();
        }

        /// <summary>
        /// Append a compressed pixel data fragment's worth of data.  It is assumed an entire frame is
        /// contained within <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The fragment data.</param>
        public void AddFrameFragment(byte[] data)
        {
            DicomFragmentSequence sequence = _sq;
			if ((data.Length % 2) == 1)
				throw new DicomCodecException("Fragment being appended is incorrectly an odd length: " + data.Length);

            uint offset = 0;
            foreach (DicomFragment fragment in sequence.Fragments)
            {
                offset += (8 + fragment.Length);
            }
            sequence.OffsetTable.Add(offset);

            ByteBuffer buffer = new ByteBuffer();
            buffer.Append(data, 0, data.Length);
            sequence.Fragments.Add(new DicomFragment(buffer));
        }

        public uint GetCompressedFrameSize(int frame)
        {
            List<DicomFragment> list = GetFrameFragments(frame);
            uint length = 0;
            foreach (DicomFragment frag in list)
                length += frag.Length;
            return length;
        }

        public byte[] GetFrameFragmentData(int frame)
        {
            List < DicomFragment > list = GetFrameFragments(frame);
            uint length = 0;
            foreach (DicomFragment frag in list)
                length += frag.Length;

            byte[] data = new byte[length];
            uint offset = 0;
            foreach (DicomFragment frag in list)
            {
                Array.Copy(frag.GetByteArray(), 0, data, (int)offset, (int)frag.Length);
                offset += frag.Length;
            }
            return data;
        }

        /// <summary>
        /// Get the pixel data fragments for a frame.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Note that if an offset table was not included within the pixel data, this method will
        /// attempt to guess which fragments are contained within a frame.
        /// </para>
        /// </remarks>
        /// <param name="frame">The zero offset frame to get the fragments for.</param>
        /// <returns>A list of fragments associated with the frame.</returns>
        public List<DicomFragment> GetFrameFragments(int frame)
        {
            if (frame < 0 || frame >= NumberOfFrames)
                throw new IndexOutOfRangeException("Requested frame out of range!");

            List<DicomFragment> fragments = new List<DicomFragment>();
            DicomFragmentSequence sequence = _sq;
            int fragmentCount = sequence.Fragments.Count;

            if (NumberOfFrames == 1)
            {
                foreach (DicomFragment frag in sequence.Fragments)
                    fragments.Add(frag);
                return fragments;
            }

            if (fragmentCount == NumberOfFrames)
            {
                fragments.Add(sequence.Fragments[frame]);
                return fragments;
            }

            if (sequence.HasOffsetTable && sequence.OffsetTable.Count == NumberOfFrames)
            {
                uint offset = sequence.OffsetTable[frame];
                uint stop = 0xffffffff;
                uint pos = 0;

                if ((frame + 1) < NumberOfFrames)
                {
                    stop = sequence.OffsetTable[frame + 1];
                }

                int i = 0;
                while (pos < offset && i < fragmentCount)
                {
                    pos += (8 + sequence.Fragments[i].Length);
                    i++;
                }

                // check for invalid offset table
                if (pos != offset)
                    goto GUESS_FRAME_OFFSET;

                while (offset < stop && i < fragmentCount)
                {
                    fragments.Add(sequence.Fragments[i]);
                    offset += (8 + sequence.Fragments[i].Length);
                    i++;
                }

                return fragments;
            }

            GUESS_FRAME_OFFSET:
            if (sequence.Fragments.Count > 0)
            {
                uint fragmentSize = sequence.Fragments[0].Length;

                bool allSameLength = true;
                for (int i = 0; i < fragmentCount; i++)
                {
                    if (sequence.Fragments[i].Length != fragmentSize)
                    {
                        allSameLength = false;
                        break;
                    }
                }

                if (allSameLength)
                {
                    if ((fragmentCount%NumberOfFrames) != 0)
                        throw new DicomDataException("Unable to determine frame length from pixel data sequence!");

                    int count = fragmentCount/NumberOfFrames;
                    int start = frame*count;

                    for (int i = 0; i < fragmentCount; i++)
                    {
                        fragments.Add(sequence.Fragments[start + i]);
                    }

                    return fragments;
                }
                else
                {
                    // what if a single frame ends on a fragment boundary?

                    int count = 0;
                    int start = 0;

                    for (int i = 0; i < fragmentCount && count < frame; i++, start++)
                    {
                        if (sequence.Fragments[i].Length != fragmentSize)
                            count++;
                    }

                    for (int i = start; i < fragmentCount; i++)
                    {
                        fragments.Add(sequence.Fragments[i]);
                        if (sequence.Fragments[i].Length != fragmentSize)
                            break;
                    }

                    return fragments;
                }
            }

            return fragments;
        }

        #endregion
    }
}
