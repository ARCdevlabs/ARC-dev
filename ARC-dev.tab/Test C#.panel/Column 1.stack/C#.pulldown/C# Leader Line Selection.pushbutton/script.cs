using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.Generic; // Dòng này để để có thể có thể gọi ra được
                                  //thư viện chứa phương thức "ICollection<ElementId>"
using Autodesk.Revit.Attributes; // Dòng này để để có thể có thể gọi ra được "TransactionAttribute"

namespace LeaderLine
{
    [TransactionAttribute(TransactionMode.Manual)] //Dòng này cần phải có trong Visual Studio thì build xong Revit mới hiểu được
                                                   //Nếu chạy trong pyRevit thì không cần nhưng tốt nhất cứ thêm vào
    public class LeaderLine : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit,
                              ref string message, ElementSet elements)
        {
            UIDocument uidoc = revit.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();
            Transaction trans = new Autodesk.Revit.DB.Transaction(doc, "Set DIM_LEADER");
            {
                trans.Start();
                foreach (ElementId id in selectedIds)
                {
                    Element getElement = doc.GetElement(id);
                    Parameter dimLeaderParam = getElement.get_Parameter(BuiltInParameter.DIM_LEADER);

                    int leaderLineValue = dimLeaderParam.AsInteger();
                    if (leaderLineValue == 1)
                    {
                        dimLeaderParam.Set(0);
                    }
                    else
                    {
                        dimLeaderParam.Set(1);
                    }
                   
                }
                trans.Commit();
            }
            return Result.Succeeded;
        }
    }
}