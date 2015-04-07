using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using Untipic.Business.EventArguments;

namespace Untipic.Business
{
    public class Server
    {
        
        public Server(IPAddress ip, int port)
        {
            _clientCount = 1;
            _clients = new Dictionary<int,Client>();

            try
            {
                _ipServer = ip;
                _port = port;
                _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            catch(Exception ex)
            {
                OnErrorCaught(new NetworkErrorEventArgs(ex.Message));
            }            
        }

        public event ClientConnectingEventHandler ClientConnecting = null;
        public event ClientConnectedEventHandler ClientConnected = null;
        public event ClientDisconnectedEventHandler ClientDisconnected = null;
        public event DataReceivedEventHandler DataReceived = null;
        public event DataSentEventHandler DataSent = null;
        public event NetworkErrorEventHandler ErrorCaught = null;

        public void Listen()
        {
            try
            {
                // Config for listening
                _serverSocket.Bind(new IPEndPoint(_ipServer, _port));
                _serverSocket.Listen(4);

                //Accept the incoming clients
                _bwListener = new BackgroundWorker { WorkerSupportsCancellation = true };
                _bwListener.DoWork += OnAccept;
                _bwListener.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                OnErrorCaught(new NetworkErrorEventArgs(ex.Message));
            } 
        }

        public void Disconnect()
        {
            foreach (var client in _clients)
                client.Value.Disconnect();

            _bwListener.CancelAsync();
            _bwListener.Dispose();
            _serverSocket.Close();
        }

        public void SendData(int receiveId)
        {
            _clients[receiveId].SendData(receiveId);
        }

        public void SendDataBroadCast(int exceptId = -1)
        {
            foreach (var client in _clients)
                if (client.Value.Id != exceptId && client.Value.Id != 0)
                {
                    client.Value.SendData(client.Value.Id);
                }
        }

        private void OnAccept(object sender, DoWorkEventArgs e)
        {
            bool exit = false;
            while (!exit)
            {
                try
                {
                    var client = new Client();
                    int id = _clientCount++;
                    client.Link(_serverSocket.Accept(), id);
                    client.DisconnectedFromServer += Client_Disconnected;
                    client.DataReceived += Client_DataReceived;
                    client.DataSent += Client_DataSent;
                    _clients.Add(id, client);

                    if (CheckAdmiting(client.IP.ToString()))
                    {
                        // Start thread
                        client.Start();

                        OnClientConnected(new ClientConnectedEventArgs(client.Socket, client.Id));
                    }
                    else
                        client.Disconnect();
                }
                catch (Exception ex)
                {
                    OnErrorCaught(new NetworkErrorEventArgs(ex.Message));
                    exit = true;
                }
            }
        }

        private void Client_Disconnected(object sender, ClientDisconnectedEventArgs e)
        {
            OnClientDisconnected(e);
            RemoveClient(e.ClientId);
        }

        private void Client_DataReceived(object sender, DataReceivedEventArgs e)
        {
            OnDataReceived(e);
        }

        private void Client_DataSent(object sender, DataSentEventArgs e)
        {
            lock (this)
            {
                if (DataSent != null)
                    DataSent(this, e);
            }
        }

        private void OnClientConnecting(ClientConnectingEventArgs e)
        {
            if (ClientConnecting != null)
                ClientConnecting(this, e);
        }

        private void OnClientConnected(ClientConnectedEventArgs e)
        {
            if (ClientConnected != null)
                ClientConnected(this, e);
        }

        private void OnClientDisconnected(ClientDisconnectedEventArgs e)
        {
            if (ClientDisconnected != null)
                ClientDisconnected(this, e);
        }

        private void OnDataReceived(DataReceivedEventArgs e)
        {
            if (DataReceived != null)
                DataReceived(this, e);
        }

        private void OnErrorCaught(NetworkErrorEventArgs e)
        {
            if (ErrorCaught != null)
                ErrorCaught(this, e);
        }

        private bool CheckAdmiting(string clientInfo)
        {
            var e = new ClientConnectingEventArgs(clientInfo, true);
            OnClientConnecting(e);
            return e.IsAccept;
        }

        //private int IndexOfClient(IPAddress ip)
        //{
        //    int index = -1;
        //    foreach (var client in _clients)
        //    {
        //        index++;
        //        if (client.Value.IP.Equals(ip))
        //            return index;
        //    }
        //    return -1;
        //}

        //private int IndexOfClient(int id)
        //{
        //    int index = -1;
        //    foreach (var client in _clients)
        //    {
        //        index++;
        //        if (client.Value.Id == id)
        //            return index;
        //    }
        //    return -1;
        //}

        /// <summary>
        /// Removes the client.
        /// </summary>
        /// <param name="id">The identifier.</param>
        private void RemoveClient(int id)
        {
            lock (this)
            {
                _clients.Remove(id);
            }
        }

        private IPAddress _ipServer;
        private int _port;
        private Socket _serverSocket;
        private BackgroundWorker _bwListener;
        private int _clientCount;
        private Dictionary<int, Client> _clients;
    }
}
