using System;
using System.Net.Sockets;

namespace Untipic.Business.EventArguments
{
    public class DataReceivedEventArgs : EventArgs
    {
        public DataReceivedEventArgs(int socketId, int signal, NetworkStream stream)
        {
            ClientId = socketId;
            Signal = signal;
            Stream = stream;
        }

        public int ClientId { get; set; }

        public int Signal { get; set; }

        public NetworkStream Stream { get; set; }
    }

    public delegate void DataReceivedEventHandler(Object sender, DataReceivedEventArgs eventArgs);
}
