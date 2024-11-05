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

selected = module.get_element(uidoc,doc, 'Select Beam', noti = False)

# pick = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element)
# beam = doc.GetElement(pick.ElementId)
beam = selected[0]

location_curve = beam.Location.Curve
start_point_of_beam = location_curve.GetEndPoint(0)
end_point_of_beam = location_curve.GetEndPoint(1)
plane_of_beam = vectortransform.create_plane_follow_line (location_curve)
origin_of_plane = plane_of_beam.Origin
normal_of_plane = plane_of_beam.Normal


pick_girder = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element)
element_of_girder = doc.GetElement(pick_girder.ElementId)

curve = element_of_girder.Location.Curve
start_point = curve.GetEndPoint(0)
end_point = curve.GetEndPoint(1)
line = DB.Line.CreateBound(start_point,end_point)

transformed_line = line

line_direction = transformed_line.Direction
start_point_transformed_line = transformed_line.GetEndPoint(0)

intersect_point = vectortransform.line_plane_intersection(start_point_transformed_line,line_direction,origin_of_plane,normal_of_plane)

tim_diem_gan_intersect_point = vectortransform.nearest_point(intersect_point, start_point_of_beam, end_point_of_beam)

if tim_diem_gan_intersect_point == "StartPoint":
    new_line = DB.Line.CreateBound(intersect_point,end_point_of_beam)
elif tim_diem_gan_intersect_point == "EndPoint":
    new_line = DB.Line.CreateBound(start_point_of_beam,intersect_point)

    
t = Transaction(doc, 'Đổi level giằng theo dầm dốc')
t.Start()
beam.Location.Curve = new_line
t.Commit()



