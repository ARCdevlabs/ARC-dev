using System;
using System.Collections.Generic;
using ADSK.JExtCom.Rvt;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ADSK.JExtRAC.AutomaticFloor.Components;

public class Geometry : ADSK.JExtCom.Rvt.Geometry
{
	private double _ToleranceInter;

	public double ToleranceInter
	{
		get
		{
			return _ToleranceInter;
		}
		set
		{
			_ToleranceInter = value;
		}
	}

	public Geometry(UIDocument rvtUIDoc)
		: base(rvtUIDoc)
	{
		_ToleranceInter = 0.0;
	}

	public bool GetCurveOnPos2D(Curve curve, XYZ pos)
	{
		bool flag = false;
		XYZ endPoint = curve.GetEndPoint(0);
		XYZ endPoint2 = curve.GetEndPoint(1);
		if (!flag)
		{
			if (Distance2D(endPoint, pos) < base.Approx0Len)
			{
				flag = true;
			}
			else if (Distance2D(endPoint2, pos) < base.Approx0Len)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			double num = Math.Abs(Angle2D(endPoint, endPoint2, pos));
			double num2 = Math.Abs(Angle2D(endPoint2, endPoint, pos));
			if (num < base.Approx0Ang && num2 < base.Approx0Ang)
			{
				flag = true;
			}
		}
		return flag;
	}

	public IList<XYZ> GetInterPosCurves2D(Curve curve1, Curve curve2)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		IList<XYZ> list = new List<XYZ>();
		XYZ endPoint = curve1.GetEndPoint(0);
		XYZ endPoint2 = curve1.GetEndPoint(1);
		XYZ endPoint3 = curve2.GetEndPoint(0);
		XYZ endPoint4 = curve2.GetEndPoint(1);
		XYZ val = null;
		bool flag = false;
		XYZ val2 = UnitVector(endPoint, endPoint2);
		XYZ pos = UnitVector(endPoint3, endPoint4);
		XYZ pos2 = new XYZ(val2.X * -1.0, val2.Y * -1.0, 0.0);
		if (Distance2D(val2, pos) < base.Approx0Len)
		{
			flag = true;
		}
		if (Distance2D(pos2, pos) < base.Approx0Len)
		{
			flag = true;
		}
		if (flag)
		{
			val = null;
			if (Distance2D(endPoint, endPoint3) < base.Approx0Len)
			{
				val = endPoint;
			}
			else if (Distance2D(endPoint, endPoint4) < base.Approx0Len)
			{
				val = endPoint;
			}
			else if (Distance2D(endPoint2, endPoint3) < base.Approx0Len)
			{
				val = endPoint2;
			}
			else if (Distance2D(endPoint2, endPoint4) < base.Approx0Len)
			{
				val = endPoint2;
			}
			if (val != null)
			{
				list.Add(val);
			}
		}
		else
		{
			IList<XYZ> interPosAry = new List<XYZ>();
			IntersecCurve2D(curve1, curve2, ref interPosAry);
			for (int i = 0; i < interPosAry.Count; i++)
			{
				XYZ val3 = new XYZ(interPosAry[i].X, interPosAry[i].Y, curve1.GetEndPoint(0).Z);
				bool flag2 = true;
				if (val != null && Distance2D(val, val3) < ToleranceInter)
				{
					flag2 = false;
				}
				if (flag2)
				{
					list.Add(val3);
				}
			}
		}
		return list;
	}

	public IList<IList<XYZ>> GetInterPosCurves(IList<Curve> curveAry)
	{
		IList<IList<XYZ>> list = new List<IList<XYZ>>();
		for (int i = 0; i < curveAry.Count; i++)
		{
			Curve curve = curveAry[i];
			IList<XYZ> list2 = new List<XYZ>();
			for (int j = 0; j < curveAry.Count; j++)
			{
				if (i != j)
				{
					Curve curve2 = curveAry[j];
					IList<XYZ> interPosCurves2D = GetInterPosCurves2D(curve, curve2);
					for (int k = 0; k < interPosCurves2D.Count; k++)
					{
						list2.Add(interPosCurves2D[k]);
					}
				}
			}
			IList<int> sortedIndex = new List<int>();
			IList<XYZ> sortedPosAry = new List<XYZ>();
			SortXYPos(list2, 1, ref sortedIndex, ref sortedPosAry);
			IList<XYZ> list3 = new List<XYZ>();
			for (int l = 0; l < sortedPosAry.Count; l++)
			{
				bool flag = false;
				for (int m = 0; m < list3.Count; m++)
				{
					if (Distance2D(sortedPosAry[l], list3[m]) < base.Approx0Len)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list3.Add(sortedPosAry[l]);
				}
			}
			list.Add(list3);
		}
		return list;
	}

	public int GetPosReClockwise(XYZ basePos, XYZ compPos, IList<XYZ> posAry, bool isComp)
	{
		int result = -1;
		XYZ val = null;
		double num = 0.0;
		IList<int> list = null;
		if (basePos == null)
		{
			return result;
		}
		if (posAry == null)
		{
			return result;
		}
		if (compPos == null)
		{
			return result;
		}
		int num2 = -1;
		list = new List<int>();
		for (int i = 0; i < posAry.Count; i++)
		{
			val = posAry[i];
			if (!(Distance2D(basePos, val) < base.Approx0Len) && !(Distance2D(compPos, val) < base.Approx0Len))
			{
				num = Angle2D(basePos, compPos, val);
				if (Math.Abs(Math.PI - Math.Abs(num)) < base.Approx0Ang)
				{
					num2 = i;
				}
				else if (Math.Abs(num) > base.Approx0Ang && num < 0.0)
				{
					list.Add(i);
				}
			}
		}
		double num3 = 0.0;
		int num4 = -1;
		for (int j = 0; j < list.Count; j++)
		{
			int num5 = list[j];
			num = Math.Abs(Angle2D(basePos, compPos, posAry[num5]));
			if (j == 0)
			{
				num3 = num;
				num4 = num5;
			}
			else if (num < num3)
			{
				num3 = num;
				num4 = num5;
			}
		}
		result = num4;
		if (result == -1 && isComp)
		{
			int num6 = -1;
			for (int k = 0; k < posAry.Count; k++)
			{
				val = posAry[k];
				if (!(Distance2D(basePos, val) < base.Approx0Len) && Distance2D(compPos, val) < base.Approx0Len)
				{
					num6 = k;
					break;
				}
			}
			if (num6 > -1)
			{
				bool flag = false;
				for (int l = 0; l < posAry.Count; l++)
				{
					if (l == num6)
					{
						continue;
					}
					val = posAry[l];
					if (!(Distance2D(basePos, val) < base.Approx0Len))
					{
						num = Angle2D(basePos, val, compPos);
						if (Math.Abs(Math.PI - Math.Abs(num)) < base.Approx0Ang)
						{
							flag = true;
							break;
						}
						if (Math.Abs(num) > base.Approx0Ang && num < 0.0)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					result = num6;
				}
			}
		}
		if (result == -1)
		{
			if (num2 > -1)
			{
				result = num2;
			}
			else if (posAry.Count == 1)
			{
				result = 0;
			}
		}
		return result;
	}

	public IList<XYZ> GetRelatedPos(XYZ basePos, XYZ exclPos, IList<IList<XYZ>> curveInterPosAryAry)
	{
		IList<XYZ> list = new List<XYZ>();
		IList<XYZ> list2 = new List<XYZ>();
		for (int i = 0; i < curveInterPosAryAry.Count; i++)
		{
			for (int j = 0; j < curveInterPosAryAry[i].Count; j++)
			{
				if (Distance2D(basePos, curveInterPosAryAry[i][j]) < base.Approx0Len)
				{
					int num = j + 1;
					if (num < curveInterPosAryAry[i].Count)
					{
						list2.Add(curveInterPosAryAry[i][num]);
					}
					num = j - 1;
					if (num > -1)
					{
						list2.Add(curveInterPosAryAry[i][num]);
					}
				}
			}
		}
		if (list2.Count > 0)
		{
			for (int k = 0; k < list2.Count; k++)
			{
				if (exclPos == null)
				{
					list.Add(list2[k]);
				}
				else if (Distance2D(exclPos, list2[k]) > base.Approx0Len)
				{
					list.Add(list2[k]);
				}
			}
		}
		return list;
	}

	public IList<Curve> GetPlanFaceCurveInterPos(XYZ basePos, XYZ relaPos, IList<IList<XYZ>> curveInterPosAryAry, double height)
	{
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		IList<Curve> result = new List<Curve>();
		XYZ val = basePos;
		XYZ val2 = null;
		XYZ val3 = null;
		int num = -1;
		int num2 = 1000;
		bool flag = false;
		IList<XYZ> list = new List<XYZ>();
		list.Add(basePos);
		IList<XYZ> relatedPos = GetRelatedPos(basePos, null, curveInterPosAryAry);
		int posReClockwise = GetPosReClockwise(basePos, relaPos, relatedPos, isComp: true);
		if (posReClockwise == -1)
		{
			return result;
		}
		val3 = relatedPos[posReClockwise];
		list.Add(val3);
		val = val3;
		val2 = basePos;
		while (!flag)
		{
			num++;
			if (num > num2)
			{
				break;
			}
			val3 = null;
			relatedPos = GetRelatedPos(val, val2, curveInterPosAryAry);
			posReClockwise = GetPosReClockwise(val, val2, relatedPos, isComp: false);
			if (posReClockwise > -1)
			{
				val3 = relatedPos[posReClockwise];
			}
			if (val3 == null)
			{
				break;
			}
			list.Add(val3);
			val2 = val;
			val = val3;
			if (Distance2D(basePos, val3) < base.Approx0Len)
			{
				flag = true;
			}
		}
		if (list.Count < 3)
		{
			return result;
		}
		if (Distance2D(list[0], list[list.Count - 1]) > base.Approx0Len)
		{
			return result;
		}
		bool flag2 = true;
		IList<Curve> list2 = new List<Curve>();
		for (int i = 1; i < list.Count; i++)
		{
			XYZ val4 = new XYZ(list[i - 1].X, list[i - 1].Y, height);
			XYZ val5 = new XYZ(list[i].X, list[i].Y, height);
			if (Distance2D(val4, val5) < base.Approx0Len)
			{
				flag2 = false;
				break;
			}
			Curve val6 = null;
			try
			{
				val6 = (Curve)(object)Line.CreateBound(val4, val5);
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				continue;
			}
			bool flag3 = true;
			for (int j = 0; j < list2.Count; j++)
			{
				IList<XYZ> interPosAry = new List<XYZ>();
				IntersecCurve2D(val6, list2[j], ref interPosAry);
				for (int k = 0; k < interPosAry.Count; k++)
				{
					if (Distance2D(val4, interPosAry[k]) > base.Approx0Len && Distance2D(val5, interPosAry[k]) > base.Approx0Len)
					{
						flag3 = false;
						break;
					}
				}
				if (!flag3)
				{
					break;
				}
			}
			if (flag3)
			{
				list2.Add(val6);
				continue;
			}
			flag2 = false;
			break;
		}
		if (flag2)
		{
			result = list2;
		}
		return result;
	}

	public bool GetPlanFaceCurveInterPos(IList<IList<XYZ>> curveInterPosAryAry, double height, out IList<IList<Curve>> ret, out Dictionary<int, List<int>> dic_indexs)
	{
		ret = new List<IList<Curve>>();
		List<List<Curve>> list = new List<List<Curve>>();
		IList<XYZ> list2 = new List<XYZ>();
		for (int i = 0; i < curveInterPosAryAry.Count; i++)
		{
			IList<XYZ> list3 = curveInterPosAryAry[i];
			for (int j = 0; j < list3.Count; j++)
			{
				XYZ basePos = list3[j];
				int num = j + 1;
				if (num > list3.Count - 1)
				{
					num = j - 1;
				}
				if (num < 0)
				{
					continue;
				}
				XYZ relaPos = list3[num];
				List<Curve> list4 = GetPlanFaceCurveInterPos(basePos, relaPos, curveInterPosAryAry, height) as List<Curve>;
				if (list4.Count <= 0)
				{
					continue;
				}
				XYZ val = PolygonGravity2D(list4);
				if (val == null)
				{
					continue;
				}
				bool flag = true;
				for (int k = 0; k < list.Count; k++)
				{
					if (Distance2D(val, list2[k]) < base.Approx0Len)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list.Add(list4);
					list2.Add(val);
				}
			}
		}
		list.Sort(delegate(List<Curve> pFaceCurveAry1, List<Curve> pFaceCurveAry2)
		{
			double num3 = Math.Abs(GetPolygonArea(pFaceCurveAry1));
			double value = Math.Abs(GetPolygonArea(pFaceCurveAry2));
			return num3.CompareTo(value);
		});
		dic_indexs = new Dictionary<int, List<int>>();
		List<int> list5 = new List<int>();
		for (int l = 0; l < list.Count; l++)
		{
			if (!dic_indexs.ContainsKey(l))
			{
				dic_indexs.Add(l, new List<int>());
			}
			List<Curve> list6 = list[l];
			List<XYZ> list7 = new List<XYZ>();
			foreach (Curve item in (IEnumerable<Curve>)list6)
			{
				list7.AddRange(item.Tessellate());
			}
			for (int m = 0; m < list.Count; m++)
			{
				if (l == m)
				{
					continue;
				}
				List<Curve> list8 = list[m];
				List<XYZ> list9 = new List<XYZ>();
				foreach (Curve item2 in (IEnumerable<Curve>)list8)
				{
					list9.AddRange(item2.Tessellate());
				}
				int num2 = 0;
				foreach (XYZ item3 in list9)
				{
					if (isPointInPolyline(list7, item3))
					{
						num2++;
					}
				}
				if (num2 == list9.Count)
				{
					dic_indexs[l].Add(m);
				}
			}
		}
		foreach (KeyValuePair<int, List<int>> dic_index in dic_indexs)
		{
			if (!list5.Contains(dic_index.Key))
			{
				list5.Add(dic_index.Key);
			}
			foreach (int item4 in dic_index.Value)
			{
				if (item4 != dic_index.Key && !list5.Contains(item4))
				{
					list5.Add(item4);
				}
			}
		}
		foreach (int item5 in list5)
		{
			ret.Add(list[item5]);
		}
		return true;
	}

	public bool isPointInPolyline(List<XYZ> polyline, XYZ pt)
	{
		int count = polyline.Count;
		double num = 0.0;
		for (int i = 0; i < count; i++)
		{
			double x = polyline[i].X - pt.X;
			double y = polyline[i].Y - pt.Y;
			double x2 = polyline[(i + 1) % count].X - pt.X;
			double y2 = polyline[(i + 1) % count].Y - pt.Y;
			num += Angle2D(x, y, x2, y2);
		}
		if (Math.Abs(num) < Math.PI)
		{
			return false;
		}
		return true;
	}

	private double Angle2D(double x1, double y1, double x2, double y2)
	{
		double num = Math.Atan2(y1, x1);
		double num2;
		for (num2 = Math.Atan2(y2, x2) - num; num2 > Math.PI; num2 -= Math.PI * 2.0)
		{
		}
		for (; num2 < -Math.PI; num2 += Math.PI * 2.0)
		{
		}
		return num2;
	}
}
