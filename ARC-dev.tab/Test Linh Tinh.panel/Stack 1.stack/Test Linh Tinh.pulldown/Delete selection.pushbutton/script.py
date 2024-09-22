# -*- coding: utf-8 -*-
__doc__ = 'nguyenthanhson1712@gmail.com'
__author__ = 'NguyenThanhSon' "Email: nguyenthanhson1712@gmail.com"
import string
import importlib, nances

ARC = string.ascii_lowercase
begin = "".join(ARC[i] for i in [13, 0, 13, 2, 4, 18])
module = importlib.import_module(str(begin))
import Autodesk
from Autodesk.Revit.DB import *
from System.Collections.Generic import *
if module.AutodeskData():
    uidoc = __revit__.ActiveUIDocument
    doc = uidoc.Document
    Ele = module.get_selected_elements(uidoc,doc)
    if not Ele:
        import sys
        sys.exit()
    t = Transaction (doc, "Delete Selected")
    t.Start()
    so_luong = len(Ele)
    check_model = []
    for i in Ele:
        category_type = i.Category.CategoryType
        if str(category_type) != "Annotation":
            check_model.append(True)

    if True in check_model:
        nances.message_box("You are going to delete " + str(so_luong) + " objects")
        for tung_element in Ele:
            doc.Delete(tung_element.Id)
    else:
        for tung_element in Ele:
            doc.Delete(tung_element.Id)
    t.Commit()        
   
