using System;
using System.Net;

namespace Untipic.Business.EventArguments
{
    public class ServerDisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// Creates an instance of ServerEventArgs class.
        /// </summary>
        /// <param name="clientSocket">The client socket of current CommandClient instance.</param>
        public ServerDisconnectedEventArgs(IPAddress ip, int port)
        {
            Ip = ip;
            Port = port;
        }

        /// <summary>
        /// The IP address of server.
        /// </summary>
        public IPAddress Ip { get; set; }

        /// <summary>
        /// The port of server.
        /// </summary>
        public int Port { get; set; }
    }

    public delegate void ServerDisconnectedEventHandler(object sender, ServerDisconnectedEventArgs e);
}
