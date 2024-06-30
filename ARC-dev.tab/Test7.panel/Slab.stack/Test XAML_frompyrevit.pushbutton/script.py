# -*- coding: utf-8 -*-
"""Test loading XAML in IronPython."""
#pylint: disable=import-error,invalid-name,broad-except,superfluous-parens
import urllib2
import json
from time import sleep
import sys
import threading

import os.path as op
from pyrevit import EXEC_PARAMS

from pyrevit import framework
from pyrevit import coreutils
from pyrevit import revit, DB
from pyrevit import forms
from pyrevit import script
logger = script.get_logger()
output = script.get_output()

class NestedObject(forms.Reactive): #Điều này có nghĩa là NestedObject 
                                    #là một lớp con của forms.Reactive. Nói cách khác,
                                    #NestedObject kế thừa tất cả các thuộc tính và phương
                                    #thức của lớp cha forms.Reactive.
    def __init__(self, text):
        self._text = text #_text quy ước là một thuộc tính "private"

    @forms.reactive
    # @forms.reactive: Đây là một decorator. Decorator là một cách để sửa đổi hoặc mở rộng hành vi của hàm hoặc phương thức mà không cần thay đổi mã gốc của nó. Trong trường hợp này, @forms.reactive là một decorator từ thư viện hoặc module forms.
    # Việc sử dụng @forms.reactive làm cho thuộc tính của bạn trở thành reactive, tự động cập nhật giao diện người dùng hoặc các thành phần khác khi giá trị thay đổi. Nếu bạn không sử dụng @forms.reactive, bạn sẽ mất tính năng này và cần quản lý cập nhật và thông báo về các thay đổi một cách thủ công. Điều này có thể làm tăng độ phức tạp và công việc quản lý trạng thái của ứng dụng.
    def text(self):
        return self._text

    @text.setter
    # Setter: Định nghĩa phương thức setter cho thuộc tính.
    # Ngoài setter còn có các decorate khác như: @abstractmethod,@staticmethod,@classmethod,@property
    def text(self, value):
        self._text = value


class ButtonData(forms.Reactive):
    def __init__(self, title, nested):
        self._title = title
        self.nested = nested

    @forms.reactive
    def son_title_trang(self):
        return self._title

    @son_title_trang.setter
    def son_title_trang(self, value):
        self._title = value

class Nested_Data_Son(forms.Reactive):
    def __init__(self, nested):
        # self._text = text #_text quy ước là một thuộc tính "private"
        self.nested = nested
    @forms.reactive
    def nested(self):
        return self._text
    
    @nested.setter
    def nested(self, value):
        self._text = value

class ButtonData_Cua_Son(forms.Reactive):
    def __init__(self, title):
        self._title = title

    @forms.reactive
    def binding_title_new_button(self):
        return self._title

    @binding_title_new_button.setter
    def binding_title_new_button(self, value):
        self._title = value

# Thay tên của các decorator "job_son" ở class EmployeeInfo. phải khai báo binding "{Binding job_son}" ở WPF nữa.
class EmployeeInfo(forms.Reactive):
    def __init__(self, name, job, supports):
        self._name = name
        self._job = job
        self.supports = supports

    @forms.reactive
    def name(self):
        return self._name

    @name.setter
    def name(self, value):
        self._name = value

    @forms.reactive
    def job_son(self):
        return self._job

    @job_son.setter
    def job_son(self, value):
        self._job = value


class Server(forms.Reactive):
    def __init__(self, url):
        self.url = url
        self._status = False

    @forms.reactive
    def status(self):
        return self._status

    @status.setter
    def status(self, value):
        self._status = value


class MyPath(forms.Reactive):
    def __init__(self, path):
        self._path = path

    @forms.reactive
    def path(self):
        return self._path

    @path.setter
    def path(self, value):
        self._path = value



class Mod(object):
    def __init__(self, abbrev, color):
        # "abbrev_son" và "color" được khai báo trong phần {Binding... của của WPF, ví dụ bên dưới:
                    # <Border Background="{Binding color}" Height="18">
                        # <TextBlock x:Name="ModifierTitle"
                        #            HorizontalAlignment="Center"
                        #            VerticalAlignment="Center"
                        #            Text="{Binding abbrev_son}"
                        #            Foreground="White"
                        #            FontSize="10"
                        #            Margin="4,0,2,0"/>
        self.binding_text_son = abbrev 
        self.color = color


class Tag(forms.Reactive):
    def __init__(self, name, modifiers):
        self._name = name
        self.modifiers = modifiers

    @forms.reactive
    def name(self):
        return self._name

    @name.setter
    def name(self, value):
        self._name = value


class DataSelectorConverter(framework.Windows.Data.IMultiValueConverter):
    def Convert(self, values, target_types, parameter, culture):
        return values[0][int(values[1]) - 1]
    # Mục đích: Phương thức này được sử dụng để chuyển đổi (convert) 
    # một giá trị dữ liệu từ một định dạng sang một định dạng khác, 
    # thường được sử dụng trong ngữ cảnh của dữ liệu đa giá trị (multi-value data).

    # Hành động: Phương thức này lấy giá trị đầu tiên từ values (là values[0]),
    #  sau đó lấy phần tử trong danh sách này ở vị trí được chỉ định bởi giá trị
    #  thứ hai của values (là values[1]). Giá trị này được chuyển đổi thành một
    #  số nguyên bằng hàm int() và sau đó sử dụng để truy xuất phần tử trong danh
    #  sách values[0]. Kết quả trả về là phần tử được lấy ra từ danh sách này.



    def ConvertBack(self, value, target_types, parameter, culture):
        pass




class ViewModel(forms.Reactive):
    def __init__(self):
        self._title = "Đây là title"


# self.employee_data: Khởi tạo một danh sách employee_data chứa các đối tượng EmployeeInfo.
# Mỗi EmployeeInfo có các thuộc tính như name, job và supports.
        self.employee_data = [
            # name = "Ehsan" thì thuộc tính name có liên quan tới "{Binding name}" ở trong code của wpf.

            # self.employee_data sẽ kế thừa tất cả thuộc tính của class EmployeeInfo, 
            # vì vậy self.employee_data cũng có thuộc tính "name" và "job_son" 
            # được khai báo ở def của class EmployeeInfo
            EmployeeInfo(
                name="Ehsan", 
                job="Architect",
                supports=[
                    "UX_Son",
                    "CLI_Son",
                    "Core_Son"
                ]),
            EmployeeInfo(
                name="Gui",
                job="Programmer",
                supports=[
                    "CLI_Son",
                ]),
            EmployeeInfo(
                name="Alex",
                job="Designer",
                supports=[
                    "Core_Son"
                ]),
            EmployeeInfo(
                name="Jeremy",
                job="Designer",
                supports=[
                    "Core_Son"
                ]),
            EmployeeInfo(
                name="Petr",
                job="Manager",
                supports=[
                    "Core_Son",
                    "CLI_Son",
                ]),
        ]

# Thuộc tính nested_data và data:
# self.nested_data: Khởi tạo một đối tượng NestedObject với thuộc tính text là "Text in Data Object".
# self.data: Khởi tạo một đối tượng ButtonData với các thuộc tính được chỉ định.
        self.nested_data = NestedObject(text="Text in Data Object_Sơn thêm vào chút")
        
        self.data_cua_son = Nested_Data_Son(nested = self.nested_data) 

        self.data = ButtonData(title="Title in Data Object",nested=self.nested_data)
                
        self.server = Server(r'https://status.epicgames.com/api/v2/status.json')

        self.tags = [
            Tag('Tag 1', [Mod('IFC', '#fc8f1b'), Mod('IFF', '#98af13')]),
            Tag('Tag 2', [Mod('As-Built', '#a51c9a')]),
            Tag('Tag 3', []),
        ]

        self.my_path = MyPath(r'C:\Users\ADMIN\AppData\Roaming\pyRevit\Extensions\ARC-dev.extension\ARC-dev.tab\Test7.panel\Slab.stack\Test XAML_frompyrevit.pushbutton\cuoi.gif')
        # print MyPath(r'C:\Users\ADMIN\AppData\Roaming\pyRevit\Extensions\ARC-dev.extension\ARC-dev.tab\Test7.panel\Slab.stack\Test XAML_frompyrevit.pushbutton\cuoi.gif')

        # self.title_button_son = \
        #     ButtonData_Cua_Son("Button SON")

    @forms.reactive
    def title(self):
        return self._title

    @title.setter
    def title(self, value):
        self._title = value

# class tim_kiem_text_in_list():
#     def __init__(self, list_dau_vao, key_tim_kiem):
#         list_ket_qua = []
#         for moi_item in list_dau_vao:
#             if str(key_tim_kiem) in str(moi_item):
#                 list_ket_qua.append(moi_item)
#         self._list_ket_qua = list_ket_qua


# code-behind for the window
list_test = list(range(1, 50))
from System.Collections.ObjectModel import ObservableCollection

class UI(forms.WPFWindow, forms.Reactive):
    def __init__(self):
        self.vm = ViewModel()
    def setup(self):  #Tên của def luôn luôn là "setup" để script.load_ui(UI(), 'ui_test.xaml') có thể hiểu được.
        mbinding = framework.Windows.Data.MultiBinding()
        mbinding.Converter = DataSelectorConverter()
        mbinding.Bindings.Add(framework.Windows.Data.Binding("."))
        binding = framework.Windows.Data.Binding("Value")
        binding.ElementName = "selector"
        mbinding.Bindings.Add(binding)
        mbinding.NotifyOnSourceUpdated = True

        self.textbox_son.DataContext = self.vm.nested_data #name "textbox_son" trong form wpf được gán bằng self.vm.nested_data
        self.emppanel.SetBinding(self.emppanel.DataContextProperty, mbinding)
        self.empinfo.DataContext = self.vm.employee_data
        self.textblock_son.DataContext = self.vm.data_cua_son #name "textblock_son" trong form wpf được gán bằng self.vm.data
        self.son_button.DataContext = self.vm.data
        self.statuslight.DataContext = self.vm.server




        self.new_button.DataContext = ButtonData_Cua_Son("Đây là button của Sơn")

        self.list_lb.ItemsSource = ObservableCollection[int](list_test)
        self.search_tb.TextChanged += self.search_tb_TextChanged      

        self.set_image_source(self.testimage, 'pepe.JPG')
        self.taglist.ItemsSource = self.vm.tags

    
    @property
    def setup_search_text_box_son(self):
        return self.search_tb.Text

    def search_tb_TextChanged(self, sender, e):
        search_text = self.search_tb.Text.lower()
        filtered_list = [item for item in list_test if search_text in str(item).lower()]
        self.list_lb.ItemsSource = ObservableCollection[int](filtered_list)

    def click_button_cua_son(self, sender, args):
        import nances
        input_textbox_son = self.setup_search_text_box_son
        nances.message_box(str(input_textbox_son))




    def set_status(self, status):
        self.vm.server.status = status is not None

    def check_status(self):
        status = json.loads(coreutils.read_url(self.vm.server.url))
        sleep(4)    # fake slow io
        self.dispatch(self.set_status, status)

    def check_fortnite_status(self, sender, args):
        self.dispatch(self.check_status)

    def button_click(self, sender, args):  #Đặt hành động Click trong form WPF bằng tham số button_click, bây giờ khai báo khi click thì điều gì xảy ra.
        self.vm.data.title = "Title in Data Object (Updated)" 
        self.vm.nested_data.text = "Text in Data Object (Updated)"
        for emp in self.vm.employee_data:
            emp.job_son = emp.job_son.replace(" (Updated)", "") + " (Updated)" #Nếu không có dòng này thì mỗi lần click vào button_click thì chữ (Updated) cứ bị cộng dồn vào liên tục


    # def new_button(self, sender, args):  #Đặt hành động Click trong form WPF bằng tham số button_click, bây giờ khai báo khi click thì điều gì xảy ra.
    #     # self.vm.title = "Đây là button Sơn tạo" 
    #     self.new_button.Content = "Đây là button Sơn tạo"

    def read_data(self, sender, args):
        forms.alert(self.vm.nested_data.text)

    def delete_stuff(self, pbar):
        try:
            walls = revit.query.get_elements_by_class(DB.Wall)
            with revit.Transaction('Delete Walls'):
                for idx, wall in enumerate(walls):
                    revit.delete.delete_elements(wall)
                    pbar.update_progress(idx + 1, len(walls))
                    sleep(0.5)
        except Exception as derr:
            logger.dev_log('delete_stuff', str(derr))

    def do_revit_work(self, sender, args):
        # self.dispatch(self.delete_stuff)
        with self.conceal():
            with forms.ProgressBar() as pbar:
                self.delete_stuff(pbar)


# init ui
ui = script.load_ui(UI(), 'ui_test.xaml')
# show modal or nonmodal
ui.show_dialog()
