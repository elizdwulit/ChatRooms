using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentTools
{
    /// <summary>
    /// ConcurrentList.cs
    /// <br/>
    /// Author: Elizabeth Dwulit
    /// <br/>
    /// Class used to provide a thread-safe List and its functions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentList<T> : IEnumerable<T>
    {
        /// <summary>
        /// List of items in the concurrent list
        /// </summary>
        private List<T> itemList = new List<T>();

        /// <summary>
        /// Lock
        /// </summary>
        object lock_ = new object();

        /// <summary>
        /// Empty constructor
        /// </summary>
        public ConcurrentList()
        {
            // empty constructor
        }

        /// <summary>
        /// Enumerator
        /// </summary>
        /// <returns>Generic type IENumerator</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)itemList).GetEnumerator();
        }

        /// <summary>
        /// Enumerator
        /// </summary>
        /// <returns>IEnumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)itemList).GetEnumerator();
        }

        /// <summary>
        /// Add an item to the list
        /// </summary>
        /// <param name="obj">item to add</param>
        public void add(T obj)
        {
            lock (lock_)
            {
                itemList.Add(obj);
                Monitor.Pulse(lock_);
            }
        }

        /// <summary>
        /// Remove an item from the list
        /// </summary>
        /// <param name="objToRemove">item to remove</param>
        public void remove(T objToRemove)
        {
            lock (lock_)
            {
                while (itemList.Count == 0)
                {
                    Monitor.Wait(lock_);
                }
                itemList.Remove(objToRemove);
            }
        }

        /// <summary>
        /// Get the number of items in the list
        /// </summary>
        /// <returns>size of list</returns>
        public int size()
        {
            lock (lock_)
            {
                return itemList.Count;
            }
        }
    }
}
