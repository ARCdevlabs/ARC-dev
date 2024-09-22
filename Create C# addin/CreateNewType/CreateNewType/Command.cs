using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;

namespace CreateNewType
{
    // Token: 0x02000002 RID: 2
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {

        public List<string> ExcelData_ParameterName
        {
            get
            {
                return this.m_ExcelData_ParameterName;
            }
        }

        public List<Family> FamilyMaps
        {
            get
            {
                return this.m_FamilyMaps;
            }
        }

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000003 RID: 3 RVA: 0x00002080 File Offset: 0x00000280
        public List<ExcelWorksheet> WorkSheets
        {
            get
            {
                return this.m_WorkSheets;
            }
        }

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000004 RID: 4 RVA: 0x00002098 File Offset: 0x00000298
        public List<Command.NewList_Parameters> TypeParameters
        {
            get
            {
                return this.m_TypeParameter;
            }
        }

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x06000005 RID: 5 RVA: 0x000020B0 File Offset: 0x000002B0
        // (set) Token: 0x06000006 RID: 6 RVA: 0x000020C8 File Offset: 0x000002C8
        public FamilySymbol m_Symbol
        {
            get
            {
                return this.m_FamilySymbol;
            }
            set
            {
                this.m_FamilySymbol = value;
            }
        }

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x06000007 RID: 7 RVA: 0x000020D4 File Offset: 0x000002D4
        public UIDocument ActiveUIDocument
        {
            get
            {
                return this.m_UIDocument;
            }
        }

        // Token: 0x06000008 RID: 8 RVA: 0x000020EC File Offset: 0x000002EC
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument activeUIDocument = commandData.Application.ActiveUIDocument;
            Document document = activeUIDocument.Document;
            this.m_UIDocument = activeUIDocument;
            Transaction transaction = new Transaction(document, "Create Types");
            transaction.Start();
            Result result;
            using (ChooseCategoryForm chooseCategoryForm = new ChooseCategoryForm(this))
            {
                bool flag = chooseCategoryForm.ShowDialog() != DialogResult.OK;
                if (flag)
                {
                    transaction.RollBack();

                    return Result.Failed;
                   
                }
                else
                {
                    this.GetFamilys();
                    this.GetExcelWorkSheets();
                    using (CreateTypesForm createTypesForm = new CreateTypesForm(this))
                    {
                        bool flag2 = createTypesForm.ShowDialog() != DialogResult.OK;
                        if (flag2)
                        {
                            transaction.RollBack();
                            return Result.Failed;
                        }
                        else
                        {
                            ISet<ElementId> familySymbolIds = this.m_Family.GetFamilySymbolIds();
                            foreach (ElementId elementId in familySymbolIds)
                            {
                                this.m_FamilySymbol = (activeUIDocument.Document.GetElement(elementId) as FamilySymbol);
                                this.CheckSymbol.Add(this.m_FamilySymbol.Name);
                            }
                            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(this.filePath)))
                            {
                                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[this.m_WorkSheetName];
                                for (int i = excelWorksheet.Dimension.Start.Row + 1; i <= excelWorksheet.Dimension.End.Row; i++)
                                {
                                    bool flag3 = excelWorksheet.Cells[i, 1].Value != null;
                                    if (!flag3)
                                    {
                                        break;
                                    }
                                    bool flag4 = this.CheckSymbol.Contains(excelWorksheet.Cells[i, 1].Value.ToString());
                                    if (flag4)
                                    {
                                        MessageBox.Show(string.Format("Type name {0} is already in use. Please check list Family type in Revit file and type name in Excel list and try again!", excelWorksheet.Cells[i, 1].Value.ToString()), "Warning!", MessageBoxButtons.OK);
                                        MessageBox.Show("Cancelled!");
                                        transaction.RollBack();
                                    }
                                    this.CheckSymbol.Add(excelWorksheet.Cells[i, 1].Value.ToString());
                                    ElementType elementType = this.m_FamilySymbol.Duplicate(excelWorksheet.Cells[i, 1].Value.ToString());
                                    int j = excelWorksheet.Dimension.Start.Column + 1;
                                    while (j <= excelWorksheet.Dimension.End.Column)
                                    {
                                        try
                                        {
                                            object value = excelWorksheet.Cells[1, j].Value;
                                            bool flag5 = value != null;
                                            if (!flag5)
                                            {
                                                break;
                                            }
                                            Parameter parameter = elementType.LookupParameter(value.ToString());
                                            object _value = excelWorksheet.Cells[i, j].Value;
                                            bool flag6 = _value != null;
                                            if (flag6)
                                            {
                                                bool flag7 = parameter.StorageType == StorageType.String;


                                                if (flag7)
                                                {
                                                    parameter.Set(_value.ToString());
                                                }
                                                else
                                                {
                                                    bool flag8 = parameter.StorageType == StorageType.Integer;
                                                    if (flag8)
                                                    {
                                                        parameter.Set(int.Parse(_value.ToString()));
                                                    }
                                                    else
                                                    {
                                                        bool flag9 = parameter.StorageType == StorageType.Double;
                                                        if (flag9)
                                                        {
                                                            parameter.Set(double.Parse(_value.ToString()) / 304.8);
                                                        }
                                                        else
                                                        {
                                                            bool flag10 = parameter.StorageType == StorageType.ElementId;
                                                            if (flag10)
                                                            {
                                                                
                                                                bool flag11 = parameter.Definition.ParameterType == ParameterType.Material;
                                                                if (flag11)
                                                                {
                                                                    Material material = (from Material m in new FilteredElementCollector(this.m_UIDocument.Document).OfClass(typeof(Material))
                                                                                         where m.Name == _value.ToString()
                                                                                         select m).First<Material>();
                                                                    parameter.Set(material.Id);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            MessageBox.Show(string.Format("Parameter name {0} does not exist in family. Cancel!", excelWorksheet.Cells[1, j].Value.ToString()));
                                            MessageBox.Show("Cancelled!");
                                            transaction.RollBack();
                                            return 1;
                                        }
                                    IL_421:
                                        j++;
                                        continue;
                                        goto IL_421;
                                    }
                                }
                                MessageBox.Show("Succeeded!");
                                transaction.Commit();
                                result = 0;
                            }
                        }
                    }
                }
            }
            return result;
        }

        // Token: 0x06000009 RID: 9 RVA: 0x00002628 File Offset: 0x00000828
        public List<Family> GetFamilys()
        {
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(this.m_UIDocument.Document).OfClass(typeof(Family));
            FilteredElementIterator elementIterator = filteredElementCollector.GetElementIterator();
            while (elementIterator.MoveNext())
            {
                Element element = elementIterator.Current;
                Family family = element as Family;
                bool flag = family != null;
                if (flag)
                {
                    ISet<ElementId> familySymbolIds = family.GetFamilySymbolIds();
                    using (IEnumerator<ElementId> enumerator = familySymbolIds.GetEnumerator())
                    {
                        if (enumerator.MoveNext())
                        {
                            ElementId elementId = enumerator.Current;
                            FamilySymbol familySymbol = this.m_UIDocument.Document.GetElement(elementId) as FamilySymbol;
                            bool flag2 = familySymbol.Category.Name == this.CategoryName;
                            if (flag2)
                            {
                                this.m_FamilyMaps.Add(family);
                            }
                        }
                    }
                }
            }
            return this.m_FamilyMaps;
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00002724 File Offset: 0x00000924
        public List<string> GetExcelData_ParameterName(ExcelWorksheet m_SheetName)
        {
            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(this.filePath)))
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[m_SheetName.Name];
                for (int i = excelWorksheet.Dimension.Start.Column; i <= excelWorksheet.Dimension.End.Column; i++)
                {
                    this.m_ExcelData_ParameterName.Add(excelWorksheet.Cells[1, i].Value.ToString());
                }
            }
            return this.m_ExcelData_ParameterName;
        }

        // Token: 0x0600000B RID: 11 RVA: 0x000027DC File Offset: 0x000009DC
        public List<ExcelWorksheet> GetExcelWorkSheets()
        {
            ExcelPackage.LicenseContext = new LicenseContext?(LicenseContext.NonCommercial);
            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(this.filePath)))
            {
                this.m_WorkSheets = excelPackage.Workbook.Worksheets.ToList<ExcelWorksheet>();
            }
            return this.m_WorkSheets;
        }

        // Token: 0x0600000C RID: 12 RVA: 0x00002848 File Offset: 0x00000A48
        public List<Command.NewList_Parameters> GetTypeParameters()
        {
            foreach (Parameter parameter in this.m_FamilySymbol.GetOrderedParameters())
            {
                Command.NewList_Parameters item = new Command.NewList_Parameters
                {
                    Name = parameter.Definition.Name,
                    n_Parameters = parameter
                };
                this.m_TypeParameter.Add(item);
            }
            return this.m_TypeParameter;
        }

        // Token: 0x04000001 RID: 1
        public string CategoryName;

        // Token: 0x04000002 RID: 2
        public Workbook m_Workbook;

        // Token: 0x04000003 RID: 3
        public UIDocument m_UIDocument;

        // Token: 0x04000004 RID: 4
        public string filePath;

        // Token: 0x04000005 RID: 5
        public Family m_Family = null;

        // Token: 0x04000006 RID: 6
        private FamilySymbol m_FamilySymbol;

        // Token: 0x04000007 RID: 7
        public string m_WorkSheetName;

        // Token: 0x04000008 RID: 8
        public List<ExcelWorksheet> m_WorkSheets = new List<ExcelWorksheet>();

        // Token: 0x04000009 RID: 9
        private List<string> m_ExcelData_ParameterName = new List<string>();

        // Token: 0x0400000A RID: 10
        private List<Command.NewList_Parameters> m_TypeParameter = new List<Command.NewList_Parameters>();

        // Token: 0x0400000B RID: 11
        private List<string> CheckSymbol = new List<string>();

        // Token: 0x0400000C RID: 12
        private List<Family> m_FamilyMaps = new List<Family>();

        // Token: 0x02000009 RID: 9
        public class NewList_Parameters
        {
            // Token: 0x17000007 RID: 7
            // (get) Token: 0x06000018 RID: 24 RVA: 0x000034F7 File Offset: 0x000016F7
            // (set) Token: 0x06000019 RID: 25 RVA: 0x000034FF File Offset: 0x000016FF
            public string Name { get; set; }

            // Token: 0x17000008 RID: 8
            // (get) Token: 0x0600001A RID: 26 RVA: 0x00003508 File Offset: 0x00001708
            // (set) Token: 0x0600001B RID: 27 RVA: 0x00003510 File Offset: 0x00001710
            public Parameter n_Parameters { get; set; }
        }
    }
}
