# -*- coding: utf-8 -*-
import os
import platform
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

    def check_license(self):
        tring = self.read_pass_code()
        try:
            info = platform.processor()
            
            if isinstance(tring, str):
                newstring= tring[:-2]
                decode_string = bytes.fromhex(newstring)
                decode_string = decode_string.decode("ascii")
                if decode_string == info:
                    return True
        except Exception:
            pass
            return False
    def machine_code(self):
        info = platform.processor()
        new_code = "/M%A*D/".join(info)
        return new_code