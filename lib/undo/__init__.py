from pyrevit import HOST_APP, UI
from Autodesk.Revit.UI import UIApplication, RevitCommandId, PostableCommand
def undo_command():
    uiapp = HOST_APP.uiapp
    uiapp.PostCommand(UI.RevitCommandId.LookupPostableCommandId(PostableCommand.Undo))


def delete_command():
    uiapp = HOST_APP.uiapp
    uiapp.PostCommand(UI.RevitCommandId.LookupPostableCommandId(PostableCommand.Delete))

def delete_son():
    from pyrevit import HOST_APP, UI
    from Autodesk.Revit.UI import UIApplication, RevitCommandId, PostableCommand
    uiapp = HOST_APP.uiapp
    uiapp.PostCommand(UI.RevitCommandId.LookupCommandId("CustomCtrl_%CustomCtrl_%ARC_test%Test5%Delete_Son"))