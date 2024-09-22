# -*- coding: utf-8 -*-
"""ARC"""
from System.Collections.Generic import *
#Get UIDocument
uidoc = __revit__.ActiveUIDocument
doc = uidoc.Document
# Def nay cho current element

def get_selected_elements(uidoc, doc, noti=True):
    selection = uidoc.Selection
    selection_ids = selection.GetElementIds()
    elements = [doc.GetElement(element_id) for element_id in selection_ids]

    if not elements:
        if noti:
            from Autodesk.Revit.UI import TaskDialog
            dialog = TaskDialog("ARC")
            from pyrevit.coreutils import applocales
            current_applocale = applocales.get_current_applocale()
            if str(current_applocale) == "日本語 / Japanese (ja)":
                message = "このツールを使用する前に要素をご選択ください。"
            else:
                message = "Please select element before use this tool."
            dialog.MainContent = message
            dialog.TitleAutoPrefix = False
            dialog.Show()
        return False
    return elements
        
