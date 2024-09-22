using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ADSK.JExtCom.Dnf;
using ADSK.JExtRAC.AutomaticFloor.Components;
using ADSK.JExtRAC.AutomaticFloor.Config;
using ADSK.JExtRAC.AutomaticFloor.Entities;
using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace ADSK.JExtRAC.AutomaticFloor.Utils;

public class FloorCreator
{
	public Result CreateFloor(ExternalCommandData cmdData, eFloorType efloorType)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b8: Invalid comparison between Unknown and I4
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Invalid comparison between Unknown and I4
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Invalid comparison between Unknown and I4
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Invalid comparison between Unknown and I4
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Invalid comparison between Unknown and I4
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		UIDocument activeUIDocument = cmdData.Application.ActiveUIDocument;
		ADSK.JExtRAC.AutomaticFloor.Components.Attribute attribute = new ADSK.JExtRAC.AutomaticFloor.Components.Attribute();
		Elements elements = new Elements(activeUIDocument);
		Geometry geometry = new Geometry(activeUIDocument);
		Parameters parameters = new Parameters(attribute, activeUIDocument);
		Settings cmpSettings = new Settings(activeUIDocument);
		Service service = new Service(attribute, elements, geometry, parameters, cmpSettings);
		Result result = (Result)1;
		TransactionGroup val = new TransactionGroup(elements.RvtDBDoc);
		val.Start("CreateFloor");
		Transaction val2 = new Transaction(elements.RvtDBDoc);
		try
		{
			if ((int)activeUIDocument.ActiveView.ViewType != 1 && (int)activeUIDocument.ActiveView.ViewType != 2 && (int)activeUIDocument.ActiveView.ViewType != 116 && (int)activeUIDocument.ActiveView.ViewType != 115)
			{
				MessageBox.Show(attribute.ResourceText("IDS_INFO_ACTIVE_VIEW"), attribute.ResourceText("IDS_TXT_ERROR"));
				parameters.SetSharedParamDefault();
				val.Assimilate();
				return result;
			}
			List<Element> elements2 = service.GetElements(activeUIDocument.ActiveView, efloorType);
			if (elements2.Count == 0)
			{
				if (efloorType == eFloorType.Arch)
				{
					MessageBox.Show(attribute.ResourceText("IDS_INFO_NO_EXIST_WALLS"), attribute.ResourceText("IDS_TXT_ERROR"));
				}
				else
				{
					MessageBox.Show(attribute.ResourceText("IDS_INFO_NO_EXIST_BEAMS"), attribute.ResourceText("IDS_TXT_ERROR"));
				}
				parameters.SetSharedParamDefault();
				val.Assimilate();
				return result;
			}
			IList<Element> floorTypes = elements.GetFloorTypes(efloorType);
			if (floorTypes.Count == 0)
			{
				parameters.SetSharedParamDefault();
				val.Assimilate();
				return result;
			}
			val2.Start("SetCommand");
			DtSlabType dtSlabType = new DtSlabType(attribute, elements, geometry, parameters, cmpSettings);
			if (dtSlabType.ErrMsg != "")
			{
				MessageBox.Show(dtSlabType.ErrMsg, attribute.ResourceText("IDS_TXT_ERROR"));
				val2.RollBack();
				parameters.SetSharedParamDefault();
				val.Assimilate();
				return result;
			}
			dtSlabType.GetData(floorTypes);
			ProjectInfo projectInfo = elements.ProjectInfo;
			DtCmd dtCmd = new DtCmd(attribute, elements, geometry, parameters, cmpSettings, projectInfo, attribute.ResourceText("IDS_SHPARAM_DEF_CMD_LOCATESLAB"), 4);
			if (dtCmd.ErrMsg != "")
			{
				MessageBox.Show(dtCmd.ErrMsg, attribute.ResourceText("IDS_TXT_ERROR"));
				val2.RollBack();
				parameters.SetSharedParamDefault();
				val.Assimilate();
				return result;
			}
			val2.Commit();
			FormConfig formConfig = new FormConfig(attribute, dtSlabType, dtCmd, efloorType);
			formConfig.ShowDialog();
			if (formConfig.DialogResult != DialogResult.OK)
			{
				parameters.SetSharedParamDefault();
				val.Assimilate();
				return result;
			}
			Level level = null;
			FloorType floorType = null;
			IList<IList<Curve>> floorsCurves = null;
			Dictionary<int, List<int>> dic_bounds = null;
			List<ElementId> list = new List<ElementId>();
			bool flag = false;
			while (!flag)
			{
				if (activeUIDocument.Document.ActiveView.SketchPlane == null)
				{
					val2.Start("Temporarily set work plane");
					Plane val3 = Plane.CreateByNormalAndOrigin(activeUIDocument.Document.ActiveView.ViewDirection, activeUIDocument.Document.ActiveView.Origin);
					SketchPlane sketchPlane = SketchPlane.Create(activeUIDocument.Document, val3);
					activeUIDocument.Document.ActiveView.SketchPlane = sketchPlane;
					activeUIDocument.Document.ActiveView.ShowActiveWorkPlane();
				}
				Selection selection = cmdData.Application.ActiveUIDocument.Selection;
				XYZ pickPoint;
				try
				{
					pickPoint = selection.PickPoint((ObjectSnapTypes)0, attribute.ResourceText("IDS_INFO_PICK_POINT"));
				}
				catch (Exception ex)
				{
					_ = ex.Message;
					flag = true;
					break;
				}
				finally
				{
					if (val2.HasStarted())
					{
						val2.RollBack();
					}
				}
				if ((floorsCurves == null || level == null || floorType == null) && !service.GetData(dtSlabType, elements2, efloorType, out level, out floorType, out floorsCurves, out dic_bounds))
				{
					continue;
				}
				val2.Start("CreateFloor");
				Floor val4 = service.CreateFloor(floorsCurves, dic_bounds, level, efloorType, floorType, pickPoint);
				if (val4 == null)
				{
					MessageBox.Show(attribute.ResourceText("IDS_TXT_NO_CREATE_FLOOR"), attribute.ResourceText("IDS_TXT_ERROR"));
					val2.RollBack();
					continue;
				}
				val2.Commit();
				if (dtCmd.Data[2] == "true")
				{
					val2.Start("Lock");
					LockFloor(cmdData.Application.ActiveUIDocument.Document, val4, elements2);
					val2.Commit();
				}
				val2.Start("FloorParam");
				if (efloorType == eFloorType.Arch)
				{
					int value = 0;
					parameters.SetValue((Element)(object)val4, (BuiltInParameter)(-1001954), value);
				}
				else
				{
					int value2 = 1;
					parameters.SetValue((Element)(object)val4, (BuiltInParameter)(-1001954), value2);
				}
				val2.Commit();
				val2.Start("SetHeight");
				double num = 0.0;
				if (UtilValue.IsNumber(dtCmd.Data[0]))
				{
					num = double.Parse(dtCmd.Data[0]);
				}
				num /= geometry.UnitCoe;
				if (val4 != null)
				{
					parameters.SetValue((Element)(object)val4, (BuiltInParameter)(-1001951), num);
				}
				val2.Commit();
				val2.Start("SetValueSlabDirectionAngle");
				double num2 = 0.0;
				if (dtCmd.DegreeAngle != 0.0)
				{
					num2 = dtCmd.DegreeAngle;
				}
				num2 *= Math.PI / 180.0;
				if (val4 != null)
				{
					val4.SpanDirectionAngle = num2;
				}
				val2.Commit();
				val2.Start("SetParamValue");
				dtCmd.SetData();
				val2.Commit();
				list.Add(((Element)val4).Id);
			}
			if (list.Count == 0)
			{
				result = (Result)1;
			}
			else
			{
				cmdData.Application.ActiveUIDocument.Selection.SetElementIds((ICollection<ElementId>)list);
				result = (Result)0;
			}
		}
		catch (Exception ex2)
		{
			_ = ex2.Message;
			MessageBox.Show(attribute.ResourceText("IDS_ERR_COMMAND"), attribute.ResourceText("IDS_TXT_ERROR"));
			if ((int)val2.GetStatus() != 3)
			{
				val2.RollBack();
			}
		}
		parameters.SetSharedParamDefault();
		val.Assimilate();
		return result;
	}

	private List<ModelCurve> GetModelCurveOfFloor(Document doc, Floor floor)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		List<ModelCurve> list = new List<ModelCurve>();
		ICollection<ElementId> collection = null;
		SubTransaction val = new SubTransaction(doc);
		try
		{
			if ((int)val.Start() == 1)
			{
				collection = doc.Delete(((Element)floor).Id);
				val.RollBack();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		foreach (ElementId item in collection)
		{
			Element element = doc.GetElement(item);
			ModelCurve val2 = (ModelCurve)(object)((element is ModelCurve) ? element : null);
			if (val2 != null)
			{
				list.Add(val2);
			}
		}
		return list;
	}

	private Dictionary<ModelCurve, List<Element>> DetectMatchedElements(Document doc, Floor floor, List<Element> elements)
	{
		List<ModelCurve> modelCurveOfFloor = GetModelCurveOfFloor(doc, floor);
		if (modelCurveOfFloor == null || modelCurveOfFloor.Count == 0)
		{
			return null;
		}
		Dictionary<ModelCurve, List<Element>> dictionary = new Dictionary<ModelCurve, List<Element>>();
		foreach (ModelCurve item in modelCurveOfFloor)
		{
			foreach (Element element in elements)
			{
				Location location = element.Location;
				Curve curve = ((LocationCurve)((location is LocationCurve) ? location : null)).Curve;
				bool flag = false;
				if (IsSameCurve(((CurveElement)item).GeometryCurve, curve))
				{
					flag = true;
				}
				else if (IsSameCurve2D(((CurveElement)item).GeometryCurve, curve))
				{
					flag = true;
				}
				if (flag)
				{
					if (!dictionary.ContainsKey(item))
					{
						dictionary.Add(item, new List<Element>());
					}
					dictionary[item].Add(element);
				}
			}
		}
		return dictionary;
	}

	private bool IsSameCurve2D(Curve curve1, Curve curve2)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0096: Expected O, but got Unknown
		XYZ endPoint = curve1.GetEndPoint(0);
		XYZ endPoint2 = curve1.GetEndPoint(1);
		Line val = Line.CreateBound(new XYZ(endPoint.X, endPoint.Y, 0.0), new XYZ(endPoint2.X, endPoint2.Y, 0.0));
		XYZ endPoint3 = curve2.GetEndPoint(0);
		XYZ endPoint4 = curve2.GetEndPoint(1);
		Line obj = Line.CreateBound(new XYZ(endPoint3.X, endPoint3.Y, 0.0), new XYZ(endPoint4.X, endPoint4.Y, 0.0));
		IntersectionResult val2 = ((Curve)obj).Project(endPoint);
		IntersectionResult val3 = ((Curve)obj).Project(endPoint2);
		if (val2 != null && val3 != null && val2.Distance < 1E-06 && val3.Distance < 1E-06)
		{
			return true;
		}
		val2 = ((Curve)val).Project(endPoint3);
		val3 = ((Curve)val).Project(endPoint4);
		if (val2 != null && val3 != null && val2.Distance < 1E-06 && val3.Distance < 1E-06)
		{
			return true;
		}
		return false;
	}

	private bool IsSameCurve(Curve curve1, Curve curve2)
	{
		if (!IsNearlyEqual(curve1.GetEndPoint(0), curve2.GetEndPoint(0)) || !IsNearlyEqual(curve1.GetEndPoint(1), curve2.GetEndPoint(1)))
		{
			if (IsNearlyEqual(curve1.GetEndPoint(0), curve2.GetEndPoint(1)))
			{
				return IsNearlyEqual(curve1.GetEndPoint(1), curve2.GetEndPoint(0));
			}
			return false;
		}
		return true;
	}

	private bool IsNearlyEqual(XYZ pos1, XYZ pos2)
	{
		if (IsNearlyEqual(pos1.X, pos2.X) && IsNearlyEqual(pos1.Y, pos2.Y))
		{
			return IsNearlyEqual(pos1.Z, pos2.Z);
		}
		return false;
	}

	private bool IsNearlyEqual(double val1, double val2, int digit = 3)
	{
		int num = (int)(val1 * Math.Pow(10.0, digit));
		int num2 = (int)(val2 * Math.Pow(10.0, digit));
		return num == num2;
	}

	private void LockFloor(Document doc, Floor floor, List<Element> elements)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		Dictionary<ModelCurve, List<Element>> dictionary = DetectMatchedElements(doc, floor, elements);
		if (dictionary == null || dictionary.Count == 0)
		{
			return;
		}
		foreach (KeyValuePair<ModelCurve, List<Element>> item in dictionary)
		{
			List<Element> value = item.Value;
			ModelCurve key = item.Key;
			try
			{
				foreach (Element item2 in value)
				{
					if (item2 is FamilyInstance)
					{
						IList<Reference> references = ((FamilyInstance)((item2 is FamilyInstance) ? item2 : null)).GetReferences((FamilyInstanceReferenceType)4);
						if (references.Count != 0)
						{
							try
							{
								((ItemFactoryBase)doc.Create).NewAlignment(doc.ActiveView, references[0], new Reference((Element)(object)key));
							}
							catch (Exception ex)
							{
								_ = ex.Message;
							}
						}
					}
					else
					{
						try
						{
							((ItemFactoryBase)doc.Create).NewAlignment(doc.ActiveView, new Reference(item2), new Reference((Element)(object)key));
						}
						catch (Exception ex2)
						{
							_ = ex2.Message;
						}
					}
				}
			}
			catch (Exception ex3)
			{
				_ = ex3.Message;
			}
		}
	}

	private static XYZ MiddlePoint(Curve curve)
	{
		double endParameter = curve.GetEndParameter(0);
		double endParameter2 = curve.GetEndParameter(1);
		return curve.Evaluate(endParameter + (endParameter2 - endParameter) / 2.0, false);
	}
}
