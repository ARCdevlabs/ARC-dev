using System.Collections.Generic;
using ADSK.JExtCom.Dnf;
using ADSK.JExtRAC.AutomaticFloor.Components;
using Autodesk.Revit.DB;

namespace ADSK.JExtRAC.AutomaticFloor.Entities;

public class SpCmd : SpBase
{
	private ProjectInfo _ElemProjInfo;

	private string _ParamNameCmd;

	private int _ItemNum;

	public SpCmd(Attribute cmpAttribute, Parameters cmpParameters, Settings cmpSettings, ProjectInfo elemProjInfo, string defName, int itemNum)
		: base(cmpAttribute, cmpParameters, cmpSettings)
	{
		_ElemProjInfo = elemProjInfo;
		_ParamNameCmd = defName;
		_ItemNum = itemNum;
		base.DefSuccess = SetDef();
	}

	private bool SetDef()
	{
		return base.CmpParameters.SetDefinition(null, base.CmpSettings.CategoryProjInfo, _ParamNameCmd, String.Text, (BuiltInParameterGroup)(-1), visible: false, 0);
	}

	public IList<string> GetData()
	{
		string value = "";
		IList<string> list = new List<string>();
		base.CmpParameters.GetValue((Element)(object)_ElemProjInfo, _ParamNameCmd, String.Text, (BuiltInParameterGroup)(-1), ref value);
		_ = -1;
		IList<string> list2 = UtilValue.SplitString(value, ",");
		bool flag = false;
		if (_ItemNum == list2.Count)
		{
			flag = true;
		}
		if (_ItemNum > 0)
		{
			for (int i = 0; i < _ItemNum; i++)
			{
				if (flag)
				{
					list.Add(list2[i]);
				}
				else
				{
					list.Add("");
				}
			}
		}
		else if (list2.Count > 0)
		{
			for (int j = 0; j < list2.Count; j++)
			{
				list.Add(list2[j]);
			}
		}
		return list;
	}

	public bool SetData(IList<string> value)
	{
		string text = null;
		string text2 = ",";
		bool result = false;
		if (value != null)
		{
			foreach (string item in value)
			{
				text = text + item + text2;
			}
		}
		if (text != null)
		{
			text = text.Substring(0, text.Length - 1);
		}
		if (text != null)
		{
			base.CmpParameters.SetValue((Element)(object)_ElemProjInfo, _ParamNameCmd, String.Text, (BuiltInParameterGroup)(-1), text);
			result = true;
		}
		return result;
	}
}
