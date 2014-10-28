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

namespace ClearCanvas.Common
{
    /// <summary>
    /// Describes an extension.  
    /// </summary>
    /// <remarks>
    /// Instances of this class are constructed by the framework when it processes
    /// plugins looking for extensions.
    /// </remarks>
    public class ExtensionInfo : IBrowsable
    {
        private Type _extensionClass;
        private Type _pointExtended;
        private string _name;
        private string _description;

        /// <summary>
        /// Internal constructor.
        /// </summary>
        internal ExtensionInfo(Type extensionClass, Type pointExtended, string name, string description)
        {
            _extensionClass = extensionClass;
            _pointExtended = pointExtended;
            _name = name;
            _description = description;
        }

        /// <summary>
        /// The class that implements the extension.
        /// </summary>
        public Type ExtensionClass
        {
            get { return _extensionClass; }
        }

        /// <summary>
        /// The class that defines the extension point which this extension extends.
        /// </summary>
        public Type PointExtended
        {
            get { return _pointExtended; }
        }

        #region IBrowsable Members

        /// <summary>
        /// Friendly name of this extension, if one exists, otherwise null.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// A friendly description of this extension, if one exists, otherwise null.
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Formal name of this extension, which is the fully qualified name of the extension class.
        /// </summary>
        public string FormalName
        {
            get { return _extensionClass.FullName; }
        }

        #endregion
    }
}
