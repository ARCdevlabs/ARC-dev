using ADSK.JExtRAC.AutomaticFloor.Components;

namespace ADSK.JExtRAC.AutomaticFloor.Entities;

public abstract class DtBase
{
	private Attribute _CmpAttribute;

	private Elements _CmpElements;

	private Geometry _CmpGeometry;

	private Parameters _CmpParameters;

	private Settings _CmpSettings;

	private string _ErrMsg;

	private string _ColNameID;

	private string _ColNameName;

	protected Attribute CmpAttribute => _CmpAttribute;

	protected Elements CmpElements => _CmpElements;

	protected Geometry CmpGeometry => _CmpGeometry;

	protected Parameters CmpParameters => _CmpParameters;

	protected Settings CmpSettings => _CmpSettings;

	public string ErrMsg
	{
		get
		{
			return _ErrMsg;
		}
		set
		{
			_ErrMsg = value;
		}
	}

	public string ColNameID
	{
		get
		{
			if (_ColNameID == null)
			{
				_ColNameID = _CmpAttribute.ResourceText("IDS_COLNAME_ID");
			}
			return _ColNameID;
		}
	}

	public string ColNameName
	{
		get
		{
			if (_ColNameName == null)
			{
				_ColNameName = _CmpAttribute.ResourceText("IDS_COLNAME_NAME");
			}
			return _ColNameName;
		}
	}

	protected DtBase(Attribute cmpAttribute, Elements cmpElements, Geometry cmpGeometry, Parameters cmpParameters, Settings cmpSettings)
	{
		_CmpAttribute = cmpAttribute;
		_CmpElements = cmpElements;
		_CmpGeometry = cmpGeometry;
		_CmpParameters = cmpParameters;
		_CmpSettings = cmpSettings;
		_ErrMsg = "";
	}
}
