   M S B A M L   `   `   `  ����A  =PT8WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:PT8WPFPT8WPF  5   	       PT8WPF.CreateElementForm   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:PT8WPF     5   	     � Title$  CreateElementForm��5   	   $	��200��6#   $	��400��60   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��Element Name:��6   �� 5   
   -   5         ��Name$ nameTextBox��5      $��1��6   6   	� 5   
   -   5         	�Name$ fileRadioButton��5   #   $��2��6   $
��File��6<    	� 	GroupName$ ElementType��6K   	� 5   
   -   5        $ directoryRadioButton��5   #   $��3��6   $��	Directory��6A   $ ElementType��6U    d� 	IsChecked$
 TrueY�6m   �� 5   
   -   5         ��Name$ readOnlyCheckBox��5       $��4��6   $��	Read Only��6:   �� 5   
   -   5        $ archiveCheckBox��5       $��5��6   $��Archive��69   �� 5   
   -   5        $ hiddenCheckBox��5       $��6��6   $��Hidden��68   �� 5   
   -   5        $ systemCheckBox��5       $��7��6   $��System��68   �� 5   
   $��8��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   .+�5      �� -   5        $��Ok��5      6"   �� 5      -	   5        $��Cancel��5      $��10,0,0,0q�6A   5       5!      5"      