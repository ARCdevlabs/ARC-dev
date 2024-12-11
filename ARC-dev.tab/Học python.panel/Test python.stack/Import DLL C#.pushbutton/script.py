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
uidoc = __revit__.ActiveUIDocument
doc = uidoc.Document

import clr
clr.AddReferenceByName("libARC") #Tên của file .dll (ví dụ libARC.dll)
from NS.WrapARC import WrapARC #từ namespace namspaceLibARC import class LibARCSecurity

# element = module.get_element(uidoc,doc, 'Select Element', noti = False)
# parameter = WrapARC.GetParameterValueByName(element[0],"Base Offset")
# print parameter

# builtin_parameter = WrapARC.GetBuiltInParameterByName(element[0],DB.BuiltInParameter.FLOOR_PARAM_IS_STRUCTURAL)
# print builtin_parameter.AsInteger()


AcView= doc.ActiveView

t = Transaction(doc, "Appy template of vn03")
t.Start()

Checkviewtemplate = str(AcView.ViewTemplateId)
#Check view have ViewTemplate or not, if not => don't need to Enable Temporary view
if Checkviewtemplate != "-1":
    tem = AcView.EnableTemporaryViewPropertiesMode(AcView.Id)
#Select Color
ColorFraming = Autodesk.Revit.DB.Color(214,214,171)
ColorFramingCut = Autodesk.Revit.DB.Color(177,177,10)
ColorFloor = Autodesk.Revit.DB.Color(232,232,255)
ColorFloorCut = Autodesk.Revit.DB.Color(170,170,213)
ColorWall = Autodesk.Revit.DB.Color(152,203,203)
ColorWallCut = Autodesk.Revit.DB.Color(152,203,10)
ColorColumn = Autodesk.Revit.DB.Color(255,128,128)
ColorColumnCut = Autodesk.Revit.DB.Color(255,128,10)
ColorFoundation = Autodesk.Revit.DB.Color(0,128,192)
ColorFoundationCut = Autodesk.Revit.DB.Color(0,128,10)
#Get FillPattern by name
GetFillPattern = Autodesk.Revit.DB.FillPatternElement.GetFillPatternElementByName(doc,FillPatternTarget.Drafting,"<Solid fill>")
if str(GetFillPattern) == "None":
    GetFillPattern = Autodesk.Revit.DB.FillPatternElement.GetFillPatternElementByName(doc,FillPatternTarget.Drafting,"<塗り潰し>")
#Def OverrideFraming (Color)
""" Framing """
OverrideFraming = Autodesk.Revit.DB.OverrideGraphicSettings()
OverrideFraming.SetSurfaceForegroundPatternColor(ColorFraming)
#Def OverrideFraming (Pattern)
OverrideFraming.SetSurfaceForegroundPatternId(GetFillPattern.Id)
OverrideFraming.SetCutForegroundPatternColor(ColorFramingCut)
OverrideFraming.SetCutForegroundPatternId(GetFillPattern.Id)
# Set override for Structure framing
SetColorFrame = AcView.SetCategoryOverrides(ElementId(-2001320),OverrideFraming)
""" Floor """
OverrideFloor = Autodesk.Revit.DB.OverrideGraphicSettings()
OverrideFloor.SetSurfaceForegroundPatternColor(ColorFloor)
OverrideFloor.SetSurfaceForegroundPatternId(GetFillPattern.Id)
OverrideFloor.SetCutForegroundPatternColor(ColorFloorCut)
OverrideFloor.SetCutForegroundPatternId(GetFillPattern.Id)
SetColorFloor = AcView.SetCategoryOverrides(ElementId(-2000032),OverrideFloor)
""" Wall """
OverrideWall = Autodesk.Revit.DB.OverrideGraphicSettings()
OverrideWall.SetSurfaceForegroundPatternColor(ColorWall)
OverrideWall.SetSurfaceForegroundPatternId(GetFillPattern.Id)
OverrideWall.SetCutForegroundPatternColor(ColorWallCut)
OverrideWall.SetCutForegroundPatternId(GetFillPattern.Id)
SetColorWall = AcView.SetCategoryOverrides(ElementId(-2000011),OverrideWall)
""" Column """
OverrideColumn = Autodesk.Revit.DB.OverrideGraphicSettings()
OverrideColumn.SetSurfaceForegroundPatternColor(ColorColumn)
OverrideColumn.SetSurfaceForegroundPatternId(GetFillPattern.Id)
OverrideColumn.SetCutForegroundPatternColor(ColorColumnCut)
OverrideColumn.SetCutForegroundPatternId(GetFillPattern.Id)
SetColorColumn = AcView.SetCategoryOverrides(ElementId(-2001330),OverrideColumn)
""" Foundation """
OverrideFoundation = Autodesk.Revit.DB.OverrideGraphicSettings()
OverrideFoundation.SetSurfaceForegroundPatternColor(ColorFoundation)
OverrideFoundation.SetSurfaceForegroundPatternId(GetFillPattern.Id)
OverrideFoundation.SetCutForegroundPatternColor(ColorFoundationCut)
OverrideFoundation.SetCutForegroundPatternId(GetFillPattern.Id)
SetColorFoundation = AcView.SetCategoryOverrides(ElementId(-2001300),OverrideFoundation)

# Override in view_floorA
collector_floor = FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Floors).WhereElementIsNotElementType()
collector_floor.ToElements()
list_floorA = []

try:
    list_floorA_CS = List[ElementId]()
    for i in collector_floor:
        # isstruc = i.LookupParameter("Structural")
        isstruc_floor = module.get_builtin_parameter_by_name(i, BuiltInParameter.FLOOR_PARAM_IS_STRUCTURAL)
        if isstruc_floor.AsInteger() == 0:
            list_floorA.append(i.Id)
            list_floorA_CS.Add(i.Id)
except:
    pass
color_yellow = Color (255,255,196)
color_yellow_2 = Color (255,255,150)
WrapARC.OverrideGraphicsInView(doc, AcView, list_floorA_CS, color_yellow,color_yellow_2)

collector_wall = FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType()
collector_wall.ToElements()
list_wallA = []
list_wallA_thick = []

list_wallA_CS = List[ElementId]()
list_wallA_thick_CS = List[ElementId]()
try:
    for i in collector_wall:
        isstruc_wall = module.get_builtin_parameter_by_name(i, BuiltInParameter.WALL_STRUCTURAL_SIGNIFICANT)
        if isstruc_wall.AsInteger()== 0:
            if i.Width <= 50*0.0032808: #Doi don vi tu mm sang feet thi nhan 0.0032808
                list_wallA_thick.append(i.Id)
                list_wallA_thick_CS.Add(i.Id)
            else:
                list_wallA.append(i.Id)
                list_wallA_CS.Add(i.Id)
except:
    pass
color_green = Color (191,255,191)
color_green_2 = Color (0,130,0)
color_orange = Color (255,198,170)
color_orange_2 = Color (255,158,62)
WrapARC.OverrideGraphicsInView(doc, AcView, list_wallA_CS, color_green,color_green_2)
WrapARC.OverrideGraphicsInView(doc, AcView, list_wallA_thick_CS, color_orange,color_orange_2)
t.Commit()
      