using ConcurrentTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Topic.cs
/// <br/>
/// Author: Elizabeth Dwulit
/// <br/>
/// Contains classes associated with topics
/// </summary>
namespace Server
{
    /// <summary>
    /// Class representing a message in a topic
    /// </summary>
    [DataContract]
    public class TopicMessage
    {
        /// <summary>
        /// The name of the topic the message is sent to
        /// </summary>
        [DataMember]
        public string topicName;

        /// <summary>
        /// The username of the message's sender
        /// </summary>
        [DataMember]
        public string sourceUser;

        /// <summary>
        /// The message
        /// </summary>
        [DataMember]
        public string message;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sourceUser">username of the sender</param>
        /// <param name="topicName">name of the topic</param>
        /// <param name="message">message</param>
        public TopicMessage(string sourceUser, string topicName, string message)
        {
            this.sourceUser = sourceUser;
            this.topicName = topicName;
            this.message = message;
        }
    }

    /// <summary>
    /// Class representing an object containing a list of TopicMessage's (needed for sending data to client)
    /// </summary>
    [DataContract]
    public class TopicMessageList
    {
        /// <summary>
        /// List of topic messages
        /// </summary>
        [DataMember]
        List<TopicMessage> topicMsgList = new List<TopicMessage>();

        /// <summary>
        /// Constructor. Adds all values of inputted list to the TopicMessageList
        /// </summary>
        /// <param name="tl">list of topic messages</param>
        public TopicMessageList(List<TopicMessage> tl)
        {
            foreach (TopicMessage t in tl)
            {
                topicMsgList.Add(t);
            }
        }

        /// <summary>
        /// Get the list represented by the TopicMessageList object
        /// </summary>
        /// <returns></returns>
        public List<TopicMessage> getList()
        {
            return topicMsgList;
        }
    }

    /// <summary>
    /// Object representing a topic
    /// </summary>
    [DataContract]
    public class Topic
    {
        /// <summary>
        /// Name of the topic
        /// </summary>
        [DataMember]
        public string name = string.Empty;

        /// <summary>
        /// List of users that are currently in the topic
        /// </summary>
        private ConcurrentList<User> users = new ConcurrentList<User>();

        /// <summary>
        /// List of messages sent to the topic
        /// </summary>
        private ConcurrentList<TopicMessage> topicMessages = new ConcurrentList<TopicMessage>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">name of topic</param>
        public Topic(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Add a user to the topic
        /// </summary>
        /// <param name="user">user to add</param>
        public void addUser(User user)
        {
            users.add(user);
        }

        /// <summary>
        /// Remove a user from the topic
        /// </summary>
        /// <param name="user">user to remove</param>
        public void removeUser(User user)
        {
            users.remove(user);
        }

        /// <summary>
        /// Get list of all users in the topic
        /// </summary>
        /// <returns>list of users</returns>
        public ConcurrentList<User> getAllUsersInTopic()
        {
            return users;
        }

        /// <summary>
        /// Add a message to the topic
        /// </summary>
        /// <param name="msg">message to add</param>
        public void addMessage(TopicMessage msg)
        {
            topicMessages.add(msg);
        }
    }

    /// <summary>
    /// Class representing an object containing a list of Topic's (needed for sending data to client)
    /// </summary>
    [DataContract]
    public class TopicsList
    {
        /// <summary>
        /// List of topics
        /// </summary>
        [DataMember]
        List<Topic> topicList = new List<Topic>();

        /// <summary>
        /// Constructor. Adds all items in the inputted list to the TopicsList
        /// </summary>
        /// <param name="tl"></param>
        public TopicsList(List<Topic> tl)
        {
            foreach (Topic t in tl)
            {
                topicList.Add(t);
            }
        }

        /// <summary>
        /// Get the list represented by the TopicsList object
        /// </summary>
        /// <returns></returns>
        public List<Topic> getList()
        {
            return topicList;
        }
    }
}
