using System;
using System.Collections.Generic;
using ADSK.JExtCom.Rvt;
using ADSK.JExtRAC.AutomaticFloor.Utils;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ADSK.JExtRAC.AutomaticFloor.Components;

public class Elements : ADSK.JExtCom.Rvt.Elements
{
	public Elements(UIDocument rvtUIDoc)
		: base(rvtUIDoc)
	{
	}

	public IList<Element> GetFloorTypes(eFloorType eFloorType)
	{
		IList<Element> list = new List<Element>();
		IList<Type> list2 = new List<Type>();
		list2.Add(typeof(FloorType));
		IList<Category> list3 = new List<Category>();
		if (eFloorType == eFloorType.Slab)
		{
			list3.Add(GetCategory((BuiltInCategory)(-2001300)));
		}
		else
		{
			list3.Add(GetCategory((BuiltInCategory)(-2000032)));
		}
		foreach (Element item in GetElementsDoc(null, list2, list3, null, null))
		{
			if (!(((object)item).GetType() == typeof(FloorType)))
			{
				continue;
			}
			FloorType val = (FloorType)(object)((item is FloorType) ? item : null);
			if (eFloorType == eFloorType.Slab)
			{
				if (!val.IsFoundationSlab)
				{
					continue;
				}
			}
			else if (val.IsFoundationSlab)
			{
				continue;
			}
			list.Add(item);
		}
		return list;
	}
}
