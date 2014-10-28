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

namespace ClearCanvas.Dicom.Iod
{
	/// <summary>
	/// Enumeration of photometric interpretations.
	/// </summary>
	public enum PhotometricInterpretation
    {
        Unknown = 0,
        Monochrome1,
        Monochrome2,
        PaletteColor,
        Rgb,
        YbrFull,
        YbrFull422,
        YbrPartial422,
        YbrIct,
        YbrRct
    }

    /// <summary>
    /// Helper class for converting between strings and <see cref="PhotometricInterpretation"/>s.
    /// </summary>
	public class PhotometricInterpretationHelper
    {
        static private Dictionary<PhotometricInterpretation, string> _dictionaryPhotometricInterpretation;
        static PhotometricInterpretationHelper()
        {
            _dictionaryPhotometricInterpretation = new Dictionary<PhotometricInterpretation, string>();
            _dictionaryPhotometricInterpretation.Add(PhotometricInterpretation.Unknown, "UNKNOWN");
            _dictionaryPhotometricInterpretation.Add(PhotometricInterpretation.Monochrome1, "MONOCHROME1");
            _dictionaryPhotometricInterpretation.Add(PhotometricInterpretation.Monochrome2, "MONOCHROME2");
            _dictionaryPhotometricInterpretation.Add(PhotometricInterpretation.PaletteColor, "PALETTE COLOR");
            _dictionaryPhotometricInterpretation.Add(PhotometricInterpretation.Rgb, "RGB");
            _dictionaryPhotometricInterpretation.Add(PhotometricInterpretation.YbrFull, "YBR_FULL");
            _dictionaryPhotometricInterpretation.Add(PhotometricInterpretation.YbrFull422, "YBR_FULL_422");
            _dictionaryPhotometricInterpretation.Add(PhotometricInterpretation.YbrIct, "YBR_ICT");
            _dictionaryPhotometricInterpretation.Add(PhotometricInterpretation.YbrPartial422, "YBR_PARTIAL_422");
            _dictionaryPhotometricInterpretation.Add(PhotometricInterpretation.YbrRct, "YBR_RCT");
        }

        public static string GetString(PhotometricInterpretation pi)
        {
            if (_dictionaryPhotometricInterpretation.ContainsKey(pi))
                return _dictionaryPhotometricInterpretation[pi];
            else
                return null;
        }

		public static PhotometricInterpretation FromString(string photometricInterpretation)
		{
			foreach (KeyValuePair<PhotometricInterpretation, string> pair in _dictionaryPhotometricInterpretation)
			{
				if (pair.Key == PhotometricInterpretation.Unknown)
					continue;

				if (photometricInterpretation == pair.Value)
					return pair.Key;
			}

			return PhotometricInterpretation.Unknown;
		}
    }
}
