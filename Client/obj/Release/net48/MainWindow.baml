   M S B A M L   `   `   `  ���� A  =Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ClientClient      Client.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     &localclr-namespace:Client     -     � Title$  Messaging Service��$	��450��$	��800��.��� $��	0,0,0,-29q� � ColumnDefinitions �� $	��13* ��� $
��787* �.+��� -    ��Name$ ServerAddressTextBox��$
��Left=�$��
123,56,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��452��$��21��$��1��$*��$http://localhost:8080/IServerMethods���� -    ��Name$ ServerAddressLabel��$��Enter Server Address:��$
��Left=�$��1,53,0,0q�$	��Top=�$	��212��$��1��$��26���� -    ��Name$ ConnectButton��$��Connect to Server��$
��Left=�$��
593,56,0,0q�$	��Top=�$	��141��$��1��$��20���� -    ��Name$ ConnectedImage��$
��Left=�$��29��$��
630,13,0,0q�$	��Top=�$	��100��$��/connected.jpg��$��1���� -   $ DisconnectedImage��$
��Left=�$��27��$��
625,14,0,0q�$	��Top=�$	��109��$��/disconnected.jpg��$��1���� -   $ 
UsersLabel��$��Online Users��$
��Left=�$��	8,154,0,0q�$	��Top=�$	��266��$��1��$��26���� -    ��Name$ TopicListBox��$��400,180,104,0q�$	��228��$	��Top=�$��1������ *	UrlsFound+ �� Mode$ OneWay=��� -	   $ TopicsLabel��$��Topics��$
��Left=�$��400,154,0,0q�$	��Top=�$	��175��$��1��$��26���� -
   $ CreateNewTopicButton��$��Create New Topic��$
��Left=�$��465,152,0,0q�$	��Top=�$	��118��$��21��$��1��${�False���� -   $ UsernameTextBox��$��1��$
��Left=�$��
101,25,0,0q�$
 Wrap=�$	��Top=�$	��271��$��19���� -   $ UsernameLabel��$��1��$��Enter Username��$
��Left=�$��1,22,0,0q�$	��Top=�$	��104��$��26���� -   $ UserPortLabel��$��1��$��Enter User Port:��$
��Left=�$��
389,18,0,0q�$	��Top=�$	��106���� -   $ UserPortTextBox��$��1��$
��Left=�$��
477,22,0,0q�$
 Wrap=�$
��4040��$	��Top=�$	��130��$��22���� -   $ SendMsgToUserTextBox��$��1��$
��Left=�$��
11,126,0,0q�$
 Wrap=�$	��Top=�$	��132��$��16��$��Enter Username���� -   $ OpenMsgWindowButton��$��1��$��Open Message Window��$
��Left=�$��151,120,0,0q�$	��Top=�$	��142��${�False���� -   $ OpenTopicWindowTextBox��$��1��$
��Left=�$��400,116,0,0q�$
 Wrap=�$	��Top=�$	��142��$��16��$��Enter Topic Name���� -   $ OpenTopicMsgWindowButton��$��1��$��Open Topic Window��$
��Left=�$��557,110,0,0q�$	��Top=�$	��149��${�False���� -   $ CreateTopicTextBox��$��1��$
��Left=�$��593,155,0,0q�$
 Wrap=�$��Enter Topic Name��$	��Top=�$	��163��$��18���� -   $ UserListBox��$��1��$��10,180,476,0q�$	��228��$	��Top=��� -   $ PeerPortLabel��$+��%Enter Port for Direct User Messaging:��$
��Left=�$��1,86,0,0q�$	��Top=�$��1���� -   $ PeerPortTextBox��$��1��$
��Left=�$��
206,90,0,0q�$
 Wrap=�$	��Top=�$	��120��$
��6060��