   M S B A M L   `   `   `  ����O  KLogAnalizerWpfClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:LogAnalizerWpfClientLogAnalizerWpfClient  5      8    2LogAnalizerWpfClient.TableDateTimeComparisonWindow   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      _ [MaterialDesignThemes.Wpf, Version=5.2.2.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      .local"clr-namespace:LogAnalizerWpfClient  5        � Title$  Table View (Log Browser)��5      $	��600��5      $
��1024��6*    � WindowStartupLocation$ CenterScreen=�5       � WindowStyle$
 None=�5	       � 
ResizeMode$ CanResizeWithGrip=�5
         MaterialDesignPaper#���   5       � 	Resources 5        AnimatedRaisedButton �� 
TargetType  MaterialDesignRaisedButton �� BasedOn �� Property �� Value	 �� ScaleX
 �� ScaleY T� Y3  -System.Windows.Media.Effects.DropShadowEffect   Color   
BlurRadius   ShadowDepth   Opacity �� Triggers P� Property P� Value N� EnterActions �� TargetProperty	 W� To g� Duration N� ExitActions  DataGridHeaderStyle=  7System.Windows.Controls.Primitives.DataGridColumnHeader  LightComboBoxStyle��%�  &	       7  &	 �    &	 {    �� 5   
   " ��6-   8   .�5      ��  �@J�RenderTransformOrigin$ 0.5,0.5&�66   �� 5       � z� 5      V� 5      .~�5      �� $	 1Q�6)   $
 1Q�64   T� 5      $ 0Q�6-   5      5      �� 5       �@J�Effect 5        5      $ #00FFFF��6'   $ 4Q�67   $ 0Q�6F   $	 0.2Q�6V   5       5      P� 5       �@J�IsMouseOver$
 True��61    5       �� 5!      .��5"      �� .��5#   "   W� $V P(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)�62    1.055$   2   $ 0:0:0.15D�6<   W� 5%   "   $V P(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)�62    1.055&   2   $ 0:0:0.15D�6<   W� 5'   "   $U O(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.Y)�62    -25(   2   $ 0:0:0.15D�6:   5)      5*       5,      �� 5-      .��5.      �� .��5/   "   W� $V P(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)�62    150   2   $ 0:0:0.15D�69   W� 51   "   $V P(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)�62    152   2   $ 0:0:0.15D�69   W� 53   "   $U O(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.Y)�62    054   2   $ 0:0:0.15D�69   55      56      58      5:      �� 5=   
   "  6,   .�5>      ��  � �� �#1A1A1A6+   �� 5?       � �� �#00FFFF6+   �� 5@       � ��$
 Bold"�6+   �� 5A       � �� �#00FFFF6,   �� 5B       � ��$ Center=�6;   5C      �� 5F   
   " ��6+   .�5G      ��  � ��
 �   �6+   �� 5H       � ��
 �����6+   �� 5I       � �� �#00FFFF6,   �� 5J       � ��$ 1q�60   �� 5K       � ��$ 4q�6(   �� 5L       � ��$ SemiBold"�6+   5M      5N      .��5P      ]� .+�5R   
   � -   5          PrimaryHueMidBrush#���  5R      $��40��6A   $	��Top=�6M    � ColumnDefinitions 5S      �� 5T      �� 5U      $
��Auto �6#   .+�5W      �� $!��📊 Alarm Log Table Viewer��6   $��Center=�5X      $
��16,0q�5Y      $
��Bold"�5Z      
�������$��16(�5\      �� 5]       �� Orientation$ 
Horizontal=�6   $��1��62   .+�5^      �� -   5         & "MaterialDesignToolForegroundButton#��[ 5^      $��40��6l   $��40��6w    � ToolTip$ Minimize��6�   .��5_      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ WindowMinimize=�6.   5`      �� 5a      -   5        #��[ 5a      $��40��6s   $��40��6~   $ Maximize/Restore��6�   .��5b        $ WindowRestore=�6.   5c      �� 5d      -   5        #��[ 5d      $��40��6i   $��40��6t   $ Close��6�   .��5e        $ Close=�6.   5f      5g      5h      � 5j   
   $��10q�6    � RowDefinitions 5k      �� 5l      $
��Auto �6    �� 5m      $
��Auto �6    �� 5n      $��* �6    �� 5o      $
��Auto �6    .+�5s      �� -   5         ��Name$ SelectedDbText��5t      $��0��5s      $��12(�5u      $
��Bold"�5v      ���#00FFFF$�� ��5x      $��	0,10,20,0q�5y      $��Right=�5z      $	��Top=�5{      �� 5~      $��1��6   $ Vertical=�6&   $��0,0,0,10q�6=   .+�5      �� $ 
Horizontal=�6   $��0,0,0,5q�66   .+�5�      �� $��Select week:��6    ���#00FFFF$	��200��6I   $��Center=�6U   �� 5�      -   5         ��Name$ 	comboWeek��5�      $	��150��5�      #��[ 5�      5�      5�      �� 5�      $ 
Horizontal=�6   $��0,0,0,5q�66   .+�5�      �� $��Select equipment:��6    ���#00FFFF$	��200��6N   $��Center=�6Z   �� 5�      -   5        $ comboEquipment��5�      $	��150��5�      #��[ 5�      5�      5�      �� 5�      $ 
Horizontal=�6   .+�5�      �� -   5         ��Name$ checkFilter��5�      $:��4Apply filtering (FinalState = G, Allowed AlarmClass)��5�      ���#00FFFF5�      5�      5�      &   System.Windows.Controls.DataGrid  5�      -	   5           Name$  dataGridLogs��5�      $��2��5�      !   AutoGenerateColumns$
! True��5�      ���#1A1A1A
�������"   AlternatingRowBackground" �#2A2A2A#   RowBackground# �#1A1A1A���#00FFFF$��1q�5�      $   HeadersVisibility$$ Column=�5�      %   GridLinesVisibility$	% All=�5�      &   ColumnHeaderStyle#& [ 5�      �� 5�      $��3��6   $ 
Horizontal=�6&   $��Center=�6?   $��0,10,0,0q�6\   .+�5�      �� -
   5        $
��Back��5�      $	��100��6(   #��[ 6J   5�      5�      5�      5�      