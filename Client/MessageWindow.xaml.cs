using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
    /// MessageWindow.xaml.cs
    /// <br/>
    /// Author: Elizabeth Dwulit
    /// <br/>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        /// <summary>
        /// The current client user
        /// </summary>
        User clientUser = null;

        /// <summary>
        /// The recepient user of the messages
        /// </summary>
        User recepientUser = null;

        /// <summary>
        /// Thread used to listen for peer messages from the recepient user
        /// </summary>
        Thread listenThread = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientUser"></param>
        /// <param name="recepientUser"></param>
        public MessageWindow(User clientUser, User recepientUser)
        {
            InitializeComponent();
            
            this.clientUser = clientUser;
            this.recepientUser = recepientUser;

            listenThread = new Thread(() =>
            {
                listenForPeerMessages();
            });
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        /// <summary>
        /// Event handler for window closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            // do nothing
        }

        /// <summary>
        /// Event handler for window closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // do nothing
        }

        /// <summary>
        /// Listen for peer messages from the recepient user
        /// </summary>
        private void listenForPeerMessages()
        {
            try
            {
                UdpClient udpServer = new UdpClient(clientUser.peerConnPort);
                IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
                while (true)
                {
                    byte[] receivedDataByteArr = udpServer.Receive(ref remoteEndpoint);
                    if (receivedDataByteArr != null && receivedDataByteArr.Length > 0)
                    {
                        string receivedDataStr = Encoding.ASCII.GetString(receivedDataByteArr, 0, receivedDataByteArr.Length);
                        Dispatcher.Invoke(() => {
                            MessagesTextBox.AppendText(recepientUser.username + ": " + receivedDataStr);
                            MessagesTextBox.AppendText(Environment.NewLine);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                //showMessagePopup("ERROR", "Failed to receive messages from " + recepientUser.username);
                Console.WriteLine("Exception receiving peer messages from " + recepientUser.username + " to " + clientUser.username + ": " + ex.ToString());
            }
        }

        /// <summary>
        /// Send a direct peer message to the recepient
        /// </summary>
        /// <param name="message">message to send</param>
        private void sendPeerMessage(string message)
        {
            try
            {
                UdpClient recepientClient = new UdpClient();
                IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Parse(recepientUser.ip), recepientUser.peerConnPort); // endpoint where server is listening
                recepientClient.Connect(remoteEndpoint);

                byte[] messageAsByteArr = Encoding.ASCII.GetBytes(message);
                recepientClient.Send(messageAsByteArr, messageAsByteArr.Length);
                MessagesTextBox.AppendText("You: " + message);
                MessagesTextBox.AppendText(Environment.NewLine);
            } catch (Exception ex)
            {
                //showMessagePopup("ERROR", "Failed to send message.");
                Console.WriteLine("Failed to send peer message to " + recepientUser.username + " to " + clientUser.username + ": " + ex.Message);
            }
            
        }

        /// <summary>
        /// Event handler for send button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageInputBox.Text;
            sendPeerMessage(message);
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
