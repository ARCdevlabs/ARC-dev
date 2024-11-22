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
try:
    Currentview = doc.ActiveView
    view_scale = Currentview.Scale

    # pick = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element)
    # beam = doc.GetElement(pick.ElementId)
    # location_curve = beam.Location.Curve
    # start_point_of_beam = location_curve.GetEndPoint(0)
    # end_point_of_beam = location_curve.GetEndPoint(1)
    # plane_of_beam = vectortransform.create_plane_follow_line (location_curve)
    # origin_of_plane = plane_of_beam.Origin
    # normal_of_plane = plane_of_beam.Normal

    current = module.get_element(uidoc,doc, 'string_warning_bar', noti = False)
    beam = current[0]

    try:
        pick_edge = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Edge)
        element_of_edge = doc.GetElement(pick_edge.ElementId)
        geometry_object_of_edge = element_of_edge.GetGeometryObjectFromReference(pick_edge)
    except:
        pass
    # Kiểm tra nếu Geometry Object là Edge và lấy Curve của nó
    if isinstance(geometry_object_of_edge, Edge):
        curve = geometry_object_of_edge.AsCurve()
        start_point = curve.GetEndPoint(0)
        end_point = curve.GetEndPoint(1)
        line = DB.Line.CreateBound(start_point,end_point)



    # '''Kiểm tra có HasModifyGeometry chưa'''
    # transform = element_of_edge.GetTransform()
    # if element_of_edge.HasModifiedGeometry():
    #     transformed_line = line
    # else:
    #     transformed_line = vectortransform.transform_line(transform, line)
    transformed_line = line.CreateReversed()
    # line_direction = transformed_line.Direction
    # start_point_transformed_line = transformed_line.GetEndPoint(0)

    # intersect_point = vectortransform.line_plane_intersection(start_point_transformed_line,line_direction,origin_of_plane,normal_of_plane)

    # tim_diem_gan_intersect_point = vectortransform.nearest_point(intersect_point, start_point_of_beam, end_point_of_beam)

    # if tim_diem_gan_intersect_point == "StartPoint":
    #     new_line = DB.Line.CreateBound(intersect_point,end_point_of_beam)
    # elif tim_diem_gan_intersect_point == "EndPoint":
    #     new_line = DB.Line.CreateBound(start_point_of_beam,intersect_point)
        
    # print start_point_of_beam, end_point_of_beam, start_point, line_direction, origin_of_plane, normal_of_plane, intersect_point
    trans_group = TransactionGroup(doc, 'Tên Tran')
    trans_group.Start()

    try:
        t = Transaction(doc, 'Đổi level giằng theo dầm dốc')
        t.Start()
        Autodesk.Revit.DB.Structure.StructuralFramingUtils.DisallowJoinAtEnd(beam, 0)
        Autodesk.Revit.DB.Structure.StructuralFramingUtils.DisallowJoinAtEnd(beam, 1)
        beam.Location.Curve = transformed_line
        t.Commit()
        t2 = Transaction(doc, 'Đổi level giằng theo dầm dốc')
        t2.Start()
        start = module.get_builtin_parameter_by_name(beam, DB.BuiltInParameter.START_EXTENSION)
        end = module.get_builtin_parameter_by_name(beam, DB.BuiltInParameter.END_EXTENSION)
        start.Set(0)
        end.Set(0)
        t2.Commit()
        trans_group.Assimilate()
    except:
        trans_group.RollBack()
        pass
except:
    pass