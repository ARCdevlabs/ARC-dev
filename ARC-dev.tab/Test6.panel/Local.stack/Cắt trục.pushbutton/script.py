# -*- coding: utf8 -*-
__doc__ = 'Rút ngắn chiều dài grid và level'
from Autodesk.Revit.UI.Selection import ObjectType
from Autodesk.Revit.DB import *
from Autodesk.Revit.DB import Line
from rpw.ui.forms import Alert, TextInput, TaskDialog
from pyrevit import forms
import Autodesk
import sys

# Get UIDocument
uidoc = __revit__.ActiveUIDocument

# Get Document 
doc = uidoc.Document

# Lấy danh sách các đối tượng đã được chọn
selection = uidoc.Selection.GetElementIds()  # Trả về 1 list ElementId
if not selection:
    TaskDialog.Show("Thông báo", "Không có đối tượng nào được chọn. Dừng chương trình.")
    sys.exit()

# Hộp thoại nhập khoảng cách
distance_str = TextInput('Nhập kc điểm đầu cuối đến scope box (mm):', default='-1000')
if not distance_str:
    TaskDialog.Show("Thông báo", "Không có giá trị khoảng cách nào được nhập. Dừng chương trình.")
    sys.exit()

distance = float(distance_str) / 304.8  # Chuyển đổi từ mm sang feet

# Chọn Scope Box
with forms.WarningBar(title='Chọn crop view'):
    scope_box_ref = uidoc.Selection.PickObject(ObjectType.Element, "Chọn Scope Box")
scope_box = doc.GetElement(scope_box_ref.ElementId)
# Lấy tọa độ của Scope Box
scopebox_boun = scope_box.get_BoundingBox(doc.ActiveView)

# Lọc các đối tượng Grid và Level
grids = []
levels = []

for elem_id in selection:
    elem = doc.GetElement(elem_id)
    if elem.Category.Id.IntegerValue == int(BuiltInCategory.OST_Grids):
        grids.append(elem)
    elif elem.Category.Id.IntegerValue == int(BuiltInCategory.OST_Levels):
        levels.append(elem)

# Tính toán chiều dài của scope box
crop_length_x = scopebox_boun.Max.X - scopebox_boun.Min.X
crop_length_y = scopebox_boun.Max.Y - scopebox_boun.Min.Y

# Hàm thay đổi khoảng cách grid ở mặt bằng
def RUTNGAN_TRUC_MAT_BANG(grid, distance, scopebox_boun, view):
    datum_extent_type = Autodesk.Revit.DB.DatumExtentType.ViewSpecific
    list_curve = grid.GetCurvesInView(datum_extent_type, view)
    if list_curve:
        curve = list_curve[0]
        if isinstance(curve, Line):
            start_point = curve.GetEndPoint(0)
            end_point = curve.GetEndPoint(1)
            
            # Kiểm tra xem trục đứng hay trục ngang
            is_vertical = abs(start_point.X - end_point.X) < 0.01
            
            # Lấy tọa độ của Scope Box
            min_point = scopebox_boun.Min
            max_point = scopebox_boun.Max

            if is_vertical:
                # Điều chỉnh điểm đầu và điểm cuối của trục đứng
                new_start_point = XYZ(start_point.X, min_point.Y - distance, start_point.Z)
                new_end_point = XYZ(end_point.X, max_point.Y + distance, end_point.Z)
            else:
                # Điều chỉnh điểm đầu và điểm cuối của trục ngang
                new_start_point = XYZ(min_point.X - distance, start_point.Y, start_point.Z)
                new_end_point = XYZ(max_point.X + distance, end_point.Y, end_point.Z)
            
            # Tạo đường cong mới
            new_curve = Line.CreateBound(new_start_point, new_end_point)
            grid.SetCurveInView(datum_extent_type, view, new_curve)

# Hàm thay đổi khoảng cách grid ở mặt đứng
def RUTNGAN_TRUC_MAT_DUNG(grid, distance, scopebox_boun, view):
    datum_extent_type = Autodesk.Revit.DB.DatumExtentType.ViewSpecific
    list_curve = grid.GetCurvesInView(datum_extent_type, view)
    if list_curve:
        curve = list_curve[0]
        if isinstance(curve, Line):
            start_point = curve.GetEndPoint(0)
            end_point = curve.GetEndPoint(1)
            
            # Kiểm tra xem trục đứng hay trục ngang
            is_vertical = abs(start_point.Z - end_point.Z) > 0.01 if scopebox_boun.Min.Z != 0 else abs(start_point.X - end_point.X) < 0.01
            
            # Lấy tọa độ của Scope Box
            min_point = scopebox_boun.Min
            max_point = scopebox_boun.Max

            if is_vertical:
                if scopebox_boun.Min.Z != 0:
                    # Điều chỉnh điểm đầu và điểm cuối của trục đứng theo phương Z
                    new_start_point = XYZ(start_point.X, start_point.Y, min_point.Z - distance)
                    new_end_point = XYZ(end_point.X, end_point.Y, max_point.Z + distance)
                else:
                    # Điều chỉnh điểm đầu và điểm cuối của trục đứng theo phương Y
                    new_start_point = XYZ(start_point.X, min_point.Y - distance, start_point.Z)
                    new_end_point = XYZ(end_point.X, max_point.Y + distance, end_point.Z)
            else:
                # Điều chỉnh điểm đầu và điểm cuối của trục ngang theo phương X hoặc Y
                if scopebox_boun.Min.Z != 0:
                    is_horizontal_x = abs(start_point.X - end_point.X) > 0.01
                    if is_horizontal_x:
                        new_start_point = XYZ(min_point.X - distance, start_point.Y, start_point.Z)
                        new_end_point = XYZ(max_point.X + distance, end_point.Y, end_point.Z)
                    else:
                        new_start_point = XYZ(start_point.X, min_point.Y - distance, start_point.Z)
                        new_end_point = XYZ(end_point.X, max_point.Y + distance, end_point.Z)
                else:
                    new_start_point = XYZ(min_point.X - distance, start_point.Y, start_point.Z)
                    new_end_point = XYZ(max_point.X + distance, end_point.Y, end_point.Z)
            
            # Tạo đường cong mới
            new_curve = Line.CreateBound(new_start_point, new_end_point)
            grid.SetCurveInView(datum_extent_type, view, new_curve)

def RUTNGAN_LEVEL(level, distance, scopebox_boun, crop_length_x, crop_length_y):
    datum_extent_type = Autodesk.Revit.DB.DatumExtentType.ViewSpecific
    list_curve = level.GetCurvesInView(datum_extent_type, doc.ActiveView)
    if list_curve:
        curve = list_curve[0]
        if isinstance(curve, Line):
            start_point = curve.GetEndPoint(0)
            end_point = curve.GetEndPoint(1)

            # Lấy tọa độ của Scope Box
            min_point = scopebox_boun.Min
            max_point = scopebox_boun.Max

            if start_point.X < end_point.X:
                # Điều chỉnh theo hướng từ trái sang phải
                new_start_point = XYZ(min_point.X - distance, start_point.Y, start_point.Z)
                new_end_point = XYZ(min_point.X - distance + crop_length_x + 2 * distance, end_point.Y, end_point.Z)
            elif start_point.X > end_point.X:
                # Điều chỉnh theo hướng từ phải sang trái
                new_start_point = XYZ(max_point.X + distance, start_point.Y, start_point.Z)
                new_end_point = XYZ(max_point.X + distance - crop_length_x - 2 * distance, end_point.Y, end_point.Z)
            elif start_point.Y < end_point.Y:
                # Điều chỉnh theo hướng từ dưới lên trên
                new_start_point = XYZ(start_point.X, min_point.Y - distance, start_point.Z)
                new_end_point = XYZ(end_point.X, min_point.Y - distance + crop_length_y + 2 * distance, end_point.Z)
            elif start_point.Y > end_point.Y:
                # Điều chỉnh theo hướng từ trên xuống dưới
                new_start_point = XYZ(start_point.X, max_point.Y + distance, start_point.Z)
                new_end_point = XYZ(end_point.X, max_point.Y + distance - crop_length_y - 2 * distance, end_point.Z)

            # Tạo đường cong mới
            new_curve = Line.CreateBound(new_start_point, new_end_point)
            level.SetCurveInView(datum_extent_type, doc.ActiveView, new_curve)

def get_builtin_parameter_by_name(element, built_in_parameter):
    param = element.get_Parameter(built_in_parameter)
    return param    

trans_group = TransactionGroup(doc, "Cắt trục và level")
trans_group.Start()

try:
    # Lưu giá trị Scope Box ban đầu
    current_scope_box = doc.ActiveView.get_Parameter(BuiltInParameter.VIEWER_VOLUME_OF_INTEREST_CROP)
    original_scope_box_id = current_scope_box.AsElementId()

    # Đổi Scope Box thành None
    t1 = Transaction(doc, "Chuyển Scope Box về None")
    t1.Start()
    current_scope_box.Set(ElementId.InvalidElementId)
    t1.Commit()

    # Rút ngắn lưới trục và level cách crop view 1 đoạn
    t2 = Transaction(doc, "Cắt trục và level")
    t2.Start()
    for grid in grids:
        if doc.ActiveView.ViewType == ViewType.FloorPlan or doc.ActiveView.ViewType == ViewType.CeilingPlan:
            RUTNGAN_TRUC_MAT_BANG(grid, distance, scopebox_boun, doc.ActiveView)
        else:
            RUTNGAN_TRUC_MAT_DUNG(grid, distance, scopebox_boun, doc.ActiveView)
    for level in levels:
        RUTNGAN_LEVEL(level, distance, scopebox_boun, crop_length_x, crop_length_y)
    t2.Commit()

    # Tắt crop view, bật crop view
    t3 = Transaction(doc, "Bật tắt crop view")
    t3.Start()
    crop_view = get_builtin_parameter_by_name(doc.ActiveView, BuiltInParameter.VIEWER_CROP_REGION)
    crop_view.Set(0)
    crop_view.Set(1)
    t3.Commit()

    # Trả lại Scope Box ban đầu
    t4 = Transaction(doc, "Trả lại Scope Box ban đầu")
    t4.Start()
    current_scope_box.Set(original_scope_box_id)
    t4.Commit()

    trans_group.Assimilate()
except Exception as e:
    print("Error: ", e)
    trans_group.RollBack()
finally:
    trans_group.Dispose()