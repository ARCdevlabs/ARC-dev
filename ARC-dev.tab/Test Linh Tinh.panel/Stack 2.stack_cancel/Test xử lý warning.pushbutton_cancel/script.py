
# -*- coding: utf-8 -*-
__doc__ = 'nguyenthanhson1712@gmail.com'
__author__ = 'NguyenThanhSon' "Email: nguyenthanhson1712@gmail.com"
import string
import importlib
ARC = string.ascii_lowercase
begin = "".join(ARC[i] for i in [13, 0, 13, 2, 4, 18])
module = importlib.import_module(str(begin))
import Autodesk
from Autodesk.Revit.DB import *
import Autodesk.Revit.DB as DB
from System.Collections.Generic import *
import traceback
from pyrevit import revit

uidoc = __revit__.ActiveUIDocument
doc = uidoc.Document

active_view = module.Active_view(doc)


def get_all_element_of_category_in_view (idoc, view, builtin_category):
    # Đây là sample của OST_category
    # category_filter = ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming)

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



level_of_view = active_view.GenLevel

id_level_of_view = level_of_view.Id

all_area_schemes = FilteredElementCollector(doc).OfClass(AreaScheme).ToElements()

for tung_area_scheme in all_area_schemes:
    area_scheme = tung_area_scheme



# t1 = Transaction (doc, "Tạo slab (tạo plan cho area và đặt area separation)")




# from Autodesk.Revit.DB import IFailuresPreprocessor, FailureProcessingResult, BuiltInFailures


# def PreprocessFailures(failuresAccessor):
#     # Lấy tất cả các cảnh báo
#     failList = failuresAccessor.GetFailureMessages()
    
#     for failure in failList:
#         # Lấy FailureDefinitionId để kiểm tra loại cảnh báo
#         failID = failure.GetFailureDefinitionId()
#         # Ngăn Revit hiển thị cảnh báo phòng không được bao quanh
#         print failID
    


# options = t1.GetFailureHandlingOptions()
# options.GetFailuresPreprocessor()
# # options.SetFailuresPreprocessor()
# options.SetDelayedMiniWarnings(True)
# khai_bao_GetDelayedMiniWarnings = options.GetDelayedMiniWarnings()

# khai_bao_FailuresAccessor = DeleteWarning.GetDescriptionText

# khai_bao_IFailuresPreprocessor = Autodesk.Revit.DB.IFailuresPreprocessor.PreprocessFailures(Autodesk.Revit.DB.FailuresAccessor.GetFailureMessages(t1))

# khai_bao_GetFailuresPreprocessor = options.SetFailuresPreprocessor(IFailuresPreprocessor)

# t1.SetFailureHandlingOptions(options)
# print (khai_bao_GetFailuresPreprocessor)
# print type(options)



with revit.Transaction("your_wish_is_my_command", swallow_errors=True):
    area_plan = create_new_area_plan(doc,area_scheme.Id,id_level_of_view)   
    
    level_plane = level_of_view.GetPlaneReference()

    sketch_plane = SketchPlane.Create(doc, level_plane)

    curve_array = Autodesk.Revit.DB.CurveArray()

    list_area_separation = []
    list_area = []

    all_beam_in_view = get_all_element_of_category_in_view (doc, active_view, BuiltInCategory.OST_StructuralFraming)

    for i in all_beam_in_view:

        beam_id = i.Id

        curve = i.Location.Curve

        curve_direction = curve.Direction

        start_point = curve.GetEndPoint(0)

        end_point = curve.GetEndPoint(1)

        line = Autodesk.Revit.DB.Line.CreateBound(XYZ(start_point.X,start_point.Y,0),XYZ(end_point.X,end_point.Y,0))

        curve_array.Append(line)

    for cur in curve_array:
        area_separation = doc.Create.NewAreaBoundaryLine(sketch_plane, cur, area_plan)
        list_area_separation.append(area_separation)

