using ADSK.JExtRAC.AutomaticFloor.Utils;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ADSK.JExtRAC.AutomaticFloor.Commands;

[Transaction(/*Could not decode attribute arguments.*/)]
public class CmdFoundationSlab : IExternalCommand
{
	public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return new FloorCreator().CreateFloor(commandData, eFloorType.Slab);
	}
}
