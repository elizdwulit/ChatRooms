   M S B A M L   `   `   `  ����A  =Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ClientClient  5   	       Client.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Client     5   	   -   5          � Title$  Messaging Service��5   	   $	��450��6#   $	��800��60   .��5	      � $��	0,0,0,-29q�6    � ColumnDefinitions 5
   
   �� 5      $	��13* �6   �� 5      $
��787* �6   .+�5   
   �� -   5         ��Name$ ServerAddressTextBox��5      $
��Left=�60   $��
123,56,0,0q�6K    �� TextWrapping$
 Wrap=�6_   $	��Top=�6s   $	��452��6�   $��21��6�   $��1��6�   $*��$http://localhost:8080/IServerMethods��6�   �� 5   
   -   5         ��Name$ ServerAddressLabel��5      $��Enter Server Address:��6,   $
��Left=�6L   $��1,53,0,0q�6g   $	��Top=�6y   $	��212��6�   $��1��6�   $��26��6�   �� 5   
   -   5         ��Name$ ConnectButton��5      $��Connect to Server��6(   $
��Left=�6D   $��
593,56,0,0q�6_   $	��Top=�6s   $	��141��6�   $��1��6�   $��20��6�   �� 5   
   -   5         ��Name$ ConnectedImage��5      $
��Left=�6(   $��29��6C   $��
630,13,0,0q�6O   $	��Top=�6c   $	��100��6{   $��/connected.jpg��6�   $��1��6�   �� 5   
   -   5        $ DisconnectedImage��5      $
��Left=�6+   $��27��6F   $��
625,14,0,0q�6R   $	��Top=�6f   $	��109��6~   $��/disconnected.jpg��6�   $��1��6�   �� 5   
   -   5        $ 
UsersLabel��5      $��Online Users��6$   $
��Left=�6;   $��	8,154,0,0q�6V   $	��Top=�6i   $	��266��6�   $��1��6�   $��26��6�   �� 5   
   -   5         ��Name$ TopicListBox��5   A   $��400,180,104,0q�6W   $	��228��6n   $	��Top=�6{   $��1��6�   ��6   �� *	UrlsFound+ �� Mode$ OneWay=�6�   �� 5   
   -	   5        $ TopicsLabel��5      $��Topics��6%   $
��Left=�66   $��400,154,0,0q�6Q   $	��Top=�6f   $	��175��6~   $��1��6�   $��26��6�   �� 5   
   -
   5        $ CreateNewTopicButton��5      $��Create New Topic��6/   $
��Left=�6J   $��465,152,0,0q�6e   $	��Top=�6z   $	��118��6�   $��21��6�   $��1��6�   ${�False��6�   �� 5   
   -   5        $ UsernameTextBox��5      $��1��6+   $
��Left=�6;   $��
101,25,0,0q�6V   $
 Wrap=�6j   $	��Top=�6~   $	��271��6�   $��19��6�   �� 5   
   -   5        $ UsernameLabel��5      $��1��6'   $��Enter Username��67   $
��Left=�6P   $��1,22,0,0q�6k   $	��Top=�6}   $	��104��6�   $��26��6�   �� 5   
   -   5        $ UserPortLabel��5      $��1��6'   $��Enter User Port:��67   $
��Left=�6R   $��
389,18,0,0q�6m   $	��Top=�6�   $	��106��6�   �� 5   
   -   5        $ UserPortTextBox��5      $��1��6+   $
��Left=�6;   $��
477,22,0,0q�6V   $
 Wrap=�6j   $
��4040��6~   $	��Top=�6�   $	��130��6�   $��22��6�   �� 5   
   -   5        $ SendMsgToUserTextBox��5      $��1��60   $
��Left=�6@   $��
11,126,0,0q�6[   $
 Wrap=�6o   $	��Top=�6�   $	��132��6�   $��16��6�   $��Enter Username��6�   �� 5   
   -   5        $ OpenMsgWindowButton��5      $��1��6.   $��Open Message Window��6>   $
��Left=�6\   $��151,120,0,0q�6w   $	��Top=�6�   $	��142��6�   ${�False��6�   6�   �� 5   
   -   5        $ OpenTopicWindowTextBox��5      $��1��62   $
��Left=�6B   $��400,116,0,0q�6]   $
 Wrap=�6r   $	��Top=�6�   $	��142��6�   $��16��6�   $��Enter Topic Name��6�   �� 5   
   -   5        $ OpenTopicMsgWindowButton��5      $��1��63   $��Open Topic Window��6C   $
��Left=�6_   $��557,110,0,0q�6z   $	��Top=�6�   $	��149��6�   ${�False��6�   6�   �� 5   
   -   5        $ CreateTopicTextBox��5      $��1��6.   $
��Left=�6>   $��593,155,0,0q�6Y   $
 Wrap=�6n   $��Enter Topic Name��6�   $	��Top=�6�   $	��163��6�   $��18��6�   �� 5    
   -   5        $ UserListBox��5       $��1��6'   $��10,180,476,0q�67   $	��228��6M   $	��Top=�6Z   �� 5!   
   -   5        $ PeerPortLabel��5!      $+��%Enter Port for Direct User Messaging:��6'   $
��Left=�6W   $��1,86,0,0q�6r   $	��Top=�6�   $��1��6�   �� 5"   
   -   5        $ PeerPortTextBox��5"      $��1��6+   $
��Left=�6;   $��
206,90,0,0q�6V   $
 Wrap=�6j   $	��Top=�6~   $	��120��6�   $
��6060��6�   5$      5%      