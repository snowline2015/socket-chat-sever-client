   M S B A M L   `   `   `  ���� B  >ChatGUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:ChatGUIChatGUI      ChatGUI.LoginWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:ChatGUI       � Title$  LoginWindow�� � WindowStartupLocation$ CenterScreen=� � AllowsTransparency .  � WindowStyle$
 None=� � SizeToContent$ WidthAndHeight=�$��562.5��$	��668����Z� ��Z�  � 	Resources    TextBox.Static.Border �� Color  TextBox.MouseOver.Border  TextBox.Focus.Border  UsrName_Login �� 
TargetType �� Property	 �� Value-  'System.Windows.Input.KeyboardNavigation
 �� 
TargetType ��Name J� SnapsToDevicePixels �� CornerRadius ��Name �� Triggers P� Property P� Value �� 
TargetName �� Triggers ]� 
Conditions �� Property �� Value  
Pass_Login  MyNextButton  MyButton  ButtonControlTemplate1'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   Transitions%  System.Windows.VisualTransition   GeneratedDuration	   To   States   System.Windows.VisualState � ColumnDefinitions � RowDefinitions �� ImageSource j� Stretch  DatePickerTextBoxStyle1:  4System.Windows.Controls.Primitives.DatePickerTextBox 	 Key    
Storyboard! g� Duration	" W� To# �� TargetProperty$ �� 
TargetName% J� Opacity& �� Padding' �Name( ��Name) J� IsHitTestVisible 
 BirthdayTemplate(  "System.Windows.Controls.DatePicker* �� EndPoint+ �� 
StartPoint, � Color- � Offset	. �� To/ g� 	BeginTime0 �� KeyTime1 �� Value2 O� KeyTime3 O� Value4 J� RenderTransformOrigin5 �� Stretch6 ��Name7 � UseLayoutRounding8  Name9 �� SelectionBrush: �� SelectionTextBrush; �� RadiusY< �� RadiusX= �Name> � AllowsTransparency? � 	StaysOpen@ � PlacementTargetA �� ElementNameB �� ValueC �� BindingD �� SourceE �� RelativeSource��%�"  &	        &	      &	 *     &	 ?     7   7  7  &	 �    7   7  7  &	 �    &	 �    &	 �    &	 �
    &	
     �� $ 	#FFABAdB3���� $ 	#FF7EB4EA���� $ 	#FF569DE5���� " ��.���  � ��#	 � ����  � ��8	   ��  � ��$	 Username����  � ��#	 � ����  � ��$	 1q���  �@ TabNavigation$
	 None=���  � ��$
	 Left=���  �@�FocusVisualStyle	 Z� ��  �@J�	AllowDrop	 . ��  �@��PanningMode$	 VerticalFirst=���  �@��IsFlicksEnabled	 .  ��  � ��	 �� "
 ��.R��� $ border��#��z��#��z��#��z�� . $ 5��._��� $ PART_ContentHost��|�.  $��Hidden=�$��Hidden=� P�  � {� .  . ��� $ border�� �@J�Opacity$
	 0.56Q�P�  �@J�IsMouseOver . . ��� $ border��8	   � ��P�  �@J�IsKeyboardFocused . . ��� $ border��8	   � �� ]�  �� + �@��#IsInactiveSelectionHighlightEnabled . ��  �@��IsSelectionActive .  ./���  �@��SelectionBrush#	 � /��� " G�.���  �@G�PasswordChar$		 ●����  � ��#	 � ����  � ��8	   ��  � ��#	 � ����  � ��$	 1q���  �@ TabNavigation$
	 None=���  � ��$
	 Left=���  �@�FocusVisualStyle	 Z� ��  �@J�	AllowDrop	 . ��  �@��PanningMode$	 VerticalFirst=���  �@��IsFlicksEnabled	 .  ��  � ��	 �� "
 G�.R��� $ border��#��z��#��z��#��z�� . $ 5��._��� $ PART_ContentHost��|�.  $��Hidden=�$��Hidden=���Z�  P�  � {� .  . ��� $ border�� �@J�Opacity$
	 0.56Q�P�  �@J�IsMouseOver . . ��� $ border��8	   � ��P�  �@J�IsKeyboardFocused . . ��� $ border��8	   � �� ]�  �� + �@G�#IsInactiveSelectionHighlightEnabled . ��  �@G�IsSelectionActive .  ./���  �@G�SelectionBrush#	 � /��� " ��.���  �@�OverridesDefaultStyle	 . ��  �@�Cursor$	 Arrow����  � ��	 �� "
 ��.R��� $ border��$	��1.5q�#��z��$ 5����Z�  P�  �@J�IsMouseOver . . ���  �@J�Opacity$	 .6Q��� " ��.���  �@�OverridesDefaultStyle	 . ��  �@�Cursor$	 Arrow����  � ��	 �� "
 ��.R��� $ border��$��0q�
�������#��z�� P�  �@J�IsMouseOver . . ���  �@J�Opacity$	 .6Q��� "
 ��.R��    CommonStates��   $ 0D�  $ 0:0:0.1D�$ 	MouseOver��  $ 0:0:0.1D�$ Pressed��.   
Normal��  	MouseOver��  Pressed��  Disabled��.+�� ���	#11FFFFFF$��LeftToRight=�$��Center=�$��18��$��0q�$��Center=�$��19�� �� $	��20* ��� $	��20* ��� $	��20* ��� $	��20* � �� $	��23* ��� $	��19* ��� $	��19* ��� $	��19* �.+��� $��4��$
��Left=�$��18��$��4��$��0��$	��Top=�$��19������ $ Assets/calendar-icon.png��$ UniformToFill=��� "  .���  � ��#	 � ����  � ��#	 � ����  �@��PanningMode$	 VerticalFirst=���  �@��IsFlicksEnabled	 .  ��  � ��	 �� "
  .R��  ���� WatermarkBrush	 $ 	#FFAAAAAA��   CommonStates��   $ 0D�  $ 0:0:0.1D�$ 	MouseOver��.   
Normal��  	MouseOver��  WatermarkStates��   $ 0D�.   Unwatermarked��  Watermarked��.  �� .��W� $! 0D�$" 0X�$# Opacity�$$ ContentElement��W� $! 0D�$" 1X�$# Opacity�$$ PART_Watermark��  FocusStates��   $ 0D�.   	Unfocused��  Focused��.  �� .��W� $! 0D�$" 1X�$# Opacity�$$ FocusVisual��.+��� $ Border��#��z��#��z��#��z��$ 1��$% 1Q�#& z��._�� $' WatermarkContent��#��z��#��z��.+��� $ ContentElement��$��1q���Z� �� $ watermark_decorator��$��1q���Z� ._��� $( PART_Watermark��|�.  ) .  $% 0Q�$��2q�$��Birthday���� $ PART_ContentHost��#��z��$��0q�#��z���� $ FocusVisual�����	#FF45D6FA$ 1��) .  $% 0Q��� "  .���  � ��	 �	#FF333333��  �@ IsTodayHighlighted	 . ��  �@ SelectedDateFormat$	 Short=���  � ��
	 ���� ��  � ��$	 2q���  � ��	 �� $
* .5,0&�$
+ .5,1&�.��� $, 	#FFA3AEB9��$- 0Q�� $, 	#FF8399A9��$- 0.375Q�� $, 	#FF718597��$- 0.375Q�� $, 	#FF617584��$- 1Q���  � ��$	 1q���  � ��$	 Stretch=���  � ��	 �� "
  .R��� #��z��#��z��#��z��#& z��   CommonStates��.   
Normal��  Disabled��.  �� .��W� $! 0D�$" 1X�$# Opacity�$$ PART_DisabledVisual��._�� $' 	PART_Root��#��z��#��z�� ���� DisabledBrush	 $ 	#A5FFFFFF���� DropDownButtonTemplate	 "
 ��.R��    CommonStates��   $ 0D�  $ 0:0:0.1D�$ 	MouseOver��  $ 0:0:0.1D�$ Pressed��.   
Normal��  	MouseOver��.  �� .���� $! 0D�$. 	#FF448DCAX�$1# +(Border.Background).(SolidColorBrush.Color)�$$ 
Background���� $/ 0X�$! 00:00:00.001D�$O# I(Border.Background).(GradientBrush.GradientStops)[3].(GradientStop.Color)�$$ BackgroundGradient��.g��� $0 0��$1 	#7FFFFFFF���� $/ 0X�$! 00:00:00.001D�$O# I(Border.Background).(GradientBrush.GradientStops)[2].(GradientStop.Color)�$$ BackgroundGradient��.g��� $0 0��$1 	#CCFFFFFF���� $/ 0X�$! 00:00:00.001D�$O# I(Border.Background).(GradientBrush.GradientStops)[1].(GradientStop.Color)�$$ BackgroundGradient��.g��� $0 0��$1 	#F2FFFFFF��  Pressed��.  �� .���� $/ 0X�$! 00:00:00.001D�$1# +(Border.Background).(SolidColorBrush.Color)�$$ 
Background��.g��� $0 0��$1 	#FF448DCA��U� $/ 0X�$! 00:00:00.001D�$# (UIElement.Opacity)�$$ 	Highlight��.\��� $2 0��$3 1Q��� $/ 0X�$! 00:00:00.001D�$O# I(Border.Background).(GradientBrush.GradientStops)[1].(GradientStop.Color)�$$ BackgroundGradient��.g��� $0 0��$1 	#EAFFFFFF���� $/ 0X�$! 00:00:00.001D�$O# I(Border.Background).(GradientBrush.GradientStops)[2].(GradientStop.Color)�$$ BackgroundGradient��.g��� $0 0��$1 	#C6FFFFFF���� $/ 0X�$! 00:00:00.001D�$O# I(Border.Background).(GradientBrush.GradientStops)[3].(GradientStop.Color)�$$ BackgroundGradient��.g��� $0 0��$1 	#6BFFFFFF���� $/ 0X�$! 00:00:00.001D�$O# I(Border.Background).(GradientBrush.GradientStops)[0].(GradientStop.Color)�$$ BackgroundGradient��.g��� $0 0��$1 	#F4FFFFFF��  Disabled��.+�� ���	#11FFFFFF$��LeftToRight=�$��Center=�$��18��$��0q�$��Center=�$��19�� �� $	��20* ��� $	��20* ��� $	��20* ��� $	��20* � �� $	��23* ��� $	��19* ��� $	��19* ��� $	��19* �.+��� $ 	Highlight�����	#FF45D6FA$��1q�$��4��$ 0,0,1,1��$��-1q�$% 0Q�$��0��$��4���� $ 
Background�����	#FFFFFFFF$��1q����	#FF1F3B53$��4��$ .5��$��0,-1,0,0q�$% 1Q�$��1��$��3���� $ BackgroundGradient�����	#BF000000$��1q�$��4��$ .5��$��0,-1,0,0q�$% 1Q�$��1��$��3������ $
* .7,1&�$
+ .7,0&�.��� $, 	#FFFFFFFF��$- 0Q�� $, 	#F9FFFFFF��$- 0.375Q�� $, 	#E5FFFFFF��$- 0.625Q�� $, 	#C6FFFFFF��$- 1Q��� $��4��$��1��$��1������ $* 0.3,-1.1&�$+ 0.46,1.6&�.��� $, 	#FF4084BD��� $, 	#FFAFCFEA��$- 1Q����� $* 0.48,-1&�$+ 	0.48,1.25&�.��� $, 	#FF494949��� $, 	#FF9F9F9F��$- 1Q�F� $��4��$��0������p�[� 4ZX�k� @Q�F� 4ZX�k� @џF� {2� �u� {2� �u�  )� �*� &"@pg� �*� &"@� � ��*�"@��� yh���#@h/��@�#@,� j����"@Sn#� l�H@�/"@��� ����k�!@�[� 4ZX�k� @p�� 4ZX�k� @Q�L4ZX�k� @Q�L�:��"@QH � �:��"@�H � ��� �U+� L�� ����E0@�=� ��� �=� ��	�=� �0ݒ� 5N=� ����� �� �b� �� ��� Ӧ� ��� �u"9��Up0@�� "9��Up0@S�n� "9��Up0@�$� Ӹ� ��� H~� �� {�� �\�� �� ��� [�� �� [�� �y�[�� ���I� ��� Ӭ�J"� sc,� ~T�~O0@c,� Ӷ�� c,� @�� �P� � � /�� ��� �� 	���	#FF2F2F2F$��Center=�$��4,3,4,3q�$��1��$��3��$4 0.5,0.5&�$
5 Fill=�$��Center=�A� $��4�����	#FFFFFFFF$��Center=�$��3��$��0��$��Center=�$��3���� $ DisabledVisual�����	#B2FFFFFF$��1q�$��4��$ 	0,0,.5,.5��$% 0Q�$��0��$��4�� �� $��* ��� $
��Auto �.+��� $6 PART_Button��$��1��#��z��|�.  $
��Left=�$��3,0,3,0q�$��0��#���  $	��Top=�$��20��7 .  $��1q�$��25��  $8 PART_TextBox��$��0��#|�z|�$��Stretch=�$��0��$��Stretch=�#���  ��Z� 9 Z� : Z� � $' PART_DisabledVisual��$��2��$��0��) .  $% 0Q�$��0��$��0,0,3,0q� �� �� $
��Auto �$��25.501��.+��� $��0�����	#A5FFFFFF$; 1��$��0��$< 1���� $��1�����	#A5FFFFFF$
��Auto��$��3,3.505,3,3.506q�$; 1��$��0��$< 1��$
��Auto��� $= 
PART_Popup��> . $��Bottom=�? .  @ �� $A PART_TextBox�� �� $B false��C �� #D Zr�.a��� $ PART_TextBox��	 �� *
Foreground+E �� *TemplatedParent+ � ��.���� -   F ��Name$F Login_Panel��$��0q�$��561.6��$
��Left=�$	��667��$	��Top=����� $ Assets/background4.png��.+�� -   $' 	LoginGrid��$��667.6��$y�Visible=�.+��� -   G ��Name$G txt_Welcome��$
��Left=�$
��40.8��$��56.4,95.6,0,0q�$
��Wrap=�$��Welcome To Chat App��$	��Top=�$��453.6��$��26(�H �� TextAlignment$H Center=�
���   ��� -   I ��Name$I Username��$
��Left=�$��43��$��56.4,252.501,0,0q�J �� TextWrapping$
J Wrap=�$	��Top=�$��453.6�����	#FFBCD2ED$	��1.5q�9 �	#FF3DA0E5$��Center=�$��18(�$�� ��#���  $��5,0,0,0q�$y�	Collapsed=���Z� : Z� G� -   K G�Name$K Password��$
��Left=�$��43��$��56.4,357.225,0,0q�$	��Top=�$��453.6��L G� SelectionBrushL �	#FF3DA0E5���	#FFBCD2ED$��Center=�$��18(�$	��1.5q�M G� Password$M  ��#���  $��5,0,0,0q�$y�	Collapsed=�N G� SelectionTextBrushN Z� ��Z� �� -   $6 LoginButton��O �� 	IsDefaultO . $
��Left=�$
��38.4��$��194.4,458.7,0,0q�$	��Top=�$��177.6��$��18(�#��[ $y�	Collapsed=����� $ Assets/next2.png��$ Uniform=��� -   $6 
ExitButton��#��[ $
��Left=�$��30��$��507.8,10,0,0q�$	��Top=�$��30��$
��Bold"�$��16(����	#FF121111��Z� ���� $ Assets/close2.png��$ Uniform=��� -   $G 
txt_SignIn��$
��Left=�$
��40.8��$��56.4,136.4,0,0q�$
��Wrap=�$��Sign In��$	��Top=�$��453.6��$��26(�$H Center=�$
��Bold"�
���   ��� -	   $	6 reg��$��
SignUp Now��$
��Left=�$
��28.8��$��286.8,634.8,0,0q�$	��Top=�$��115.2��$
��Bold"�$��16(����	#FFDA2121$y�	Collapsed=���Z� ��Z� �� -
   P ��Name$P 
some_label��$��Haven't got account?��$
��Left=�$
��28.8��$��140,634.8,0,0q�$	��Top=�$��141.8��$��Right=�$��Center=�$��14(�
���   �$y�	Collapsed=���Z� �� -   $G warning��$
��Left=�$��40��$��
56,507,0,0q�$
��Wrap=�$	��Top=�$	��454�����	#FFF00F0F$��15(�$��SemiBold"�$H Center=��� -   $I ServerIP��$
��Left=�$��43��$��56.4,317.501,0,0q�$
J Wrap=�$	��Top=�$��453.6�����	#FFBCD2ED$	��1.5q�9 �	#FF3DA0E5$��Center=�$��18(�$�� ��#���  $��5,0,0,0q���Z� : Z� �� -   $6 ConnectServer��O . $
��Left=�$
��38.4��$��194.4,403.7,0,0q�$	��Top=�$��177.6��$��18(�#��[ ���� $ Assets/next2.png��$ Uniform=��� -   $P label_username��$��	Username:��$
��Left=�$
��28.8��$��56.4,218.701,0,0q�$	��Top=�$
��87.8��$��Center=�$��15(�
���   �$��SemiBold"�$y�	Collapsed=���Z� �� -   $P 
label_pass��$��	Password:��$
��Left=�$
��28.8��$��56.4,323.425,0,0q�$	��Top=�$
��87.8��$��Center=�$��15(�
���   �$��SemiBold"�$y�	Collapsed=���Z� �� -   $P label_ip��$��Server IP Address:��$
��Left=�$
��28.8��$��56.4,283.701,0,0q�$	��Top=�$��147.8��$��Center=�$��15(�
���   �$��SemiBold"���Z� � -   $' RegisterGrid��$��667.9��$y�	Collapsed=�.+��� -   $
I Name��$
��Left=�$
��44.4��$��65.6,119.5,0,0q�$
J Wrap=�$
��Name��$	��Top=�$��416.4��$��16(�$��0q�$��Center=����	#FFBCD2ED#���  ��Z� 9 Z� �� -   $I UsrName_reg��$
��Left=�$
��44.4��$��65.6,215.3,0,0q�$
J Wrap=�$��Username��$	��Top=�$��416.4��$��Center=�$��16(����	#FFBCD2ED#���  ��Z� 9 Z� �� -   $I Pass_reg��$
��Left=�$
��44.4��$��65.6,312.1,0,0q�$
J Wrap=�$��Password��$	��Top=�$��416.4��$��Center=�$��16(����	#FFBCD2ED#���  ��Z� 9 Z� : Z� �� -   $I Mail_reg��$
��Left=�$
��44.4��$��65.6,406.1,0,0q�$
J Wrap=�$��E-mail��$	��Top=�$��416.4�����	#FFBCD2ED$��16(�$��Center=�#���  ��Z� 9 Z� : Z�   -   Q  Name$Q Birthday��$
��Left=�$��31.011��$��165,499.334,0,0q�$	��Top=�$��299.501��$4 0.496,0.538&�R   SelectedDateFormat$R Short=�$��16(�#��� 
 ��Z� ��Z� �� -   $6 ExitButton_regis��#��[ $
��Left=�$��30��$��507.8,10,0,0q�$	��Top=�$��30��$
��Bold"�$��16(����	#FF121111��Z� ���� $ Assets/close2.png��$ Uniform=��� -   $6 FinishRegister��$��Finish��$
��Left=�$
��32.4��$��279.2,584.666,0,0q�$	��Top=�$��225.8��$
��Bold"�$��18(����	#FF1624C0��Z� ��Z� �� -   S ��Name$S birthday_regt��$
��Left=�$��27.602��$��165,499.334,0,0q�
���   �$	��Top=�$��299.501��$; 4.601��$< 4.601��F� �����wU8����wU@��	���	#FFF4F4F5$
��Left=�$��18��$��428.833,504.334,0,0q�$
5 Fill=�
���   �$	��Top=�$��1��$��2���� $
��Left=�$��27.602��$��65.6,499.334,0,0q�$
��Wrap=�$��Birthday��$	��Top=�$
��94.4��$��16(�7 .  T �� Padding$T 2,4,0,0q�
���   ��� -   $6 	CancelReg��$��Cancel��$
��Left=�$
��32.4��$��53.4,584.666,0,0q�$	��Top=�$��225.8��$
��Bold"�$��18(����	#FF1624C0��Z� ��Z� 