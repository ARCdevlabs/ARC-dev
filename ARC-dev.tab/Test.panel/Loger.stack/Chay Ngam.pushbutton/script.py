# -*- coding: utf-8 -*-

import clr
clr.AddReference('RevitAPI')
from Autodesk.Revit.DB import *
clr.AddReference('RevitServices')
import RevitServices
from RevitServices.Persistence import DocumentManager

# Thêm khai báo của lớp IExternalApplication
clr.AddReference('RevitAPI')
from Autodesk.Revit.UI import IExternalApplication
from Autodesk.Revit.ApplicationServices import Application

# Tạo một lớp mới kế thừa từ ExternalApplication để triển khai add-in
class MyExternalApplication(IExternalApplication):
    # Phương thức khởi tạo của lớp
    def __init__(self):
        self.document = None

    # Phương thức Start của ExternalApplication
    def OnStartup(self, application):
        # Đăng ký sự kiện DocumentOpened để theo dõi khi một tài liệu mở
        application.DocumentOpened += self.OnDocumentOpened

    # Phương thức Stop của ExternalApplication
    def OnShutdown(self, application):
        # Hủy đăng ký sự kiện khi ứng dụng dừng
        application.DocumentOpened -= self.OnDocumentOpened

    # Phương thức sự kiện DocumentOpened
    def OnDocumentOpened(self, sender, args):
        # Lấy thông tin của tài liệu đã mở
        self.document = args.Document

        # Gọi phương thức xử lý ở đây
        self.ProcessDocument()

    # Phương thức để xử lý tài liệu
    def ProcessDocument(self):
        # In ra tên của tài liệu đã mở
        print("Document Opened:", self.document.Title)

# Tạo một thể hiện của lớp MyExternalApplication
myApp = MyExternalApplication()

# Đăng ký MyExternalApplication với ứng dụng Revit
# (Thường được thực hi
