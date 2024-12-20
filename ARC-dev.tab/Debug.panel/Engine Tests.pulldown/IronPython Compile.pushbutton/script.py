# -*- coding: utf-8 -*-
import sys
import os.path as op
import clr

from pyrevit import USER_SYS_TEMP
from pyrevit import script
from pyrevit.framework import IO

# compile
try:
    source = script.get_bundle_file('ReadPassCode241125Ver1.py') #Tên file python cần mã hóa
    dest = op.join(USER_SYS_TEMP, 'ReadPassCode241125Ver1.dll') #Tên file dll sau khi mã hóa, lưu ý hãy để tên .dll giống với tên .py ban đầu
    clr.CompileModules(dest, source)
    print dest
except IO.IOException as ioerr:
    print('DLL file already exists...')
except Exception as cerr:
    print('Compilation failed: {}'.format(cerr))

# import test
# sys.path.append(USER_SYS_TEMP)
# clr.AddReferenceToFileAndPath(dest)

# import ReadPassCode241125  #Đây là dòng import file .dll
# pass_code = ReadPassCode241125.ClassReadPassCode() #Đây là dòng import class ClassReadPassCode ở chưa trong file .dll
# print pass_code.read_pass_code() #Đây là dòng gọi thực thi lệnh read_pass_code() có trong Class "ClassReadPassCode"
