# -*- coding: utf-8 -*-
import os
import shutil
import nances

__doc__ = 'python for revit api'
__author__ = 'NguyenThanhSon' "Email: nguyenthanhson1712@gmail.com"
import string
import codecs
import importlib
ARC = string.ascii_lowercase
begin = "".join(ARC[i] for i in [13, 0, 13, 2, 4, 18])
module = importlib.import_module(str(begin))

list_bi_trung = []
def copy_folder(source_folder, destination_folder):
    """
    Sao chép một thư mục và tất cả các tệp con và thư mục con của nó từ nguồn đến đích.
    """
    try:
        # Kiểm tra xem thư mục nguồn tồn tại hay không
        if os.path.exists(source_folder):
            # Kiểm tra xem thư mục đích đã tồn tại chưa, nếu không, tạo mới
            if not os.path.exists(destination_folder):
                tin_nhan = "Ổ đĩa H không có sẵn folder với đường dẫn" + str(destination_folder)
                module.message_box(tin_nhan)
                
                # os.makedirs(destination_folder)

            # Sao chép tất cả các tệp và thư mục con
            for item in os.listdir(source_folder):
                source_item = os.path.join(source_folder, item)
                destination_item = os.path.join(destination_folder, item)

                # Nếu là thư mục, tiến hành đệ quy để sao chép thư mục con
                if os.path.isdir(source_item):
                    copy_folder(source_item, destination_item)
                # Nếu là tệp
                else:
                    # Kiểm tra xem tệp đã tồn tại ở thư mục đích chưa
                    if not os.path.exists(destination_item):
                        shutil.copy2(source_item, destination_item)
                    else:
                        list_bi_trung.append(destination_item)
                        print("Đã sao chép thành công file:" + str(destination_item))
        else:
            print("Thư mục nguồn không tồn tại.")

        for i in  list_bi_trung:
            print ("Các file bị trùng:" + str(i))
        
    except Exception as e:
        print("Lỗi khi sao chép thư mục: {e}")

# Đường dẫn thư mục nguồn
source_path = r"C:\Users\Admin\Desktop\d\◆167- LOGI'Q Sendai (LOGI'Q仙台)\SEND FILE"
# Đường dẫn thư mục đích
destination_path = r"C:\Users\Admin\Desktop\h\◆167- LOGI'Q Sendai (LOGI'Q仙台)\SEND FILEe"

# Gọi hàm để sao chép thư mục
copy_folder(source_path, destination_path)
