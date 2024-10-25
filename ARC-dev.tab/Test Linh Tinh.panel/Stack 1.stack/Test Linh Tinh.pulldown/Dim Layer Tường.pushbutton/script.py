# -*- coding: utf-8 -*-
from codecs import Codec
import string
import importlib
import Autodesk
from Autodesk.Revit.DB import *
import Autodesk.Revit.DB as DB
from System.Collections.Generic import List
from Autodesk.Revit.UI.Selection import ObjectType
import traceback
import sys
import os
pyrevit_extensions_path = os.path.join(os.getenv('APPDATA'), 'pyRevit', 'Extensions')

nances_lib_path = os.path.join(pyrevit_extensions_path, 'ARC extension.extension', 'lib')
# Thêm đường dẫn vào sys.path nếu chưa có
if nances_lib_path not in sys.path:
    sys.path.append(nances_lib_path)
    import nances as module
if module.AutodeskData():
    import math
    from rpw import ui
    from rpw.ui.forms import Alert
    #Get UIDocument
    uidoc = __revit__.ActiveUIDocument
    #Get Document 
    doc = uidoc.Document
    Currentview = doc.ActiveView
    Curve = []

    def get_wall_reference_by_magic(doc,uid,index):
        format = "{0}:{1}:{2}"
        nines = -9999
        ref_string = str.Format(format,uid,nines,index)

        # exterior_face = str.Format(format,uid,nines,6)
        # core_exterior = str.Format(format,uid,nines,2)
        # wall_centre = str.Format(format,uid,nines,1)
        # core_centre = str.Format(format,uid,nines,4)
        # core_interior = str.Format(format,uid,nines,3)
        # interior_face = str.Format(format,uid,nines,5)

        reference = Reference.ParseFromStableRepresentation(doc,ref_string)
        return reference

    def create_plane_from_point_and_normal(point, normal):
        plane = Plane(normal, point)
        return plane

    def are_planes_parallel(normal1, normal2):
        tolerance=0.0000001
        cross_product = normal1.CrossProduct(normal2)
        return cross_product.GetLength() < tolerance

    def distance_between_planes(normal1, point_on_plane1, normal2):
        vector_between_planes = point_on_plane1 - (point_on_plane1.DotProduct(normal2) - normal2.DotProduct(normal1)) / normal1.DotProduct(normal2) * normal1
        distance = vector_between_planes.GetLength()
        return distance
    def get_point_at_center_line(wall):
        wall_location = wall.Location
        wall_location_curve = wall_location.Curve
        start_point = wall_location_curve.GetEndPoint(0)
        return start_point
    def get_center_plane (wall):
        wall_location = wall.Location
        wall_location_curve = wall_location.Curve
        start_point = wall_location_curve.GetEndPoint(0)
        endpoint = wall_location_curve.GetEndPoint(1)
        mid_point = wall_location_curve.Evaluate(0.5, True)
        offset_mid_point = XYZ(start_point.X, start_point.Y, mid_point.Z +10000)
        point1 = start_point
        point2 = endpoint
        point3 =offset_mid_point
        vector1 = point2 - point1
        vector2 = point3 - point1
        normal_vector = vector1.CrossProduct(vector2).Normalize()
        plane = Plane.CreateByNormalAndOrigin(normal_vector, mid_point)
        return plane
    
    def get_geometry(element):
        option = Options()
        option.ComputeReferences = True
        geo_ref =  element.get_Geometry(option)
        return geo_ref

    def get_face(geometry):
        for solid in geometry:
            face = solid.Faces
        return face

    def distance_to_plane(point, plane):
        distance = plane.Normal.DotProduct(point - plane.Origin)
        return distance
    def distance_between_parallel_planes(plane1, plane2):
        point_on_plane = XYZ(0, 0, 0)
        distance1 = abs(distance_to_plane(point_on_plane, plane1))
        distance2 = abs(distance_to_plane(point_on_plane, plane2))
        distance = (distance1 - distance2)
        return distance

    def get_rotate_90_location_wall (wall):
        from Autodesk.Revit.DB import Line, BuiltInParameter
        wall_location = wall.Location
        wall_location_curve = wall_location.Curve
        mid_point = wall_location_curve.Evaluate(0.5, True)
        Z_point = XYZ(mid_point.X, mid_point.Y, mid_point.Z + 10)
        Z_axis = Line.CreateBound(mid_point, Z_point)
        curve_of_location_curve = Line.CreateBound(wall_location_curve.GetEndPoint(0),wall_location_curve.GetEndPoint(1))
        detail_curve_of_location_curve = doc.Create.NewDetailCurve(Currentview,curve_of_location_curve)
        locate_detail_curve_of_location_curve = detail_curve_of_location_curve.Location
        rotate_locate_detail_curve_of_location_curve = locate_detail_curve_of_location_curve.Rotate(Z_axis, 2 * math.pi / 4)
        direction_of_wall = wall_location_curve.Direction
        Scale = Currentview.Scale
        Snap_dim = 4.75 * 0.0032808 #1mm bang 0.0032808feet
        Vector_for_scale = Snap_dim * Scale *direction_of_wall
        # move_detail_curve = locate_detail_curve_of_location_curve.Move(Vector_for_scale)
        return detail_curve_of_location_curve
            

    # def get_wall_reference_by_magic(uid,index):
    #     format = "{0}:{1}:{2}"
    #     nine = -9999
    #     refString = str.Format(format,uid,nine,index)
    #     return refString


    def get_wall_reference_by_type(uid,index):
        from Autodesk.Revit.DB import Reference
        format = "{0}:{1}:{2}"
        type = 'SURFACE'
        refString = str.Format(format,uid,index,type)
        return refString

    def move_point_along_vector(point, vector, distance):
        new_point = point + vector.Normalize() * distance
        return new_point

    def width(wall):
        type = doc.GetElement(wall.GetTypeId())
        para_width = type.get_Parameter(BuiltInParameter.WALL_ATTR_WIDTH_PARAM).AsValueString()
        # para_width.Set(height_offset)
        return para_width

    def get_all_grid():
        collector = FilteredElementCollector(doc).OfClass(Grid)
        grids = collector.ToElements()
        return grids
    def get_all_geometry_of_grids(grid, DatumExtentType = DatumExtentType.ViewSpecific):
        all_geometry = []
        DatumExtentType = DatumExtentType.ViewSpecific
        geometry_element = grid.GetCurvesInView(DatumExtentType,Currentview)
        all_geometry.append(geometry_element)
        return all_geometry

    def create_plane_follow_line (curve):
        start_point = curve.GetEndPoint(0)
        endpoint = curve.GetEndPoint(1)
        mid_point = curve.Evaluate(0.5, True)
        offset_mid_point = XYZ(start_point.X, start_point.Y, mid_point.Z +10000)
        point1 = start_point
        point2 = endpoint
        point3 =offset_mid_point
        vector1 = point2 - point1
        vector2 = point3 - point1
        normal_vector = vector1.CrossProduct(vector2).Normalize()
        plane = Plane.CreateByNormalAndOrigin(normal_vector, mid_point)
        return plane

    Ele = module.get_elements(uidoc,doc, "Select Wall", noti = False)
    if Ele:
        t = Transaction(doc,"Dimension wall (centered)")
        t.Start()
        for wall in Ele:

            unique_id = wall.UniqueId
            detail_line = get_rotate_90_location_wall (wall)
            line = detail_line.Location.Curve
            clone_curve = line.Clone()
            delete_detail_curve = doc.Delete(detail_line.Id)
            # Cai nay de dim 
            wall_reference = ReferenceArray()

            ref_core_exterior = get_wall_reference_by_magic(doc, unique_id, 2)
            ref_core_interior = get_wall_reference_by_magic(doc, unique_id, 3)
            # ref_exterior_face = get_wall_reference_by_magic(doc, unique_id, 3)
            # ref_interior_face = get_wall_reference_by_magic(doc, unique_id, 5)

            # exterior_face = str.Format(format,uid,nines,6)
            # core_exterior = str.Format(format,uid,nines,2)
            # wall_centre = str.Format(format,uid,nines,1)
            # core_centre = str.Format(format,uid,nines,4)
            # core_interior = str.Format(format,uid,nines,3)
            # interior_face = str.Format(format,uid,nines,5)

            wall_reference.Append(ref_core_exterior)
            wall_reference.Append(ref_core_interior)
            # wall_reference.Append(ref_exterior_face)
            # wall_reference.Append(ref_interior_face)
            
            try:
                dim = doc.Create.NewDimension(Currentview, clone_curve, wall_reference)
                curve_dim_direction = dim.Curve.Direction
                seg_1_position = dim.Segments.Item[0].TextPosition 
                seg_2_position = dim.Segments.Item[1].TextPosition
                seg_1_value = float(dim.Segments.Item[0].Value * 304.8)
                seg_2_value = float(dim.Segments.Item[1].Value * 304.8)
                round_format_value_1 = round(seg_1_value,2)
                round_format_value_2 = round(seg_2_value,2)
                formatted_value_1 = str(round_format_value_1).rstrip('0').rstrip('.')
                formatted_value_2 = str(round_format_value_2).rstrip('0').rstrip('.')
                len_formatted_value_1 = len(formatted_value_1)
                len_formatted_value_2 = len(formatted_value_2)
                one_unit_width = 2 #Chieu rong 1 don vi text
                width_text_1 = float(len_formatted_value_1 * one_unit_width * (Currentview.Scale))
                width_text_2 = float(len_formatted_value_2 * one_unit_width * (Currentview.Scale))
                total_value = seg_1_value + seg_2_value
                ti_le_1 = seg_1_value / (total_value)
                ti_le_2 = seg_2_value / (total_value)
                width_1 = float(width(wall))
                if (width_1)/2 < width_text_1: 
                    width_offset_text_1 = (width_1/2 + width_text_1/2)
                else:
                    width_offset_text_1 = 0
                move_seg_1 = move_point_along_vector(seg_1_position,curve_dim_direction, -(width_offset_text_1/304.8))
                if (width_1)/2 < width_text_2: 
                    width_offset_text_2 = (width_1/2 + width_text_2/2)
                else:
                    width_offset_text_2 = 0            
                move_seg_2 = move_point_along_vector(seg_2_position,curve_dim_direction, (width_offset_text_2/304.8))
                dim.Segments.Item[0].TextPosition = move_seg_1
                dim.Segments.Item[1].TextPosition = move_seg_2
                leader_dim = dim.get_Parameter(BuiltInParameter.DIM_LEADER)
                leader_dim.Set(False)
                # delete_detail_curve = doc.Delete(detail_line.Id)
            except:
                # delete_detail_curve = doc.Delete(detail_line.Id)
                continue

        t.Commit()





