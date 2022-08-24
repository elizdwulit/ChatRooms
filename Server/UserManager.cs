using ConcurrentTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server
{
    /// <summary>
    /// UserManager.cs
    /// <br/>
    /// Author: Elizabeth Dwulit
    /// <br/>
    /// Singleton class that handles actions on users and manages users
    /// </summary>
    public sealed class UserManager
    {
        /// <summary>
        /// Lock used for singleton class
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// UserManager instance
        /// </summary>
        private static UserManager instance = null;

        /// <summary>
        /// Thread-safe list of users
        /// </summary>
        ConcurrentList<User> users = new ConcurrentList<User>();

        /// <summary>
        /// Constructor
        /// </summary>
        UserManager()
        {
            Console.WriteLine("Created user manager");
        }
        
        /// <summary>
        /// UserManager Instance
        /// </summary>
        public static UserManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new UserManager();
                    }
                    return instance;
                }
            }
        }
        
        /// <summary>
        /// Add a user to the server
        /// </summary>
        /// <param name="username">username of user to add</param>
        /// <param name="ip">IP address of user to add</param>
        /// <param name="serverConnPort">port the user used to connect to the server</param>
        /// <param name="peerConnPort">port the user uses to communicate with other users (peer messages)</param>
        public void addUser(string username, string ip, int serverConnPort, int peerConnPort)
        {
            User u = new User(username, ip, serverConnPort, peerConnPort);
            users.add(u);
        }

        /// <summary>
        /// Delete a user from the server
        /// </summary>
        /// <param name="username">username of user to delete</param>
        public void deleteUser(string username)
        {
            User user = null;
            try
            {
                user = users.First(u => (u.username).Equals(username));
            }
            catch (Exception e)
            {
                // do nothing
            }
            if (user != null)
            {
                users.remove(user);
            }
        }

        /// <summary>
        /// Get the User corresponding to a username
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>User corresponding to username</returns>
        public User getUser(string username)
        {
            User user = null;
            try
            {
                user = users.First(u => (u.username).Equals(username));
            }
            catch (Exception e)
            {
                // do nothing
            }
            return user;
        }

        /// <summary>
        /// Get thread-safe list of all users in the server
        /// </summary>
        /// <returns></returns>
        public ConcurrentList<User> getAllUsers()
        {
            return users;
        }

        /// <summary>
        /// Checks if a user's connection is unique
        /// </summary>
        /// <param name="ip">IP address of user</param>
        /// <param name="port">port user is attempting to use to connect to the server</param>
        /// <returns>true if connection info is unique, else false</returns>
        public bool isUniqueConnection(string ip, int port)
        {
            User user = null;
            try
            {
                user = users.First(u => (u.ip).Equals(ip) && (u.serverConnPort == port));
            }
            catch (Exception e)
            {
                // do nothing
            }
            if (user == null)
            {
                return true;
            }
            return false;
        }
    }
}