# -*- coding: utf8 -*-
__doc__ = 'Rút ngắn chiều dài grid'
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

# Hộp thoại nhập khoảng cách
# distance_str = TextInput('Nhập kc điểm đầu cuối đến crop view (mm):', default='-1000')
# if not distance_str:
#     TaskDialog.Show("Thông báo", "Không có giá trị khoảng cách nào được nhập. Dừng chương trình.")
#     sys.exit()

# distance = float(distance_str) / 304.8  # Chuyển đổi từ mm sang feet

# Chọn đối tượng Scope Box
with forms.WarningBar(title='Pick crop view'):
    scope_box_ref = uidoc.Selection.PickObject(ObjectType.Element, "Chọn Scope Box")
scope_box = doc.GetElement(scope_box_ref.ElementId)
line = scope_box.Location.Curve


# Lấy tọa độ của Scope Box
scopebox_boun = scope_box.get_BoundingBox(doc.ActiveView)

# Chọn grid
with forms.WarningBar(title='Select grid need to modify and click Finish'):
    selected_elements = uidoc.Selection.PickObject(ObjectType.Element, "Chọn các lưới trục (Grids)")

def RUTNGAN_TRUC(grid,curve,view):
    datum_extent_type = Autodesk.Revit.DB.DatumExtentType.ViewSpecific
    ket_qua = grid.SetCurveInView(datum_extent_type, view, curve)

    return ket_qua

def get_builtin_parameter_by_name(element, built_in_parameter):
    param = element.get_Parameter(built_in_parameter)
    return param    

trans_group = TransactionGroup(doc, "Cắt trục")
trans_group.Start()


# Lưu giá trị Scope Box ban đầu
current_scope_box = doc.ActiveView.get_Parameter(BuiltInParameter.VIEWER_VOLUME_OF_INTEREST_CROP)
original_scope_box_id = current_scope_box.AsElementId()

# Đổi Scope Box thành None

# Rút ngắn lưới trục cách crop view 1 đoạn
t2 = Transaction(doc, "Cắt trục")
t2.Start()

element = doc.GetElement(selected_elements.ElementId)
datum_extent_type = Autodesk.Revit.DB.DatumExtentType.ViewSpecific
curve_level = element.GetCurvesInView(datum_extent_type, doc.ActiveView)
print element, line, doc.ActiveView, curve_level
ket_qua = element.SetCurveInView(datum_extent_type, doc.ActiveView, curve_level[0])
# RUTNGAN_TRUC(element, line , doc.ActiveView)
t2.Commit()


