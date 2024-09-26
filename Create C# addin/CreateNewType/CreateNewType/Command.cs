

// CreateNewType, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// CreateNewType.Command
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CreateNewType;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;

[Transaction(TransactionMode.Manual)]
public class Command : IExternalCommand
{
    public class NewList_Parameters
    {
        public string Name { get; set; }

        public Autodesk.Revit.DB.Parameter n_Parameters { get; set; }
    }

    public string CategoryName;

    public Workbook m_Workbook;

    public UIDocument m_UIDocument;

    public string filePath;

    public Family m_Family = null;

    private FamilySymbol m_FamilySymbol;

    public string m_WorkSheetName;

    public List<ExcelWorksheet> m_WorkSheets = new List<ExcelWorksheet>();

    private List<string> m_ExcelData_ParameterName = new List<string>();

    private List<NewList_Parameters> m_TypeParameter = new List<NewList_Parameters>();

    private List<string> CheckSymbol = new List<string>();

    private List<Family> m_FamilyMaps = new List<Family>();

    public List<string> ExcelData_ParameterName => m_ExcelData_ParameterName;

    public List<Family> FamilyMaps => m_FamilyMaps;

    public List<ExcelWorksheet> WorkSheets => m_WorkSheets;

    public List<NewList_Parameters> TypeParameters => m_TypeParameter;

    public FamilySymbol m_Symbol
    {
        get
        {
            return m_FamilySymbol;
        }
        set
        {
            m_FamilySymbol = value;
        }
    }

    public UIDocument ActiveUIDocument => m_UIDocument;

    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        //IL_0021: Unknown result type (might be due to invalid IL or missing references)
        //IL_0027: Expected O, but got Unknown
        //IL_0028: Unknown result type (might be due to invalid IL or missing references)
        //IL_0412: Unknown result type (might be due to invalid IL or missing references)
        //IL_0419: Unknown result type (might be due to invalid IL or missing references)
        //IL_04b1: Unknown result type (might be due to invalid IL or missing references)
        //IL_004a: Unknown result type (might be due to invalid IL or missing references)
        //IL_0051: Unknown result type (might be due to invalid IL or missing references)
        //IL_0085: Unknown result type (might be due to invalid IL or missing references)
        //IL_008c: Unknown result type (might be due to invalid IL or missing references)
        //IL_0481: Unknown result type (might be due to invalid IL or missing references)
        //IL_0488: Unknown result type (might be due to invalid IL or missing references)
        //IL_01cc: Unknown result type (might be due to invalid IL or missing references)
        //IL_02a8: Unknown result type (might be due to invalid IL or missing references)
        //IL_02ae: Invalid comparison between Unknown and I4
        //IL_02d3: Unknown result type (might be due to invalid IL or missing references)
        //IL_02d9: Invalid comparison between Unknown and I4
        //IL_0303: Unknown result type (might be due to invalid IL or missing references)
        //IL_0309: Invalid comparison between Unknown and I4
        //IL_033d: Unknown result type (might be due to invalid IL or missing references)
        //IL_0343: Invalid comparison between Unknown and I4
        //IL_0356: Unknown result type (might be due to invalid IL or missing references)
        //IL_035b: Unknown result type (might be due to invalid IL or missing references)
        //IL_0386: Unknown result type (might be due to invalid IL or missing references)
        UIDocument activeUIDocument = commandData.Application.ActiveUIDocument;
        Document document = activeUIDocument.Document;
        m_UIDocument = activeUIDocument;
        Transaction val = new Transaction(document, "Create Types");
        val.Start();
        ChooseCategoryForm chooseCategoryForm = new ChooseCategoryForm(this);
        if (chooseCategoryForm.ShowDialog() != DialogResult.OK)
        {
            val.RollBack();
            return (Result)1;
        }
        GetFamilys();
        GetExcelWorkSheets();
        CreateTypesForm createTypesForm = new CreateTypesForm(this);
        if (createTypesForm.ShowDialog() != DialogResult.OK)
        {
            val.RollBack();
            return (Result)1;
        }
        ISet<ElementId> familySymbolIds = m_Family.GetFamilySymbolIds();
        foreach (ElementId item in familySymbolIds)
        {
            ref FamilySymbol familySymbol = ref m_FamilySymbol;
            Element element = activeUIDocument.Document.GetElement(item);
            familySymbol = (FamilySymbol)(object)((element is FamilySymbol) ? element : null);
            CheckSymbol.Add(((Element)m_FamilySymbol).Name);
        }
        ExcelPackage excelPackage = new ExcelPackage(new FileInfo(filePath));
        ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[m_WorkSheetName];
        for (int i = excelWorksheet.Dimension.Start.Row + 1; i <= excelWorksheet.Dimension.End.Row && excelWorksheet.Cells[i, 1].Value != null; i++)
        {
            if (CheckSymbol.Contains(excelWorksheet.Cells[i, 1].Value.ToString()))
            {
                MessageBox.Show($"Type name {excelWorksheet.Cells[i, 1].Value.ToString()} is already in use. Please check list Family type in Revit file and type name in Excel list and try again!", "Warning!", MessageBoxButtons.OK);
                MessageBox.Show("Cancelled!");
                val.RollBack();
            }
            CheckSymbol.Add(excelWorksheet.Cells[i, 1].Value.ToString());
            ElementType val2 = ((ElementType)m_FamilySymbol).Duplicate(excelWorksheet.Cells[i, 1].Value.ToString());
            for (int j = excelWorksheet.Dimension.Start.Column + 1; j <= excelWorksheet.Dimension.End.Column; j++)
            {
                try
                {
                    object value = excelWorksheet.Cells[1, j].Value;
                    if (value == null)
                    {
                        break;
                    }
                    Autodesk.Revit.DB.Parameter val3 = ((Element)val2).LookupParameter(value.ToString());
                    object _value = excelWorksheet.Cells[i, j].Value;
                    if (_value == null)
                    {
                        continue;
                    }
                    if ((int)val3.StorageType == 3)
                    {
                        val3.Set(_value.ToString());
                    }
                    else if ((int)val3.StorageType == 1)
                    {
                        val3.Set(int.Parse(_value.ToString()));
                    }
                    else if ((int)val3.StorageType == 2)
                    {
                        val3.Set(double.Parse(_value.ToString()) / 304.8);
                    }
                    else
                    {
                        if ((int)val3.StorageType != 4)
                        {
                            continue;
                        }
                        //ParameterType parameterType = val3.Definition.ParameterType;

                        ForgeTypeId forgeTypeId = val3.Definition.GetGroupTypeId();



                        if (forgeTypeId.ToString() == "autodesk.parameter.group:materials-1.0.0")
                        {
                            Material val4 = (from Material m in (IEnumerable)new FilteredElementCollector(m_UIDocument.Document).OfClass(typeof(Material))
                                             where ((Element)m).Name == _value.ToString()
                                             select m).First();
                            val3.Set(((Element)val4).Id);
                        }
                    }
                    continue;
                }
                catch
                {
                    MessageBox.Show($"Parameter name {excelWorksheet.Cells[1, j].Value.ToString()} does not exist in family. Cancel!");
                    MessageBox.Show("Cancelled!");
                    val.RollBack();
                    return (Result)1;
                }
            }
        }
        MessageBox.Show("Succeeded!");
        val.Commit();
        return (Result)0;
    }

    public List<Family> GetFamilys()
    {
        //IL_000c: Unknown result type (might be due to invalid IL or missing references)
        FilteredElementCollector val = new FilteredElementCollector(m_UIDocument.Document).OfClass(typeof(Family));
        FilteredElementIterator elementIterator = val.GetElementIterator();
        while (elementIterator.MoveNext())
        {
            Element current = elementIterator.Current;
            Family val2 = (Family)(object)((current is Family) ? current : null);
            if (val2 == null)
            {
                continue;
            }
            ISet<ElementId> familySymbolIds = val2.GetFamilySymbolIds();
            IEnumerator<ElementId> enumerator = familySymbolIds.GetEnumerator();
            if (enumerator.MoveNext())
            {
                ElementId current2 = enumerator.Current;
                Element element = m_UIDocument.Document.GetElement(current2);
                FamilySymbol val3 = (FamilySymbol)(object)((element is FamilySymbol) ? element : null);
                if (((Element)val3).Category.Name == CategoryName)
                {
                    m_FamilyMaps.Add(val2);
                }
            }
        }
        return m_FamilyMaps;
    }

    public List<string> GetExcelData_ParameterName(ExcelWorksheet m_SheetName)
    {
        using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(filePath)))
        {
            ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[m_SheetName.Name];
            for (int i = excelWorksheet.Dimension.Start.Column; i <= excelWorksheet.Dimension.End.Column; i++)
            {
                m_ExcelData_ParameterName.Add(excelWorksheet.Cells[1, i].Value.ToString());
            }
        }
        return m_ExcelData_ParameterName;
    }

    public List<ExcelWorksheet> GetExcelWorkSheets()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(filePath)))
        {
            m_WorkSheets = excelPackage.Workbook.Worksheets.ToList();
        }
        return m_WorkSheets;
    }

    public List<NewList_Parameters> GetTypeParameters()
    {
        foreach (Autodesk.Revit.DB.Parameter orderedParameter in ((Element)m_FamilySymbol).GetOrderedParameters())
        {
            NewList_Parameters item = new NewList_Parameters
            {
                Name = orderedParameter.Definition.Name,
                n_Parameters = orderedParameter
            };
            m_TypeParameter.Add(item);
        }
        return m_TypeParameter;
    }
}
