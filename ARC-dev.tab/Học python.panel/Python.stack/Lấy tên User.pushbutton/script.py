# -*- coding: utf-8 -*-
import clr
clr.AddReference("RevitAPI")
clr.AddReference("RevitAPIUI")
clr.AddReference("System")
import Autodesk
# from Autodesk.Revit.DB import *

# Khởi tạo đối tượng UIApplication từ đối tượng Revit hiện tại
uiapp = __revit__.Application
# Lấy tên người dùng đã đăng nhập
username = uiapp.Username

if "kawamura" in username:
    print "Dùng được", username
# In ra tên người dùng
# print("Tên account đã đăng nhập vào Revit:{}".format(username))
