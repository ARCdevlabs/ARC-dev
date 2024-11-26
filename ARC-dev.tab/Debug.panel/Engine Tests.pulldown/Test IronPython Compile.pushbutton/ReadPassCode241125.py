# -*- coding: utf-8 -*-
import os
class ClassReadPassCode:
    def read_pass_code(self):

        content = False
        appdata_path_1 = os.getenv('APPDATA')
        file_path_1 = os.path.join(appdata_path_1, "pyRevit", "pyRevit_pyrevit.dll")
    
        appdata_path_2 = os.getenv('PROGRAMDATA')
        file_path_2 = os.path.join(appdata_path_2, "pyRevit", "pyRevit_pyrevit.dll")
        if file_path_1:
            try:
                with open(file_path_1, 'r') as file:
                    content = file.read()
            except:
                pass
        elif file_path_2:
            try:
                with open(file_path_2, 'r') as file:
                    content = file.read()
            except:
                pass
        else:
            return False
        if content:
            start = content.find("'") + 1
            end = content.rfind("'")

            if start > 0 and end > start:
                extracted_value = content[start:end]
                return extracted_value
            else:
                return False
        else:
            return False

