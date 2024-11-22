# -*- coding: utf-8 -*-
__doc__ = 'python for revit api'
__author__ = 'SonKawamura'
from Autodesk.Revit.UI.Selection.Selection import PickObject
from Autodesk.Revit.UI.Selection  import ObjectType
from Autodesk.Revit.DB import *
from Autodesk.Revit.DB import FailuresAccessor
from Autodesk.Revit.DB import Line
import Autodesk.Revit.DB as DB
from Autodesk.Revit.Creation import ItemFactoryBase
from System.Collections.Generic import *
from Autodesk.Revit.DB import Reference
import math
#Get UIDocument
uidoc = __revit__.ActiveUIDocument
#Get Document 
doc = uidoc.Document
import sys
import string
import importlib
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
    import nances as module
from nances import vectortransform, geometry

import Autodesk
from Autodesk.Revit.DB import *
from System.Collections.Generic import *
Currentview = doc.ActiveView
view_scale = Currentview.Scale

selected = module.get_element(uidoc,doc, 'Select Beam', noti = False)


pick_element = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element)
beam = doc.GetElement(pick_element.ElementId)
                                                                              
# Lấy Solid của đối tượng A
get_element_intersection = geometry.find_intersect_elements(doc, beam, selected)
for i in get_element_intersection:
    print i



