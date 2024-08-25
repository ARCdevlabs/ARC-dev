using System.Collections.Generic;
using System.Data;
using ADSK.JExtCom.Dnf;
using ADSK.JExtRAC.AutomaticFloor.Components;
using Autodesk.Revit.DB;

namespace ADSK.JExtRAC.AutomaticFloor.Entities;

public class DtCmd : DtBase
{
	private SpCmd _EntSpCmd;

	private IList<string> _Data;

	private DataTable _DataDirection;

	private double _DegreeAngle;

	private int _numberMin;

	private int _numberMax;

	public IList<string> Data => _Data;

	public double DegreeAngle
	{
		get
		{
			return _DegreeAngle;
		}
		set
		{
			_DegreeAngle = value;
		}
	}

	public DataTable DataDirection
	{
		get
		{
			if (_DataDirection == null)
			{
				_DataDirection = new DataTable();
				_DataDirection.Columns.Add("Name", typeof(string));
				_DataDirection.Columns.Add("Value", typeof(string));
				DataRow dataRow = _DataDirection.NewRow();
				dataRow["Name"] = "-90";
				dataRow["Value"] = "-90";
				_DataDirection.Rows.Add(dataRow);
				dataRow = _DataDirection.NewRow();
				dataRow["Name"] = "-75";
				dataRow["Value"] = "-75";
				_DataDirection.Rows.Add(dataRow);
				dataRow = _DataDirection.NewRow();
				dataRow["Name"] = "-60";
				dataRow["Value"] = "-60";
				_DataDirection.Rows.Add(dataRow);
				dataRow = _DataDirection.NewRow();
				dataRow["Name"] = "-45";
				dataRow["Value"] = "-45";
				_DataDirection.Rows.Add(dataRow);
				dataRow = _DataDirection.NewRow();
				dataRow["Name"] = "-30";
				dataRow["Value"] = "-30";
				_DataDirection.Rows.Add(dataRow);
				dataRow = _DataDirection.NewRow();
				dataRow["Name"] = "-15";
				dataRow["Value"] = "-15";
				_DataDirection.Rows.Add(dataRow);
				dataRow = _DataDirection.NewRow();
				dataRow["Name"] = "0";
				dataRow["Value"] = "0";
				_DataDirection.Rows.Add(dataRow);
				dataRow = _DataDirection.NewRow();
				dataRow["Name"] = "15";
				dataRow["Value"] = "15";
				_DataDirection.Rows.Add(dataRow);
				dataRow = _DataDirection.NewRow();
				dataRow["Name"] = "30";
				dataRow["Value"] = "30";
				_DataDirection.Rows.Add(dataRow);
				dataRow = _DataDirection.NewRow();
				dataRow["Name"] = "45";
				dataRow["Value"] = "45";
				_DataDirection.Rows.Add(dataRow);
				dataRow = _DataDirection.NewRow();
				dataRow["Name"] = "60";
				dataRow["Value"] = "60";
				_DataDirection.Rows.Add(dataRow);
				dataRow = _DataDirection.NewRow();
				dataRow["Name"] = "75";
				dataRow["Value"] = "75";
				_DataDirection.Rows.Add(dataRow);
				dataRow = _DataDirection.NewRow();
				dataRow["Name"] = "90";
				dataRow["Value"] = "90";
				_DataDirection.Rows.Add(dataRow);
			}
			return _DataDirection;
		}
	}

	public int NumberMin => _numberMin;

	public int NumberMax => _numberMax;

	public DtCmd(Attribute cmpAttribute, Elements cmpElements, Geometry cmpGeometry, Parameters cmpParameters, Settings cmpSettings, ProjectInfo elemProjInfo, string defName, int itemNum)
		: base(cmpAttribute, cmpElements, cmpGeometry, cmpParameters, cmpSettings)
	{
		_EntSpCmd = new SpCmd(cmpAttribute, cmpParameters, cmpSettings, elemProjInfo, defName, itemNum);
		if (!_EntSpCmd.DefSuccess)
		{
			base.ErrMsg = cmpAttribute.ResourceText("IDS_ERR_PARAMDEF");
			return;
		}
		_Data = _EntSpCmd.GetData();
		_numberMin = -90;
		_numberMax = 90;
	}

	public void SetData()
	{
		_EntSpCmd.SetData(_Data);
	}

	public string SetErrPvdDecimalText(string value)
	{
		string text = "";
		if (UtilValue.IsNull(value))
		{
			text = base.CmpAttribute.ResourceText("IDS_ERR_VALNULL");
		}
		if (text == "" && !UtilValue.IsNumber(value))
		{
			text = base.CmpAttribute.ResourceText("IDS_ERR_VALNUMBER");
		}
		if (text == "")
		{
			double num = double.Parse(value);
			if (num < (double)NumberMin || num > (double)NumberMax)
			{
				text = base.CmpAttribute.ResourceText("IDS_ERR_VALOUT");
			}
		}
		return text;
	}
}
