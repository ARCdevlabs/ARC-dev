# -*- coding: utf-8 -*-
import clr
import System

clr.AddReference("ReadPassCode241125Ver1.dll")
import ReadPassCode241125Ver1 #Load Assembly

import_class = ReadPassCode241125Ver1.ClassReadPassCode() #import Class
result = import_class.read_pass_code() #gọi method trong Class
new_code = import_class.machine_code()
print result, new_code

""""Đây là code để kiểm tra lại vị trí Assembly"""
assembly = System.Reflection.Assembly.Load("ReadPassCode241125Ver1")
assembly_location = assembly.Location
print(assembly_location)