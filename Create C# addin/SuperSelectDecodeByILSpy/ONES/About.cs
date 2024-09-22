using System.Diagnostics;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ONES;

[Transaction(TransactionMode.Manual)]
public class About : IExternalCommand
{
	public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
	{
		UIApplication application = commandData.Application;
		UIDocument activeUIDocument = application.ActiveUIDocument;
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		AboutBox aboutBox = new AboutBox();
		aboutBox.ShowDialog();
		stopwatch.Stop();
		Utils.ONESLogs(commandData.Application.ActiveUIDocument, ToString(), stopwatch);
		return Result.Succeeded;
	}
}
