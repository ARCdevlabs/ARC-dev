
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
import Autodesk.Revit.DB as DB
from System.Collections.Generic import *

import traceback
uidoc = __revit__.ActiveUIDocument
doc = uidoc.Document

active_view = module.Active_view(doc)


t1 = Transaction (doc, "Duplicate Area Scheme")

t1.Start()
id = 418447
# id = 32840
area_scheme_type = doc.GetElement(ElementId(id))

# dup = area_scheme_type.Duplicate("new area scheme")


# ElementTransformUtils.CopyElement(doc, areaScheme.Id, XYZ.Zero)
copy_element = Autodesk.Revit.DB.ElementTransformUtils.CopyElement(doc, ElementId(id), XYZ(0,0,0))
get_copy = doc.GetElement(copy_element[0])
get_copy.Name = "New Area Scheme"

new_area_plan = Autodesk.Revit.DB.ViewPlan.CreateAreaPlan(doc, copy_element[0], ElementId(418448))

print type(get_copy), dir(get_copy)
# print type(area_scheme_type)
# print dir(area_scheme_type)
t1.Commit()
# except:
#     # print(traceback.format_exc())
#     pass
