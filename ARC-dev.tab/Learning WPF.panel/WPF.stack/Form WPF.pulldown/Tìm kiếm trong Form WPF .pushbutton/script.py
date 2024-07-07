# -*- coding: utf-8 -*-
import clr
clr.AddReference("System.Windows.Forms")
clr.AddReference("System")
from System.Collections.ObjectModel import ObservableCollection
from System.ComponentModel import INotifyPropertyChanged, PropertyChangedEventArgs
from System.Windows.Input import Key, Keyboard
from nances import script, forms

class ButtonData_Cua_Son(forms.Reactive):
    def __init__(self, title):
        self.__title = title

    @forms.reactive
    def binding_title_new_button(self):
        return self.__title

    @binding_title_new_button.setter
    def binding_title_new_button(self, value):
        self.__title = value

class ListViewItem(forms.Reactive, INotifyPropertyChanged):
    def __init__(self, value):
        self._value = value
        self._is_checked = False
        self._property_changed = []

    @property
    def Value(self):
        return self._value

    @property
    def IsChecked(self):
        return self._is_checked

    @IsChecked.setter
    def IsChecked(self, value):
        if self._is_checked != value:
            self._is_checked = value
            self.OnPropertyChanged("IsChecked")

    def OnPropertyChanged(self, propertyName):
        for handler in self._property_changed:
            handler(self, PropertyChangedEventArgs(propertyName))

    def add_PropertyChanged(self, value):
        self._property_changed.append(value)

    def remove_PropertyChanged(self, value):
        self._property_changed.remove(value)

    PropertyChanged = property(lambda self: self._property_changed, add_PropertyChanged, remove_PropertyChanged)

list_test = [ListViewItem(i) for i in range(1, 51)]
class UI(forms.WPFWindow, forms.Reactive):
    in_check = False
    in_uncheck = False

    def __init__(self):
        return

    def setup(self):
        self.new_button.DataContext = ButtonData_Cua_Son("Đây là button của Sơn")
        self.search_tb.TextChanged += self.search_tb_TextChanged      
        self.list_lb.ItemsSource = ObservableCollection[ListViewItem](list_test)


    @property
    def setup_search_text_box_son(self):
        return self.search_tb.Text

    def search_tb_TextChanged(self, sender, e):
        search_text = self.search_tb.Text.lower()
        filtered_list = [item for item in list_test if search_text in str(item.Value).lower()]
        self.list_lb.ItemsSource = ObservableCollection[ListViewItem](filtered_list)

    def click_button_cua_son(self, sender, args):
        import nances
        input_textbox_son = self.setup_search_text_box_son
        nances.message_box(str(input_textbox_son))

    def hanh_dong_button_print(self, sender, args):
        import nances
        checked_values = [str(item.Value) for item in self.list_lb.ItemsSource if item.IsChecked]
        nances.message_box(", ".join(checked_values))
                
    def check_box_Checked(self, sender, e):
        # if Keyboard.IsKeyDown(Key.LeftShift) or Keyboard.IsKeyDown(Key.RightShift):
        if not self.in_check:
            try:
                for item in self.list_lb.SelectedItems:
                    item.IsChecked = True
            finally:
                self.in_check = False

    def check_box_Unchecked(self, sender, e):
        # if Keyboard.IsKeyDown(Key.LeftShift) or Keyboard.IsKeyDown(Key.RightShift):
        if not self.in_uncheck:
            try:
                for item in self.list_lb.SelectedItems:
                    item.IsChecked = False
            finally:
                self.in_uncheck = False

ui = script.load_ui(UI(), 'tim_kiem.xaml')
ui.show_dialog()
