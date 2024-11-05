# -*- coding: utf-8 -*-
from codecs import Codec
import string
import Autodesk
from Autodesk.Revit.DB import CurveLoop, XYZ,TransactionGroup,Transaction
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
    import nances as module
    from nances import revit, forms
if module.AutodeskData():
    uidoc = __revit__.ActiveUIDocument
    doc = uidoc.Document
    active_view = module.Active_view(doc)

    '''
    def create_slabs_with_arrow(idoc, CurveLoop, floor_type, offset, level_Id, is_struc, slope_arrow, slope):
        slab = Autodesk.Revit.DB.Floor.Create(idoc, CurveLoop, floor_type, level_Id,is_struc, slope_arrow, slope)
        param = module.get_builtin_parameter_by_name(slab, DB.BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM)
        param.Set(offset)
        return slab


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

    def change_specify_of_floor (arrow):
        param_set = arrow.Parameters
        for param in param_set:
            try:
                if str(param.AsValueString()) == "Slope" or str(param.AsValueString()) == "勾配":
                    set_para = param.Set(0)
                    # 1 la slope, 0 la Height at tail
            except:
                pass
        return 
    '''

    def get_slope_arrow (idoc, slope_floor):
        sketch_id = slope_floor.SketchId
        sketch = idoc.GetElement(sketch_id)
        all_sketch = sketch.GetAllElements()
        for line_id in all_sketch:
            try:
                line = doc.GetElement(line_id)
                line_name = line.Name
                if line_name == "Slope Arrow" or line_name == "勾配矢印":
                    arrow_element = line
            except:
                pass
        return arrow_element


    ele = module.get_elements(uidoc,doc, "Select all floors need to make slope", noti = False)

    
    with forms.WarningBar(title="Select slope floor"):
        try:
            pick = uidoc.Selection.PickObject(ObjectType.Element)
        except:
            import sys
            sys.exit()
       
        covert_reference_to_element = []
        
        element_id = pick.ElementId
       
        slope_floor_goc = doc.GetElement(element_id)

        '''
        XY_plane = Autodesk.Revit.DB.Plane.CreateByThreePoints(XYZ(0,0,0),XYZ(0,1,0), XYZ(1,1,0))
        
        ref_top_face = Autodesk.Revit.DB.HostObjectUtils.GetTopFaces(slope_floor_goc)
        
        for top_face in ref_top_face:
            surface = slope_floor_goc.GetGeometryObjectFromReference(top_face).GetSurface()
        
        angle = angle_between_planes (surface, XY_plane)
        
        radian = degrees_to_radians(180 - angle)

        '''
        
        sketch_id = slope_floor_goc.SketchId
        
        sketch = doc.GetElement(sketch_id)
        
        all_sketch = sketch.GetAllElements()
        
        para_floor_offset = module.get_builtin_parameter_by_name(slope_floor_goc, DB.BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM)
        
        floor_offset = para_floor_offset.AsDouble()

        level_id = slope_floor_goc.LevelId

        for line_id in all_sketch:

            line = doc.GetElement(line_id)

            line_name = line.Name

            if line_name == "Slope Arrow" or line_name == "勾配矢印":

                model_line_slope = line

                line_slope = model_line_slope.Location.Curve

                arrow_element = line

                para_specify = module.get_builtin_parameter_by_name(arrow_element, DB.BuiltInParameter.SPECIFY_SLOPE_OR_OFFSET)
                
                para_height_tail = module.get_builtin_parameter_by_name(arrow_element, DB.BuiltInParameter.SLOPE_START_HEIGHT)

                height_tail = para_height_tail.AsDouble()

                para_level_tail = module.get_builtin_parameter_by_name(arrow_element, DB.BuiltInParameter.SLOPE_ARROW_LEVEL_START)
            
                para_height_head = module.get_builtin_parameter_by_name(arrow_element, DB.BuiltInParameter.SLOPE_END_HEIGHT)

                height_head = para_height_head.AsDouble()
            
                para_level_head = module.get_builtin_parameter_by_name(arrow_element, DB.BuiltInParameter.SLOPE_ARROW_LEVEL_END)

                para_level_head.AsElementId()
        
        trans_group = TransactionGroup(doc, "Create slope floor")
        trans_group.Start()
        list_new_slab =[]

        with revit.Transaction("Tạm sửa vị trí mũi tên", swallow_errors=True):
            for tung_san in ele:

                '''Setting mũi tên dốc theo một curve có sẵn'''
                slope_arrow_moi = get_slope_arrow (doc,tung_san)

                offset_line_slope = line_slope.CreateOffset(1,XYZ(0,0,1))

                slope_arrow_moi.SetGeometryCurve(offset_line_slope,True)

        with revit.Transaction("Tạo sàn dốc", swallow_errors=True):
            for tung_san in ele:

                '''Setting mũi tên dốc theo một curve có sẵn'''
                slope_arrow_moi = get_slope_arrow (doc,tung_san)

                slope_arrow_moi.SetGeometryCurve(line_slope,True)

                para_specify_moi = module.get_builtin_parameter_by_name(slope_arrow_moi, DB.BuiltInParameter.SPECIFY_SLOPE_OR_OFFSET)

                para_specify_moi.Set(para_specify.AsInteger())

                if para_specify_moi.AsInteger() == 0:

                    para_height_tail_moi = module.get_builtin_parameter_by_name(slope_arrow_moi, DB.BuiltInParameter.SLOPE_START_HEIGHT)

                    para_height_tail_moi.Set(height_tail)

                    para_level_tail_moi = module.get_builtin_parameter_by_name(slope_arrow_moi, DB.BuiltInParameter.SLOPE_ARROW_LEVEL_START)

                    para_level_tail_moi.Set(para_level_tail.AsElementId())

                    para_height_head_moi = module.get_builtin_parameter_by_name(slope_arrow_moi, DB.BuiltInParameter.SLOPE_END_HEIGHT)

                    para_height_head_moi.Set(height_head)

                    para_level_head_moi = module.get_builtin_parameter_by_name(slope_arrow_moi, DB.BuiltInParameter.SLOPE_ARROW_LEVEL_END)

                    para_level_head_moi.Set(para_level_head.AsElementId())

                else:

                    para_height_tail_moi = module.get_builtin_parameter_by_name(slope_arrow_moi, DB.BuiltInParameter.SLOPE_START_HEIGHT)

                    para_height_tail_moi.Set(height_tail)

                    para_level_tail_moi = module.get_builtin_parameter_by_name(slope_arrow_moi, DB.BuiltInParameter.SLOPE_ARROW_LEVEL_START)

                    para_level_tail_moi.Set(para_level_tail.AsElementId())

    trans_group.Assimilate()