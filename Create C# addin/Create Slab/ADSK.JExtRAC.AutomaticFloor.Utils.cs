using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADSK.JExtRAC.AutomaticFloor.Utils
{
    // Token: 0x02000002 RID: 2
    public class Common
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public static void NumberCheck(object sender, KeyPressEventArgs e, bool allowNegativeValue = false)
        {
            if (!allowNegativeValue)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                {
                    e.Handled = true;
                }
                if (sender is System.Windows.Forms.TextBox && e.KeyChar == '.' && (sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1)
                {
                    e.Handled = true;
                }
                if (sender is System.Windows.Forms.ComboBox && e.KeyChar == '.' && (sender as System.Windows.Forms.ComboBox).Text.IndexOf('.') > -1)
                {
                    e.Handled = true;
                    return;
                }
            }
            else
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-')
                {
                    e.Handled = true;
                }
                if (sender is System.Windows.Forms.TextBox)
                {
                    if (e.KeyChar == '.' && (sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1)
                    {
                        e.Handled = true;
                    }
                    if (e.KeyChar == '-' && (sender as System.Windows.Forms.TextBox).Text.IndexOf('-') > -1)
                    {
                        e.Handled = true;
                    }
                }
                if (sender is System.Windows.Forms.ComboBox)
                {
                    if (e.KeyChar == '.' && (sender as System.Windows.Forms.ComboBox).Text.IndexOf('.') > -1)
                    {
                        e.Handled = true;
                    }
                    if (e.KeyChar == '-' && (sender as System.Windows.Forms.ComboBox).Text.IndexOf('-') > -1)
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        // Token: 0x06000002 RID: 2 RVA: 0x000021D1 File Offset: 0x000003D1
        public Common()
        {
        }
    }


    // Token: 0x02000003 RID: 3
    public enum eFloorType
    {
        // Token: 0x04000002 RID: 2
        Arch,
        // Token: 0x04000003 RID: 3
        Struct,
        // Token: 0x04000004 RID: 4
        Slab
    }
    // Token: 0x02000004 RID: 4

    public class FloorCreator
    {
        // Token: 0x06000003 RID: 3 RVA: 0x000021DC File Offset: 0x000003DC
        public Result CreateFloor(ExternalCommandData cmdData, eFloorType efloorType)
        {
            UIDocument activeUIDocument = cmdData.Application.ActiveUIDocument;
            ADSK.JExtRAC.AutomaticFloor.Components.Attribute attribute = new ADSK.JExtRAC.AutomaticFloor.Components.Attribute();
            Elements elements = new Autodesk.Revit.DB.Elements(activeUIDocument);
            Geometry geometry = new Geometry(activeUIDocument);
            Parameters parameters = new Parameters(attribute, activeUIDocument);
            ADSK.JExtRAC.AutomaticFloor.Components.Settings cmpSettings = new ADSK.JExtRAC.AutomaticFloor.Components.Settings(activeUIDocument);
            Service service = new Service(attribute, elements, geometry, parameters, cmpSettings);
            Result result = Result.Cancelled;
            TransactionGroup transactionGroup = new TransactionGroup(elements.RvtDBDoc);
            transactionGroup.Start("CreateFloor");
            Transaction transaction = new Transaction(elements.RvtDBDoc);
            try
            {
                if (activeUIDocument.ActiveView.ViewType != ViewType.FloorPlan && activeUIDocument.ActiveView.ViewType != ViewType.CeilingPlan && activeUIDocument.ActiveView.ViewType != ViewType.AreaPlan && activeUIDocument.ActiveView.ViewType != ViewType.EngineeringPlan)
                {
                    MessageBox.Show(attribute.ResourceText("IDS_INFO_ACTIVE_VIEW"), attribute.ResourceText("IDS_TXT_ERROR"));
                    parameters.SetSharedParamDefault();
                    transactionGroup.Assimilate();
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
                    transactionGroup.Assimilate();
                    return result;
                }
                IList<Element> floorTypes = elements.GetFloorTypes(efloorType);
                if (floorTypes.Count == 0)
                {
                    parameters.SetSharedParamDefault();
                    transactionGroup.Assimilate();
                    return result;
                }
                transaction.Start("SetCommand");
                DtSlabType dtSlabType = new DtSlabType(attribute, elements, geometry, parameters, cmpSettings);
                if (dtSlabType.ErrMsg != "")
                {
                    MessageBox.Show(dtSlabType.ErrMsg, attribute.ResourceText("IDS_TXT_ERROR"));
                    transaction.RollBack();
                    parameters.SetSharedParamDefault();
                    transactionGroup.Assimilate();
                    return result;
                }
                dtSlabType.GetData(floorTypes);
                ProjectInfo projectInfo = elements.ProjectInfo;
                DtCmd dtCmd = new DtCmd(attribute, elements, geometry, parameters, cmpSettings, projectInfo, attribute.ResourceText("IDS_SHPARAM_DEF_CMD_LOCATESLAB"), 4);
                if (dtCmd.ErrMsg != "")
                {
                    MessageBox.Show(dtCmd.ErrMsg, attribute.ResourceText("IDS_TXT_ERROR"));
                    transaction.RollBack();
                    parameters.SetSharedParamDefault();
                    transactionGroup.Assimilate();
                    return result;
                }
                transaction.Commit();
                FormConfig formConfig = new FormConfig(attribute, dtSlabType, dtCmd, efloorType);
                formConfig.ShowDialog();
                if (formConfig.DialogResult != DialogResult.OK)
                {
                    parameters.SetSharedParamDefault();
                    transactionGroup.Assimilate();
                    return result;
                }
                Level level = null;
                FloorType floorType = null;
                IList<IList<Curve>> list = null;
                Dictionary<int, List<int>> dic_indexs = null;
                List<ElementId> list2 = new List<ElementId>();
                bool flag = false;
                while (!flag)
                {
                    if (activeUIDocument.Document.ActiveView.SketchPlane == null)
                    {
                        transaction.Start("Temporarily set work plane");
                        Plane plane = Plane.CreateByNormalAndOrigin(activeUIDocument.Document.ActiveView.ViewDirection, activeUIDocument.Document.ActiveView.Origin);
                        SketchPlane sketchPlane = SketchPlane.Create(activeUIDocument.Document, plane);
                        activeUIDocument.Document.ActiveView.SketchPlane = sketchPlane;
                        activeUIDocument.Document.ActiveView.ShowActiveWorkPlane();
                    }
                    Selection selection = cmdData.Application.ActiveUIDocument.Selection;
                    XYZ pickPoint;
                    try
                    {
                        pickPoint = selection.PickPoint(ObjectSnapTypes.None, attribute.ResourceText("IDS_INFO_PICK_POINT"));
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                        flag = true;
                        break;
                    }
                    finally
                    {
                        if (transaction.HasStarted())
                        {
                            transaction.RollBack();
                        }
                    }
                    if ((list != null && level != null && floorType != null) || service.GetData(dtSlabType, elements2, efloorType, out level, out floorType, out list, out dic_indexs))
                    {
                        transaction.Start("CreateFloor");
                        Floor floor = service.CreateFloor(list, dic_indexs, level, efloorType, floorType, pickPoint);
                        if (floor == null)
                        {
                            MessageBox.Show(attribute.ResourceText("IDS_TXT_NO_CREATE_FLOOR"), attribute.ResourceText("IDS_TXT_ERROR"));
                            transaction.RollBack();
                        }
                        else
                        {
                            transaction.Commit();
                            if (dtCmd.Data[2] == "true")
                            {
                                transaction.Start("Lock");
                                this.LockFloor(cmdData.Application.ActiveUIDocument.Document, floor, elements2);
                                transaction.Commit();
                            }
                            transaction.Start("FloorParam");
                            if (efloorType == eFloorType.Arch)
                            {
                                int value = 0;
                                parameters.SetValue(floor, BuiltInParameter.FLOOR_PARAM_IS_STRUCTURAL, value);
                            }
                            else
                            {
                                int value2 = 1;
                                parameters.SetValue(floor, BuiltInParameter.FLOOR_PARAM_IS_STRUCTURAL, value2);
                            }
                            transaction.Commit();
                            transaction.Start("SetHeight");
                            double num = 0.0;
                            if (UtilValue.IsNumber(dtCmd.Data[0]))
                            {
                                num = double.Parse(dtCmd.Data[0]);
                            }
                            num /= geometry.UnitCoe;
                            if (floor != null)
                            {
                                parameters.SetValue(floor, BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM, num);
                            }
                            transaction.Commit();
                            transaction.Start("SetValueSlabDirectionAngle");
                            double num2 = 0.0;
                            if (dtCmd.DegreeAngle != 0.0)
                            {
                                num2 = dtCmd.DegreeAngle;
                            }
                            num2 *= 0.017453292519943295;
                            if (floor != null)
                            {
                                floor.SpanDirectionAngle = num2;
                            }
                            transaction.Commit();
                            transaction.Start("SetParamValue");
                            dtCmd.SetData();
                            transaction.Commit();
                            list2.Add(floor.Id);
                        }
                    }
                }
                if (list2.Count == 0)
                {
                    result = Result.Cancelled;
                }
                else
                {
                    cmdData.Application.ActiveUIDocument.Selection.SetElementIds(list2);
                    result = Result.Succeeded;
                }
            }
            catch (Exception ex2)
            {
                string message2 = ex2.Message;
                MessageBox.Show(attribute.ResourceText("IDS_ERR_COMMAND"), attribute.ResourceText("IDS_TXT_ERROR"));
                if (transaction.GetStatus() != TransactionStatus.Committed)
                {
                    transaction.RollBack();
                }
            }
            parameters.SetSharedParamDefault();
            transactionGroup.Assimilate();
            return result;
        }

        // Token: 0x06000004 RID: 4 RVA: 0x00002810 File Offset: 0x00000A10
        private List<ModelCurve> GetModelCurveOfFloor(Document doc, Floor floor)
        {
            List<ModelCurve> list = new List<ModelCurve>();
            ICollection<ElementId> collection = null;
            using (SubTransaction subTransaction = new SubTransaction(doc))
            {
                if (subTransaction.Start() == TransactionStatus.Started)
                {
                    collection = doc.Delete(floor.Id);
                    subTransaction.RollBack();
                }
            }
            foreach (ElementId id in collection)
            {
                ModelCurve modelCurve = doc.GetElement(id) as ModelCurve;
                if (modelCurve != null)
                {
                    list.Add(modelCurve);
                }
            }
            return list;
        }

        // Token: 0x06000005 RID: 5 RVA: 0x000028B4 File Offset: 0x00000AB4
        private Dictionary<ModelCurve, List<Element>> DetectMatchedElements(Document doc, Floor floor, List<Element> elements)
        {
            List<ModelCurve> modelCurveOfFloor = this.GetModelCurveOfFloor(doc, floor);
            if (modelCurveOfFloor == null || modelCurveOfFloor.Count == 0)
            {
                return null;
            }
            Dictionary<ModelCurve, List<Element>> dictionary = new Dictionary<ModelCurve, List<Element>>();
            foreach (ModelCurve modelCurve in modelCurveOfFloor)
            {
                foreach (Element element in elements)
                {
                    Curve curve = (element.Location as LocationCurve).Curve;
                    bool flag = false;
                    if (this.IsSameCurve(modelCurve.GeometryCurve, curve))
                    {
                        flag = true;
                    }
                    else if (this.IsSameCurve2D(modelCurve.GeometryCurve, curve))
                    {
                        flag = true;
                    }
                    if (flag)
                    {
                        if (!dictionary.ContainsKey(modelCurve))
                        {
                            dictionary.Add(modelCurve, new List<Element>());
                        }
                        dictionary[modelCurve].Add(element);
                    }
                }
            }
            return dictionary;
        }

        // Token: 0x06000006 RID: 6 RVA: 0x000029BC File Offset: 0x00000BBC
        private bool IsSameCurve2D(Curve curve1, Curve curve2)
        {
            XYZ endPoint = curve1.GetEndPoint(0);
            XYZ endPoint2 = curve1.GetEndPoint(1);
            Line line = Line.CreateBound(new XYZ(endPoint.X, endPoint.Y, 0.0), new XYZ(endPoint2.X, endPoint2.Y, 0.0));
            XYZ endPoint3 = curve2.GetEndPoint(0);
            XYZ endPoint4 = curve2.GetEndPoint(1);
            Line line2 = Line.CreateBound(new XYZ(endPoint3.X, endPoint3.Y, 0.0), new XYZ(endPoint4.X, endPoint4.Y, 0.0));
            IntersectionResult intersectionResult = line2.Project(endPoint);
            IntersectionResult intersectionResult2 = line2.Project(endPoint2);
            if (intersectionResult != null && intersectionResult2 != null && intersectionResult.Distance < 1E-06 && intersectionResult2.Distance < 1E-06)
            {
                return true;
            }
            intersectionResult = line.Project(endPoint3);
            intersectionResult2 = line.Project(endPoint4);
            return intersectionResult != null && intersectionResult2 != null && intersectionResult.Distance < 1E-06 && intersectionResult2.Distance < 1E-06;
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00002AE0 File Offset: 0x00000CE0
        private bool IsSameCurve(Curve curve1, Curve curve2)
        {
            return (this.IsNearlyEqual(curve1.GetEndPoint(0), curve2.GetEndPoint(0)) && this.IsNearlyEqual(curve1.GetEndPoint(1), curve2.GetEndPoint(1))) || (this.IsNearlyEqual(curve1.GetEndPoint(0), curve2.GetEndPoint(1)) && this.IsNearlyEqual(curve1.GetEndPoint(1), curve2.GetEndPoint(0)));
        }

        // Token: 0x06000008 RID: 8 RVA: 0x00002B48 File Offset: 0x00000D48
        private bool IsNearlyEqual(XYZ pos1, XYZ pos2)
        {
            return this.IsNearlyEqual(pos1.X, pos2.X, 3) && this.IsNearlyEqual(pos1.Y, pos2.Y, 3) && this.IsNearlyEqual(pos1.Z, pos2.Z, 3);
        }

        // Token: 0x06000009 RID: 9 RVA: 0x00002B94 File Offset: 0x00000D94
        private bool IsNearlyEqual(double val1, double val2, int digit = 3)
        {
            int num = (int)(val1 * Math.Pow(10.0, (double)digit));
            int num2 = (int)(val2 * Math.Pow(10.0, (double)digit));
            return num == num2;
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00002BCC File Offset: 0x00000DCC
        private void LockFloor(Document doc, Floor floor, List<Element> elements)
        {
            Dictionary<ModelCurve, List<Element>> dictionary = this.DetectMatchedElements(doc, floor, elements);
            if (dictionary == null || dictionary.Count == 0)
            {
                return;
            }
            foreach (KeyValuePair<ModelCurve, List<Element>> keyValuePair in dictionary)
            {
                List<Element> value = keyValuePair.Value;
                ModelCurve key = keyValuePair.Key;
                try
                {
                    foreach (Element element in value)
                    {
                        if (element is FamilyInstance)
                        {
                            IList<Reference> references = (element as FamilyInstance).GetReferences(FamilyInstanceReferenceType.CenterFrontBack);
                            if (references.Count == 0)
                            {
                                continue;
                            }
                            try
                            {
                                doc.Create.NewAlignment(doc.ActiveView, references[0], new Reference(key));
                                continue;
                            }
                            catch (Exception ex)
                            {
                                string message = ex.Message;
                                continue;
                            }
                        }
                        try
                        {
                            doc.Create.NewAlignment(doc.ActiveView, new Reference(element), new Reference(key));
                        }
                        catch (Exception ex2)
                        {
                            string message2 = ex2.Message;
                        }
                    }
                }
                catch (Exception ex3)
                {
                    string message3 = ex3.Message;
                }
            }
        }

        // Token: 0x0600000B RID: 11 RVA: 0x00002D24 File Offset: 0x00000F24
        private static XYZ MiddlePoint(Curve curve)
        {
            double endParameter = curve.GetEndParameter(0);
            double endParameter2 = curve.GetEndParameter(1);
            return curve.Evaluate(endParameter + (endParameter2 - endParameter) / 2.0, false);
        }

        // Token: 0x0600000C RID: 12 RVA: 0x00002D57 File Offset: 0x00000F57
        public FloorCreator()
        {
        }
    }

}
