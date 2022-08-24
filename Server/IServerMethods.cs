using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ConcurrentTools;

namespace Server
{
	/// <summary>
	/// IServerMethods.cs
	/// <br/>
	/// Author: Elizabeth Dwulit
	/// <br/>
	/// ServiceContract interface for methods used by the Server
	/// </summary>
	[ServiceContract]
	public interface IServerMethods
	{
		/// <summary>
		/// Add a user to the server
		/// </summary>
		/// <param name="username">username of user to add</param>
		/// <param name="ip">ip address of user to add</param>
		/// <param name="serverConnPort">port used by user to add to connect to the server</param>
		/// <param name="peerConnPort">port used by the user to add to connect to other users</param>
		[OperationContract]
		void connect(string username, string ip, int serverConnPort, int peerConnPort);

		/// <summary>
		/// Remove a user from the server
		/// </summary>
		/// <param name="username">username of user to remove</param>
		[OperationContract]
		void disconnect(string username);

		/// <summary>
		/// Receive topic messages
		/// </summary>
		/// <param name="topicName">name of topic to receive messages for</param>
		/// <returns></returns>
		[OperationContract]
		(UsersList, TopicMessageList) receiveTopicMessages(string topicName);

		/// <summary>
		/// Create a topic on the server
		/// </summary>
		/// <param name="user">user creating the topic</param>
		/// <param name="topicName">name of topic to create</param>
		[OperationContract]
		void createTopic(User user, string topicName);

		/// <summary>
		/// Add a user to a topic
		/// </summary>
		/// <param name="user">user to add</param>
		/// <param name="topicName">name of topic to add the user to</param>
		[OperationContract]
		void joinTopic(User user, string topicName);

		/// <summary>
		/// Remove a user from a topic
		/// </summary>
		/// <param name="user">user to remove</param>
		/// <param name="topicName">name of topic to remove the user from</param>
		[OperationContract]
		void leaveTopic(User user, string topicName);

		/// <summary>
		/// Send a message to a topic
		/// </summary>
		/// <param name="username">username of user sending the message</param>
		/// <param name="topic">name of topic to send the message to</param>
		/// <param name="message">message</param>
		[OperationContract]
		void sendMessageToTopic(string username, string topic, string message);
	}
}
