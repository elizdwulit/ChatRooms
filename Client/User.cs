using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// User.cs
/// <br/>
/// Author: Elizabeth Dwulit
/// <br/>
/// Contains classes associated with users
/// </summary>
namespace Server
{
    /// <summary>
    /// Class representing a user
    /// </summary>
    [DataContract]
    public class User
    {
        /// <summary>
        /// username of user
        /// </summary>
        [DataMember]
        public string username = string.Empty;

        /// <summary>
        /// IP address of user
        /// </summary>
        [DataMember]
        public string ip = string.Empty;

        /// <summary>
        /// Port user used to connect to the server
        /// </summary>
        [DataMember]
        public int serverConnPort = -1;

        /// <summary>
        /// Port the user uses to communicate with other users (peers)
        /// </summary>
        [DataMember]
        public int peerConnPort = -1;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="ip">IP address of user</param>
        /// <param name="serverConnPort">port user used to connect to the server</param>
        /// <param name="peerConnPort">port user uses to communicate with peers</param>
        public User(string username, string ip, int serverConnPort, int peerConnPort)
        {
            this.username = username;
            this.ip = ip;
            this.serverConnPort = serverConnPort;
            this.peerConnPort = peerConnPort;
        }

        /// <summary>
        /// Get connection information of user
        /// </summary>
        /// <returns>string of connection info in format http://ipaddress:serverport/IServerMethods</returns>
        public string getConnectionInfo()
        {
            return "http://" + ip + ":" + serverConnPort + "/IServerMethods";
        }
    }

    /// <summary>
    /// Class representing an object containing a list of users
    /// </summary>
    [DataContract]
    public class UsersList
    {
        /// <summary>
        /// List of users
        /// </summary>
        [DataMember]
        List<User> userList = new List<User>();

        /// <summary>
        /// Constructor. Adds all items in the inputted list to the UsersList
        /// </summary>
        /// <param name="ul">list of users to add</param>
        public UsersList(List<User> ul)
        {
            foreach (User u in ul)
            {
                userList.Add(u);
            }
        }

        /// <summary>
        /// Get list of users represented by the UsersList
        /// </summary>
        /// <returns>list of users</returns>
        public List<User> getList()
        {
            return userList;
        }
    }
}
