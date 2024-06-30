"""Reusable WPF forms for pyRevit.

Example:
    >>> from pyrevit.forms import WPFWindow
"""

import sys
import os
import os.path as op
import string
from collections import OrderedDict, namedtuple
import threading
from functools import wraps
import datetime
import webbrowser
import Autodesk.Revit.DB as DB

from pyrevit import HOST_APP, EXEC_PARAMS, DOCS, BIN_DIR
from pyrevit import PyRevitCPythonNotSupported, PyRevitException, PyRevitCPythonNotSupported
import pyrevit.compat as compat
from pyrevit.compat import safe_strtype

if compat.PY3:
    raise PyRevitCPythonNotSupported('pyrevit.forms')

from pyrevit import coreutils
from pyrevit.coreutils.logger import get_logger
from pyrevit.coreutils import colors
from pyrevit import framework
from pyrevit.framework import System
from pyrevit.framework import Threading
from pyrevit.framework import Interop
from pyrevit.framework import Input
from pyrevit.framework import wpf, Forms, Controls, Media
from pyrevit.framework import CPDialogs
from pyrevit.framework import ComponentModel
from pyrevit.framework import ObservableCollection
from pyrevit.api import AdWindows
from pyrevit import revit, UI, DB
from pyrevit.forms import utils
from pyrevit.forms import toaster
from pyrevit import versionmgr

import pyevent #pylint: disable=import-error


#pylint: disable=W0703,C0302,C0103
mlogger = get_logger(__name__)


DEFAULT_CMDSWITCHWND_WIDTH = 600
DEFAULT_SEARCHWND_WIDTH = 600
DEFAULT_SEARCHWND_HEIGHT = 100
DEFAULT_INPUTWINDOW_WIDTH = 500
DEFAULT_INPUTWINDOW_HEIGHT = 600
DEFAULT_RECOGNIZE_ACCESS_KEY = False

WPF_HIDDEN = framework.Windows.Visibility.Hidden
WPF_COLLAPSED = framework.Windows.Visibility.Collapsed
WPF_VISIBLE = framework.Windows.Visibility.Visible


XAML_FILES_DIR = op.dirname(__file__)


ParamDef = namedtuple('ParamDef', ['name', 'istype', 'definition', 'isreadonly'])
"""Parameter definition tuple.

Attributes:
    name (str): parameter name
    istype (bool): true if type parameter, otherwise false
    definition (Autodesk.Revit.DB.Definition): parameter definition object
    isreadonly (bool): true if the parameter value can't be edited
"""


# https://gui-at.blogspot.com/2009/11/inotifypropertychanged-in-ironpython.html
class reactive(property):
    """Decorator for WPF bound properties"""
    def __init__(self, getter):
        def newgetter(ui_control):
            try:
                return getter(ui_control)
            except AttributeError:
                return None
        super(reactive, self).__init__(newgetter)

    def setter(self, setter):
        def newsetter(ui_control, newvalue):
            oldvalue = self.fget(ui_control)
            if oldvalue != newvalue:
                setter(ui_control, newvalue)
                ui_control.OnPropertyChanged(setter.__name__)
        return property(
            fget=self.fget,
            fset=newsetter,
            fdel=self.fdel,
            doc=self.__doc__)


class Reactive(ComponentModel.INotifyPropertyChanged):
    """WPF property updator base mixin"""
    PropertyChanged, _propertyChangedCaller = pyevent.make_event()

    def add_PropertyChanged(self, value):
        self.PropertyChanged += value

    def remove_PropertyChanged(self, value):
        self.PropertyChanged -= value

    def OnPropertyChanged(self, prop_name):
        if self._propertyChangedCaller:
            args = ComponentModel.PropertyChangedEventArgs(prop_name)
            self._propertyChangedCaller(self, args)


class WindowToggler(object):
    def __init__(self, window):
        self._window = window

    def __enter__(self):
        self._window.hide()

    def __exit__(self, exception, exception_value, traceback):
        self._window.show_dialog()


class WPFWindow(framework.Windows.Window):
    r"""WPF Window base class for all pyRevit forms.

    Args:
        xaml_source (str): xaml source filepath or xaml content
        literal_string (bool): xaml_source contains xaml content, not filepath
        handle_esc (bool): handle Escape button and close the window
        set_owner (bool): set the owner of window to host app window

    Example:
        >>> from pyrevit import forms
        >>> layout = '<Window ' \
        >>>          'xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" ' \
        >>>          'xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" ' \
        >>>          'ShowInTaskbar="False" ResizeMode="NoResize" ' \
        >>>          'WindowStartupLocation="CenterScreen" ' \
        >>>          'HorizontalContentAlignment="Center">' \
        >>>          '</Window>'
        >>> w = forms.WPFWindow(layout, literal_string=True)
        >>> w.show()
    """

    def __init__(self, xaml_source, literal_string=False, handle_esc=True, set_owner=True):
        """Initialize WPF window and resources."""
        # load xaml
        self.load_xaml(
            xaml_source,
            literal_string=literal_string,
            handle_esc=handle_esc,
            set_owner=set_owner
            )

    def load_xaml(self, xaml_source, literal_string=False, handle_esc=True, set_owner=True):
        # create new id for this window
        self.window_id = coreutils.new_uuid()

        if not literal_string:
            if not op.exists(xaml_source):
                wpf.LoadComponent(self,
                                  os.path.join(EXEC_PARAMS.command_path,
                                               xaml_source))
            else:
                wpf.LoadComponent(self, xaml_source)
        else:
            wpf.LoadComponent(self, framework.StringReader(xaml_source))

        # set properties
        self.thread_id = framework.get_current_thread_id()
        if set_owner:
            self.setup_owner()
        self.setup_icon()
        WPFWindow.setup_resources(self)
        if handle_esc:
            self.setup_default_handlers()

    def setup_owner(self):
        wih = Interop.WindowInteropHelper(self)
        wih.Owner = AdWindows.ComponentManager.ApplicationWindow

    @staticmethod
    def setup_resources(wpf_ctrl):
        #2c3e50
        wpf_ctrl.Resources['pyRevitDarkColor'] = \
            Media.Color.FromArgb(0xFF, 0x2c, 0x3e, 0x50)

        #23303d
        wpf_ctrl.Resources['pyRevitDarkerDarkColor'] = \
            Media.Color.FromArgb(0xFF, 0x23, 0x30, 0x3d)

        #ffffff
        wpf_ctrl.Resources['pyRevitButtonColor'] = \
            Media.Color.FromArgb(0xFF, 0xff, 0xff, 0xff)

        #f39c12
        wpf_ctrl.Resources['pyRevitAccentColor'] = \
            Media.Color.FromArgb(0xFF, 0xf3, 0x9c, 0x12)

        wpf_ctrl.Resources['pyRevitDarkBrush'] = \
            Media.SolidColorBrush(wpf_ctrl.Resources['pyRevitDarkColor'])
        wpf_ctrl.Resources['pyRevitAccentBrush'] = \
            Media.SolidColorBrush(wpf_ctrl.Resources['pyRevitAccentColor'])

        wpf_ctrl.Resources['pyRevitDarkerDarkBrush'] = \
            Media.SolidColorBrush(wpf_ctrl.Resources['pyRevitDarkerDarkColor'])

        wpf_ctrl.Resources['pyRevitButtonForgroundBrush'] = \
            Media.SolidColorBrush(wpf_ctrl.Resources['pyRevitButtonColor'])

        wpf_ctrl.Resources['pyRevitRecognizesAccessKey'] = \
            DEFAULT_RECOGNIZE_ACCESS_KEY

    def setup_default_handlers(self):
        self.PreviewKeyDown += self.handle_input_key    #pylint: disable=E1101

    def handle_input_key(self, sender, args):    #pylint: disable=W0613
        """Handle keyboard input and close the window on Escape."""
        if args.Key == Input.Key.Escape:
            self.Close()

    def set_icon(self, icon_path):
        """Set window icon to given icon path."""
        self.Icon = utils.bitmap_from_file(icon_path)

    def setup_icon(self):
        """Setup default window icon."""
        self.set_icon(op.join(BIN_DIR, 'pyrevit_settings.png'))

    def hide(self):
        self.Hide()

    def show(self, modal=False):
        """Show window."""
        if modal:
            return self.ShowDialog()
        # else open non-modal
        self.Show()

    def show_dialog(self):
        """Show modal window."""
        return self.ShowDialog()

    @staticmethod
    def set_image_source_file(wpf_element, image_file):
        """Set source file for image element.

        Args:
            element_name (System.Windows.Controls.Image): xaml image element
            image_file (str): image file path
        """
        if not op.exists(image_file):
            wpf_element.Source = \
                utils.bitmap_from_file(
                    os.path.join(EXEC_PARAMS.command_path,
                                 image_file)
                    )
        else:
            wpf_element.Source = utils.bitmap_from_file(image_file)

    def set_image_source(self, wpf_element, image_file):
        """Set source file for image element.

        Args:
            element_name (System.Windows.Controls.Image): xaml image element
            image_file (str): image file path
        """
        WPFWindow.set_image_source_file(wpf_element, image_file)

    def dispatch(self, func, *args, **kwargs):
        if framework.get_current_thread_id() == self.thread_id:
            t = threading.Thread(
                target=func,
                args=args,
                kwargs=kwargs
                )
            t.start()
        else:
            # ask ui thread to call the func with args and kwargs
            self.Dispatcher.Invoke(
                System.Action(
                    lambda: func(*args, **kwargs)
                    ),
                Threading.DispatcherPriority.Background
                )

    def conceal(self):
        return WindowToggler(self)

    @property
    def pyrevit_version(self):
        """Active pyRevit formatted version e.g. '4.9-beta'"""
        return 'pyRevit {}'.format(
            versionmgr.get_pyrevit_version().get_formatted()
            )

    @staticmethod
    def hide_element(*wpf_elements):
        """Collapse elements.

        Args:
            *wpf_elements: WPF framework elements to be collaped
        """
        for wpfel in wpf_elements:
            wpfel.Visibility = WPF_COLLAPSED

    @staticmethod
    def show_element(*wpf_elements):
        """Show collapsed elements.

        Args:
            *wpf_elements: WPF framework elements to be set to visible.
        """
        for wpfel in wpf_elements:
            wpfel.Visibility = WPF_VISIBLE

    @staticmethod
    def toggle_element(*wpf_elements):
        """Toggle visibility of elements.

        Args:
            *wpf_elements: WPF framework elements to be toggled.
        """
        for wpfel in wpf_elements:
            if wpfel.Visibility == WPF_VISIBLE:
                WPFWindow.hide_element(wpfel)
            elif wpfel.Visibility == WPF_COLLAPSED:
                WPFWindow.show_element(wpfel)

    @staticmethod
    def disable_element(*wpf_elements):
        """Enable elements.

        Args:
            *wpf_elements: WPF framework elements to be enabled
        """
        for wpfel in wpf_elements:
            wpfel.IsEnabled = False

    @staticmethod
    def enable_element(*wpf_elements):
        """Enable elements.

        Args:
            *wpf_elements: WPF framework elements to be enabled
        """
        for wpfel in wpf_elements:
            wpfel.IsEnabled = True

    def handle_url_click(self, sender, args): #pylint: disable=unused-argument
        """Callback for handling click on package website url"""
        return webbrowser.open_new_tab(sender.NavigateUri.AbsoluteUri)
class TemplateListItem(Reactive):
    """Base class for checkbox option wrapping another object."""

    def __init__(self, orig_item,
                 checked=False, checkable=True, name_attr=None):
        """Initialize the checkbox option and wrap given obj.

        Args:
            orig_item (any): Object to wrap (must have name property
                             or be convertable to string with str()
            checkable (bool): Use checkbox for items
            name_attr (str): Get this attribute of wrapped object as name
        """
        super(TemplateListItem, self).__init__()
        self.item = orig_item
        self.state = checked
        self._nameattr = name_attr
        self._checkable = checkable

    def __nonzero__(self):
        return self.state

    def __str__(self):
        return self.name or str(self.item)

    def __contains__(self, value):
        return value in self.name

    def __getattr__(self, param_name):
        return getattr(self.item, param_name)

    @property
    def name(self):
        """Name property."""
        # get custom attr, or name or just str repr
        if self._nameattr:
            return safe_strtype(getattr(self.item, self._nameattr))
        elif hasattr(self.item, 'name'):
            return getattr(self.item, 'name', '')
        else:
            return safe_strtype(self.item)

    def unwrap(self):
        """Unwrap and return wrapped object."""
        return self.item

    @reactive
    def checked(self):
        """Id checked"""
        return self.state

    @checked.setter
    def checked(self, value):
        self.state = value

    @property
    def checkable(self):
        """List Item CheckBox Visibility."""
        return WPF_VISIBLE if self._checkable \
            else WPF_COLLAPSED

    @checkable.setter
    def checkable(self, value):
        self._checkable = value




class TemplateUserInputWindow(WPFWindow):
    """Base class for pyRevit user input standard forms.

    Args:
        context (any): window context element(s)
        title (str): window title
        width (int): window width
        height (int): window height
        **kwargs: other arguments to be passed to :func:`_setup`
    """

    xaml_source = 'BaseWindow.xaml'

    def __init__(self, context, title, width, height, **kwargs):
        """Initialize user input window."""
        WPFWindow.__init__(self,
                           op.join(XAML_FILES_DIR, self.xaml_source),
                           handle_esc=True)
        self.Title = title or 'pyRevit'
        self.Width = width
        self.Height = height

        self._context = context
        self.response = None

        # parent window?
        owner = kwargs.get('owner', None)
        if owner:
            # set wpf windows directly
            self.Owner = owner
            self.WindowStartupLocation = \
                framework.Windows.WindowStartupLocation.CenterOwner

        self._setup(**kwargs)

    def _setup(self, **kwargs):
        """Private method to be overriden by subclasses for window setup."""
        pass

    @classmethod
    def show(cls, context,  #pylint: disable=W0221
             title='User Input',
             width=DEFAULT_INPUTWINDOW_WIDTH,
             height=DEFAULT_INPUTWINDOW_HEIGHT, **kwargs):
        """Show user input window.

        Args:
            context (any): window context element(s)
            title (str): window title
            width (int): window width
            height (int): window height
            **kwargs (any): other arguments to be passed to window
        """
        dlg = cls(context, title, width, height, **kwargs)
        dlg.ShowDialog()
        return dlg.response


class SelectFromList(TemplateUserInputWindow):

    xaml_source = 'SelectFromList.xaml'

    @property
    def use_regex(self):
        """Is using regex?"""
        return self.regexToggle_b.IsChecked

    def _setup(self, **kwargs):
        # custom button name?
        button_name = kwargs.get('button_name', 'Select')
        if button_name:
            self.select_b.Content = button_name

        # attribute to use as name?
        self._nameattr = kwargs.get('name_attr', None)

        # multiselect?
        if kwargs.get('multiselect', False):
            self.multiselect = True
            self.list_lb.SelectionMode = Controls.SelectionMode.Extended
            self.show_element(self.checkboxbuttons_g)
        else:
            self.multiselect = False
            self.list_lb.SelectionMode = Controls.SelectionMode.Single
            self.hide_element(self.checkboxbuttons_g)

        # info panel?
        self.info_panel = kwargs.get('info_panel', False)

        # return checked items only?
        self.return_all = kwargs.get('return_all', False)

        # filter function?
        self.filter_func = kwargs.get('filterfunc', None)

        # reset function?
        self.reset_func = kwargs.get('resetfunc', None)
        if self.reset_func:
            self.show_element(self.reset_b)

        # context group title?
        self.ctx_groups_title = \
            kwargs.get('group_selector_title', 'List Group')
        self.ctx_groups_title_tb.Text = self.ctx_groups_title

        self.ctx_groups_active = kwargs.get('default_group', None)

        # check for custom templates
        items_panel_template = kwargs.get('items_panel_template', None)
        if items_panel_template:
            self.Resources["ItemsPanelTemplate"] = items_panel_template

        item_container_template = kwargs.get('item_container_template', None)
        if item_container_template:
            self.Resources["ItemContainerTemplate"] = item_container_template

        item_template = kwargs.get('item_template', None)
        if item_template:
            self.Resources["ItemTemplate"] = \
                item_template

        # nicely wrap and prepare context for presentation, then present
        self._prepare_context()

        # setup search and filter fields
        self.hide_element(self.clrsearch_b)

        # active event listeners
        self.search_tb.TextChanged += self.search_txt_changed
        self.ctx_groups_selector_cb.SelectionChanged += self.selection_changed

        self.clear_search(None, None)

    def _prepare_context_items(self, ctx_items):
        new_ctx = []
        # filter context if necessary
        if self.filter_func:
            ctx_items = filter(self.filter_func, ctx_items)

        for item in ctx_items:
            if isinstance(item, TemplateListItem):
                item.checkable = self.multiselect
                new_ctx.append(item)
            else:
                new_ctx.append(
                    TemplateListItem(item,
                                     checkable=self.multiselect,
                                     name_attr=self._nameattr)
                    )

        return new_ctx

    def _prepare_context(self):
        if isinstance(self._context, dict) and self._context.keys():
            self._update_ctx_groups(sorted(self._context.keys()))
            new_ctx = {}
            for ctx_grp, ctx_items in self._context.items():
                new_ctx[ctx_grp] = self._prepare_context_items(ctx_items)
            self._context = new_ctx
        else:
            self._context = self._prepare_context_items(self._context)

    def _update_ctx_groups(self, ctx_group_names):
        self.show_element(self.ctx_groups_dock)
        self.ctx_groups_selector_cb.ItemsSource = ctx_group_names
        if self.ctx_groups_active in ctx_group_names:
            self.ctx_groups_selector_cb.SelectedIndex = \
                ctx_group_names.index(self.ctx_groups_active)
        else:
            self.ctx_groups_selector_cb.SelectedIndex = 0

    def _get_active_ctx_group(self):
        return self.ctx_groups_selector_cb.SelectedItem

    def _get_active_ctx(self):
        if isinstance(self._context, dict):
            return self._context[self._get_active_ctx_group()]
        else:
            return self._context

    def _list_options(self, option_filter=None):
        if option_filter:
            self.checkall_b.Content = 'Check'
            self.uncheckall_b.Content = 'Uncheck'
            self.toggleall_b.Content = 'Toggle'
            # get a match score for every item and sort high to low
            fuzzy_matches = sorted(
                [(x,
                  coreutils.fuzzy_search_ratio(
                      target_string=x.name,
                      sfilter=option_filter,
                      regex=self.use_regex))
                 for x in self._get_active_ctx()],
                key=lambda x: x[1],
                reverse=True
                )
            # filter out any match with score less than 80
            self.list_lb.ItemsSource = \
                ObservableCollection[TemplateListItem](
                    [x[0] for x in fuzzy_matches if x[1] >= 80]
                    )
        else:
            self.checkall_b.Content = 'Check All'
            self.uncheckall_b.Content = 'Uncheck All'
            self.toggleall_b.Content = 'Toggle All'
            self.list_lb.ItemsSource = \
                ObservableCollection[TemplateListItem](self._get_active_ctx())

    @staticmethod
    def _unwrap_options(options):
        unwrapped = []
        for optn in options:
            if isinstance(optn, TemplateListItem):
                unwrapped.append(optn.unwrap())
            else:
                unwrapped.append(optn)
        return unwrapped

    def _get_options(self):
        if self.multiselect:
            if self.return_all:
                return [x for x in self._get_active_ctx()]
            else:
                return self._unwrap_options(
                    [x for x in self._get_active_ctx()
                     if x.state or x in self.list_lb.SelectedItems]
                    )
        else:
            return self._unwrap_options([self.list_lb.SelectedItem])[0]

    def _set_states(self, state=True, flip=False, selected=False):
        if selected:
            current_list = self.list_lb.SelectedItems
        else:
            current_list = self.list_lb.ItemsSource
        for checkbox in current_list:
            # using .checked to push ui update
            if flip:
                checkbox.checked = not checkbox.checked
            else:
                checkbox.checked = state

    def _toggle_info_panel(self, state=True):
        if state:
            # enable the info panel
            self.splitterCol.Width = System.Windows.GridLength(8)
            self.infoCol.Width = System.Windows.GridLength(self.Width/2)
            self.show_element(self.infoSplitter)
            self.show_element(self.infoPanel)
        else:
            self.splitterCol.Width = self.infoCol.Width = \
                System.Windows.GridLength.Auto
            self.hide_element(self.infoSplitter)
            self.hide_element(self.infoPanel)

    def toggle_all(self, sender, args):    #pylint: disable=W0613
        """Handle toggle all button to toggle state of all check boxes."""
        self._set_states(flip=True)

    def check_all(self, sender, args):    #pylint: disable=W0613
        """Handle check all button to mark all check boxes as checked."""
        self._set_states(state=True)

    def uncheck_all(self, sender, args):    #pylint: disable=W0613
        """Handle uncheck all button to mark all check boxes as un-checked."""
        self._set_states(state=False)

    def check_selected(self, sender, args):    #pylint: disable=W0613
        """Mark selected checkboxes as checked."""
        self._set_states(state=True, selected=True)

    def uncheck_selected(self, sender, args):    #pylint: disable=W0613
        """Mark selected checkboxes as unchecked."""
        self._set_states(state=False, selected=True)

    def button_reset(self, sender, args):#pylint: disable=W0613
        if self.reset_func:
            all_items = self.list_lb.ItemsSource
            self.reset_func(all_items)

    def button_select(self, sender, args):    #pylint: disable=W0613
        """Handle select button click."""
        self.response = self._get_options()
        self.Close()

    def search_txt_changed(self, sender, args):    #pylint: disable=W0613
        """Handle text change in search box."""
        if self.info_panel:
            self._toggle_info_panel(state=False)

        if self.search_tb.Text == '':
            self.hide_element(self.clrsearch_b)
        else:
            self.show_element(self.clrsearch_b)

        self._list_options(option_filter=self.search_tb.Text)

    def selection_changed(self, sender, args):
        if self.info_panel:
            self._toggle_info_panel(state=False)

        self._list_options(option_filter=self.search_tb.Text)

    def selected_item_changed(self, sender, args):
        if self.info_panel and self.list_lb.SelectedItem is not None:
            self._toggle_info_panel(state=True)
            self.infoData.Text = \
                getattr(self.list_lb.SelectedItem, 'description', '')

    def toggle_regex(self, sender, args):
        """Activate regex in search"""
        self.regexToggle_b.Content = \
            self.Resources['regexIcon'] if self.use_regex \
                else self.Resources['filterIcon']
        self.search_txt_changed(sender, args)
        self.search_tb.Focus()

    def clear_search(self, sender, args):    #pylint: disable=W0613
        """Clear search box."""
        self.search_tb.Text = ' '
        self.search_tb.Clear()
        self.search_tb.Focus()



