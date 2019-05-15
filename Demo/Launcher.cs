using HTTP.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using WebServer;
using WebServer.Routing;
using WebServer.Routing.Contracts;

namespace Demo
{
    public class Launcher
    {
        public static void Main( string[] args)
        {
            IServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            serverRoutingTable.Add
                (HttpRequestMethod.Get, "/", request => new HomeController().Index(request));

            Server server = new Server(8000, serverRoutingTable);
            server.Run();
        }
    }
}
