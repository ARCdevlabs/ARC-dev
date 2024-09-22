using System.Collections.Generic;
using System.Data;
using ADSK.JExtRAC.AutomaticFloor.Components;
using Autodesk.Revit.DB;

namespace ADSK.JExtRAC.AutomaticFloor.Entities;

public class DtSlabType : DtBase
{
	private SpSlabType _EntSpSlabType;

	private DataTable _Data;

	private Element _WorkElem;

	public DataTable Data => _Data;

	public Element WorkElem => _WorkElem;

	public DtSlabType(Attribute cmpAttribute, Elements cmpElements, Geometry cmpGeometry, Parameters cmpParameters, Settings cmpSettings)
		: base(cmpAttribute, cmpElements, cmpGeometry, cmpParameters, cmpSettings)
	{
		_EntSpSlabType = new SpSlabType(cmpAttribute, cmpParameters, cmpSettings);
		if (!_EntSpSlabType.DefSuccess)
		{
			string text = cmpAttribute.ResourceText("IDS_TXT_CATEGORY");
			string text2 = cmpAttribute.ResourceText("IDS_TXT_PARAMETER");
			base.ErrMsg = cmpAttribute.ResourceText("IDS_ERR_PARAMDEF") + "\n" + text + " = " + _EntSpSlabType.DefCatName + "\n    " + text2 + "[" + _EntSpSlabType.ErrDefName + "]";
			_Data = null;
			_WorkElem = null;
		}
	}

	private void DefDataFormat(ref DataTable data)
	{
		data.Columns.Add(base.ColNameID, typeof(int));
		data.Columns.Add(base.ColNameName, typeof(string));
	}

	public DataRow GetData(Element elem)
	{
		DataRow dataRow = null;
		if (_Data == null)
		{
			_Data = new DataTable();
			DefDataFormat(ref _Data);
		}
		dataRow = _Data.NewRow();
		if (elem != null)
		{
			_EntSpSlabType.CurrentElem = elem;
			dataRow[base.ColNameID] = elem.Id.IntegerValue;
			dataRow[base.ColNameName] = _EntSpSlabType.FamilyTypeName;
		}
		return dataRow;
	}

	public void GetData(IList<Element> elems)
	{
		if (_Data == null)
		{
			_Data = new DataTable();
			DefDataFormat(ref _Data);
		}
		if (elems != null)
		{
			foreach (Element elem in elems)
			{
				DataRow data = GetData(elem);
				if (data != null)
				{
					_Data.Rows.Add(data);
				}
			}
		}
		_Data.DefaultView.Sort = base.ColNameName + " ASC";
	}

	public void GetWorkElem(int elemID)
	{
		_WorkElem = base.CmpElements.GetElementDoc(elemID);
	}
}
