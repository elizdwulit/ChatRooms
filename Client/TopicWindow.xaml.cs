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
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// TopicWindow.xaml.cs
    /// <br/>
    /// Author: Elizabeth Dwulit
    /// <br/>
    /// Interaction logic for TopicWindow.xaml
    /// </summary>
    public partial class TopicWindow : Window
    {
        // Channels for connecting the client and server
        IServerMethods serverChannel;
        ClientMethods peerListener = null;

        /// <summary>
        /// Thread that handles refresh of topic messages
        /// </summary>
        Thread topicMsgRefreshThread;

        /// <summary>
        /// User corresponding to current client
        /// </summary>
        User clientUser = null;

        /// <summary>
        /// Name of the topic
        /// </summary>
        string topicName = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">current client user</param>
        /// <param name="serverAddress">address of server</param>
        /// <param name="topicName">name of the client</param>
        public TopicWindow(User client, string serverAddress, string topicName)
        {
            InitializeComponent();

            clientUser = client;
            this.topicName = topicName;

            EndpointAddress endpointAddress = new EndpointAddress(serverAddress);
            WSHttpBinding binding = new WSHttpBinding();
            ChannelFactory<IServerMethods> factory = new ChannelFactory<IServerMethods>(binding, endpointAddress);
            serverChannel = factory.CreateChannel();
            serverChannel.joinTopic(clientUser, topicName);

            peerListener = new ClientMethods();
            createRefreshTopicMsgsThread();
            peerListener.startPeerListener(ListenerCreated);
            topicMsgRefreshThread.Start();
        }

        /// <summary>
        /// Listener Created
        /// </summary>
        /// <param name="endpoint"></param>
        private void ListenerCreated(string endpoint)
        {
        }

        /// <summary>
        /// Event handler for window closed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            serverChannel.leaveTopic(clientUser, topicName);
            topicMsgRefreshThread.Abort();
        }

        /// <summary>
        /// Event handler for window closing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            serverChannel.leaveTopic(clientUser, topicName);
            topicMsgRefreshThread.Abort();
        }

        /// <summary>
        /// Create thread used for refreshing the topic messages
        /// </summary>
        private void createRefreshTopicMsgsThread()
        {
            topicMsgRefreshThread = new Thread(() =>
            {
                while (true)
                {
                    if (peerListener == null)
                    {
                        continue;
                    }
                    List<TopicMessage> topicMsgs = peerListener.getTopicMessages(topicName).ToList();
                    try
                    {
                        Dispatcher.Invoke(() =>
                        {
                            foreach (TopicMessage msg in topicMsgs)
                            {
                                TopicMessagesTextBox.AppendText(msg.sourceUser + ": " + msg.message);
                                TopicMessagesTextBox.AppendText(Environment.NewLine);
                            }
                        });
                    } catch (Exception ex)
                    {
                        Console.WriteLine("Exception refreshing topic messages: " + ex.Message);
                    }
                    
                }
            });
            topicMsgRefreshThread.IsBackground = true;
        }

        /// <summary>
        /// Event handler for send message button click. Sends a message to the topic.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageInputBox.Text;
            TopicMessage topicMessage = new TopicMessage(clientUser.username, topicName, message);
            serverChannel.sendMessageToTopic(topicMessage.sourceUser, topicName, message);
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
