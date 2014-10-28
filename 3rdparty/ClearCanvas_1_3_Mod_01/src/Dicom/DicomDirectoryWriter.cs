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
using System.Collections.Generic;
using System.IO;
using ClearCanvas.Common;

namespace ClearCanvas.Dicom
{
	//TODO: Move to Utilities?  Should it just be DicomDirectory with both add/remove functionality (and both Load/Save methods).

    /// <summary>
    /// This class writes a Dicom Directory file.  
    /// </summary>
    /// <example>
    /// using (DicomDirectoryWriter dicomDirectory = new DicomDirectoryWriter())
    /// {
    ///     dicomDirectory.SourceApplicationEntityTitle = "UNO";
    ///     dicomDirectory.FileSetId = "My File Set Desc";
    ///     dicomDirectory.AddFile("C:\DicomImages\SomeFile.dcm", "DIR001\\IMAGE001.DCM");
    ///     dicomDirectory.AddFile("C:\DicomImages\AnotherFile.dcm", "DIR002\\IMAGE002.DCM");
    ///     dicomDirectory.AddFile("C:\DicomImages\AnotherFile3.dcm", null);
    ///     dicomDirectory.Save("C:\\Temp\\DICOMDIR");
    ///  }
    /// </example>
    public class DicomDirectoryWriter : IDisposable
    {
        #region Internal Constants
        internal const string DirectoryRecordTypePatient = "PATIENT";
        internal const string DirectoryRecordTypeStudy = "STUDY";
        internal const string DirectoryRecordTypeSeries = "SERIES";
        internal const string DirectoryRecordTypeImage = "IMAGE";
        #endregion

        #region Private Variables
        /// <summary>Contains all the Dicom Image files to be added to the directory</summary>
        private readonly Dictionary<DicomFile, string> _dicomFiles = new Dictionary<DicomFile, string>();

        /// <summary>The directory record sequence item that all the directory record items gets added to.</summary>
        private readonly DicomAttributeSQ _directoryRecordSequence;

        /// <summary>The Dicom Directory File</summary>
        private DicomFile _dicomDirFile;

        /// <summary>File Name to be saved to (Param to Save method)</summary>
        private string _saveFileName;

        /// <summary>Contains the ongoing fileOffset to determine the offset tags for each Item</summary>
        private uint _fileOffset;

        /// <summary>Contains the first directory record of in the root of the DICOMDIR.</summary>
        private DirectoryRecordSequenceItem _rootRecord = null;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the DicomDirectory class.
        /// </summary>
        /// <remarks>Sets most default values which can be changed via </remarks>
        public DicomDirectoryWriter()
        {
            try
            {
                _dicomDirFile = new DicomFile();

                _dicomDirFile.MetaInfo[DicomTags.FileMetaInformationVersion].Values = new byte[2] { 0x00, 0x01 };
                _dicomDirFile.MediaStorageSopClassUid = DicomUids.MediaStorageDirectoryStorage.UID;
                _dicomDirFile.SourceApplicationEntityTitle = String.Empty;
                _dicomDirFile.TransferSyntax = TransferSyntax.ExplicitVrLittleEndian;

                //_dicomDirFile.PrivateInformationCreatorUid = String.Empty;
                _dicomDirFile.DataSet[DicomTags.FileSetId].Values = String.Empty;
                ImplementationVersionName = DicomImplementation.Version;
                ImplementationClassUid = DicomImplementation.ClassUID.UID;

                 _dicomDirFile.MediaStorageSopInstanceUid = DicomUid.GenerateUid().UID;

                // Set zero value so we can calculate the file Offset
                _dicomDirFile.DataSet[DicomTags.OffsetOfTheFirstDirectoryRecordOfTheRootDirectoryEntity].Values = (uint)0;
                _dicomDirFile.DataSet[DicomTags.OffsetOfTheLastDirectoryRecordOfTheRootDirectoryEntity].Values = 0;
                _dicomDirFile.DataSet[DicomTags.FileSetConsistencyFlag].Values = 0;

                _directoryRecordSequence = (DicomAttributeSQ)_dicomDirFile.DataSet[DicomTags.DirectoryRecordSequence];
            }
            catch (Exception ex)
            {
				Platform.Log(LogLevel.Error, ex, "Exception initializing DicomDirectory");
                throw;
            }
        }
        #endregion

        #region Public Properties

        //NOTE: these are mostly wrappers around the DicomFile properties

        /// <summary>
        /// Gets or sets the file set id.
        /// </summary>
        /// <value>The file set id.</value>
        /// <remarks>User or implementation specific Identifier (up to 16 characters), intended to be a short human readable label to easily (but
        /// not necessarily uniquely) identify a specific File-set to
        /// facilitate operator manipulation of the physical media on
        /// which the File-set is stored. </remarks>
        public string FileSetId
        {
            get { return _dicomDirFile.DataSet[DicomTags.FileSetId].GetString(0, String.Empty); }
            set
            {
                if (value != null && value.Trim().Length > 16)
                    throw new ArgumentException("fileSetId can only be a maximum of 16 characters", "value");

                _dicomDirFile.DataSet[DicomTags.FileSetId].SetString(0, value == null ? "" : value.Trim());
            }
        }

        /// <summary>
        /// The DICOM Application Entity (AE) Title of the AE which wrote this file's 
        /// content (or last updated it).  If used, it allows the tracin of the source 
        /// of errors in the event of media interchange problems.  The policies associated
        /// with AE Titles are the same as those defined in PS 3.8 of the DICOM Standard. 
        /// </summary>
        public string SourceApplicationEntityTitle
        {
            get { return _dicomDirFile.SourceApplicationEntityTitle; }
            set
            {
                _dicomDirFile.SourceApplicationEntityTitle = value;
            }
        }

        /// <summary>
        /// Identifies a version for an Implementation Class UID (002,0012) using up to 
        /// 16 characters of the repertoire.  It follows the same policies as defined in 
        /// PS 3.7 of the DICOM Standard (association negotiation).
        /// </summary>
        public string ImplementationVersionName
        {
            get { return _dicomDirFile.ImplementationVersionName; }
            set
            {
                _dicomDirFile.ImplementationVersionName = value;
            }
        }

        /// <summary>
        /// Uniquely identifies the implementation which wrote this file and its content.  It provides an 
        /// unambiguous identification of the type of implementation which last wrote the file in the 
        /// event of interchagne problems.  It follows the same policies as defined by PS 3.7 of the DICOM Standard
        /// (association negotiation).
        /// </summary>        
        public string ImplementationClassUid
        {
            get { return _dicomDirFile.ImplementationClassUid; }
            set
            {
                _dicomDirFile.ImplementationClassUid = value;
            }
        }

        /// <summary>
        /// The transfer syntax the file is encoded in.
        /// </summary>
        /// <remarks>
        /// This property returns a TransferSyntax object for the transfer syntax encoded 
        /// in the tag Transfer Syntax UID (0002,0010).
        /// </remarks>
        public TransferSyntax TransferSyntax
        {
            get { return _dicomDirFile.TransferSyntax; }
            set { _dicomDirFile.TransferSyntax = value; }
        }

        /// <summary>
        /// Uniquiely identifies the SOP Instance associated with the Data Set placed in the file and following the File Meta Information.
        /// </summary>
        public string MediaStorageSopInstanceUid
        {
            get { return _dicomDirFile.MediaStorageSopInstanceUid; }
            set { _dicomDirFile.MediaStorageSopInstanceUid = value; }
        }

        /// <summary>
        /// Identifies a version for an Implementation Class UID (002,0012) using up to 
        /// 16 characters of the repertoire.  It follows the same policies as defined in 
        /// PS 3.7 of the DICOM Standard (association negotiation).
        /// </summary>
        public string PrivateInformationCreatorUid
        {
            get { return _dicomDirFile.PrivateInformationCreatorUid; }
            set { _dicomDirFile.PrivateInformationCreatorUid = value; }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Saves the Dicom Dir to the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void Save(string fileName)
        {
            DicomWriteOptions options = DicomWriteOptions.None;

            if (_dicomFiles.Count == 0)
                throw new InvalidOperationException("No Dicom Files added, cannot save dicom directory");

            _saveFileName = fileName;

            foreach (DicomFile dicomFile in _dicomFiles.Keys)
            {
                try
                {
                    if (dicomFile.DataSet.Count == 0)
                        dicomFile.Load(DicomReadOptions.Default);
                    DirectoryRecordSequenceItem patientRecord;
                    DirectoryRecordSequenceItem studyRecord;
                    DirectoryRecordSequenceItem seriesRecord;

                    if (this._rootRecord == null)
                        _rootRecord = patientRecord = CreatePatientItem(dicomFile);
                    else
                        patientRecord = GetExistingOrCreateNewPatient(_rootRecord, dicomFile);

                    if (patientRecord.LowerLevelRecord == null)
                        patientRecord.LowerLevelRecord = studyRecord = CreateStudyItem(dicomFile);
                    else
                        studyRecord = GetExistingOrCreateNewStudy(patientRecord.LowerLevelRecord, dicomFile);

                    if (studyRecord.LowerLevelRecord == null)
                        studyRecord.LowerLevelRecord = seriesRecord = CreateSeriesItem(dicomFile);
                    else
                        seriesRecord = GetExistingOrCreateNewSeries(studyRecord.LowerLevelRecord, dicomFile);

                    if (seriesRecord.LowerLevelRecord == null)
                        seriesRecord.LowerLevelRecord = CreateImageItem(dicomFile, _dicomFiles[dicomFile]);
                    else
                        GetExistingOrCreateNewImage(seriesRecord.LowerLevelRecord, dicomFile, _dicomFiles[dicomFile]);

                }
                catch (Exception ex)
                {
					Platform.Log(LogLevel.Error, ex, "Error adding image {0} to directory file", dicomFile.Filename);
                }
            }

            _directoryRecordSequence.ClearSequenceItems();

            //Set initial offset of where the directory record sequence tag starts
            // based on the 128 byte preamble, the DICM characters and the tags themselves.
            _fileOffset = 128 + 4 + _dicomDirFile.MetaInfo.CalculateWriteLength(_dicomDirFile.TransferSyntax, DicomWriteOptions.Default) 
                + _dicomDirFile.DataSet.CalculateWriteLength(_dicomDirFile.TransferSyntax, DicomWriteOptions.Default);

            //Add the offset for the Directory Record sequence tag itself
            _fileOffset += 4; // element tag
            if (_dicomDirFile.TransferSyntax.ExplicitVr)
            {
                _fileOffset += 2; // vr
                _fileOffset += 6; // length
            }
            else
            {
                _fileOffset += 4; // length
            }

            // go through every _tempSequenceRecordDictionary and add it to _directoryRecordSequence (which is the
            // DirectoryRecordSequence connected to _dicomDirFile.DataSet. While it does this it determines the correct offsets.
            AddDirectoryRecordsToSequenceItem(_rootRecord);

            // Double check to make sure at least one file was added.
            if (_rootRecord != null)
            {
                // Calculate offsets for each directory record
                CalculateOffsets(_dicomDirFile.TransferSyntax, options);

                // Traverse through the tree and set the offsets.
                SetOffsets(_rootRecord);

                //Set the offsets in the dataset 
                _dicomDirFile.DataSet[DicomTags.OffsetOfTheFirstDirectoryRecordOfTheRootDirectoryEntity].Values = _rootRecord.Offset;

                DirectoryRecordSequenceItem lastRoot = _rootRecord;
                while (lastRoot.NextRecord != null) lastRoot = lastRoot.NextRecord;

                _dicomDirFile.DataSet[DicomTags.OffsetOfTheLastDirectoryRecordOfTheRootDirectoryEntity].Values =
                    lastRoot.Offset;
            }
            else
            {
                _dicomDirFile.DataSet[DicomTags.OffsetOfTheFirstDirectoryRecordOfTheRootDirectoryEntity].Values = 0;
                _dicomDirFile.DataSet[DicomTags.OffsetOfTheLastDirectoryRecordOfTheRootDirectoryEntity].Values = 0;
            }

            // Add this at the end so it does not mess up offset vlue...
            _dicomDirFile.DataSet[DicomTags.SopClassUid].Values = DicomUids.MediaStorageDirectoryStorage.UID;

            try
            {
                _dicomDirFile.Save(fileName, DicomWriteOptions.Default);

            }
            catch (Exception ex)
            {
				Platform.Log(LogLevel.Error, ex, "Error saving dicom File {0}", fileName);
            }
        }

        /// <summary>
        /// Adds the dicom image file to the list of images to add.
        /// </summary>
        /// <param name="dicomFile">The dicom file.</param>
        /// <param name="optionalDicomDirFileLocation">specifies the file location in the Directory Record ReferencedFileId 
        /// tag.  If is null or empty, it will use a relative path to the dicom File from the specified DICOM Dir filename in the Save() method.</remarks></param>
        public void AddFile(DicomFile dicomFile, string optionalDicomDirFileLocation)
        {
            if (dicomFile == null)
                throw new ArgumentNullException("dicomFile");

            _dicomFiles.Add(dicomFile, optionalDicomDirFileLocation);
        }

        /// <summary>
        /// Adds the dicom image file to the list of images to add.
        /// </summary>
        /// <param name="dicomFileName">Name of the dicom file.</param>
        /// <param name="optionalDicomDirFileLocation">specifies the file location in the Directory Record ReferencedFileId 
        /// tag.  If is null or empty, it will use a relative path to the dicom File from the specified DICOM Dir filename in the Save() method.</remarks></param>
        public void AddFile(string dicomFileName, string optionalDicomDirFileLocation)
        {
            if (String.IsNullOrEmpty(dicomFileName))
                throw new ArgumentNullException("dicomFileName");

            if (File.Exists(dicomFileName))
                _dicomFiles.Add(new DicomFile(dicomFileName), optionalDicomDirFileLocation);
            else
                throw new FileNotFoundException("cannot add DicomFile, does not exist", dicomFileName);

        }

        /// <summary>
        /// Dumps the contents of the dicomDirFile.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="options">The dump options.</param>
        /// <returns></returns>
        public string Dump(string prefix, DicomDumpOptions options)
        {
            return this._dicomDirFile.Dump(prefix, options);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Traverse the directory record tree and insert them into the directory record sequence.
        /// </summary>
        private void AddDirectoryRecordsToSequenceItem(DirectoryRecordSequenceItem root)
        {
            if (root == null)
                return;

            _directoryRecordSequence.AddSequenceItem(root);

            if (root.LowerLevelRecord != null)
                AddDirectoryRecordsToSequenceItem(root.LowerLevelRecord);

            if (root.NextRecord != null)
                AddDirectoryRecordsToSequenceItem(root.NextRecord);
        }

        /// <summary>
        /// Finds the next directory record of the specified <paramref name="recordType"/>, starting at the specified <paramref name="startIndex"/>
        /// </summary>
        /// <param name="recordType">Type of the record.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        private void CalculateOffsets(TransferSyntax syntax, DicomWriteOptions options)
        {
            foreach (DicomSequenceItem sq in (DicomSequenceItem[])_dicomDirFile.DataSet[DicomTags.DirectoryRecordSequence].Values)
            {
                DirectoryRecordSequenceItem record = sq as DirectoryRecordSequenceItem;
                if (record == null)
                    throw new ApplicationException("Unexpected type for directory record: " + sq.GetType());

                record.Offset = _fileOffset;

                _fileOffset += 4 + 4; // Sequence Item Tag

                _fileOffset += record.CalculateWriteLength(syntax, options & ~DicomWriteOptions.CalculateGroupLengths);
                if (!Flags.IsSet(options, DicomWriteOptions.ExplicitLengthSequenceItem))
                    _fileOffset += 4 + 4; // Sequence Item Delimitation Item
            }
            if (!Flags.IsSet(options, DicomWriteOptions.ExplicitLengthSequence))
                _fileOffset += 4 + 4; // Sequence Delimitation Item
        }

        /// <summary>
        /// Traverse at the image level to see if the image exists or create a new image if it doesn't.
        /// </summary>
        /// <param name="images"></param>
        /// <param name="file"></param>
        /// <param name="optionalDicomDirFileLocation"></param>
        /// <returns></returns>
        private DirectoryRecordSequenceItem GetExistingOrCreateNewImage(DirectoryRecordSequenceItem images, DicomFile file, string optionalDicomDirFileLocation)
        {
            DirectoryRecordSequenceItem currentImage = images;
            while (currentImage != null)
            {
                if (currentImage[DicomTags.ReferencedSopInstanceUidInFile].Equals(file.DataSet[DicomTags.SopInstanceUid]))
                {
                    return currentImage;
                }
                if (currentImage.NextRecord == null)
                {
                    currentImage.NextRecord = CreateImageItem(file, optionalDicomDirFileLocation);
                    return currentImage.NextRecord;
                }
                currentImage = currentImage.NextRecord;
            }
            return null;
        }

        /// <summary>
        /// Create an image Directory record
        /// </summary>
        /// <param name="dicomFile">The dicom file.</param>
        /// <param name="optionalDicomDirFileLocation">The optional dicom dir file location.</param>
        private DirectoryRecordSequenceItem CreateImageItem(DicomFile dicomFile, string optionalDicomDirFileLocation)
        {
            if (String.IsNullOrEmpty(optionalDicomDirFileLocation))
            {
                optionalDicomDirFileLocation = EvaluateRelativePath(_saveFileName, dicomFile.Filename);
            }

            //TODO: Deal w/ Non-Image directory record types here

            IDictionary<uint, object> dicomTags = new Dictionary<uint, object>();
            dicomTags.Add(DicomTags.ReferencedFileId, optionalDicomDirFileLocation);
            dicomTags.Add(DicomTags.ReferencedSopClassUidInFile, dicomFile.SopClass.Uid);
            dicomTags.Add(DicomTags.ReferencedSopInstanceUidInFile, dicomFile.MediaStorageSopInstanceUid); // DataSet[DicomTags.SopInstanceUid].GetUid(0, new DicomUid()));
            dicomTags.Add(DicomTags.ReferencedTransferSyntaxUidInFile, dicomFile.TransferSyntaxUid);
            dicomTags.Add(DicomTags.InstanceNumber, null);

            return AddSequenceItem(DirectoryRecordTypeImage, dicomFile.DataSet, dicomTags);
        }
        #endregion

        #region Private Static Methods
        /// <summary>
        /// Traverse through the tree of directory records, and set the values for the offsets for each 
        /// record.
        /// </summary>
        private static void SetOffsets(DirectoryRecordSequenceItem root)
        {
            if (root.NextRecord != null)
            {
                root[DicomTags.OffsetOfTheNextDirectoryRecord].Values = root.NextRecord.Offset;
                SetOffsets(root.NextRecord);
            }
            else
                root[DicomTags.OffsetOfTheNextDirectoryRecord].Values = 0;

            if (root.LowerLevelRecord != null)
            {
                root[DicomTags.OffsetOfReferencedLowerLevelDirectoryEntity].Values = root.LowerLevelRecord.Offset;
                SetOffsets(root.LowerLevelRecord);
            }
            else
                root[DicomTags.OffsetOfReferencedLowerLevelDirectoryEntity].Values = 0;
        }

        /// <summary>
        /// Traverse at the Patient level to check if a Patient exists or create a Patient if it doesn't exist.
        /// </summary>
        /// <param name="patients"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        private static DirectoryRecordSequenceItem GetExistingOrCreateNewPatient(DirectoryRecordSequenceItem patients, DicomFile file)
        {
            DirectoryRecordSequenceItem currentPatient = patients;
            while (currentPatient != null)
            {
                if (currentPatient[DicomTags.PatientId].Equals(file.DataSet[DicomTags.PatientId])
                    && currentPatient[DicomTags.PatientsName].Equals(file.DataSet[DicomTags.PatientsName]))
                {
                    return currentPatient;
                }
                if (currentPatient.NextRecord == null)
                {
                    currentPatient.NextRecord = CreatePatientItem(file);
                    return currentPatient.NextRecord;
                }
                currentPatient = currentPatient.NextRecord;
            }
            return null;
        }

        /// <summary>
        /// Traverse at the Study level to check if a Study exists or create a Study if it doesn't exist.
        /// </summary>
        /// <param name="studies"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        private static DirectoryRecordSequenceItem GetExistingOrCreateNewStudy(DirectoryRecordSequenceItem studies, DicomFile file)
        {
            DirectoryRecordSequenceItem currentStudy = studies;
            while (currentStudy != null)
            {
                if (currentStudy[DicomTags.StudyInstanceUid].Equals(file.DataSet[DicomTags.StudyInstanceUid]))
                {
                    return currentStudy;
                }
                if (currentStudy.NextRecord == null)
                {
                    currentStudy.NextRecord = CreateStudyItem(file);
                    return currentStudy.NextRecord;
                }
                currentStudy = currentStudy.NextRecord;
            }
            return null;
        }

        /// <summary>
        /// Traverse at the Series level to check if a Series exists, or create a Series if it doesn't exist.
        /// </summary>
        /// <param name="series"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        private static DirectoryRecordSequenceItem GetExistingOrCreateNewSeries(DirectoryRecordSequenceItem series, DicomFile file)
        {
            DirectoryRecordSequenceItem currentSeries = series;
            while (currentSeries != null)
            {
                if (currentSeries[DicomTags.SeriesInstanceUid].Equals(file.DataSet[DicomTags.SeriesInstanceUid]))
                {
                    return currentSeries;
                }
                if (currentSeries.NextRecord == null)
                {
                    currentSeries.NextRecord = CreateSeriesItem(file);
                    return currentSeries.NextRecord;
                }
                currentSeries = currentSeries.NextRecord;
            }
            return null;
        }

        /// <summary>
        /// Adds a sequence item to temporarydictionary with the current offset.
        /// </summary>
        /// <param name="recordType">Type of the record.</param>
        /// <param name="dataSet">The data set.</param>
        /// <param name="tags">The tags.</param>
        /// <returns>The newly created DirectoryRecord</returns>
        /// <remarks>Tags are a dictionary of tags and optional values - if the value is null, then it will get the value from the specified dataset</remarks>
        private static DirectoryRecordSequenceItem AddSequenceItem(string recordType, DicomAttributeCollection dataSet, IDictionary<uint, object> tags)
        {
            DirectoryRecordSequenceItem dicomSequenceItem = new DirectoryRecordSequenceItem();
            dicomSequenceItem[DicomTags.OffsetOfTheNextDirectoryRecord].Values = 0;
            dicomSequenceItem[DicomTags.RecordInUseFlag].Values = 0xFFFF;
            dicomSequenceItem[DicomTags.OffsetOfReferencedLowerLevelDirectoryEntity].Values = 0;
            dicomSequenceItem[DicomTags.DirectoryRecordType].Values = recordType;
            foreach (uint dicomTag in tags.Keys)
            {
                try
                {
                    DicomTag dicomTag2 = DicomTagDictionary.GetDicomTag(dicomTag);
                	DicomAttribute attrib;
                    if (tags[dicomTag] != null)
                    {
                        dicomSequenceItem[dicomTag].Values = tags[dicomTag];
                    }
                    else if (dataSet.TryGetAttribute(dicomTag, out attrib))
                    {
                        dicomSequenceItem[dicomTag].Values = attrib.Values;
                    }
                    else
                    {
                        Platform.Log(LogLevel.Info, 
							"Cannot find dicomTag {0} for record type {1}", dicomTag2 != null ? dicomTag2.ToString() : dicomTag.ToString(), recordType);
                    }
                }
                catch (Exception ex)
                {
					Platform.Log(LogLevel.Error, ex, "Exception adding dicomTag {0} to directory record for record type {1}", dicomTag, recordType);
                }
            }

            return dicomSequenceItem;
        }

        /// <summary>
        /// Create a Patient Directory Record
        /// </summary>
        /// <param name="dicomFile">The dicom file.</param>
        private static DirectoryRecordSequenceItem CreatePatientItem(DicomFile dicomFile)
        {
            if (dicomFile == null)
                throw new ArgumentNullException("dicomFile");

            IDictionary<uint, object> dicomTags = new Dictionary<uint, object>();
            dicomTags.Add(DicomTags.PatientsName, null);
            dicomTags.Add(DicomTags.PatientId, null);
            dicomTags.Add(DicomTags.PatientsBirthDate, null);
            dicomTags.Add(DicomTags.PatientsSex, null);

            return AddSequenceItem(DirectoryRecordTypePatient, dicomFile.DataSet, dicomTags);
        }

        /// <summary>
        /// Create a Study Directory Record
        /// </summary>
        /// <param name="dicomFile">The dicom file.</param>
        private static DirectoryRecordSequenceItem CreateStudyItem(DicomFile dicomFile)
        {
            IDictionary<uint, object> dicomTags = new Dictionary<uint, object>();
            dicomTags.Add(DicomTags.StudyInstanceUid, null);
            dicomTags.Add(DicomTags.StudyId, null);
            dicomTags.Add(DicomTags.StudyDate, null);
            dicomTags.Add(DicomTags.StudyTime, null);
            dicomTags.Add(DicomTags.AccessionNumber, null);
            dicomTags.Add(DicomTags.StudyDescription, null);

            return AddSequenceItem(DirectoryRecordTypeStudy, dicomFile.DataSet, dicomTags);
        }

        /// <summary>
        /// Create a Series Directory Record
        /// </summary>
        /// <param name="dicomFile">The dicom file.</param>
        private static DirectoryRecordSequenceItem CreateSeriesItem(DicomFile dicomFile)
        {
            IDictionary<uint, object> dicomTags = new Dictionary<uint, object>();
            dicomTags.Add(DicomTags.SeriesInstanceUid, null);
            dicomTags.Add(DicomTags.Modality, null);
            dicomTags.Add(DicomTags.SeriesDate, null);
            dicomTags.Add(DicomTags.SeriesTime, null);
            dicomTags.Add(DicomTags.SeriesNumber, null);
            dicomTags.Add(DicomTags.SeriesDescription, null);
            //dicomTags.Add(DicomTags.SeriesDescription, dicomFile.DataSet[DicomTags.SeriesDescription].GetString(0, String.Empty));

            return AddSequenceItem(DirectoryRecordTypeSeries, dicomFile.DataSet, dicomTags);
        }
        /// <summary>
        /// Evaluates the relative path to <paramref name="absoluteFilePath"/> from <paramref name="mainDirPath"/>.
        /// </summary>
        /// <param name="mainDirPath">The main dir path.</param>
        /// <param name="absoluteFilePath">The absolute file path.</param>
        /// <returns></returns>
        private static string EvaluateRelativePath(string mainDirPath, string absoluteFilePath)
        {
            string[] firstPathParts = mainDirPath.Trim(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);
            string[] secondPathParts = absoluteFilePath.Trim(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);

            int sameCounter = 0;
            for (int i = 0; i < Math.Min(firstPathParts.Length, secondPathParts.Length); i++)
            {
                if (!firstPathParts[i].ToLower().Equals(secondPathParts[i].ToLower()))
                {
                    break;
                }
                sameCounter++;
            }

            if (sameCounter == 0)
            {
                return absoluteFilePath;
            }

            string newPath = String.Empty;

            for (int i = sameCounter; i < firstPathParts.Length; i++)
            {
                if (i > sameCounter)
                {
                    newPath += Path.DirectorySeparatorChar;
                }
                newPath += "..";
            }

            if (newPath.Length == 0)
            {
                newPath = ".";
            }

            for (int i = sameCounter; i < secondPathParts.Length; i++)
            {
                newPath += Path.DirectorySeparatorChar;
                newPath += secondPathParts[i];
            }

            return newPath;
        }
        #endregion

        #region IDisposable Members

        private bool m_Disposed;
        public void Dispose()
        {
            if (!m_Disposed)
            {
                if (_dicomDirFile != null)
                    _dicomDirFile = null;

                m_Disposed = true;
            }
        }

        #endregion
    }
}
