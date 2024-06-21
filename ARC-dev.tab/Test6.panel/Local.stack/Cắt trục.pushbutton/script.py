# -*- coding: utf8 -*-
__doc__ = 'chọn và lấy thông tin đối tượng'
from Autodesk.Revit.UI.Selection import ObjectType
from Autodesk.Revit.DB import *
import Autodesk.Revit.DB as DB
from Autodesk.Revit.DB import Line
import math
from rpw import ui
from rpw.ui.forms import Alert

# Get UIDocument
uidoc = __revit__.ActiveUIDocument

# Get Document 
doc = uidoc.Document

# Chọn đối tượng
pick = uidoc.Selection.PickObjects(ObjectType.Element)
list_element = []
for element in pick:
    ele = doc.GetElement(element.ElementId)
    list_element.append(ele)
# 	elementtype = doc.GetElement(ele.GetTypeId())

# 	# Lấy tọa độ của scopebox
# 	boun = ele.get_BoundingBox(doc.ActiveView)
# 	if boun:
# 		Diem_cao_nhat = boun.Max
# 		Diem_thap_nhat = boun.Min
# 		print("thông tin scope box:")
# 		print("Điểm cao nhất: ({0}, {1}, {2})".format(Diem_cao_nhat.X, Diem_cao_nhat.Y, Diem_cao_nhat.Z))
# 		print("Điểm thấp nhất: ({0}, {1}, {2})".format(Diem_thap_nhat.X, Diem_thap_nhat.Y, Diem_thap_nhat.Z))

# Lấy danh sách các lưới trục trong mô hình
all_grids = FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Grids).WhereElementIsNotElementType().ToElements()
list_grids = []
for grid in all_grids:
	list_grids.append(grid)

def Lay_thong_tin_truc(grid):
	curve = grid.Curve
	if isinstance(curve, Line):
		start_point = curve.GetEndPoint(0)
		end_point = curve.GetEndPoint(1)
		length = curve.Length
		return start_point, end_point, length
	return None, None, None

def lay_thong_tin_truc_Son(grids, view):
    DatumExtentType = DB.DatumExtentType.ViewSpecific
    for i in grids:
        try:
            # curve_in_current_view = i.GetCurvesInView(DatumExtentType,currentview)
            list_curve = i.GetCurvesInView(DatumExtentType,view)
            grid_name = i.Name
            curve = list_curve[0]
            #need to change to line in plan view because some floor is not on a plane.
            start_point = curve.GetEndPoint(0)
            end_point = curve.GetEndPoint(1)
            plan_start_point = XYZ(start_point.X, start_point.Y, 0)
            plan_end_point = XYZ(end_point.X, end_point.Y, 0)
            print "Tên lưới trục: {}      start point {}".format(grid_name, plan_start_point)
            print "Tên lưới trục: {}      end point {}".format(grid_name, plan_end_point) 
            # set_curve_in_view = i.SetCurveInView(DatumExtentType, currentview, L1)
        except:
            pass
    return
        
# Chọn các đối tượng grid thủ công
# selected_grids = uidoc.Selection.PickObjects(ObjectType.Element, "Chọn các lưới trục (Grids)")

lay_thong_tin_truc_Son(list_element,doc.ActiveView)


# In ra thông tin của từng Grid đã chọn
# print("Thông tin trục:")
# for grid_ref in selected_grids:
# 	grid = doc.GetElement(grid_ref.ElementId)
# 	start_point, end_point, length = Lay_thong_tin_truc(grid)
# 	if start_point and end_point:
# 		print("Grid Id: {0}".format(grid.Id))
# 		print("Start Point: ({0}, {1}, {2})".format(start_point.X, start_point.Y, start_point.Z))
# 		print("End Point: ({0}, {1}, {2})".format(end_point.X, end_point.Y, end_point.Z))
# 		print("Length: {0}".format(length))