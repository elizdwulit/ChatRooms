using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// MainWindow.xaml.cs
    /// <br/>
    /// Author: Elizabeth Dwulit
    /// <br/>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Channels for connecting the client and server
        ClientMethods peerListener = null;
        IServerMethods serverChannel;

        /// <summary>
        /// timer used for refreshing data on main window
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// The user corresponding to this client window
        /// </summary>
        User mainUser = null;

        // Port used for peer to peer communication
        int peerConnPort = -1;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DisconnectedImage.Visibility = Visibility.Visible;
            ConnectedImage.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Create timer to refresh user and topic data in the window
        /// </summary>
        /// <param name="state"></param>
        public void ClientTimer(object state)
        {
            try
            {
                refreshUsers();
                refreshTopics();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception refreshing client ui data " + ex.ToString());
            }
        }

        /// <summary>
        /// Event handler for window closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            if (serverChannel != null && mainUser != null)
            {
                try
                {
                    serverChannel.disconnect(mainUser.username);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception disconnecting: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Event handler for window closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (serverChannel != null && mainUser != null)
            {
                try
                {
                    serverChannel.disconnect(mainUser.username);
                } catch (Exception ex)
                {
                    Console.WriteLine("Exception disconnecting: " + ex.Message);
                }
                
            }
        }

        /// <summary>
        /// Alter UI after user is connected to the server
        /// </summary>
        /// <param name="endpoint"></param>
        private void ListenerCreated(string endpoint)
        {
            Dispatcher.Invoke(() => {
                DisconnectedImage.Visibility = Visibility.Collapsed;
                ConnectedImage.Visibility = Visibility.Visible;
                CreateNewTopicButton.IsEnabled = true;
                OpenMsgWindowButton.IsEnabled = true;
                OpenTopicMsgWindowButton.IsEnabled = true;
            });

        }

        /// <summary>
        /// Refresh the list of users in the window
        /// </summary>
        private void refreshUsers()
        {
            if (peerListener == null)
            {
                return;
            }

            List<string> allUsernames = peerListener.getAllUsernames();
            if (mainUser != null)
            {
                allUsernames.Remove(mainUser.username);
            }
            Dispatcher.Invoke(() =>
            {
                UserListBox.Items.Clear();
                foreach (string username in allUsernames)
                {
                    UserListBox.Items.Add(username);
                }
            });
        }

        /// <summary>
        /// Refresh the list of topics in the window
        /// </summary>
        private void refreshTopics()
        {
            if (peerListener == null)
            {
                return;
            }
            List<string> allTopicNames = peerListener.getAllTopicNames();
            Dispatcher.Invoke(() =>
            {
                TopicListBox.Items.Clear();
                foreach (string topicName in allTopicNames)
                {
                    TopicListBox.Items.Add(topicName);
                }

            });
        }

        /// <summary>
        /// Event handler for connect button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            string serverAddress = ServerAddressTextBox.Text;
            if (string.IsNullOrEmpty(serverAddress))
            {
                showMessagePopup("ERROR", "No server address provided.");
                return;
            }

            string username = UsernameTextBox.Text;
            if (string.IsNullOrEmpty(username))
            {
                showMessagePopup("ERROR", "Username not provided.");
                return;
            }

            int serverConnPort = -1;
            try
            {
                serverConnPort = Int32.Parse(UserPortTextBox.Text);
            } catch (Exception ex)
            {
                showMessagePopup("ERROR", "Invalid port for server communication provided. Please provide a different value.");
                return;
            }
            if (serverConnPort == -1)
            {
                showMessagePopup("ERROR", "Server connection port not specified. Cannot connect.");
                return;
            }

            try
            {
                peerConnPort = Int32.Parse(PeerPortTextBox.Text);
            }
            catch (Exception ex)
            {
                showMessagePopup("ERROR", "Invalid port for peer communication provided. Please provide a different value.");
                return;
            }
            if (peerConnPort == -1)
            {
                showMessagePopup("ERROR", "Peer connection port not specified. Cannot connect.");
                return;
            }

            EndpointAddress endpointAddress = new EndpointAddress(serverAddress);
            WSHttpBinding binding = new WSHttpBinding();
            ChannelFactory<IServerMethods> factory = new ChannelFactory<IServerMethods>(binding, endpointAddress);
            serverChannel = factory.CreateChannel();

            peerListener = new ClientMethods(serverConnPort);
            peerListener.startPeerListener(ListenerCreated);

            TimerCallback cb = new TimerCallback(ClientTimer);
            _timer = new Timer(cb, null, 0, 1000);
            
            serverChannel.connect(username, "127.0.0.1", serverConnPort, peerConnPort);
            Console.WriteLine("Connected to Server");

            mainUser = new User(username, "127.0.0.1", serverConnPort, peerConnPort);
        }

        /// <summary>
        /// Event handler for create new topic button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewTopicButton_Click(object sender, RoutedEventArgs e)
        {
            string topicName = CreateTopicTextBox.Text;
            serverChannel.createTopic(mainUser, topicName);
        }

        /// <summary>
        /// Event handler for open topic message window button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenTopicMsgWindowButton_Click(object sender, RoutedEventArgs e)
        {
            string serverAddress = ServerAddressTextBox.Text;
            string topicName = OpenTopicWindowTextBox.Text;
            if (string.IsNullOrEmpty(topicName))
            {
                showMessagePopup("No topic name provided", "No topic name provided, please specifiy a topic to connect to.");
                return; 
            }

            try
            {
                TopicWindow topicWindow = new TopicWindow(mainUser, serverAddress, topicName);
                topicWindow.Title = mainUser.username + " - " + topicName + " Messages";
                topicWindow.Show();
            } catch (Exception ex)
            {
                Console.WriteLine("Exception creating and showing topic window: " + ex.Message);
            }
            
        }

        /// <summary>
        /// Event handler for peer to peer message window open button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenMsgWindowButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string recepientUsername = SendMsgToUserTextBox.Text;
                if (string.IsNullOrEmpty(recepientUsername))
                {
                    showMessagePopup("User not specified", "No recepient username provided");
                    return;
                }

                User recepientUser = peerListener.getUser(recepientUsername);
                if (recepientUser == null)
                {
                    showMessagePopup("User not available", "The selected user is not available to chat.");
                    return;
                }
                MessageWindow messageWindow = new MessageWindow(mainUser, recepientUser);
                messageWindow.Title = mainUser.username + " - Messages to " + recepientUsername;
                messageWindow.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception creating and showing topic window: " + ex.Message);
            }
        }

        /// <summary>
        /// Opens a message popup box
        /// </summary>
        /// <param name="title">title of message box</param>
        /// <param name="message">message to show</param>
        private void showMessagePopup(string title, string message)
        {
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBox.Show(message, title, button, icon, MessageBoxResult.OK);
        }
    }
}
