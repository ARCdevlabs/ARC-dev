# -*- coding: utf-8 -*-
#pylint: disable=import-error,invalid-name,broad-except,superfluous-parens
import Autodesk
from Autodesk.Revit.DB import *
import Autodesk.Revit.DB as DB
from System.Collections.Generic import List
from Autodesk.Revit.UI.Selection import ObjectType
import traceback
import sys
import os
# Tìm đường dẫn thư mục pyRevit Extensions
pyrevit_extensions_path = os.path.join(os.getenv('APPDATA'), 'pyRevit', 'Extensions')
# hoặc sử dụng PROGRAMDATA nếu thư viện được cài đặt ở đó
# pyrevit_extensions_path = os.path.join(os.getenv('PROGRAMDATA'), 'pyRevit', 'Extensions')
# Đường dẫn thư viện nances
nances_lib_path = os.path.join(pyrevit_extensions_path, 'ARC extension.extension', 'lib')
# Thêm đường dẫn vào sys.path nếu chưa có
if nances_lib_path not in sys.path:
    sys.path.append(nances_lib_path)
    print nances_lib_path
    import nances



uidoc = __revit__.ActiveUIDocument
doc = uidoc.Document
idsToDelete = List[ElementId]()

constraintIds = FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Constraints).ToElementIds()

[idsToDelete.Add(id) for id in constraintIds]

elements = nances.get_elements(uidoc,doc, 'Select Element', noti = False)

n = 0
for tung_ele in elements:
    n += 1
    get_dependent_element = tung_ele.GetDependentElements(None)
    chuoi_xuat_ra = ""
    for tung_dep in get_dependent_element:
        get_ele = doc.GetElement(tung_dep)
        try:
            cate = get_ele.Category.Name
            if cate == "Constraints":
                chuoi_xuat_ra = chuoi_xuat_ra + str(tung_dep) + ","
        except:
            pass
    loai_bo_ky_tu_cuoi = "ID of constraint: " + chuoi_xuat_ra[:-1]

    print (str(n) + "." + ("ID of Element: " + str(tung_ele.Id))),  loai_bo_ky_tu_cuoi



