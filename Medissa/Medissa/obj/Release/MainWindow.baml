   M S B A M L   `   `   `  ���� f  bПредварительная запись, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:MedissaMedissa      Medissa.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     #lclr-namespace:Medissa       �Language$  ru-RU�-    �Name$ _this�� � Title$ Расписание��$��641.394��$��1083.364�� � 	Resources !   Medissa.RowToIndexConverter   RowToIndexConverter��%   &	          .��� .+��� -    ��Name$ 
TabControl��$��10,10,292,10q�.@��� $��Расписание��.��� .+�&   System.Windows.Controls.DataGrid  -     Name$ TimeTableDataGrid��   AutoGenerateColumns .     CanUserAddRows .     CanUserDeleteRows .  	   CanUserResizeColumns	 . 
   CanUserResizeRows
 .     CanUserReorderColumns .     CanUserSortColumns .     
IsReadOnly . ��Z�    RowStyle ��  �� 
TargetType)  #System.Windows.Controls.DataGridRow"   �� Triggers ��  �� Binding ��  �� 	Converter# [   �� Value true.a���  �� Property � �� �� Value$ 0,0,0,2q���  � ��
 �   ���  �� # [  	 false.a���  � ��$ 0,0,0,1q���  � �� ��  �� ElementName$ _this�� �� Path$ 
dotedBrush��� -    ��Name$ TurnsTab��$��
Смены��$
��Left=�.��� 
�������.+��� -    ��Name$ PlaceComboBox��$
��Left=�$��	10,10,0,0q�$	��Top=�$	��120���� -   $ DoctorsNameComboBox��$
��Left=�$��	10,43,0,0q�$	��Top=�$	��120��(  "System.Windows.Controls.DatePicker  -     Name$ 
DatePicker��$
��Left=�$��
146,13,0,0q�$	��Top=�$	��111���� -   $ TurnesСomboBox��$
��Left=�$��	10,87,0,0q�$	��Top=�$	��120���� -	    ��Name$ AddTurnButton��$��Добавить��$
��Left=�$��
10,206,0,0q�$	��Top=�$	��120���� -
    ��Name$ TurnCheckBox��$7��1Заполнить с выбранного дня��$
��Left=�$��
10,134,0,0q�$	��Top=��� -    ��Name$ 
StateLabel��$�� ��$
��Left=�$��
10,175,0,0q�$	��Top=�$	��133���� -   $ FiveDayCheckBox��$��Без суббот��$
��Left=�$��
10,160,0,0q�$	��Top=��� -   $ MainAdminTab��$��Пользователи��.��� 
������� � ColumnDefinitions �� $
��610* ��� $
��157* �.+��� -   $ button��$��Добавить��$��1��$��
13,31,13,0q�$	��Top=�$��30���� -   $ button1��$ ��Редактировать��$��1��$��
13,76,13,0q�$	��Top=�$��34���� -   $ button2��$��Удалить��$��1��$��13,124,13,0q�$	��Top=�$��35���� -    ��Name$ MembersListView��$	��468��$	��Top=����	#FFCFCFCF�� -   $ MainAdminDoctorTab��$��
Врачи��.��� 
������� �� $
��608* ��� $
��159* �.+��� -   $ AddDoctorButton��$��Добавить��$��1��$��
13,31,13,0q�$	��Top=�$��35���� -   $ DeleteDoctorButton��$��Удалить��$��1��$��13,139,13,0q�$	��Top=�$��35���� -   $ EditDoctorButton��$ ��Редактировать��$��1��$��
13,84,13,0q�$	��Top=�$��35���� -   $ DoctorsListView�����	#FFCFCFCF$��0,0,0,63q��� -   $ UpButton��$��
Вверх��$
��Left=�$��10,0,0,0q�$��72��$��42��$��Bottom=��� -   $ 
DownButton��$��Вниз��$
��Left=�$��	112,0,0,0q�$��72��$��42��$��Bottom=�&   System.Windows.Controls.Calendar  -   $��Calendar��$��0,10,9,0q�$	��Top=�$��Right=�$	��278���� -   $��WorkPlacesComboBox��$��
0,0,10,193q�$��Bottom=�$��Right=�$	��268��$��28��