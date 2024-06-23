
import clr
clr.AddReference('System.Windows.Forms')
clr.AddReference('IronPython.Wpf')
clr.AddReference("System")
import wpf
from System import Windows
from pyrevit import UI
from pyrevit import script
xamlfile = script.get_bundle_file('SuperSelection.xaml')

class SuperSelectionClass(Windows.Window):
    def __init__(self):
        wpf.LoadComponent(self, xamlfile)
        self.set_data_tree_view()

    def set_data_tree_view(self):
        self.data_tree_view.ItemsSource = ["category 1", "category 2"]

SuperSelectionClass().ShowDialog()


