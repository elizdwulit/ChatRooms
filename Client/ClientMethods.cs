using Server;
using ConcurrentTools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// ClientMethods.cs 
    /// <br/>
    /// Author: Elizabeth Dwulit
    /// <br/>
    /// Class used to provide methods to the client that receives data from the server
    /// </summary>
    public class ClientMethods : IClientMethods
    {
        // variables used to interact with the server
        ServiceHost service = null;
        public delegate void ListenerCreated(string localEndPoint);
        public event ListenerCreated OnListenerCreated;

        // data structures used to hold data sent from the server to the client
        private static HashSet<string> receivedTopicNamesSet = new HashSet<string>();
        private static HashSet<User> receivedUsersSet = new HashSet<User>();
        private static HashSet<string> receivedUsernamesSet = new HashSet<string>();

        // queue used to hold messages from topics from the server
        private static ConcurrentQueue<TopicMessageList> topicMessagesQueue = new ConcurrentQueue<TopicMessageList>();

        // port the client uses to communicate with the server
        private int clientPort = 4040;

        /// <summary>
        /// Empty constructor
        /// </summary>
        public ClientMethods()
        {
            // empty constructor
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientPort">port the client uses to communicate with the server</param>
        public ClientMethods(int clientPort)
        {
            this.clientPort = clientPort;
        }

        /// <summary>
        /// Create the service used to host client methods
        /// </summary>
        /// <param name="address"></param>
        private void createRecvChannel(string address)
        {
            WSHttpBinding binding = new WSHttpBinding();
            Uri baseAddress = new Uri(address);
            service = new ServiceHost(typeof(ClientMethods), baseAddress);
            service.AddServiceEndpoint(typeof(IClientMethods), binding, baseAddress);
            service.Open();
        }

        /// <summary>
        /// Start listening to server communication
        /// </summary>
        /// <param name="onListenerCreatedHandler">event handler</param>
        public void startPeerListener(Action<string> onListenerCreatedHandler)
        {
            OnListenerCreated += new ListenerCreated(onListenerCreatedHandler);

            Thread startListener = new Thread(startListening);
            startListener.IsBackground = true;
            startListener.Start();
        }

        /// <summary>
        /// Create channel to server
        /// </summary>
        void startListening()
        {
            while (true)
            {
                try
                {
                    string endpoint = "http://localhost:" + clientPort + "/IServerMethods";
                    createRecvChannel(endpoint);
                    OnListenerCreated.Invoke(endpoint);
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception starting listening: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Get a user corrresponding to a username
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>User object of user corresponding to username</returns>
        public User getUser(string username)
        {
            foreach (User u in receivedUsersSet)
            {
                if ((u.username).Equals(username))
                {
                    return u;
                }
            }
            return null;
        }

        /// <summary>
        /// Receive a list of topics from the server
        /// </summary>
        /// <param name="topicsList">list of topics from the server</param>
        public void receiveTopics(TopicsList topicsList)
        {
            receivedTopicNamesSet.Clear();
            foreach (Topic t in topicsList.getList())
            {
                receivedTopicNamesSet.Add(t.name);
            }
            Console.Write("Received " + receivedTopicNamesSet.Count + " topics from server");
        }

        /// <summary>
        /// Get list of all all topics identified by their topic name
        /// </summary>
        /// <returns>list of topic names</returns>
        public List<string> getAllTopicNames()
        {
            return receivedTopicNamesSet.ToList();
        }

        /// <summary>
        /// Receive list of topic messages
        /// </summary>
        /// <param name="topicMessages"></param>
        public void receiveTopicMessages(TopicMessageList topicMessages)
        {
            topicMessagesQueue.Enqueue(topicMessages);
        }

        /// <summary>
        /// Get list of users connected to the server
        /// </summary>
        /// <param name="users">list of users from the server</param>
        public void receiveUsersList(UsersList users)
        {
            receivedUsernamesSet.Clear();
            receivedUsersSet.Clear();
            foreach (User u in users.getList())
            {
                receivedUsernamesSet.Add(u.username);
                receivedUsersSet.Add(u);
            }
            Console.Write("Received users from server");
        }

        /// <summary>
        /// Get list of all users
        /// </summary>
        /// <returns>list of users</returns>
        public List<User> getAllUsers()
        {
            return receivedUsersSet.ToList();
        }

        /// <summary>
        /// Get list of all usernames
        /// </summary>
        /// <returns></returns>
        public List<string> getAllUsernames() 
        {
            return receivedUsernamesSet.ToList();
        }

        /// <summary>
        /// Gets  list of topic messages sent to a specified topic
        /// </summary>
        /// <param name="topicName">name of topic to get messages for</param>
        /// <returns>list of topic messages for the specified topic</returns>
        public List<TopicMessage> getTopicMessages(string topicName)
        {
            List<TopicMessage> msgs = new List<TopicMessage>();
            int queueSize = topicMessagesQueue.Count;
            for (int i = 0; i < queueSize; i++)
            {
                try
                {
                    List<TopicMessage> topicMsgList = new List<TopicMessage>();
                    TopicMessageList msgList = null;
                    topicMessagesQueue.TryDequeue(out msgList);
                    foreach (TopicMessage msg in msgList.getList())
                    {
                        if ((msg.topicName).Equals(topicName))
                        {
                            msgs.Add(msg);
                        } else
                        {
                            topicMsgList.Add(msg);
                        }
                    }
                    TopicMessageList newMsgList = new TopicMessageList(topicMsgList);
                    topicMessagesQueue.Enqueue(newMsgList);
                } catch (Exception ex) { 
                    //Console.WriteLine("Exception dequeuing topic messages list: " + ex);
                }
            }
            return msgs;
        }
    }
}
