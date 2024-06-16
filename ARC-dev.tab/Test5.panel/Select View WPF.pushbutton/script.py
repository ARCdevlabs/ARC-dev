# -*- coding: utf-8 -*-
__doc__ = 'python for revit api'
__author__ = 'NguyenThanhSon' "Email: nguyenthanhson1712@gmail.com"
import string
import codecs
import importlib
ARC = string.ascii_lowercase
begin = "".join(ARC[i] for i in [13, 0, 13, 2, 4, 18])
module = importlib.import_module(str(begin))
try:
    if module.AutodeskData():
        from nances import forms
        import clr
        clr.AddReference("System")
        from System import Array
        import System
        import sys
        import random
        from System.Drawing import Size, Color, Font, FontStyle
        from System import Drawing
        from System.Collections.Generic import List
        import Autodesk
        from Autodesk.Revit import DB
        from Autodesk.Revit.DB import View, OverrideGraphicSettings, FilteredElementCollector,FillPatternTarget, ElementParameterFilter, FilterRule, ParameterFilterRuleFactory, FilterElement, FilteredElementCollector, FilteredElementCollector, BuiltInCategory, ElementCategoryFilter, ParameterFilterElement, Transaction, BuiltInParameter, BuiltInCategory, WallType, ElementId, FilterRule, ParameterFilterElement, ElementParameterFilter, ParameterFilterRuleFactory
        import Autodesk.Revit.UI.Selection
        uidoc = __revit__.ActiveUIDocument
        doc = uidoc.Document
        view = doc.ActiveView
        def get_filter_elements():
            filter_elements = FilteredElementCollector(doc).OfClass(ParameterFilterElement).ToElements()
            return filter_elements
        all_view_filters = get_filter_elements()
        name = []
        for i in all_view_filters:
            name.append(Autodesk.Revit.DB.Element.Name.GetValue(i))
            name.sort()

        def get_elements_by_view_filter(doc, view_filter):
            # Lấy tất cả các đối tượng trong view hiện tại
            collector = FilteredElementCollector(doc, doc.ActiveView.Id)
            all_elements = collector.ToElements()
            get_ele_filter = view_filter.GetElementFilter()
            # Tạo danh sách để lưu trữ các đối tượng thỏa mãn điều kiện của view filter
            selected_elements = []

            # Kiểm tra từng đối tượng xem nó có thỏa mãn view filter không
            for element in all_elements:
                try:
                    if get_ele_filter.PassesFilter(element):
                        selected_elements.append(element)
                except:
                    pass
            return selected_elements

        select_view = forms.select_views_ARC()
        if str(select_view) == "None":
            module.message_box("0 filter was selected")
            sys.exit()
        else:
            for moi_element in select_view:  
                print moi_element.Name
except:
    import traceback
    # print(traceback.format_exc())
    pass

