   M S B A M L   `   `   `  ����f  bПредварительная запись, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:MedissaMedissa  5   	       Medissa.ShowingRecord   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Medissa     5   	     � Title$  Запись��5   	   $��472.826��6   $��293.983��6)    � 
ResizeMode$ NoResize=�69   .��5	      � .+�5
   
   �� -   5         ��Name$ DeleteButton��5
      $��Удалить��6'   $
��Left=�69   $��Center=�6T   $��
166,0,0,11q�6x   $	��110��6�   $��30��6�   $��Bottom=�6�   �� 5   
   -   5         ��Name$ AuthorLabel��5      $��Label��6%   $
��Left=�65   $��	86,10,0,0q�6Q   $	��Top=�6d   $	��190��6|   $��30��6�   �� 5   
   -   5        $ DoctorLabel��5      $��Label��6%   $
��Left=�65   $��	86,50,0,0q�6Q   $	��Top=�6d   $	��190��6|   $��30��6�   �� 5   
   -   5        $ WorkPlaceLabel��5      $��Label��6(   $
��Left=�68   $��
86,234,0,0q�6T   $	��Top=�6h   $	��190��6�   $��30��6�   �� 5   
   -   5        $ 	DateLabel��5      $��Label��6#   $
��Left=�63   $��
86,274,0,0q�6O   $	��Top=�6c   $	��190��6{   $��30��6�   �� 5   
   -   5        $ label��5      $��30��6   $��
Автор��6+   $
��Left=�6;   $��	10,10,0,0q�6V   $	��Top=�6i   $��62��6�   �� 5   
   -   5        $ 
label_Copy��5      $��Доктор��6$   $
��Left=�65   $��	10,50,0,0q�6P   $	��Top=�6c   $��62��6{   $��30��6�   �� 5   
   -   5        $ label_Copy1��5      $��Пациент��6%   $
��Left=�67   $��	10,90,0,0q�6R   $	��Top=�6e   $��62��6}   $��30��6�   �� 5   
   -	   5        $ label_Copy2��5      $��Процедура��6%   $
��Left=�69   $��
10,130,0,0q�6T   $	��Top=�6h   $��71��6�   $��30��6�   �� 5   
   -
   5        $ label_Copy3��5      $��
Место��6%   $
��Left=�65   $��
10,234,0,0q�6P   $	��Top=�6d   $��62��6|   $��30��6�   �� 5   
   -   5        $ label_Copy4��5      $��Дата��6%   $
��Left=�64   $��
10,274,0,0q�6O   $	��Top=�6c   $��62��6{   $��30��6�   �� 5   
   -   5        $ 
SaveButton��5      $��Сохранить��6%   $
��Left=�69   $��Center=�6T   $��	10,0,0,11q�6x   $	��110��6�   $��30��6�   $��Bottom=�6�   6�   �� 5   
   -   5        $ TimeFromLabel��5      $��Label��6'   $
��Left=�67   $��
86,314,0,0q�6S   $	��Top=�6g   $	��190��6   $��30��6�   �� 5   
   -   5        $ label_Copy5��5      $��С��6%   $
��Left=�61   $��
10,314,0,0q�6L   $	��Top=�6`   $��62��6x   $��30��6�   �� 5   
   -   5        $ label_Copy6��5      $
��До��6%   $
��Left=�62   $��
10,354,0,0q�6M   $	��Top=�6a   $��62��6y   $��30��6�   �� 5   
   -   5         ��Name$ PatientTextBox��5      $
��Left=�6*   $��30��6E   $��	86,90,0,0q�6Q    �� TextWrapping$
 Wrap=�6d   $�� ��6x   $	��Top=�6�   $	��190��6�   �� 5   
   -   5        $ ProcedureTextBox��5      $
��Left=�6,   $��30��6G   $��
86,130,0,0q�6S   $
 Wrap=�6g   $�� ��6{   $	��Top=�6�   $	��190��6�   �� 5   
   -   5         ��Name$ TimeComboBox��5      $
��Left=�6)   $��
86,354,0,0q�6D   $	��Top=�6X   $��84��6p   $��30��6{   �� 5   
   -   5        $ label_Copy7��5      $��Комментарий��6%   $
��Left=�6;   $��
10,191,0,0q�6V   $	��Top=�6j   $��87��6�   $��30��6�   �� 5   
   -   5        $ CabinetTextBox��5       �� AcceptsReturn . $
��Left=�6?   $��51��6Z   $��102,170,0,0q�6f   $
 Wrap=�6{   $�� ��6�   $	��Top=�6�   $	��174��6�   5      5      