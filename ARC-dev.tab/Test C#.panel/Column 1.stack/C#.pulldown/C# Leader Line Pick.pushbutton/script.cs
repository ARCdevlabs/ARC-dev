using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using System.Collections.Generic;
namespace LeaderLine
{
   [TransactionAttribute(TransactionMode.Manual)]
   public class LeaderLine : IExternalCommand 
   {
      public Result Execute(ExternalCommandData revit,
                            ref string message, ElementSet elements) 
      {
         UIDocument uidoc = revit.Application.ActiveUIDocument;
         Document doc = uidoc.Document;
         Reference pickedObjectRef = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
         ElementId elementId = pickedObjectRef.ElementId;
         Element getElement = doc.GetElement(elementId);
         Parameter dimLeaderParam = getElement.get_Parameter(BuiltInParameter.DIM_LEADER);
         Transaction trans = new Autodesk.Revit.DB.Transaction(doc, "Set DIM_LEADER");
         {
            trans.Start();
            int leaderLineValue = dimLeaderParam.AsInteger();
            if (leaderLineValue == 1)
            {
               dimLeaderParam.Set(0);
            }
            else
            {
               dimLeaderParam.Set(1);
            }
            trans.Commit();
            Console.WriteLine(leaderLineValue);
         }
         return Result.Succeeded;
      }
   }
}