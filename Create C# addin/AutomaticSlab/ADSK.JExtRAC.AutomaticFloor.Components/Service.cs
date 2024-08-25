using System;
using System.Collections.Generic;
using ADSK.JExtRAC.AutomaticFloor.Entities;
using ADSK.JExtRAC.AutomaticFloor.Utils;
using Autodesk.Revit.DB;

namespace ADSK.JExtRAC.AutomaticFloor.Components;

public class Service
{
	private Attribute _CmpAttribute;

	private Elements _CmpElements;

	private Geometry _CmpGeometry;

	private Parameters _CmpParameters;

	private Settings _CmpSettings;

	private DtItems _EntDtItems;

	private string _ErrMsg;

	public string ErrMsg => _ErrMsg;

	public Service(Attribute cmpAttribute, Elements cmpElements, Geometry cmpGeometry, Parameters cmpParameters, Settings cmpSettings)
	{
		_CmpAttribute = cmpAttribute;
		_CmpElements = cmpElements;
		_CmpGeometry = cmpGeometry;
		_CmpParameters = cmpParameters;
		_CmpSettings = cmpSettings;
		_EntDtItems = new DtItems(_CmpAttribute);
		_ErrMsg = "";
	}

	public List<Element> GetElements(View view, eFloorType eFloorType)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		List<Element> list = new List<Element>();
		ElementCategoryFilter val = null;
		val = ((eFloorType != 0) ? new ElementCategoryFilter((BuiltInCategory)(-2001320)) : new ElementCategoryFilter((BuiltInCategory)(-2000011)));
		FilteredElementCollector val2 = new FilteredElementCollector(((Element)view).Document, ((Element)view).Id).WhereElementIsNotElementType().WherePasses((ElementFilter)(object)val);
		if (val2.ToElements().Count > 0)
		{
			foreach (Element item in val2.ToElements())
			{
				if (eFloorType == eFloorType.Arch)
				{
					int value = 0;
					if (_CmpParameters.GetValue(item, (BuiltInParameter)(-1001007), ref value) == 0 && value == 1)
					{
						list.Add(item);
					}
					continue;
				}
				int value2 = 0;
				_CmpParameters.GetValue(item, (BuiltInParameter)(-1001381), ref value2);
				_ = -1;
				switch (value2)
				{
				case 3:
					list.Add(item);
					break;
				case 4:
					list.Add(item);
					break;
				}
			}
		}
		return list;
	}

	public bool GetData(DtSlabType entDtSlabType, List<Element> elementList, eFloorType eFloorType, out Level level, out FloorType floorType, out IList<IList<Curve>> floorsCurves, out Dictionary<int, List<int>> dic_bounds)
	{
		_ErrMsg = "";
		floorsCurves = null;
		dic_bounds = null;
		level = null;
		floorType = null;
		Element workElem = entDtSlabType.WorkElem;
		if (workElem != null)
		{
			floorType = (FloorType)(object)((workElem is FloorType) ? workElem : null);
		}
		if (floorType == null)
		{
			_ErrMsg = _CmpAttribute.ResourceText("IDS_ERR_SLABTYPE");
			return false;
		}
		if (elementList.Count == 0)
		{
			_ErrMsg = _CmpAttribute.ResourceText("IDS_ERR_BEAMS");
			return false;
		}
		level = workElem.Document.ActiveView.GenLevel;
		Element val = elementList[0];
		if (eFloorType == eFloorType.Arch)
		{
			if (level == null && val.LevelId != ElementId.InvalidElementId)
			{
				Element element = _CmpElements.RvtDBDoc.GetElement(val.LevelId);
				level = (Level)(object)((element is Level) ? element : null);
			}
		}
		else
		{
			Parameter val2 = val[(BuiltInParameter)(-1001383)];
			if (val2 != null)
			{
				Element elementDoc = _CmpElements.GetElementDoc(val2.AsElementId().IntegerValue);
				if (elementDoc != null)
				{
					level = (Level)(object)((elementDoc is Level) ? elementDoc : null);
				}
			}
		}
		if (level == null)
		{
			_ErrMsg = _CmpAttribute.ResourceText("IDS_ERR_LEVELBEAMS");
			return false;
		}
		double elevation = level.Elevation;
		IList<Curve> list = new List<Curve>();
		foreach (Element element2 in elementList)
		{
			Curve elementLocCurve = _CmpGeometry.GetElementLocCurve(element2);
			list.Add(elementLocCurve);
		}
		if (list.Count == 0)
		{
			_ErrMsg = _CmpAttribute.ResourceText("IDS_ERR_CURVBEAMS");
			return false;
		}
		_CmpGeometry.ToleranceInter = _EntDtItems.ToleranceInter / _CmpGeometry.UnitCoe;
		IList<IList<XYZ>> interPosCurves = _CmpGeometry.GetInterPosCurves(list);
		_CmpGeometry.GetPlanFaceCurveInterPos(interPosCurves, elevation, out floorsCurves, out dic_bounds);
		return true;
	}

	public Floor CreateFloor(IList<IList<Curve>> floorsCurvesTmp, Dictionary<int, List<int>> dic_indexs, Level level, eFloorType eFloorType, FloorType floorType, XYZ pickPoint)
	{
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		foreach (KeyValuePair<int, List<int>> dic_index in dic_indexs)
		{
			int key = dic_index.Key;
			IList<Curve> list = floorsCurvesTmp[key];
			CurveLoop val = new CurveLoop();
			List<XYZ> list2 = new List<XYZ>();
			for (int i = 0; i < list.Count; i++)
			{
				Curve val2 = list[i];
				val.Append(val2);
				list2.AddRange(val2.Tessellate());
			}
			if (!_CmpGeometry.isPointInPolyline(list2, pickPoint))
			{
				continue;
			}
			List<CurveLoop> list3 = new List<CurveLoop> { val };
			Floor val3 = Floor.Create(_CmpElements.RvtDBDoc, (IList<CurveLoop>)list3, ((Element)floorType).Id, ((Element)level).Id);
			if (dic_index.Value.Count != 0 && val3 != null)
			{
				_CmpElements.RvtDBDoc.Regenerate();
				foreach (int item in dic_index.Value)
				{
					IList<Curve> list4 = floorsCurvesTmp[item];
					CurveArray val4 = new CurveArray();
					for (int j = 0; j < list4.Count; j++)
					{
						Curve val5 = list4[j];
						val4.Append(val5);
					}
					try
					{
						_CmpElements.RvtDBDoc.Create.NewOpening((Element)(object)val3, val4, false);
					}
					catch (Exception ex)
					{
						_ = ex.Message;
					}
				}
			}
			return val3;
		}
		return null;
	}
}
