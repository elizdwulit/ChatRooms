using Client;
using ConcurrentTools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// ServerMethods.cs 
    /// <br/>
    /// Author: Elizabeth Dwulit
    /// <br/>
    /// Class used to provide methods to the server that interacts with the clients
    /// </summary>
    class ServerMethods : IServerMethods
    {
        /// <summary>
        /// Service host
        /// </summary>
        ServiceHost service = null;

        /// <summary>
        /// Manager to manage all users in the server
        /// </summary>
        UserManager userManager;

        /// <summary>
        /// Manager to manage all topics in the server
        /// </summary>
        TopicManager topicManager;

        /// <summary>
        /// Client channel
        /// </summary>
        IClientMethods channel;

        /// <summary>
        /// Dictionary containing queue of topic messages, indexed by topic name
        /// </summary>
        private ConcurrentDictionary<string, ConcurrentQueue<TopicMessage>> topicMessagesQueue = new ConcurrentDictionary<string, ConcurrentQueue<TopicMessage>>();
       
        // Timer for update tasks
        private Timer _timer;

        /// <summary>
        /// Constructor
        /// </summary>
        public ServerMethods()
        {
            if (topicManager == null)
            {
                topicManager = TopicManager.Instance;
            }

            if (userManager == null)
            {
                userManager = UserManager.Instance;
            }
            TimerCallback cb = new TimerCallback(ServerTimer);
            _timer = new Timer(cb, null, 0, 5000);
        }

        /// <summary>
        /// Refresh the users, topics, and topic messages data
        /// </summary>
        /// <param name="state"></param>
        public void ServerTimer(object state)
        {
            try
            {
                refreshUsers();
                refreshTopics();
                updateTopicMessages();
            } catch (Exception ex)
            {
                Console.WriteLine("xception refreshing server data: " + ex.ToString());
            }
        }

        /// <summary>
        /// Refresh the list of users currently connected to the server and send the updated list to the clients
        /// </summary>
        private void refreshUsers()
        {
            Console.WriteLine("Refreshing users list...");
            List<User> users = userManager.getAllUsers().ToList();
            if (users.Count > 0)
            {
                foreach (User user in users)
                {
                    try
                    {
                        string endpoint = user.getConnectionInfo();
                        EndpointAddress baseAddress = new EndpointAddress(endpoint);
                        WSHttpBinding binding = new WSHttpBinding();
                        ChannelFactory<IClientMethods> factory = new ChannelFactory<IClientMethods>(binding, endpoint);
                        channel = factory.CreateChannel();
                        UsersList ulObj = new UsersList(users);
                        channel.receiveUsersList(ulObj);
                        Console.WriteLine("Sent list of " + users.Count() + " users to user: " + user.username);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception updating users list: " + e.Message);
                    }
                }
            } else
            {
                Console.WriteLine("No users currently connected");
            }
            
        }

        /// <summary>
        /// Refresh the list of topics currently in the server and send the updated list to the clients
        /// </summary>
        private void refreshTopics()
        {
            Console.WriteLine("Refreshing topics list...");
            List<User> usersList = userManager.getAllUsers().ToList();
            ConcurrentList<Topic> topicsList = topicManager.getAllTopics();
            //Console.WriteLine("Counted " + topicsList.Count() + " topics");
            if (topicsList.Count() > 0)
            {
                foreach (User user in usersList)
                {
                    string endpoint = user.getConnectionInfo();
                    EndpointAddress baseAddress = new EndpointAddress(endpoint);
                    WSHttpBinding binding = new WSHttpBinding();
                    ChannelFactory<IClientMethods> factory = new ChannelFactory<IClientMethods>(binding, endpoint);
                    channel = factory.CreateChannel();
                    List<Topic> list = topicsList.ToList();
                    TopicsList tlObj = new TopicsList(list);
                    channel.receiveTopics(tlObj);
                    Console.WriteLine("Sent " + topicsList.Count() + " topics to user: " + user.username);
                }
            } else
            {
                Console.WriteLine("No topics to broadcast");
;           }
            
        }

        /// <summary>
        /// Refresh the list of topic messages currently in the server and send the updated list 
        /// to the clients that are in the topics corresponding to the topic messages
        /// </summary>
        private void updateTopicMessages()
        {
            Console.WriteLine("Sending topic messages...");
            ConcurrentList<Topic> topics = topicManager.getAllTopics();
            foreach (Topic topic in topics)
            {
                (UsersList, TopicMessageList) result = receiveTopicMessages(topic.name);
                UsersList users = result.Item1;
                TopicMessageList messages = result.Item2;
                if (messages != null && messages.getList().Count > 0)
                {
                    var userslist = users.getList();
                    foreach (User user in userslist)
                    {
                        string endpoint = user.getConnectionInfo();
                        EndpointAddress baseAddress = new EndpointAddress(endpoint);
                        WSHttpBinding binding = new WSHttpBinding();
                        ChannelFactory<IClientMethods> factory = new ChannelFactory<IClientMethods>(binding, endpoint);
                        channel = factory.CreateChannel();
                        channel.receiveTopicMessages(messages);
                        Console.WriteLine("Sent " + messages.getList().Count + " topic messages to user: " + user.username);
                    }
                } else
                {
                    Console.WriteLine("No new topic messages to send");
                }
            }
        }

        /// <summary>
        /// Create server service
        /// </summary>
        /// <param name="address">address of server</param>
        public void createServerService(string address)
        {
            WSHttpBinding binding = new WSHttpBinding();
            Uri baseAddress = new Uri(address);
            service = new ServiceHost(typeof(ServerMethods), baseAddress);
            service.AddServiceEndpoint(typeof(IServerMethods), binding, baseAddress);
            service.Open();
            Console.WriteLine("Server Service created");
        }

        /// <summary>
        /// Close the server service
        /// </summary>
        public void Close()
        {
            service.Close();
        }

        /// <summary>
        /// Add a user to the server
        /// </summary>
        /// <param name="username">username of user to add</param>
        /// <param name="ip">IP address of user to add</param>
        /// <param name="serverConnPort">port user used to connect to the server</param>
        /// <param name="peerConnPort">port user will use to send peer messages to other users</param>
        public void connect(string username, string ip, int serverConnPort, int peerConnPort)
        {
            if (userManager.isUniqueConnection(ip, serverConnPort))
            {
                Console.WriteLine($"CONNECTED TO USER {username}");
                userManager.addUser(username, ip, serverConnPort, peerConnPort);
            }
            else
            {
                Console.WriteLine("Failed to connect " + username + ". User connection info is not unique.");
            }

        }

        /// <summary>
        /// Remove a user from the server
        /// </summary>
        /// <param name="username"></param>
        public void disconnect(string username)
        {
            userManager.deleteUser(username);
        }

        /// <summary>
        /// Create a topic
        /// </summary>
        /// <param name="user">user that created the topic</param>
        /// <param name="topicName">name of topic to create</param>
        public void createTopic(User user, string topicName)
        {
            topicManager.createTopic(topicName);
            Console.WriteLine("Created new topic: " + topicName);
        }

        /// <summary>
        /// Add a user to a topic
        /// </summary>
        /// <param name="user">user to add</param>
        /// <param name="topicName">name of topic to add user to</param>
        public void joinTopic(User user, string topicName)
        {
            topicManager.addUserToTopic(user, topicName);
            Console.WriteLine("User " + user.username + " joined topic " + topicName);
        }

        /// <summary>
        /// Remove a user from a topic
        /// </summary>
        /// <param name="user">user to remove</param>
        /// <param name="topicName">name of topic to remove the user from</param>
        public void leaveTopic(User user, string topicName)
        {
            topicManager.removeUserFromTopic(user, topicName);
            Console.WriteLine("User " + user.username + " left topic " + topicName);
        }

        /// <summary>
        /// Get list of topic messages and list of users in the topic
        /// </summary>
        /// <param name="topicName">name of topic to get messages and users for</param>
        /// <returns>(list of users connected to topic, list of messages sent to the topic)</returns>
        public (UsersList, TopicMessageList) receiveTopicMessages(string topicName)
        {
            if (!topicMessagesQueue.ContainsKey(topicName))
            {
                return (new UsersList(new List<User>()), new TopicMessageList(new List<TopicMessage>()));
            }

            List<TopicMessage> msgs = new List<TopicMessage>();
            Topic topic = topicManager.getTopic(topicName);
            ConcurrentList<User> usersInTopic = topic.getAllUsersInTopic();
            if (usersInTopic.size() > 0)
            {
                ConcurrentQueue<TopicMessage> queuedTopicMsgs = new ConcurrentQueue<TopicMessage>();
                queuedTopicMsgs = topicMessagesQueue[topicName];
                foreach (TopicMessage msg in queuedTopicMsgs)
                {
                    msgs.Add(msg);
                    TopicMessage dequeueOut = null;
                    queuedTopicMsgs.TryDequeue(out dequeueOut);
                }
            }
            List<User> users = usersInTopic.ToList();
            UsersList usersList = new UsersList(users);
            TopicMessageList topicMsgList = new TopicMessageList(msgs);
            return (usersList, topicMsgList);
        }

        /// <summary>
        /// Send a message to a topic
        /// </summary>
        /// <param name="username">username of user sending the message</param>
        /// <param name="topicName">name of the topic to send message to</param>
        /// <param name="message">message to send</param>
        public void sendMessageToTopic(string username, string topicName, string message)
        {
            if (string.IsNullOrEmpty(topicName))
            {
                // do nothing
            }

            topicManager.sendMessageToTopic(username, topicName, message);
            TopicMessage topicMsg = new TopicMessage(username, topicName, message);

            ConcurrentQueue<TopicMessage> queue = null;
            if (!topicMessagesQueue.ContainsKey(topicName))
            {
                queue = new ConcurrentQueue<TopicMessage>();
                topicMessagesQueue.TryAdd(topicName, queue);
                topicMessagesQueue[topicName].Enqueue(topicMsg);
            } else
            {
                queue = topicMessagesQueue[topicName];
                queue.Enqueue(topicMsg);
            }
        }
    }
}
