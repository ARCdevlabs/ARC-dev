using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Module2.ViewModels;
using Module2.Views;
using Nice3point.Revit.Toolkit.External;
using NS.WPFLearningCSharp;

namespace RevitAddIn.Commands;

[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class Module4StartupCommand : ExternalCommand
{
    public override void Execute()
    {
        MainWindow m_form = new MainWindow();
        m_form.ShowDialog();
    }
}