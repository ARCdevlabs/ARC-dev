using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SuperSelectForm;


namespace SuperSelect
{
    // Token: 0x02000058 RID: 88
    [Transaction(TransactionMode.Manual)]
    public class SelectFilter : IExternalCommand
    {
        // Token: 0x06000306 RID: 774 RVA: 0x0003EFC4 File Offset: 0x0003D1C4
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication application = commandData.Application;
            UIDocument activeUIDocument = application.ActiveUIDocument;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SelectFilterForm selectFilterForm = new SelectFilterForm(activeUIDocument);
            DialogResult dialogResult = selectFilterForm.ShowDialog();
            bool flag = dialogResult == DialogResult.OK;
            if (flag)
            {
                List<ElementId> checkedIds = selectFilterForm.checkedIds;
                activeUIDocument.Selection.SetElementIds(checkedIds);
            }
            stopwatch.Stop();
            return Result.Succeeded;
        }

        // Token: 0x06000307 RID: 775 RVA: 0x000021C8 File Offset: 0x000003C8
        public SelectFilter()
        {
        }
    }
}
