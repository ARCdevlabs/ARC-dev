# encoding: utf-8
from pyrevit import HOST_APP, UI
from Autodesk.Revit.UI import UIApplication, RevitCommandId, PostableCommand
uiapp = HOST_APP.uiapp
uiapp.PostCommand(UI.RevitCommandId.LookupCommandId("CustomCtrl_%Add-Ins%Create and Update Type"))
# import Autodesk
# from pyrevit import HOST_APP, UI
# from Autodesk.Revit.UI import UIApplication, RevitCommandId, PostableCommand

# uiapp = HOST_APP.uiapp
# # revit_app = Autodesk.Revit.ApplicationServices.Documents
# # uiapp = Autodesk.Revit.UI.UIApplication(revit_app)
# print type(uiapp)
# uiapp.PostCommand(UI.RevitCommandId.LookupPostableCommandId(PostableCommand.Undo))

# uiapp = HOST_APP.uiapp
# uiapp.PostCommand(UI.RevitCommandId.LookupPostableCommandId(PostableCommand.Delete))


# import undo
# undo.undo_command()

# from pyrevit import HOST_APP, UI
# from Autodesk.Revit.UI import UIApplication, RevitCommandId, PostableCommand
# uiapp = HOST_APP.uiapp
# uiapp.PostCommand(UI.RevitCommandId.LookupCommandId("CustomCtrl_%CustomCtrl_%ARC_test%Test5%Delete_Son"))
