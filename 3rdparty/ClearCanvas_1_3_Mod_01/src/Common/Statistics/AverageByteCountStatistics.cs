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

using System.Diagnostics;

namespace ClearCanvas.Common.Statistics
{
    /// <summary>
    /// Average statistics class based on samples of <see cref="ByteCountStatistics"/>.
    /// </summary>
    public class AverageByteCountStatistics : AverageStatistics<ulong>
    {
        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="AverageByteCountStatistics"/>
        /// </summary>
        public AverageByteCountStatistics()
            : this("AverageByteCountStatistics")
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="AverageByteCountStatistics"/> with a specific name.
        /// </summary>
        /// <param name="name">Name of the <see cref="AverageByteCountStatistics"/> instance object to be created</param>
        public AverageByteCountStatistics(string name)
            : base(name)
        {
            ValueFormatter = ByteCountFormatter.Format;
        }


        /// <summary>
        /// Creates an copy instance of <see cref="AverageByteCountStatistics"/> based on another <see cref="AverageByteCountStatistics"/> instance.
        /// </summary>
        /// <param name="source">The original <see cref="AverageByteCountStatistics"/> object</param>
        public AverageByteCountStatistics(ByteCountStatistics source)
            : base(source)
        {
            Debug.Assert(Value == source.Value);
        }

        #endregion

        #region Overridden Public Methods

        /// <summary>
        /// Adds a sample to the <see cref="AverageStatistics{T}.Samples"/> list.
        /// </summary>
        /// <typeparam name="TSample">Type of the sample value to be inserted</typeparam>
        /// <param name="sample"></param>
        public override void AddSample<TSample>(TSample sample)
        {
            if (sample is ulong)
            {
                Samples.Add((ulong) (object) sample);
                NewSamepleAdded = true;
            }
            else if (sample is long)
            {
                Samples.Add( (ulong) (long) (object) sample);
                NewSamepleAdded = true;
            }
            else if (sample is int)
            {
                Samples.Add((ulong) (int) (object) sample);
                NewSamepleAdded = true;
            }
            else if (sample is uint)
            {
                Samples.Add((ulong) (object) sample);
                NewSamepleAdded = true;
            }
            else if (sample is ByteCountStatistics)
            {
                Samples.Add(((ByteCountStatistics) (object) sample).Value);
                NewSamepleAdded = true;
            }
            else
            {
                base.AddSample(sample);
            }
        }

        #endregion

        #region Overridden Protected Methods

        /// <summary>
        /// Computes the average for the samples in <see cref="AverageStatistics{T}"/> list.
        /// </summary>
        protected override void ComputeAverage()
        {
            if (NewSamepleAdded)
            {
                Debug.Assert(Samples.Count > 0);

                double sum = 0;
                foreach (ulong sample in Samples)
                {
                    sum += sample;
                }
                Value = (ulong) (sum/Samples.Count);
                NewSamepleAdded = false;
            }
        }

        #endregion
    }
}