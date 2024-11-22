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

from nances import vectortransform

import Autodesk
from Autodesk.Revit.DB import *
from System.Collections.Generic import *
Currentview = doc.ActiveView
view_scale = Currentview.Scale

pick_face = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Face)
element_of_face = doc.GetElement(pick_face.ElementId)
geometry_object_of_face= element_of_face.GetGeometryObjectFromReference(pick_face)



pick_face_2 = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Face)
element_of_face_2 = doc.GetElement(pick_face_2.ElementId)
geometry_object_of_face = element_of_face_2.GetGeometryObjectFromReference(pick_face_2)

intersect = geometry_object_of_face.Intersect(geometry_object_of_face)

print intersect




