# -*- coding: utf-8 -*-
__doc__ = 'nguyenthanhson1712@gmail.com'
__author__ = 'NguyenThanhSon' "Email: nguyenthanhson1712@gmail.com"
import string
import importlib

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
    for i in Ele:
        category_type = i.Category.CategoryType
        if str(category_type) == "Model":
            module.message_box("Bạn đang chuẩn bị xóa đối tượng tên là "+ i)
        else:
            doc.Delete(i.Id)
    t.Commit()        
   
