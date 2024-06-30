# -*- coding: utf-8 -*-
import clr
clr.AddReference('PresentationFramework')
clr.AddReference('WindowsBase')

from System.Windows import Application, Window, Controls, Markup
from System.Windows.Data import CollectionViewSource
from System.ComponentModel import ICollectionView

# Lớp dữ liệu của bạn
class ListItem(object):
    def __init__(self, name, is_checked=False):
        self.Name = name
        self.IsChecked = is_checked

class MyWindow(Window):
    def __init__(self):
        self.Title = "Search Example"
        self.Width = 800
        self.Height = 450

        # Load XAML
        xaml = "SuperSelect.xaml"
        # <Window x:Class="MyNamespace.MyWindow"
        #         xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        #         xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        #         Title="Search Example" Height="450" Width="800">
        #     <Grid>
        #         <TextBox x:Name="search_tb" 
        #                  Height="25"
        #                  Padding="5,0,24,0"
        #                  VerticalContentAlignment="Center"
        #                  TextChanged="OnSearchTextChanged"/>
                
        #         <ListView x:Name="list_lb"
        #                   Margin="0,35,0,0"
        #                   SelectionMode="Extended"
        #                   HorizontalContentAlignment="Stretch"
        #                   ScrollViewer.VerticalScrollBarVisibility="Visible"
        #                   Height="400">
        #             <ListView.ItemTemplate>
        #                 <DataTemplate>
        #                     <StackPanel Orientation="Horizontal">
        #                         <CheckBox IsChecked="{Binding IsChecked}" VerticalAlignment="Center" Margin="5"/>
        #                         <TextBlock Text="{Binding Name}" VerticalAlignment="Center" Margin="5"/>
        #                     </StackPanel>
        #                 </DataTemplate>
        #             </ListView.ItemTemplate>
        #         </ListView>
        #     </Grid>
        # </Window>
        # '''
        
        self.Content = Markup.XamlReader.Parse(xaml)
        
        # Tạo các mục mẫu và thêm vào ListView
        self.items = [
            ListItem("Item 1", True),
            ListItem("Item 2", False),
            ListItem("Another Item", True),
            ListItem("Searchable Item", True),
            ListItem("Different Item", False)
        ]

        # Tạo CollectionViewSource để quản lý lọc
        self.collectionViewSource = CollectionViewSource()
        self.collectionViewSource.Source = self.items
        self.listView = self.Content.FindName("list_lb")
        self.listView.ItemsSource = self.collectionViewSource.View

        # Đăng ký sự kiện TextChanged cho TextBox
        self.searchTextBox = self.Content.FindName("search_tb")
        self.searchTextBox.TextChanged += self.OnSearchTextChanged

    def OnSearchTextChanged(self, sender, e):
        self.collectionViewSource.View.Refresh()

    def FilterItems(self, item):
        searchText = self.searchTextBox.Text.lower()
        return searchText in item.Name.lower()

    def OnSearchTextChanged(self, sender, e):
        self.collectionViewSource.View.Filter = self.FilterItems
        self.collectionViewSource.View.Refresh()

# Khởi động ứng dụng
if __name__ == "__main__":
    app = Application()
    window = MyWindow()
    window.Show()
    app.Run()
