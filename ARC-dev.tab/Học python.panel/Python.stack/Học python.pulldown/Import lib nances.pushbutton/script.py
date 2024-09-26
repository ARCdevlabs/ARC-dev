# -*- coding: utf-8 -*-
import sys
import os

# Tìm đường dẫn thư mục pyRevit Extensions
pyrevit_extensions_path = os.path.join(os.getenv('APPDATA'), 'pyRevit', 'Extensions')
# hoặc sử dụng PROGRAMDATA nếu thư viện được cài đặt ở đó
# pyrevit_extensions_path = os.path.join(os.getenv('PROGRAMDATA'), 'pyRevit', 'Extensions')

# Đường dẫn thư viện nances
nances_lib_path = os.path.join(pyrevit_extensions_path, 'ARC extension.extension', 'lib')

# Thêm đường dẫn vào sys.path nếu chưa có
if nances_lib_path not in sys.path:
    sys.path.append(nances_lib_path)

# Import thư viện nances
try:
    import nances
    print("Thư viện nances đã được import thành công.")
except ImportError as e:
    print("Lỗi import thư viện nances: ", e)




# Import thư viện khác
pyrevit_master_path = os.path.join(os.getenv('APPDATA'), 'pyRevit-Master', 'extensions')
nances_manage_lib_path = os.path.join(pyrevit_master_path, 'pyRevitCore.extension', 'lib')
if nances_manage_lib_path not in sys.path:
    sys.path.append(nances_manage_lib_path)
try:
    import nancesmanage
    print("Thư viện nancesmanage đã được import thành công.")
except ImportError as e:
    print("Lỗi import thư viện nancesmanage: ", e)
