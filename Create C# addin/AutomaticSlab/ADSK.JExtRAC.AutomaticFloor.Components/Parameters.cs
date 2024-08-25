using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ADSK.JExtCom.Rvt;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ADSK.JExtRAC.AutomaticFloor.Components;

public class Parameters : ADSK.JExtCom.Rvt.Parameters
{
	private Attribute _CmpAttribute;

	private string _ShParamDefaultFileName;

	private string _ShParamFolderName;

	private string _ShParamFileName;

	private string _ShParamGroupName;

	public Parameters(Attribute cmpAttribute, UIDocument rvtUIDoc)
		: base(rvtUIDoc)
	{
		_CmpAttribute = cmpAttribute;
		_ShParamDefaultFileName = null;
		DefinitionFile sharedParameterFile = GetSharedParameterFile();
		if (sharedParameterFile != null)
		{
			_ShParamDefaultFileName = sharedParameterFile.Filename;
		}
		_ShParamFolderName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		_ShParamFileName = _CmpAttribute.ResourceText("IDS_SHPARAM_FILE");
		_ShParamGroupName = _CmpAttribute.ResourceText("IDS_SHPARAM_GROUP");
		if (_ShParamDefaultFileName == null)
		{
			_ShParamDefaultFileName = _ShParamFolderName + "\\" + _ShParamFileName;
		}
	}

	public bool SetSharedParamDefault()
	{
		bool result = false;
		if (SetSharedParameterFile(null, _ShParamDefaultFileName) != null)
		{
			result = true;
		}
		return result;
	}

	public bool SetDefinition(Element elem, IList<Category> categories, string defName, ForgeTypeId paramType, BuiltInParameterGroup bltParamGroup, bool visible, int bindingMode)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		return SetDefinition(elem, _ShParamFolderName, _ShParamFileName, _ShParamGroupName, categories, defName, paramType, bltParamGroup, visible, bindingMode);
	}

	public bool SetDefinition(Element elem, Category category, string defName, ForgeTypeId paramType, BuiltInParameterGroup bltParamGroup, bool visible, int bindingMode)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		IList<Category> list = new List<Category>();
		list.Add(category);
		return SetDefinition(elem, list, defName, paramType, bltParamGroup, visible, bindingMode);
	}
}
