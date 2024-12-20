# -*- coding: utf-8 -*-
import sys
import os.path as op
import os
import clr
from pyrevit import USER_SYS_TEMP
from pyrevit import script
from pyrevit.framework import IO
import System.Windows.Forms as WinForms
import shutil

dialog = WinForms.OpenFileDialog()
dialog.Title = "Select an python file"
if dialog.ShowDialog() == WinForms.DialogResult.OK:
    file_path = dialog.FileName
    directory_path = os.path.dirname(file_path)
    file_name = os.path.basename(file_path)  # Lấy tên tệp từ đường dẫn
    
# print(directory_path)
# print (file_path)
# print (file_name)
replace_py_with_dll = file_name.replace('.py', '.dll')
try:
    source = file_path #Tên file python cần mã hóa
    dest = op.join(USER_SYS_TEMP, replace_py_with_dll) #Tên file dll sau khi mã hóa, lưu ý hãy để tên .dll giống với tên .py ban đầu
    clr.CompileModules(dest, source)
    shutil.copy(dest, directory_path)
    # print dest
except IO.IOException as ioerr:
    print('DLL file already exists...')
except Exception as cerr:
    print('Compilation failed: {}'.format(cerr))
