# -*- coding: utf-8 -*-
import clr
import System
import sys
import os
import os.path as op
pyrevit_extensions_path = os.path.join(os.getenv('APPDATA'), 'pyRevit', 'Extensions')
nances_lib_path = os.path.join(pyrevit_extensions_path, 'ARC extension.extension', 'bin')

path_string_thieu = r"C:\Users\Admin\AppData\Roaming\pyRevit\Extensions\ARC extension.extension\bin"
path_string = op.join(path_string_thieu, 'ReadPassCode241125Ver1.dll')
print path_string

sys.path.append(path_string_thieu)
clr.AddReferenceToFileAndPath(path_string)
import ReadPassCode241125Ver1 #Load Assembly


import_class = ReadPassCode241125Ver1.ClassReadPassCode() #import Class
result = import_class.read_pass_code() #gọi method trong Class
new_code = import_class.machine_code()
print result, new_code

""""Đây là code để kiểm tra lại vị trí Assembly"""
assembly = System.Reflection.Assembly.Load("ReadPassCode241125Ver1")
assembly_location = assembly.Location
print(assembly_location)

name = assembly.GetName().Name
version = assembly.GetName().Version
culture = assembly.GetName().CultureInfo
print name, version,culture