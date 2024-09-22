using System.Collections.Generic;
using ADSK.JExtCom.Dnf;
using ADSK.JExtRAC.AutomaticFloor.Components;
using Autodesk.Revit.DB;

namespace ADSK.JExtRAC.AutomaticFloor.Entities;

public abstract class SpBase
{
	private Attribute _CmpAttribute;

	private Parameters _CmpParameters;

	private Settings _CmpSettings;

	private bool _DefSuccess;

	private string _ErrDefName;

	private string _DefCatName;

	private Element _CurrentElem;

	protected Attribute CmpAttribute => _CmpAttribute;

	protected Parameters CmpParameters => _CmpParameters;

	protected Settings CmpSettings => _CmpSettings;

	public bool DefSuccess
	{
		get
		{
			return _DefSuccess;
		}
		set
		{
			_DefSuccess = value;
		}
	}

	public string ErrDefName
	{
		get
		{
			return _ErrDefName;
		}
		set
		{
			_ErrDefName = value;
		}
	}

	public string DefCatName => _DefCatName;

	public Element CurrentElem
	{
		get
		{
			return _CurrentElem;
		}
		set
		{
			_CurrentElem = value;
		}
	}

	public string FamilyTypeName
	{
		get
		{
			string value = "";
			if (CurrentElem != null)
			{
				CmpParameters.GetValue(CurrentElem, (BuiltInParameter)(-1002003), ref value);
				_ = -1;
			}
			return value;
		}
	}

	protected SpBase(Attribute cmpAttribute, Parameters cmpParameters, Settings cmpSettings)
	{
		_CmpAttribute = cmpAttribute;
		_CmpParameters = cmpParameters;
		_CmpSettings = cmpSettings;
		_DefSuccess = true;
		_ErrDefName = "";
		_DefCatName = "";
	}

	protected void SetDefCatName(Category category)
	{
		if (!UtilValue.SplitString(_DefCatName, ",").Contains(category.Name))
		{
			if (_DefCatName != "")
			{
				_DefCatName += ",";
			}
			_DefCatName += category.Name;
		}
	}

	protected void SetDefCatName(IList<Category> categories)
	{
		foreach (Category category in categories)
		{
			SetDefCatName(category);
		}
	}
}
