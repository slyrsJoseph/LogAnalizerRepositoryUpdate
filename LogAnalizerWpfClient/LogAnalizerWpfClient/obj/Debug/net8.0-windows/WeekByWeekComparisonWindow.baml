   M S B A M L   `   `   `  ����O  KLogAnalizerWpfClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:LogAnalizerWpfClientLogAnalizerWpfClient  5      5    /LogAnalizerWpfClient.WeekByWeekComparisonWindow   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      4local"clr-namespace:LogAnalizerWpfClient     5      _ [MaterialDesignThemes.Wpf, Version=5.2.2.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5        � Title$  Week Comparison��5      $	��600��5      $
��1024��6*    � WindowStartupLocation$ CenterScreen=�5       � WindowStyle$
 None=�5	       � 
ResizeMode$ CanResizeWithGrip=�5
         MaterialDesignPaper#���   5       � 	Resources 5        AnimatedRaisedButton �� 
TargetType  MaterialDesignRaisedButton �� BasedOn �� Property �� Value	 �� ScaleX
 �� ScaleY T� Y3  -System.Windows.Media.Effects.DropShadowEffect   Color   
BlurRadius   ShadowDepth   Opacity �� Triggers P� Property P� Value N� EnterActions �� TargetProperty	 W� To g� Duration N� ExitActions  WeekButtonStyle �� 
TargetType �� CornerRadius��%9
  &	       7  &	 2    �� 5   
   " ��6-   8   .�5      ��  �@J�RenderTransformOrigin$ 0.5,0.5&�66   �� 5       � z� 5      V� 5      .~�5      �� $	 1Q�6)   $
 1Q�64   T� 5      $ 0Q�6-   5      5      �� 5       �@J�Effect 5        5      $ #00FFFF��6'   $ 4Q�67   $ 0Q�6F   $	 0.2Q�6V   5       5      P� 5       �@J�IsMouseOver$
 True��61    5       �� 5!      .��5"      �� .��5#   "   W� $V P(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)�62    1.055$   2   $ 0:0:0.15D�6<   W� 5%   "   $V P(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)�62    1.055&   2   $ 0:0:0.15D�6<   W� 5'   "   $U O(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.Y)�62    -25(   2   $ 0:0:0.15D�6:   W� 5)   "   $3 -(Button.Effect).(DropShadowEffect.BlurRadius)�62    125*   2   $ 0:0:0.15D�6:   W� 5+   "   $0 *(Button.Effect).(DropShadowEffect.Opacity)�62    0.65,   2   $ 0:0:0.15D�6;   5-      5.       50      �� 51      .��52      �� .��53   "   W� $V P(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)�62    154   2   $ 0:0:0.15D�69   W� 55   "   $V P(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)�62    156   2   $ 0:0:0.15D�69   W� 57   "   $U O(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.Y)�62    058   2   $ 0:0:0.15D�69   W� 59   "   $3 -(Button.Effect).(DropShadowEffect.BlurRadius)�62    45:   2   $ 0:0:0.15D�69   W� 5;   "   $0 *(Button.Effect).(DropShadowEffect.Opacity)�62    0.25<   2   $ 0:0:0.15D�6;   5=      5>      5@      5B      �� 5E   
   " ��6(   .�5F      ��  � ��
 �  ��6+   �� 5G       � ��
 �����6+   �� 5H       � ��$
 Bold"�6+   �� 5I       � ��$ 4q�6'   �� 5J       � ��$ 50��6&   �� 5K       � ��$ 30��6'   �� 5L       � �� �#00FFFF6,   �� 5M       � ��$ 2q�60   �� 5N       �@�Cursor$
 Hand��6'   �� 5O       � �� 5P      �� 5Q      " ��6&   .R�5R      �� $ 6��6!   #��z��5S   !   #��z��5T   !   #��z��5U   !   ._�5V      �� $��Center=�6/   $��Center=�6L   5W      5X      5Z       5[      P� 5\       �@J�IsMouseOver$
 True��61   . �5]      ��  � �� �#00FFFF63   �� 5^       � ��
 �   �63   5_      5a      5b      .��5d      ]� .+�5e   
   � -   5          PrimaryHueMidBrush#���  5e      $��40��6A   $	��Top=�6M    � ColumnDefinitions 5f      �� 5g      �� 5h      $
��Auto �6#   .+�5j      �� $��Week Comparison��6   $��Center=�5k      $
��16,0q�5l      $
��Bold"�5m      
�������$��16(�5o      �� 5p       �� Orientation$ 
Horizontal=�6   $��1��62   .+�5q      �� -   5         & "MaterialDesignToolForegroundButton#��[ 5q      $��40��6i   $��40��6t    � ToolTip$ Close��6�   .��5r      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Close=�6.   5s      5t      5u      � 5w   
   #���   6   $��20q�6B    � RowDefinitions 5x      �� 5y      $
��Auto �6    �� 5z      $��* �6    �� 5{      $
��Auto �6    .+�5~      �� $0��*Select week to import and logs comparision��6   $��16(�5      $
��Bold"�6&   ���#00FFFF$��0,0,0,10q�5�   -   $��Center=�5�      A� 5�      -   5         A�Name$ WeekGrid��5�      $��1��6,     A� Columns$  10��69   ! A� Rows$! 6��6F   $��	0,10,0,10q�6O   $��Center=�6b   �� 5�      -   5        " ��Name$" SelectedDbText��5�      $�� ��5�      ���#00FFFF$��12(�5�      $
��Left=�5�      $��Bottom=�5�      $��10q�5�      $��2��5�      �� 5�      $��2��6   $ 
Horizontal=�6&   $��Center=�6?   $��0,20,0,0q�6\   .+�5�      �� -   5        $��Import��5�      $	��120��6*   #��[ 6N   $��5q�6|   �� 5�      -   5        $��🗑 Delete Log for Week��5�      #��[ 6:   $��5q�6�   �� 5�      -   5        $��Compare��5�      $	��120��6+   #��[ 6P   $��5q�6~   �� 5�      -   5        $
��Back��5�      $	��100��6(   #��[ 6J   $��5q�6x   5�      5�      5�      5�      