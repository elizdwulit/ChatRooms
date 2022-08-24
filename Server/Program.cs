using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string endpoint = "http://localhost:8080/IServerMethods";

            ServerMethods serverMethods;
            serverMethods = new ServerMethods();
            serverMethods.createServerService(endpoint);


            Console.WriteLine("Running");
            Console.ReadKey();

        }
    }
}
