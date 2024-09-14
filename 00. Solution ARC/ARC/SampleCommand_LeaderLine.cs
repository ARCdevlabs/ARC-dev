using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Nice3point.Revit.Extensions;

namespace ARC
{
    //[TransactionAttribute(TransactionMode.Manual)] //Dòng này cần phải có trong Visual Studio thì build xong mới hiểu được
    //Nếu chạy trong pyRevit thì không cần

    [Transaction(TransactionMode.Manual)] //Dòng này cần phải có trong Visual Studio thì build xong mới hiểu được
                                          //Nếu chạy trong pyRevit thì không cần
    public class LeaderLine : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit,
                              ref string message, ElementSet elements)

        {
            UIDocument uidoc = revit.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            ARCLibrary lib = new ARCLibrary();


            Element pickElement = lib.PickElement(uidoc, doc);

            if (pickElement != null)
            {


                Transaction trans = new Transaction(doc, "Set DIM_LEADER");
                {
                    trans.Start();
                    {
                        try
                        {
                            //Parameter dimLeaderParam = pickElement.get_Parameter(BuiltInParameter.DIM_LEADER);
                            Parameter dimLeaderParam = pickElement.FindParameter(BuiltInParameter.DIM_LEADER);

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
            }
            return Result.Succeeded;
        }
    }
}