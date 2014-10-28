using System.Runtime.Serialization;

namespace ClearCanvas.Dicom.ServiceModel.Query
{
	[DataContract(Namespace = QueryNamespace.Value)]
	public class SeriesIdentifier : Identifier
	{
		#region Private Fields

		private string _studyInstanceUid;
		private string _seriesInstanceUid;
		private string _modality;
		private string _seriesDescription;
		private string _seriesNumber;
		private int? _numberOfSeriesRelatedInstances;

		#endregion

		#region Public Constructors

		public SeriesIdentifier()
		{
		}

		public SeriesIdentifier(DicomAttributeCollection attributes)
			: base(attributes)
		{
		}

		#endregion

		#region Public Properties

		public override string QueryRetrieveLevel
		{
			get { return "SERIES"; }
		}

		[DicomField(DicomTags.StudyInstanceUid, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = true)]
		public string StudyInstanceUid
		{
			get { return _studyInstanceUid; }
			set { _studyInstanceUid = value; }
		}

		[DicomField(DicomTags.SeriesInstanceUid, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = true)]
		public string SeriesInstanceUid
		{
			get { return _seriesInstanceUid; }
			set { _seriesInstanceUid = value; }
		}

		[DicomField(DicomTags.Modality, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string Modality
		{
			get { return _modality; }
			set { _modality = value; }
		}

		[DicomField(DicomTags.SeriesDescription, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string SeriesDescription
		{
			get { return _seriesDescription; }
			set { _seriesDescription = value; }
		}

		[DicomField(DicomTags.SeriesNumber, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string SeriesNumber
		{
			get { return _seriesNumber; }
			set { _seriesNumber = value; }
		}

		[DicomField(DicomTags.NumberOfSeriesRelatedInstances, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public int? NumberOfSeriesRelatedInstances
		{
			get { return _numberOfSeriesRelatedInstances; }
			set { _numberOfSeriesRelatedInstances = value; }
		}

		#endregion
	}
}
