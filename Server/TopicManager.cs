using ConcurrentTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server
{
    /// <summary>
    /// TopicManager.cs
    /// <br/>
    /// Author: Elizabeth Dwulit
    /// <br/>
    /// Singleton class that handles actions on topics and manages topics
    /// </summary>
    public sealed class TopicManager
    {
        /// <summary>
        /// Lock used for singleton class
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// TopicManager instance
        /// </summary>
        private static TopicManager instance = null;

        /// <summary>
        /// Thread-safe list of topics
        /// </summary>
        private ConcurrentList<Topic> topics = new ConcurrentList<Topic>();

        /// <summary>
        /// Constructor
        /// </summary>
        TopicManager() 
        {
            Console.WriteLine("Created topic manager");
        }

        /// <summary>
        /// TopicManager Instance
        /// </summary>
        public static TopicManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new TopicManager();
                    }
                    return instance;
                }
            }
        }

        /// <summary>
        /// Get a topic corresponding to a topic name
        /// </summary>
        /// <param name="topicName">name of topic</param>
        /// <returns>Topic</returns>
        public Topic getTopic(string topicName)
        {
            if (topicName == null)
            {
                return null;
            }

            foreach (Topic t in topics)
            {
                if (t.name.Equals(topicName))
                {
                    return t;
                }
            }

            return null;
        }

        /// <summary>
        /// Creates and adds a topic to the server
        /// </summary>
        /// <param name="topicName">name of topic create</param>
        /// <returns>Created Topic</returns>
        public Topic createTopic(string topicName)
        {
            if (topicName == null)
            {
                // do nothing
            }

            Topic t = new Topic(topicName);
            topics.add(t);
            return t;
        }

        /// <summary>
        /// Removes a topic from the server
        /// </summary>
        /// <param name="topicName">name of topic to remove</param>
        public void deleteTopic(string topicName)
        {
            if (topicName == null)
            {
                // do nothing
            }

            Topic t = getTopic(topicName);
            if (t != null)
            {
                topics.remove(t);
            }
        }

        /// <summary>
        /// Add a user to a topic
        /// </summary>
        /// <param name="user">user to add</param>
        /// <param name="topicName">name of topic to add user to</param>
        public void addUserToTopic(User user, string topicName)
        {
            if (topicName == null)
            {
                // do nothing
            }

            Topic topic = getTopic(topicName);
            if (topic != null)
            {
                topics.remove(topic);
                if (topic != null)
                {
                    topic.addUser(user);
                }
                topics.add(topic);
            }
        }

        /// <summary>
        /// Remove a user from a topic
        /// </summary>
        /// <param name="user">user to remove</param>
        /// <param name="topicName">name of topic to remove from</param>
        public void removeUserFromTopic(User user, string topicName)
        {
            if (topicName == null)
            {
                // do nothing
            }

            Topic topic = getTopic(topicName);
            if (topic != null)
            {
                topics.remove(topic);
                if (topic != null)
                {
                    topic.removeUser(user);
                }
                topics.add(topic);
            }
        }

        /// <summary>
        /// Add a message to a topic
        /// </summary>
        /// <param name="username">name of user that is sending the message</param>
        /// <param name="topicName">name of topic to add to</param>
        /// <param name="message">message</param>
        public void sendMessageToTopic(string username, string topicName, string message)
        {
            if (topicName == null)
            {
                // do nothing
            }

            Topic topic = getTopic(topicName);
            if (topic != null)
            {
                TopicMessage tmsg = new TopicMessage(username, topicName, message);
                topic.addMessage(tmsg);
            }
        }

        /// <summary>
        /// Get list of all topics
        /// </summary>
        /// <returns>thread-safe list of topics</returns>
        public ConcurrentList<Topic> getAllTopics()
        {
            return topics;
        }
    }
}