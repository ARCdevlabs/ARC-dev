using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace NS.WPFLearningCSharp
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command: IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                MainWindow m_form = new MainWindow();
                m_form.ShowDialog();

            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }
}
