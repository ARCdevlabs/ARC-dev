# pylint: skip-file
import os.path as op
from pyrevit import HOST_APP, EXEC_PARAMS
from pyrevit import revit, script

from pyrevit import HOST_APP, UI
from Autodesk.Revit.UI import UIApplication, RevitCommandId, PostableCommand

args = EXEC_PARAMS.event_args


from pyrevit import HOST_APP, UI
from Autodesk.Revit.UI import UIApplication, RevitCommandId, PostableCommand
uiapp = HOST_APP.uiapp
uiapp.PostCommand(UI.RevitCommandId.LookupCommandId("CustomCtrl_%CustomCtrl_%ARC_test%Test5%Delete_Son"))


# output = script.get_output()
# output.print_image(
#     op.join(op.dirname(__file__), op.basename(__file__).replace('.py', '.gif'))
# )
