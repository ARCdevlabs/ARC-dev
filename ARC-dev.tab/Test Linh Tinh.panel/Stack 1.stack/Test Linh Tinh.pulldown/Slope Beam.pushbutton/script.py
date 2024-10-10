# -*- coding: utf-8 -*-
import Autodesk
from Autodesk.Revit.DB import *
import Autodesk.Revit.DB as DB
from System.Collections.Generic import *
from Autodesk.Revit.UI.Selection import ObjectType

import sys
import traceback
import math
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
    from nances import forms
def angle_between_planes(plane1, plane2):
    import math
    normal1 = plane1.Normal
    normal2 = plane2.Normal
    dot_product = normal1.DotProduct(normal2)
    magnitude1 = normal1.GetLength()
    magnitude2 = normal2.GetLength()
    
    if magnitude1 == 0 or magnitude2 == 0:
        return None    
    cos_angle = dot_product / (magnitude1 * magnitude2)
    angle_rad = math.acos(cos_angle)
    angle_deg = math.degrees(angle_rad)
    return angle_deg


def degrees_to_radians(degrees):
    import math
    radians = degrees * (math.pi / 180)
    return radians

def pick_top_bot_beams(iuidoc, idoc):

    pick_1 = iuidoc.Selection.PickObject(ObjectType.Element)
    get_pick_1 = idoc.GetElement(pick_1.ElementId)
    location_curve_1 = get_pick_1.Location.Curve
    start_point_1 = location_curve_1.GetEndPoint(0)
    end_point_1 = location_curve_1.GetEndPoint(1)

    pick_2 = uidoc.Selection.PickObject(ObjectType.Element)
    get_pick_2 = doc.GetElement(pick_2.ElementId)
    location_curve_2 = get_pick_2.Location.Curve
    start_point_2 = location_curve_2.GetEndPoint(0)

    XYZ_plane = Autodesk.Revit.DB.Plane.CreateByThreePoints(start_point_1,end_point_1, start_point_2)

    return XYZ_plane

uidoc = __revit__.ActiveUIDocument
doc = uidoc.Document
active_view = module.Active_view(doc)

Ele = module.get_elements(uidoc,doc, "Pick Beams", noti = False)

if Ele: 
    with forms.WarningBar(title='Pick Top and Bottom Beam'):
        try:
            ref_plane = pick_top_bot_beams (uidoc,doc)
        except:
            sys.exit()

    # ref_plane = pick_top_bot_beams (uidoc,doc)

    XY_plane = Autodesk.Revit.DB.Plane.CreateByThreePoints(XYZ(0,0,0),XYZ(0,1,0), XYZ(1,1,0))
    degree = angle_between_planes (XY_plane,ref_plane)
    radian = module.degrees_to_radians(degree)

    t = Transaction (doc, "Slope Beam_Start and End Offset")
    t.Start()
    for i in Ele:
        if i.Category.Name == "Structural Columns":
            try:              
                top_level = module.get_builtin_parameter_by_name(i, DB.BuiltInParameter.SCHEDULE_TOP_LEVEL_PARAM)
                top_level = doc.GetElement(top_level.AsElementId())
                top_elevation = top_level.Elevation
                get_top_offset = module.get_builtin_parameter_by_name(i, DB.BuiltInParameter.SCHEDULE_TOP_LEVEL_OFFSET_PARAM)
                top_height = top_elevation
                location = i.Location.Point
                top_point= XYZ(location.X,location.Y,top_height)
                distance_top_point = module.distance_from_point_to_plane(top_point, ref_plane)
                H_projection_top = distance_top_point/math.cos(radian)
                get_top_offset.Set(H_projection_top)
            except:
                pass
        try:
            beam_location = i.Location.Curve
            start_point = beam_location.GetEndPoint(0)
            distance_start = module.distance_from_point_to_plane(start_point, ref_plane)
            H_projection_start = distance_start/math.cos(radian)

            get_value_start = module.get_builtin_parameter_by_name(i, DB.BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION)

            to_mm_start = get_value_start.AsDouble()

            set_param_start = module.get_builtin_parameter_by_name(i, DB.BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION).Set(H_projection_start + to_mm_start)

            end_point = beam_location.GetEndPoint(1)
            distance_end = module.distance_from_point_to_plane(end_point,ref_plane)
            H_projection_end = distance_end/math.cos(radian)

            get_value_end = module.get_builtin_parameter_by_name(i, DB.BuiltInParameter.STRUCTURAL_BEAM_END1_ELEVATION)
            to_mm_end = get_value_end.AsDouble()

            set_param_end = module.get_builtin_parameter_by_name(i, DB.BuiltInParameter.STRUCTURAL_BEAM_END1_ELEVATION).Set(H_projection_end + to_mm_end)
        except:
            pass
    t.Commit()

