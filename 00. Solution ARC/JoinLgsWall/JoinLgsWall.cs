using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using ARC.Library;


namespace ARCCommand
{
    [TransactionAttribute(TransactionMode.Manual)] //Dòng này cần phải có trong Visual Studio thì build xong mới hiểu được
                                                   //Nếu chạy trong pyRevit thì không cần
    public class JoinLsgWall : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit,
                              ref string message, ElementSet elements)
        {
            UIDocument uidoc = revit.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            ARCLibrary lib = new ARCLibrary();

            Element pickElement = lib.PickElement(uidoc,doc);

            ElementId element_id = pickElement.Id;


            //ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();

            Transaction trans = new Autodesk.Revit.DB.Transaction(doc, "Set DIM_LEADER");
            {
                trans.Start();
                {
                    try
                    {
                        Parameter dimLeaderParam = pickElement.get_Parameter(BuiltInParameter.DIM_LEADER);

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
                    catch
                    {
                        //Không làm gì
                    }
                }
                trans.Commit();
            }
            return Result.Succeeded;
        }
    }
}