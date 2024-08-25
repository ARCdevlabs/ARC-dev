using System.Collections.Generic;
using ADSK.JExtRAC.AutomaticFloor.Components;
using Autodesk.Revit.DB;

namespace ADSK.JExtRAC.AutomaticFloor.Entities;

public class SpSlabType : SpBase
{
	private IList<Category> _ParamCategories;

	public SpSlabType(Attribute cmpAttribute, Parameters cmpParameters, Settings cmpSettings)
		: base(cmpAttribute, cmpParameters, cmpSettings)
	{
		_ParamCategories = base.CmpSettings.CategorySlab;
		SetDefCatName(_ParamCategories);
		SetDef();
	}

	private void SetDef()
	{
		bool defSuccess = true;
		base.DefSuccess = defSuccess;
	}
}
