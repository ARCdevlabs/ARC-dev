using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ONES;

[Transaction(TransactionMode.Manual)]
public class SelectFilter : IExternalCommand
{
	public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
	{
		UIApplication application = commandData.Application;
		UIDocument activeUIDocument = application.ActiveUIDocument;
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		SelectFilterForm selectFilterForm = new SelectFilterForm(activeUIDocument);
		DialogResult dialogResult = selectFilterForm.ShowDialog();
		if (dialogResult == DialogResult.OK)
		{
			List<ElementId> checkedIds = selectFilterForm.checkedIds;
			activeUIDocument.Selection.SetElementIds(checkedIds);
		}
		stopwatch.Stop();
		Utils.ONESLogs(activeUIDocument, ToString(), stopwatch);
		return Result.Succeeded;
	}
}
