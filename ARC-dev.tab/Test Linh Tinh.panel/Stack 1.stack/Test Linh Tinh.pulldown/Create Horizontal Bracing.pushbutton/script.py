
# # -*- coding: utf-8 -*-
# import string
# import importlib

# import Autodesk.Revit
# import Autodesk.Revit.DB
# ARC = string.ascii_lowercase
# begin = "".join(ARC[i] for i in [13, 0, 13, 2, 4, 18])
# module = importlib.import_module(str(begin))
# import Autodesk
# from Autodesk.Revit.DB import *
# import Autodesk.Revit.DB as DB
# from System.Collections.Generic import *
# import traceback
# import sys
# if module.AutodeskData():
#     uidoc = __revit__.ActiveUIDocument
#     doc = uidoc.Document
#     active_view = module.Active_view(doc)

#     def all_type_of_floor():
#         all_type_of_floor = FilteredElementCollector(doc).OfClass(FloorType).OfCategory(BuiltInCategory.OST_Floors)
#         return all_type_of_floor

#     def get_all_element_of_category_in_view (idoc, view, builtin_category):
#         # Đây là sample của OST_category
#         # category_filter = ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming)

#         category_filter = DB.ElementCategoryFilter(builtin_category)
#         beam_collector = DB.FilteredElementCollector(idoc, view.Id).WherePasses(category_filter)
#         return beam_collector

#     def create_new_area_plan (idoc, area_scheme_id, level_id):
#         new_area_plan = Autodesk.Revit.DB.ViewPlan.CreateAreaPlan(idoc, area_scheme_id, level_id)
#         return new_area_plan

#     def create_new_area_boundary( idoc, sketchPlane, geometryCurve, areaView):
#         new_area_boundary = idoc.NewAreaBoundaryLine(sketchPlane,geometryCurve, areaView)
#         return new_area_boundary

#     def create_area (idoc, area_view, uv_point):
#         area = idoc.Create.NewArea(area_view, uv_point)
#         return area

#     list_floor = all_type_of_floor()
#     from rpw.ui.forms import (FlexForm, Label, ComboBox, TextBox, TextBox,
#                                 Separator, Button, CheckBox)
#     components = [Label('Select type of floor:'),
#                     ComboBox('combobox1', [Autodesk.Revit.DB.Element.Name.GetValue(x) for x in list_floor]),
#                     Label('Height offset:'),
#                     TextBox('textbox1', Text="0"),
#                     Separator(),
#                     Button('Create Slab')]
#     form = FlexForm('ARC', components)
#     form.show()
#     form.values
#     try:
#         selected_floor_type = form.values["combobox1"]
#         height_offset = form.values["textbox1"]
#     except: 
#         # print(traceback.format_exc())
#         sys.exit()
#     level_of_view = active_view.GenLevel

#     id_level_of_view = level_of_view.Id

#     all_area_schemes = FilteredElementCollector(doc).OfClass(AreaScheme).ToElements()

#     area_scheme = all_area_schemes[0] #Lấy area scheme đầu tiên

#     trans_group = TransactionGroup(doc, "Create slab")
#     trans_group.Start()

#     from pyrevit import revit

#     with revit.Transaction("Tạo area plan và đặt area separation", swallow_errors=True):

#         area_plan = create_new_area_plan(doc,area_scheme.Id,id_level_of_view)   
        
#         level_plane = level_of_view.GetPlaneReference()

#         sketch_plane = SketchPlane.Create(doc, level_plane)

#         curve_array = Autodesk.Revit.DB.CurveArray()

#         list_area_separation = []
#         list_area = []

#         all_beam_in_view = get_all_element_of_category_in_view (doc, active_view, BuiltInCategory.OST_StructuralFraming)

#         for i in all_beam_in_view:
#             try:

#                 beam_id = i.Id

#                 curve = i.Location.Curve

#                 start_point = curve.GetEndPoint(0)

#                 end_point = curve.GetEndPoint(1)

#                 line = Autodesk.Revit.DB.Line.CreateBound(XYZ(start_point.X,start_point.Y,0),XYZ(end_point.X,end_point.Y,0))

#                 curve_array.Append(line)

#             except:
#                 # print(traceback.format_exc())
#                 pass

#         for cur in curve_array:
#             area_separation = doc.Create.NewAreaBoundaryLine(sketch_plane, cur, area_plan)
#             list_area_separation.append(area_separation)

#     try:
#         def main():
#             # Bat dau vong lap lua chon
#             while True:
#                 try:

#                     def create_slab(area, floor_type, offset, level_Id):
#                         try:

#                             area_boundaries = area.GetBoundarySegments(SpatialElementBoundaryOptions())

#                             curve_loop_list  = List[CurveLoop]()

#                             for area_boundary in area_boundaries:
#                                 area_curve_loop = CurveLoop()
#                                 for boundary_segment in area_boundary:
#                                     curve = boundary_segment.GetCurve()
#                                     area_curve_loop.Append(curve)
#                                 curve_loop_list.Add(area_curve_loop)

#                             if curve_loop_list:

#                                 slab = Autodesk.Revit.DB.Floor.Create(doc, curve_loop_list, floor_type, level_Id)

#                                 param = slab.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM)

#                                 param_struc = slab.get_Parameter(BuiltInParameter.FLOOR_PARAM_IS_STRUCTURAL)

#                                 param_struc.Set(1)

#                                 param.Set(offset)

#                             return slab
#                         except:
#                             return None
                                        
#                     with revit.Transaction("tạo area khi pick vào ô dầm", swallow_errors=True):
#                         try:

#                             return_point = module.pick_point_with_nearest_snap(uidoc)

#                         except:
                            
#                             return_point = False

#                         if return_point != False:
                            
#                             UV_point = UV(return_point.X,return_point.Y)

#                             new_area = create_area(doc,area_plan, UV_point)
#                             list_area.append(new_area)

#                         else:

#                             new_area = None


#                     list_type_floors = module.all_type_of_class_and_OST(doc, FloorType, BuiltInCategory.OST_Floors)
#                     for floor in list_type_floors:
#                         type_name = Autodesk.Revit.DB.Element.Name.GetValue(floor)
#                         if str(type_name) == str(selected_floor_type):
#                             type_floor = floor
#                     type_floor_id = type_floor.Id
       
#                     if new_area != None:
#                         try:
#                             height_offset_float = float(height_offset)/304.8
#                         except:
#                             height_offset_float = 0
#                         with revit.Transaction("Tạo sàn từ area", swallow_errors=True):

#                             create_slab (new_area, type_floor_id, height_offset_float, id_level_of_view)

#                     else:
#                         break

#                 except Exception as ex:
#                     # print(traceback.format_exc())
#                     if "Operation canceled by user." in str(ex):
#                         break
#                     else:
#                         break
#         main()

#     except:
#         # print(traceback.format_exc())
#         pass
#     try:
#         t6 = Transaction (doc, "Xóa Area Plan")
#         t6.Start()
#         doc.Delete(area_plan.Id)
#         for tung_area in list_area:
#             try:
#                 doc.Delete(tung_area.Id)
#             except:
#                 pass
#         for tung_area_sp in list_area_separation:
#             try:
#                 doc.Delete(tung_area_sp.Id)
#             except:
#                 pass
#         t6.Commit()
#     except:
#         pass
#     trans_group.Assimilate()



# -*- coding: utf-8 -*-
import string
import importlib

import Autodesk.Revit
import Autodesk.Revit.DB
ARC = string.ascii_lowercase
begin = "".join(ARC[i] for i in [13, 0, 13, 2, 4, 18])
module = importlib.import_module(str(begin))
import Autodesk
from Autodesk.Revit.DB import *
import Autodesk.Revit.DB as DB
from System.Collections.Generic import *
import traceback
import sys
if module.AutodeskData():
    uidoc = __revit__.ActiveUIDocument
    doc = uidoc.Document
    active_view = module.Active_view(doc)

    def distance_3d(point1, point2):
        import math
        return math.sqrt(
            (point2.X - point1.X) ** 2 +
            (point2.Y - point1.Y) ** 2 +
            (point2.Z - point1.Z) ** 2)

    def find_corners(points):
        n = len(points)
        # Tính tọa độ trung bình
        x_avg = sum([p.X for p in points]) / n
        y_avg = sum([p.Y for p in points]) / n
        z_avg = sum([p.Z for p in points]) / n
        
        avg_point = XYZ(x_avg, y_avg, z_avg)

        # Tính khoảng cách từ trung tâm đến mỗi điểm
        distances = [(point, distance_3d(point, avg_point)) for point in points]
        
        # Sắp xếp các điểm theo khoảng cách giảm dần
        distances.sort(key=lambda x: x[1], reverse=True)
        
        # Lấy 4 điểm xa nhất, tức là 4 điểm góc
        corners = [d[0] for d in distances[:4]]
    
        return corners
    
    def cross_product_3d(v1, v2):
        # Tính tích chéo trong không gian 3D giữa hai vector v1 và v2
        return XYZ(
            v1.Y * v2.Z - v1.Z * v2.Y,
            v1.Z * v2.X - v1.X * v2.Z,
            v1.X * v2.Y - v1.Y * v2.X
        ) 
    
    def subtract_points(p1, p2):
        # Tạo vector bằng cách trừ điểm p2 khỏi điểm p1
        return XYZ(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z)
    
    def dot_product_3d(v1, v2):
        # Tính tích vô hướng giữa hai vector trong không gian 3D
        return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z
    
    def is_intersecting_3d(p1, q1, p2, q2):
        # Tạo các vector từ các đoạn thẳng
        v1 = subtract_points(q1, p1)
        v2 = subtract_points(q2, p2)
        
        # Tính vector pháp tuyến bằng tích chéo của v1 và v2
        normal = cross_product_3d(v1, v2)
        
        # Kiểm tra tích vô hướng giữa vector pháp tuyến và các vector từ p1, q1 đến p2
        vec_p1_p2 = subtract_points(p2, p1)
        
        # Nếu tích vô hướng = 0, các đoạn thẳng không giao nhau
        return dot_product_3d(normal, vec_p1_p2) == 0

    def find_diagonals_3d(points):
        A = points [0]
        B = points [1]
        C = points [2]
        D = points [3]
        
        # Kiểm tra các cặp điểm nào tạo thành đường chéo trong không gian 3D
        if is_intersecting_3d(A, C, B, D):
            return (A, C), (B, D)
        elif is_intersecting_3d(A, B, C, D):
            return (A, B), (C, D)
        elif is_intersecting_3d(A, D, B, C):
            return (A, D), (B, C)
        else:
            return None

    def all_type_of_floor():
        all_type_of_floor = FilteredElementCollector(doc).OfClass(FloorType).OfCategory(BuiltInCategory.OST_Floors)
        return all_type_of_floor

    def get_all_element_of_category_in_view (idoc, view, builtin_category):
        category_filter = DB.ElementCategoryFilter(builtin_category)
        beam_collector = DB.FilteredElementCollector(idoc, view.Id).WherePasses(category_filter)
        return beam_collector

    def create_new_area_plan (idoc, area_scheme_id, level_id):
        new_area_plan = Autodesk.Revit.DB.ViewPlan.CreateAreaPlan(idoc, area_scheme_id, level_id)
        return new_area_plan

    def create_new_area_boundary( idoc, sketchPlane, geometryCurve, areaView):
        new_area_boundary = idoc.NewAreaBoundaryLine(sketchPlane,geometryCurve, areaView)
        return new_area_boundary

    def create_area (idoc, area_view, uv_point):
        area = idoc.Create.NewArea(area_view, uv_point)
        return area

    list_floor = all_type_of_floor()

    # from rpw.ui.forms import (FlexForm, Label, ComboBox, TextBox, TextBox,
    #                             Separator, Button, CheckBox)
    # components = [Label('Select type of floor:'),
    #                 ComboBox('combobox1', [Autodesk.Revit.DB.Element.Name.GetValue(x) for x in list_floor]),
    #                 Label('Height offset:'),
    #                 TextBox('textbox1', Text="0"),
    #                 Separator(),
    #                 Button('Create Slab')]
    # form = FlexForm('ARC', components)
    # form.show()
    # form.values
    # try:
    #     selected_floor_type = form.values["combobox1"]
    #     height_offset = form.values["textbox1"]
    # except: 
    #     # print(traceback.format_exc())
    #     sys.exit()
    level_of_view = active_view.GenLevel

    id_level_of_view = level_of_view.Id

    all_area_schemes = FilteredElementCollector(doc).OfClass(AreaScheme).ToElements()

    area_scheme = all_area_schemes[0] #Lấy area scheme đầu tiên

    trans_group = TransactionGroup(doc, "Create slab")
    trans_group.Start()

    from pyrevit import revit

    with revit.Transaction("Tạo area plan và đặt area separation", swallow_errors=True):

        area_plan = create_new_area_plan(doc,area_scheme.Id,id_level_of_view)   
        
        level_plane = level_of_view.GetPlaneReference()

        sketch_plane = SketchPlane.Create(doc, level_plane)

        curve_array = Autodesk.Revit.DB.CurveArray()

        list_area_separation = []
        list_area = []

        all_beam_in_view = get_all_element_of_category_in_view (doc, active_view, BuiltInCategory.OST_StructuralFraming)

        for i in all_beam_in_view:
            try:

                beam_id = i.Id

                curve = i.Location.Curve

                start_point = curve.GetEndPoint(0)

                end_point = curve.GetEndPoint(1)

                line = Autodesk.Revit.DB.Line.CreateBound(XYZ(start_point.X,start_point.Y,0),XYZ(end_point.X,end_point.Y,0))

                curve_array.Append(line)

            except:
                # print(traceback.format_exc())
                pass

        for cur in curve_array:
            area_separation = doc.Create.NewAreaBoundaryLine(sketch_plane, cur, area_plan)
            list_area_separation.append(area_separation)

    try:
        def main():
            # Bat dau vong lap lua chon
            while True:
                try:

                    def create_slab(area):
                        try:
                            direction_all_bounding_line = []
                            area_boundaries = area.GetBoundarySegments(SpatialElementBoundaryOptions())                            
                            curve_loop_list  = List[CurveLoop]()
                            all_end_point = []
                            for area_boundary in area_boundaries:       
                                area_curve_loop = CurveLoop()
                                for boundary_segment in area_boundary:
                                    curve = boundary_segment.GetCurve()
                                    direction_all_bounding_line.append(str(curve.Direction)) 
                                    direction_curve = curve.Direction
                                    
                                    direction_all_bounding_line.append(str(abs(direction_curve.X)) + str(abs(direction_curve.Y)) + str(abs(direction_curve.Z)))      
                                    all_end_point.append(curve.GetEndPoint(0))                                    
                                    area_curve_loop.Append(curve)
                                curve_loop_list.Add(area_curve_loop)

                            corners = find_corners(all_end_point)

                            for i in range(len(corners)):
                                for j in range(i + 1, len(corners)):
                                    boun_line =  Autodesk.Revit.DB.Line.CreateBound(corners[i],corners[j])
                                    string_of_direction = str(abs(boun_line.Direction.X)) + str(abs(boun_line.Direction.Y)) + str(abs(boun_line.Direction.Z))
                                    if string_of_direction not in direction_all_bounding_line:
                                        
                                        detail_line = doc.Create.NewDetailCurve(active_view,boun_line)


                                
                            # if curve_loop_list:

                            #     slab = Autodesk.Revit.DB.Floor.Create(doc, curve_loop_list, floor_type, level_Id)

                            #     param = slab.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM)

                            #     param_struc = slab.get_Parameter(BuiltInParameter.FLOOR_PARAM_IS_STRUCTURAL)

                            #     param_struc.Set(1)

                            #     param.Set(offset)

                            return detail_line
                        except:
                            import traceback
                            print(traceback.format_exc())
                            return None
                                        
                    with revit.Transaction("tạo area khi pick vào ô dầm", swallow_errors=True):
                        try:

                            return_point = module.pick_point_with_nearest_snap(uidoc)

                        except:
                            
                            return_point = False

                        if return_point != False:
                            
                            UV_point = UV(return_point.X,return_point.Y)

                            new_area = create_area(doc,area_plan, UV_point)
                            list_area.append(new_area)
                        else:

                            new_area = None


                    # list_type_floors = module.all_type_of_class_and_OST(doc, FloorType, BuiltInCategory.OST_Floors)
                    # for floor in list_type_floors:
                    #     type_name = Autodesk.Revit.DB.Element.Name.GetValue(floor)
                    #     if str(type_name) == str(selected_floor_type):
                    #         type_floor = floor
                    # type_floor_id = type_floor.Id
       
                    if new_area != None:
                        # try:
                        #     height_offset_float = float(height_offset)/304.8
                        # except:
                        #     height_offset_float = 0
                        with revit.Transaction("Tạo sàn từ area", swallow_errors=True):

                            create_slab (new_area)
                    else:
                        break

                except Exception as ex:
                    # print(traceback.format_exc())
                    if "Operation canceled by user." in str(ex):
                        break
                    else:
                        break
        main()

    except:
        # print(traceback.format_exc())
        pass
    try:
        t6 = Transaction (doc, "Xóa Area Plan")
        t6.Start()
        doc.Delete(area_plan.Id)
        for tung_area in list_area:
            try:
                doc.Delete(tung_area.Id)
            except:
                pass
        for tung_area_sp in list_area_separation:
            try:
                doc.Delete(tung_area_sp.Id)
            except:
                pass
        t6.Commit()
    except:
        pass
    trans_group.Assimilate()


# -*- coding: utf-8 -*-
import string
import importlib
import Autodesk.Revit
import Autodesk.Revit.DB
ARC = string.ascii_lowercase
begin = "".join(ARC[i] for i in [13, 0, 13, 2, 4, 18])
module = importlib.import_module(str(begin))
import Autodesk
from Autodesk.Revit.DB import *
import Autodesk.Revit.DB as DB
from System.Collections.Generic import *
import traceback
import sys
if module.AutodeskData():
    uidoc = __revit__.ActiveUIDocument
    doc = uidoc.Document
    active_view = module.Active_view(doc)

    def distance_3d(point1, point2):
        import math
        return math.sqrt(
            (point2.X - point1.X) ** 2 +
            (point2.Y - point1.Y) ** 2 +
            (point2.Z - point1.Z) ** 2)

    def find_corners(points):
        n = len(points)
        # Tính tọa độ trung bình
        x_avg = sum([p.X for p in points]) / n
        y_avg = sum([p.Y for p in points]) / n
        z_avg = sum([p.Z for p in points]) / n
       
        avg_point = XYZ(x_avg, y_avg, z_avg)

        # Tính khoảng cách từ trung tâm đến mỗi điểm
        distances = [(point, distance_3d(point, avg_point)) for point in points]
        
        # Sắp xếp các điểm theo khoảng cách giảm dần
        distances.sort(key=lambda x: x[1], reverse=True)
        
        # Lấy 4 điểm xa nhất, tức là 4 điểm góc
        corners = [d[0] for d in distances[:4]]
    
        return corners
    
    def active_symbol(element):
        try:
            if element.IsActive == False:
                element.Activate()
        except:
            pass
        return 
    def create_beam(curve,beam_type,level, offset):
        active_symbol(beam_type)
        beam = doc.Create.NewFamilyInstance(curve, beam_type, level, Autodesk.Revit.DB.Structure.StructuralType.Beam)
        param_start_offset = module.get_builtin_parameter_by_name(beam, DB.BuiltInParameter.STRUCTURAL_BEAM_END0_ELEVATION)
        param_end_offset = module.get_builtin_parameter_by_name(beam, DB.BuiltInParameter.STRUCTURAL_BEAM_END1_ELEVATION)
        param_start_offset.Set(offset/304.8)
        param_end_offset.Set(offset/304.8)
        return beam

    def all_type_of_framing():
        all_type_of_framing = FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralFraming)
        return all_type_of_framing

    def get_all_element_of_category_in_view (idoc, view, builtin_category):
        category_filter = DB.ElementCategoryFilter(builtin_category)
        beam_collector = DB.FilteredElementCollector(idoc, view.Id).WherePasses(category_filter)
        return beam_collector

    def create_new_area_plan (idoc, area_scheme_id, level_id):
        new_area_plan = Autodesk.Revit.DB.ViewPlan.CreateAreaPlan(idoc, area_scheme_id, level_id)
        return new_area_plan

    def create_new_area_boundary( idoc, sketchPlane, geometryCurve, areaView):
        new_area_boundary = idoc.NewAreaBoundaryLine(sketchPlane,geometryCurve, areaView)
        return new_area_boundary

    def create_area (idoc, area_view, uv_point):
        area = idoc.Create.NewArea(area_view, uv_point)
        return area

    list_type_framing = all_type_of_framing()

    from rpw.ui.forms import (FlexForm, Label, ComboBox, TextBox, TextBox,
                                Separator, Button, CheckBox)
    components = [Label('Select type of Brace:'),
                    ComboBox('combobox1', [Autodesk.Revit.DB.Element.Name.GetValue(x) for x in list_type_framing]),
                    Label('Height offset:'),
                    TextBox('textbox1', Text="0"),
                    Separator(),
                    Button('Create Brace')]
    
    form = FlexForm('ARC', components)
    form.show()
    form.values

    try:
        height_offset = float(form.values["textbox1"])
        selected_framing_type = form.values["combobox1"]
    except: 
        sys.exit()

    level_of_view = active_view.GenLevel
    id_level_of_view = level_of_view.Id
    all_area_schemes = FilteredElementCollector(doc).OfClass(AreaScheme).ToElements()
    area_scheme = all_area_schemes[0] #Lấy area scheme đầu tiên

    # Lấy type của Brace.
    for framing_type in list_type_framing:
        type_name = Autodesk.Revit.DB.Element.Name.GetValue(framing_type)
        if str(type_name) == str(selected_framing_type):
            type_framing = framing_type
    
    

    trans_group = TransactionGroup(doc, "Create Horizontal Brace")
    trans_group.Start()

    from pyrevit import revit
    with revit.Transaction("Tạo area plan và đặt area separation", swallow_errors=True):

        area_plan = create_new_area_plan(doc,area_scheme.Id,id_level_of_view)   
        
        level_plane = level_of_view.GetPlaneReference()

        sketch_plane = SketchPlane.Create(doc, level_plane)

        curve_array = Autodesk.Revit.DB.CurveArray()

        list_area_separation = []

        list_area = []

        all_beam_in_view = get_all_element_of_category_in_view (doc, active_view, BuiltInCategory.OST_StructuralFraming)

        for i in all_beam_in_view:
            try:

                beam_id = i.Id

                curve = i.Location.Curve

                start_point = curve.GetEndPoint(0)

                end_point = curve.GetEndPoint(1)

                line = Autodesk.Revit.DB.Line.CreateBound(XYZ(start_point.X,start_point.Y,0),XYZ(end_point.X,end_point.Y,0))

                curve_array.Append(line)

            except:
                import traceback
                print(traceback.format_exc())
                pass

        for cur in curve_array:

            area_separation = doc.Create.NewAreaBoundaryLine(sketch_plane, cur, area_plan)

            list_area_separation.append(area_separation)
    try:
        def main():
        # Bat dau vong lap lua chon
            while True:
                try:

                    def create_horizontal_brace(input_area,type_beam,level,height):
                        try:
                            direction_all_bounding_line = []
                            all_end_point = []
                            area_boundaries = input_area.GetBoundarySegments(SpatialElementBoundaryOptions())
                            curve_loop_list  = List[CurveLoop]()
                            for area_boundary in area_boundaries:
                                area_curve_loop = CurveLoop()
                                for boundary_segment in area_boundary:
                                    curve = boundary_segment.GetCurve()
                                    direction_curve = curve.Direction
                                    direction_all_bounding_line.append(str(abs(direction_curve.X)) + str(abs(direction_curve.Y)) + str(abs(direction_curve.Z)))
                                    all_end_point.append(curve.GetEndPoint(0))
                                    area_curve_loop.Append(curve)
                                curve_loop_list.Add(area_curve_loop)
                            corners = find_corners(all_end_point)

                            for i in range(len(corners)):
                                for j in range(i + 1, len(corners)):
                                    boun_line =  Autodesk.Revit.DB.Line.CreateBound(corners[i],corners[j])
                                    string_of_direction = str(abs(boun_line.Direction.X)) + str(abs(boun_line.Direction.Y)) + str(abs(boun_line.Direction.Z))
                                    if string_of_direction not in direction_all_bounding_line:
                                        detail_line = doc.Create.NewDetailCurve(active_view,boun_line)                         
                                        new_beam = create_beam(boun_line, type_beam, level, height)   
                                                            
                            return detail_line
                        
                        except:
                            import traceback
                            print(traceback.format_exc())
                            return None
                                        
                    with revit.Transaction("Tạo area khi pick vào ô dầm", swallow_errors=True):
                        try:

                            return_point = module.pick_point_with_nearest_snap(uidoc)

                        except:
                            
                            return_point = False

                        if return_point != False:
                            
                            UV_point = UV(return_point.X,return_point.Y)

                            new_area = create_area(doc,area_plan, UV_point)

                            list_area.append(new_area)
                        else:

                            new_area = None


                    if new_area != None:
                        # try:
                        #     height_offset_float = float(height_offset)/304.8
                        # except:
                        #     height_offset_float = 0
                        with revit.Transaction("Tạo sàn từ area", swallow_errors=True):
                            
                            create_horizontal_brace (new_area,type_framing,level_of_view,height_offset)

                    else:
                        break

                except:
                    import traceback
                    print(traceback.format_exc())
                    break


        # Khởi tạo vòng lặp While cho tới khi dừng         
        main()


    except:
        # print(traceback.format_exc())
        pass
    try:
        t6 = Transaction (doc, "Xóa Area Plan")
        t6.Start()
        doc.Delete(area_plan.Id)
        for tung_area in list_area:
            try:
                doc.Delete(tung_area.Id)
            except:
                pass
        for tung_area_sp in list_area_separation:
            try:
                doc.Delete(tung_area_sp.Id)
            except:
                pass
        t6.Commit()
    except:
        pass
    trans_group.Assimilate()

