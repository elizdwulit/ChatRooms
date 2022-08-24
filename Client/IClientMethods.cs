using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// IClientMethods.cs
    /// <br/>
    /// Author: Elizabeth Dwulit
    /// <br/>
    /// ServiceContract interface for methods used by the Client
    /// </summary>
    [ServiceContract]
    public interface IClientMethods
    {
        /// <summary>
        /// Send topics to the client
        /// </summary>
        /// <param name="topic">list of topics</param>
        [OperationContract]
        void receiveTopics(TopicsList topic);

        /// <summary>
        /// Send topic messages to the client
        /// </summary>
        /// <param name="topicMessages">list of topic messages</param>
        [OperationContract]
        void receiveTopicMessages(TopicMessageList topicMessages);

        /// <summary>
        /// Send list of users to the client
        /// </summary>
        /// <param name="users">list of users</param>
        [OperationContract]
        void receiveUsersList(UsersList users);

    }
}
