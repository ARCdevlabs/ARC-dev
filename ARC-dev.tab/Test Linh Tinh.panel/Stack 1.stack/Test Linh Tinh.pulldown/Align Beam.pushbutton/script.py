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
pyrevit_extensions_path = os.path.join(os.getenv('APPDATA'), 'pyRevit', 'Extensions')
nances_lib_path = os.path.join(pyrevit_extensions_path, 'ARC extension.extension', 'lib')
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

def plane_from_element(element):
    location_detail_line = element.Location.Curve
    start_point = location_detail_line.GetEndPoint(0)
    end_point = location_detail_line.GetEndPoint(1)
    XYZ_plane = Autodesk.Revit.DB.Plane.CreateByThreePoints(start_point,end_point, XYZ(end_point.X,end_point.Y,1.23456789))
    return XYZ_plane

uidoc = __revit__.ActiveUIDocument
doc = uidoc.Document
active_view = module.Active_view(doc)

pick_edge = uidoc.Selection.PickObject(ObjectType.Edge)
if pick_edge:
    beam = doc.GetElement(pick_edge)
    edge = beam.GetGeometryObjectFromReference(pick_edge)
    edge_curve = edge.AsCurve()
    start_point_edge_curve = edge_curve.GetEndPoint(0)
    end_point_edge_curve = edge_curve.GetEndPoint(1)
if beam: 
    try:
        detail_line = module.get_element(uidoc,doc, "Pick Detail Line", noti = False)
    except:
        sys.exit()

    detail_line_plane = plane_from_element(detail_line[0])
    beam_plane = plane_from_element(beam)
    location_beam = beam.Location.Curve
    start_point = location_beam.GetEndPoint(0)
    end_point = location_beam.GetEndPoint(1)
    degree = angle_between_planes (detail_line_plane,beam_plane)
    radian = module.degrees_to_radians(degree)
    start_distance = module.distance_from_point_to_plane(start_point,detail_line_plane)
    start_distance_from_edge = module.distance_from_point_to_plane(start_point_edge_curve,beam_plane)

    end_distance = module.distance_from_point_to_plane(end_point,detail_line_plane)
    end_distance_from_edge = module.distance_from_point_to_plane(end_point_edge_curve,beam_plane)

    t = Transaction (doc, "Align Beam")
    t.Start()
    try:
        yz_jus = module.get_builtin_parameter_by_name(beam, DB.BuiltInParameter.YZ_JUSTIFICATION)

        yz_jus.Set(1)

        gia_tri_start = (start_distance_from_edge-start_distance)/math.cos(radian)

        gia_tri_end = (end_distance_from_edge-end_distance)/math.cos(radian)

        get_para_start = module.get_builtin_parameter_by_name(beam, DB.BuiltInParameter.START_Y_OFFSET_VALUE)

        get_para_start.Set(gia_tri_start)

        get_para_end = module.get_builtin_parameter_by_name(beam, DB.BuiltInParameter.END_Y_OFFSET_VALUE)

        get_para_end.Set(gia_tri_end)

    except:
        print(traceback.format_exc())
        pass
    t.Commit()
# trans_group.Assimilate()