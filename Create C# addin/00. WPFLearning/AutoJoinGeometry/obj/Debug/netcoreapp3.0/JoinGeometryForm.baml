   M S B A M L   `   `   `  ����K  GAutoJoinGeometry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:AutoJoinGeometryAutoJoinGeometry  5   	   U QSystem.Runtime, Version=4.2.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a7,clr-namespace:System;assembly=System.RuntimeSystem 5      b ^PresentationFramework.Aero2, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35hKclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero2Microsoft.Windows.Themes 5      '    !AutoJoinGeometry.JoinGeometryForm   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:AutoJoinGeometry     5   	   ?System,clr-namespace:System;assembly=System.Runtime     5      ]themeKclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero2     5      $	��550��5   	   $	��500��6     � WindowStartupLocation$  CenterScreen=�5	   	    � Title$ 	Auto Join��5       � 	Resources 5         ButtonContent1  ComboBoxBackground1 �� EndPoint � Color � Offset  FocusVisual �� Property �� Value �� StrokeDashArray	 J� SnapsToDevicePixels  TextBox.Static.Background
 �� Color  ComboBoxEditableTextBox �� 
TargetType �� 
TargetType ��Name  ComboBox.Static.Background �� 
StartPoint  ComboBox.Static.Border  ComboBox.Static.Glyph ' #ComboBox.Static.Editable.Background #	 ComboBox.Static.Editable.Border .
 *ComboBox.Static.Editable.Button.Background * &ComboBox.Static.Editable.Button.Border ! ComboBox.MouseOver.Background  ComboBox.MouseOver.Border  ComboBox.MouseOver.Glyph * &ComboBox.MouseOver.Editable.Background & "ComboBox.MouseOver.Editable.Border 1 -ComboBox.MouseOver.Editable.Button.Background - )ComboBox.MouseOver.Editable.Button.Border  ComboBox.Pressed.Background  ComboBox.Pressed.Border  ComboBox.Pressed.Glyph ( $ComboBox.Pressed.Editable.Background $  ComboBox.Pressed.Editable.Border / +ComboBox.Pressed.Editable.Button.Background + 'ComboBox.Pressed.Editable.Button.Border   ComboBox.Disabled.Background  ComboBox.Disabled.Border  ComboBox.Disabled.Glyph ) %ComboBox.Disabled.Editable.Background % !ComboBox.Disabled.Editable.Border 0 ,ComboBox.Disabled.Editable.Button.Background ,  (ComboBox.Disabled.Editable.Button.Border ! ComboBoxToggleButton ��Name F�Name �� Triggers ^� 
Conditions �� Binding �� RelativeSource �� AncestorType �� Value �� Mode �� 
TargetName P� Property P� Value " ComboBoxEditableTemplate �Name � ColumnDefinitions �Name � AllowsTransparency5  /Microsoft.Windows.Themes.SystemDropShadowChrome  Name    Color! �� MaxDropDownHeight" �� ElementName# �� ClearTypeHint$ ��Name% ��Name& ��Name-  'System.Windows.Input.KeyboardNavigation'   DirectionalNavigation( d�Name) d� 	IsChecked* �� Mode+ ��Name, �� 
IsReadOnly- P� 
SourceName. ]� 
Conditions/ �� Property # ComboBoxTemplate0 ��Name#1 �� SelectionBoxItemStringFormat2 �� ContentStringFormat3 �� SelectionBoxItemTemplate4 �� SelectionBoxItem5 J� IsHitTestVisible6 �� Triggers��%�/  &	        &	      &	 �     &	 @    &	 c    &	 �    &	 .    &	 Q    &	 t    &		 �    &	
 �    &	 �    &	     &	 �    &	 �    &	 �    &	 	    &	 ,    &	 �    &	 �    &	 �    &	 �    &	 �    &	 �    &	     &	 �    &	 �    &	 �    &	 	    &	 ;	    &	 ^	    &	 �	    &	  �	    &	! �	    7  7  7  7  7 	 7 
 7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7   &	" 7    7 ! 7  7  &	#  "    7 ! '�� �*    7  7  7  7 # 7 " ��5   
   +��62   �� 5   
   $	 0,1&�6:   .��5      � $ White��6   � 5      $ White��6   $ 1Q�6)   5      �� 5   
   .�5      ��  � �� 5      �� 5      .R�5      �� $��2q�6$   $	 1 2R�6/   #��� ��6E   $
	 true��6�   $��1��6�   5      5      5       �� 5!   
   $
 	#FFFFFFFF��6<   �� 5"   
   " ��60   .�5#      ��  �@�OverridesDefaultStyle$
 true��66   �� 5$       �@J�	AllowDrop$
 true��6*   �� 5%       � ��$ 0��6)   �� 5&       � ��$ 0��6*   �� 5'       �@�FocusVisualStyle 61   Z� �� 5(       �@��PanningMode$ VerticalFirst=�69   �� 5)       �@��IsFlicksEnabled$ False��67   �� 5*       � �� 5+      �� 5,      " ��6&   .R�5-      �� $ PART_ContentHost��6'   
������ $|�false��6Z   $��Hidden=�6l   $��Hidden=�6�   5.      50      51      �� 52   
   $	 0,1&�6A   $	 0,0&�6P   .��53      � $ White��6   $	 0.0Q�6)   � 54      $ White��6   $	 1.0Q�6)   55      �� 56   
   $
 	#FFACACAC��69   �� 57   
   $
 	#FF606060��68   �� 58   
   $
 	#FFFFFFFF��6F   �� 59   
   $
 	#FFABADB3��6B   �� 5:   
   $
 Transparent��6M   �� 5;   
   $
 Transparent��6I   �� 5<   
   $	 0,1&�6D   $	 0,0&�6S   .��5=      � $ 	#FFECF4FC��6   $	 0.0Q�6-   � 5>      $ 	#FFDCECFC��6   $	 1.0Q�6-   5?      �� 5@   
   $
 	#FF7EB4EA��6<   �� 5A   
   $
 	#FF000000��6;   �� 5B   
   $
 	#FFFFFFFF��6I   �� 5C   
   $
 	#FF7EB4EA��6E   �� 5D   
   $	 0,1&�6T   $	 0,0&�6c   .��5E      � $ 	#FFEBF4FC��6   $	 0.0Q�6-   � 5F      $ 	#FFDCECFC��6   $	 1.0Q�6-   5G      �� 5H   
   $
 	#FF7EB4EA��6L   �� 5I   
   $	 0,1&�6B   $	 0,0&�6Q   .��5J      � $ 	#FFDAECFC��6   $	 0.0Q�6-   � 5K      $ 	#FFC4E0FC��6   $	 1.0Q�6-   5L      �� 5M   
   $
 	#FF569DE5��6:   �� 5N   
   $
 	#FF000000��69   �� 5O   
   $
 	#FFFFFFFF��6G   �� 5P   
   $
 	#FF569DE5��6C   �� 5Q   
   $	 0,1&�6R   $	 0,0&�6a   .��5R      � $ 	#FFDAEBFC��6   $	 0.0Q�6-   � 5S      $ 	#FFC4E0FC��6   $	 1.0Q�6-   5T      �� 5U   
   $
 	#FF569DE5��6J   �� 5V   
   $
 	#FFF0F0F0��6?   �� 5W   
   $
 	#FFD9D9D9��6;   �� 5X   
   $
 	#FFBFBFBF��6:   �� 5Y   
   $
 	#FFFFFFFF��6H   �� 5Z   
   $
 	#FFBFBFBF��6D   �� 5[   
   $
 Transparent��6O   �� 5\   
   $
 Transparent��6K   �� 5]   
   " d�6-   .�5^      ��  �@�OverridesDefaultStyle$
 true��66   �� 5_       � ��$ false��6*   �� 5`       � |�$ false��6*   �� 5a       �@��	ClickMode$ Press=�6*   �� 5b       � �� 5c      �� 5d      " d�6&   .R�5e      �� $ templateRoot��6!   8��  8�� #��z��6�   $
	 true��6�   ._�5f      �� $ splitBorder��6%   
������ $��1q�6T   $��Right=�6h   $��0q�6�   $
	 true��6�   #��� r�6�   ._�5g   "   F� $ arrow��6'   F����        ���( ��( јaQ     јaQ P����̰( � �    P����        8�� $��Center=�6�   $��0q�6�   $��Center=�6�   5h      5i       5j      ^� 5k       5l   "   �� 5m   &    60   �� *
IsEditable+ �� " �� true6�   �� 5n   &    60   �� *IsMouseOver+ �� $
 Self=�	 false6{   �� 5o   &    60   �� *	IsPressed+ �� $
 Self=�	 false6y   �� 5p   &    60   �� *	IsEnabled+ �� $
 Self=� true6y   .0�5r   "   �� $ templateRoot��6?   8   � ��6Y   �� 5s   "   $ templateRoot��6@   8   � ��6Z   �� 5t   "   $ splitBorder��6?   8   � ��6X   �� 5u   "   $ splitBorder��6@   8   � ��6Y   5v      P� 5w       �@J�IsMouseOver$
 true��6=   . �5x   "   �� $ arrow��69   8   � ��6L   5y      ^� 5z       5{   "   �� 5|   &    60   �� *IsMouseOver+ �� $
 Self=� true6{   �� 5}   &    60   �� *
IsEditable+ �� " ��	 false6�   .0�5   "   �� $ templateRoot��6?   8   � ��6Y   �� 5�   "   $ templateRoot��6@   8 	  � ��6Z   5�      ^� 5�       5�   "   �� 5�   &    60   �� *IsMouseOver+ �� $
 Self=� true6{   �� 5�   &    60   �� *
IsEditable+ �� " �� true6�   .0�5�   "   �� $ templateRoot��6?   8 
  � ��6Y   �� 5�   "   $ templateRoot��6@   8   � ��6Z   �� 5�   "   $ splitBorder��6?   8   � ��6X   �� 5�   "   $ splitBorder��6@   8   � ��6Y   5�      P� 5�       � ��$
 true��6;   . �5�   "   �� $ arrow��69   8   � ��6L   5�      ^� 5�       5�   "   �� 5�   &    60   �� *	IsPressed+ �� $
 Self=� true6y   �� 5�   &    60   �� *
IsEditable+ �� " ��	 false6�   .0�5�   "   �� $ templateRoot��6?   8   � ��6Y   �� 5�   "   $ templateRoot��6@   8   � ��6Z   5�      ^� 5�       5�   "   �� 5�   &    60   �� *	IsPressed+ �� $
 Self=� true6y   �� 5�   &    60   �� *
IsEditable+ �� " �� true6�   .0�5�   "   �� $ templateRoot��6?   8   � ��6Y   �� 5�   "   $ templateRoot��6@   8   � ��6Z   �� 5�   "   $ splitBorder��6?   8   � ��6X   �� 5�   "   $ splitBorder��6@   8   � ��6Y   5�      P� 5�       � {�$ false��6;   . �5�   "   �� $ arrow��69   8   � ��6L   5�      ^� 5�       5�   "   �� 5�   &    60   �� *	IsEnabled+ �� $
 Self=�	 false6y   �� 5�   &    60   �� *
IsEditable+ �� " ��	 false6�   .0�5�   "   �� $ templateRoot��6?   8   � ��6Y   �� 5�   "   $ templateRoot��6@   8   � ��6Z   5�      ^� 5�       5�   "   �� 5�   &    60   �� *	IsEnabled+ �� $
 Self=�	 false6y   �� 5�   &    60   �� *
IsEditable+ �� " �� true6�   .0�5�   "   �� $ templateRoot��6?   8   � ��6Y   �� 5�   "   $ templateRoot��6@   8   � ��6Z   �� 5�   "   $ splitBorder��6?   8   � ��6X   �� 5�   "   $ splitBorder��6@   8   � ��6Y   5�      5�      5�      5�      �� 5�   
   " ��6;   .R�5�      � $ templateRoot��6   $
	 true��6)    5�      �� 5�      $��* �6'   �� 5�      #��� r�6'   $��0 �6z   .+�5�      � $ 
PART_Popup��6   $
 true��6,   $��2��6F   $��Bottom=�6�   #��� .�6�   ��6Z   �� *IsDropDownOpen+ �� $ TemplatedParent=�.��5�        $ shadow��63   $  Transparent��6C   #��z! 6�   ��6W   �� *ActualWidth+$" templateRoot��._�5�      �� $ dropDownBorder��6!   #��� ��69   #��� ��6   $��1q�6�   ._�5�      �� $ DropDownScrollViewer��6+   .��5�   "   � $
 grid��6'   $# Enabled=�65   .+�5�   &   �� $$ canvas��6-   $
��Left=�6=   $��0��6X   $	��Top=�6c   $��0��6{   .+�5�   *   �� $% 
opaqueRect��64   ��6H   �� *
Background+$" dropDownBorder����6�   �� *ActualHeight+$" dropDownBorder����6�   �� *ActualWidth+$" dropDownBorder��5�   '   �� 5�   &   $& ItemsPresenter��65   $' 	Contained=�6M   #	 z	 6�   5�   #   5�      5�      5�      5�      d� 5�      $( toggleButton��6   #��z��65   #��z��6_   #��z��6�   $��2��6�   8��  ) 6�   �� *IsDropDownOpen+$* TwoWay=� �� $ TemplatedParent=�6;  �� 5�      $ border��6   8�� #��z��6a   ._�5�      �� $+ PART_EditableTextBox��6   #��z��6<   #��z��6�   8�� #��z��62  , 6�   �� *
IsReadOnly+ �� $ TemplatedParent=�62  5�      5�       5�      P� 5�       � {�$ false��6/   . �5�      �� $ border��60    �@J�Opacity$
 0.56Q�6D   5�      P� 5�       �@J�IsKeyboardFocusWithin$
 true��6;   . �5�      ��  � ��
 �   �63   5�      P� 5�      $- 
PART_Popup��63    �@�HasDropShadow$
 true��6K   . �5�      �� $ shadow��6/    � ��$ 0,0,5,5q�6C   �� 5�      $ shadow��6.    �@ Color$ 	#71000000��6B   5�      P� 5�       �@��HasItems$ false��6.   . �5�      �� $ dropDownBorder��6/    � ��$ 95��6K   5�      ]� 5�      . 5�      �� 5�      / �@��
IsGrouping$
 true��6:   �� 5�      "/ �@%�IsVirtualizingWhenGrouping$ false��6\   ./�5�      ��  � ��$ false��6F   5�      P� 5�      $- DropDownScrollViewer��6C    � ��$ false��6e   . �5�      �� $ 
opaqueRect��63    6K   �� *VerticalOffset+$" DropDownScrollViewer�� �@��Top�� 5�      $ 
opaqueRect��64    6L   �� *HorizontalOffset+$" DropDownScrollViewer�� �@��Left5�      5�      �� 5�   
   " ��63   .R�5�      � $ templateRoot��6   $
	 true��6)    5�      �� 5�      $��* �6'   �� 5�      #��� r�6'   $��0 �6z   .+�5�      � $ 
PART_Popup��6   $
 true��6,   $��2��6F   $��1q�6�   $��Bottom=�6�   #��� .�6�   ��6Z   �� *IsDropDownOpen+$* TwoWay=� �� $ TemplatedParent=�.��5�        $ shadow��63   $  Transparent��6C   #��z! 6�   ��6W   �� *ActualWidth+$" templateRoot��._�5�      �� $ dropDownBorder��6!   #��� ��69   #��� ��6   $��1q�6�   ._�5�      �� $ DropDownScrollViewer��6+   .��5�   "   � $
 grid��6'   $# Enabled=�65   .+�5�   &   �� $$ canvas��6-   $
��Left=�6=   $��0��6X   $	��Top=�6c   $��0��6{   .+�5�   *   �� $% 
opaqueRect��64   ��6H   �� *
Background+$" dropDownBorder����6�   �� *ActualHeight+$" dropDownBorder����6�   �� *ActualWidth+$" dropDownBorder��5�   '   �� 5�   &   $& ItemsPresenter��65   $' 	Contained=�6M   #	 z	 6�   5�   #   5�      5      5     5     d� 5     $( toggleButton��6   #��z��65   #��z��6_   #��z��6�   $��2��6�   8��  ) 6�   �� *IsDropDownOpen+$* TwoWay=� �� $ TemplatedParent=�6;  �� 5     $0 contentPresenter��6#   #2 z1 6=   #��z3 6�   #��z4 6�   #��z��6�   #��z��6-  $5 false��6p  #��z��6�  #	 z	 6�  #��z��6�  5      5     P� 5     $- 
PART_Popup��63    �@�HasDropShadow$
 true��6K   . �5     �� $ shadow��6/    � ��$ 0,0,5,5q�6C   �� 5	     $ shadow��6.    �@ Color$ 	#71000000��6B   5
     P� 5      �@��HasItems$ false��6.   . �5     �� $ dropDownBorder��6/    � ��$ 95��6K   5     ]� 5     . 5     �� 5     / �@��
IsGrouping$
 true��6:   �� 5     "/ �@%�IsVirtualizingWhenGrouping$ false��6\   ./�5     ��  � ��$ false��6F   5     P� 5     $- DropDownScrollViewer��6C    � ��$ false��6e   . �5     �� $ 
opaqueRect��63    6K   �� *VerticalOffset+$" DropDownScrollViewer�� �@��Top�� 5     $ 
opaqueRect��64    6L   �� *HorizontalOffset+$" DropDownScrollViewer�� �@��Left5     5     �� 5  
   " ��6   .�5     ��  �@�FocusVisualStyle8   61   �� 5      � ��8  6+   �� 5      � ��8  6,   �� 5      � ��# � ��6+   �� 5       � ��$ 1q�60   �� 5!      � ��$
 Auto=�6K   �� 5"      � ��$
 Auto=�6I   �� 5#      � ��$ 6,3,5,3q�6(   �� 5$      � ��$
 true��6>   �� 5%      �@��PanningMode$
 Both=�69   �� 5&      �@��IsFlicksEnabled$ False��67   �� 5'      � ��8  6)   6 5(     P� 5)      �@��
IsEditable$
 true��60   . �5*     ��  � ��$ false��62   �� 5+      � ��$ 2q�60   �� 5,      � ��8  61   5-     5/     50     .��51     � 7 � RowDefinitions7 53  
   �� 54     $��60 �6   6+   �� 55     $��30 �6   6+   �� 56     6   �� 58     $��80 �6   6*   .+�5;  
   � $��0��6   $��0 4 0 0 q�6   .+�5<     �� ���	#FFB0E5FB.��5=     � .+�5>     	� -   5        8 	�Name$8 RadioBtnAllSelected��5>  "   $!��Join all selected elements ��6?   $��	10 10 0 0q�6e   6{   	� 5?     -   5        $8 RadioCurrentView��5?  "   $(��"Join all elements in current view ��6<   $��10 3 0 3q�6i   $��Bottom=�6{   6�   5A     5D     5E     � 5G  
   $��1��6    5H     �� 5I     5J     �� 5K     $��80 �6#   60   �� 5L     6%   .+�5N     �� $��Category��6   $��0��6(   $��2,0,0,0q�68   $��0 0 0 0q�6I   ���	#FFB0E5FB5P     �� 5R     $��Reverse��6   $��1��6'   $��0.5 0 0.5 0q�67   ���	#FFB0E5FB5T     �� 5U     $��Join with Category��6   $��2��62   $��0 0 0 0q�6B   $��0,0,2,0q�6\   ���	#FFB0E5FB6m   5V     � 5X  
   $��2��6   .+�5Z     �� -   5        9 ��Name$9 AutoJoinCategory��5Z     $��2,0,2,0q�63   #���   6D   5\     5^     � 5`  
   $��3��6   #��� /�6   .+�5b     �� -   5        : ��Name$: btnOk��5b     $��OK��6$   ; �� 	IsDefault$
; True��61   $��25��6B   $��70��6N   $��
0,15,10,10q�6Y   $��Right=�6m   $��Bottom=�6�   ���	#FF00B3FF 5c     ��%^   '��       �� 5d     " ��6   .�5e     ��  �@��CornerRadius$ 2��69   5f     5g     5h     �� 5i     -   5        $: btnAdd��5i     $
��Left=�6%   $��	40,10,0,0q�6@   $	��Top=�6S   < J� RenderTransformOrigin$< 
16.48,0.44&�6�    5j     ��%�   '��       �� 5k     " ��6   .�5l     ��  �@��CornerRadius$ 6��69   �� 5m      � ��$ 25��63   �� 5n      � ��$ 25��62   5o     5p     ��5q     �� 5r     = �� ImageSource$:= 4/AutoJoinGeometry;component/Resources/add-button.png��6!   5t     �� 5u     -   5        $: 	btnRemove��5u     $
��Left=�6(   $��	10,10,0,0q�6C   $	��Top=�6V    5v     ��%�   '��       �� 5w     " ��6   .�5x     ��  �@��CornerRadius$ 6��69   �� 5y      � ��$ 25��63   �� 5z      � ��$ 25��62   5{     5|     ��5}     �� 5~     $6= 0/AutoJoinGeometry;component/Resources/Remove.png��6!   5�     5�     5�     5�     