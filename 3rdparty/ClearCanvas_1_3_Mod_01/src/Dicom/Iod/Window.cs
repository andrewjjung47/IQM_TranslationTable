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

namespace ClearCanvas.Dicom.Iod
{
    /// <summary>
    /// Represents a window/level value pair.
    /// </summary>
	public class Window : IEquatable<Window>
    {
		#region Private Members

		private double _width;
		private double _center;

		#endregion

    	/// <summary>
		/// Constructor.
		/// </summary>
		public Window(double width, double center)
		{
			_width = width;
			_center = center;
		}

		/// <summary>
		/// Protected constructor.
		/// </summary>
		protected Window()
		{
		}

		#region Public Properties

		/// <summary>
		/// Gets the window width.
		/// </summary>
		public virtual double Width
        {
            get { return _width; }
			protected set { _width = value; }
        }

		/// <summary>
		/// Gets the window center.
		/// </summary>
		public virtual double Center
        {
            get { return _center; }
			protected set { _center = value; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets a string representing the window width/center pair.
		/// </summary>
		public override string ToString()
		{
			return String.Format(@"{0:F2}/{1:F2}", _width, _center);
		}

    	#region IEquatable<Window> Members

		public bool Equals(Window other)
		{
			if (other == null)
				return false;

			return _width == other._width && _center == other._center;
		}

		#endregion

    	public override bool Equals(object obj)
    	{
    		if (obj == null)
    			return false;

			return this.Equals(obj as Window);
    	}

    	/// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
		}
		#endregion
	}
}
