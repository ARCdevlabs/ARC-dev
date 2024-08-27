# -*- coding: utf-8 -*-
import clr
clr.AddReferenceByName("libARC") #Tên của file .dll (ví dụ libARC.dll)
from namspaceLibARC import LibARCSecurity #từ namespace namspaceLibARC import class LibARCSecurity
print LibARCSecurity.CheckLicense() #từ class LibARCSecurity import phương thức CheckLicense()



