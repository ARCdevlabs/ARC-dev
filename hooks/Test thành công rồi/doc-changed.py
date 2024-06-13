'''
# pylint: skip-file
# -*- coding: utf-8 -*-
from pyrevit import HOST_APP, EXEC_PARAMS
from pyrevit import revit, script
import hooks_logger as hl

args = EXEC_PARAMS.event_args

import Autodesk
from Autodesk.Revit.DB import *
from System.Collections.Generic import *

ui = Autodesk.Revit.UI
uidoc = ui.UIApplication.ActiveUIDocument
doc = args.GetDocument()

list_element = []
list_delete =  args.GetDeletedElementIds()
if len(list_delete) > 0:
    print (list_delete)

'''