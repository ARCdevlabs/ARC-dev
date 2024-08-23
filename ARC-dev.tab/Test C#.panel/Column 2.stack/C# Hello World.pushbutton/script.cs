using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace HelloWorld 
{
   public class Test_Class_Hello_World : IExternalCommand 
   {
      public Result Execute(ExternalCommandData revit,
                            ref string message, ElementSet elements) 
      {
      // TaskDialog.Show("Revit", "Hello World from Link Button!!!");
      Console.WriteLine("Hello World from Link Button!!!");
         return Result.Succeeded;
      }
   }
}