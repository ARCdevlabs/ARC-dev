# -*- coding: utf-8 -*-
import clr
clr.AddReference('System.Windows.Forms')
import System.Windows.Forms as WinForms
import xlrd
import xlsxwriter
# import lxml
import sys
from pyrevit import revit, DB
__doc__ = 'nguyenthanhson1712@gmail.com'
__author__ = 'NguyenThanhSon' "Email: nguyenthanhson1712@gmail.com"
from codecs import Codec
import string
import importlib
from Autodesk.Revit.UI.Selection.Selection import PickObject
from Autodesk.Revit.UI.Selection  import ObjectType
from Autodesk.Revit.DB import*
import Autodesk
from Autodesk.Revit.DB import *
from Autodesk.Revit.DB import Element
from System.Collections.Generic import *
import math
from rpw import ui
from rpw.ui.forms import Alert
#Get UIDocument
uidoc = __revit__.ActiveUIDocument
#Get Document 
doc = uidoc.Document
# Ham de tim kiem loai co cung ten trong family

from Autodesk.Revit.DB import FilteredElementCollector

# Lấy tất cả các loại đối tượng từ mô hình
type_collector = FilteredElementCollector(doc).OfClass(ElementType)

# # Duyệt qua danh sách các loại đối tượng và in ra tên của chúng
# for type_element in type_collector:
#     print(type_element.Name)

# import pandas as pd

# data = pd.read_excel(dialog.FileName)


# type_ban_dau = doc.GetElement(ElementId(5240815))

dialog = WinForms.OpenFileDialog()
dialog.Filter = "Excel Files (*.xlsx)|*.xlsx"
dialog.Title = "Select an Excel file"
clr.AddReference("Microsoft.Office.Interop.Excel")
from Microsoft.Office.Interop.Excel import ApplicationClass
excel_app = ApplicationClass()
if dialog.ShowDialog() == WinForms.DialogResult.OK:
    # workbook = xlrd.open_workbook(dialog.FileName)
    file_path = dialog.FileName
    workbook = excel_app.Workbooks.Open(file_path)
    # print dir(workbook)
    list_sheet_names = []
    list_sheets = []
    for number in range(1,100):
        try:
            list_sheet_names.append(workbook.Sheets[number].Name)
            list_sheets.append(workbook.Sheets[number])
        except:
            pass


    for tung_sheet, tung_sheet_name in zip(list_sheets,list_sheet_names):
        if tung_sheet_name == "So sánh":
            sheet = tung_sheet
            

    column_data_1 = []
    for row in range(1, sheet.UsedRange.Rows.Count + 1):
        cell_value = sheet.Cells[row, 1].Value2
        column_data_1.append(cell_value)

    column_data_2 = []
    for row in range(1, sheet.UsedRange.Rows.Count + 1):
        cell_value = sheet.Cells[row, 2].Value2
        column_data_2.append(cell_value)
    workbook.Close()
    excel_app.Quit()

result = [item for item in column_data_2 if item not in column_data_1]
row = 0
col = 0
new_file_path = "C:\Users\Admin\Desktop\Export Project Parameter_Kết quả.xlsx"
workbook = xlsxwriter.Workbook(new_file_path)
worksheet = workbook.add_worksheet("Kết quả")
for tung_gia_tri in result:
    worksheet.write(row,col,tung_gia_tri)
    row += 1

import os
os.startfile(new_file_path)
