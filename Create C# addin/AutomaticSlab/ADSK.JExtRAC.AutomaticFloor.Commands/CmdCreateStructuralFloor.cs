using System;
using ADSK.JExtRAC.AutomaticFloor.Utils;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ADSK.JExtRAC.AutomaticFloor.Commands
{
    // Token: 0x02000015 RID: 21
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class CmdCreateStructuralFloor : IExternalCommand
    {
        // Token: 0x06000095 RID: 149 RVA: 0x00006065 File Offset: 0x00004265
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return new FloorCreator().CreateFloor(commandData, eFloorType.Struct);
        }

        // Token: 0x06000096 RID: 150 RVA: 0x00006073 File Offset: 0x00004273
        public CmdCreateStructuralFloor()
        {
        }
    }
}
