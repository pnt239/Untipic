using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Untipic.Business.EventArguments;

namespace Untipic.Business
{
    public class Client
    {
        public event EventHandler ConnectingFailed = null;
        public event EventHandler ConnectingSuccessed = null;
        public event ServerDisconnectedEventHandler ServerDisconnected = null;
        public event ClientDisconnectedEventHandler DisconnectedFromServer = null;
        public event DataReceivedEventHandler DataReceived = null;
        public event DataSentEventHandler DataSent = null;

        public int Id
        {
            get { return _id; } 
            set { _id = value; }
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Gets the IP address of connected remote client.This is 'IPAddress.None' if the client is not connected.
        /// </summary>
        /// <value>
        /// The ip.
        /// </value>
        public IPAddress IP
        {
            get
            {
                if (_socket != null)
                    return ((IPEndPoint)_socket.RemoteEndPoint).Address;
                return IPAddress.None;
            }
        }

        /// <summary>
        /// Gets the port number of connected remote client.This is -1 if the client is not connected.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public int Port
        {
            get
            {
                if (_socket != null)
                    return ((IPEndPoint)_socket.RemoteEndPoint).Port;
                return -1;
            }
        }

        /// <summary>
        /// Gets the socket of client.
        /// </summary>
        /// <value>
        /// The socket.
        /// </value>
        public Socket Socket
        {
            get { return _socket; }
        }

        /// <summary>
        /// Starts receive this client.
        /// </summary>
        public void Start()
        {
            var t = new Thread(OnReceive) { IsBackground = true };
            t.Start();
        }

        /// <summary>
        /// Links the specified accepted client socket.
        /// </summary>
        /// <param name="clientSocket">The accepted client socket.</param>
        /// <param name="id">The identifier.</param>
        public void Link(Socket clientSocket, int id)
        {
            _socket = clientSocket;
            _id = id;
            _networkStream = new NetworkStream(Socket);
        }

        public void Connect(string ipServer, int port)
        {
            Connect(IPAddress.Parse(ipServer), port);
        }

        public void Connect(IPAddress ipServer, int port)
        {
            _ipServer = ipServer;
            _port = port;

            var bwConnector = new BackgroundWorker();
            bwConnector.DoWork += bwConnector_DoWork;
            bwConnector.RunWorkerCompleted += bwConnector_RunWorkerCompleted;
            bwConnector.RunWorkerAsync();
        }

        public bool Disconnect()
        {
            if (_socket != null && _socket.Connected)
            {
                try
                {
                    _networkStream.Close();
                    _networkStream.Dispose();
                    _networkStream = null;

                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.Close();
                    _socket.Dispose();
                    _socket = null;
                    OnDisconnectedFromServer(new ClientDisconnectedEventArgs(_id));
                    return true;
                }
                catch
                {
                    return false;
                }

            }
            return true;
        }

        public bool SendData(int receiveId)
        {
            if (_socket != null && _socket.Connected)
            {
                var bwSender = new BackgroundWorker();
                bwSender.DoWork += bwSender_DoWork;
                bwSender.RunWorkerCompleted += bwSender_RunWorkerCompleted;
                bwSender.WorkerSupportsCancellation = true;
                bwSender.RunWorkerAsync(receiveId);
                return true;
            }
            
            return false;
        }

        private void bwConnector_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!((bool)e.Result))
                OnConnectingFailed(EventArgs.Empty);
            else
                OnConnectingSuccessed(EventArgs.Empty);

            ((BackgroundWorker)sender).Dispose();
            GC.Collect();
        }

        private void bwSender_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if (!e.Cancelled && e.Error == null && ((bool)e.Result))
            //    this.OnCommandSent(new EventArgs());
            //else
            //    this.OnCommandFailed(new EventArgs());

            ((BackgroundWorker)sender).Dispose();
            GC.Collect();
        }

        private void bwConnector_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.Connect(new IPEndPoint(_ipServer, _port));
                _networkStream = new NetworkStream(_socket);
                e.Result = true;
                Start();
            }
            catch
            {
                e.Result = false;
            }
        }

        private void bwSender_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _semaphor.WaitOne();

                OnDataSent(new DataSentEventArgs((int)e.Argument, _networkStream));

                _semaphor.Release();
                e.Result = true;
            }
            catch
            {
                _semaphor.Release();
                e.Result = false;
            }
        }

        private void OnReceive()
        {
            while (Socket.Connected)
            {
                int nbyte;
                byte[] byteData;
                int actionType;

                try
                {
                    byteData = new byte[4];
                    nbyte = _networkStream.Read(byteData, 0, 4);

                    if (nbyte == 0) break;

                    actionType = BitConverter.ToInt32(byteData, 0);
                }
                catch
                {
                    break;
                }
                if (nbyte == 0) break;

                OnDataReceived(new DataReceivedEventArgs(_id, actionType, _networkStream));
            }

            OnServerDisconnected(new ServerDisconnectedEventArgs(_ipServer, _port));

            Disconnect();
        }

        private void OnConnectingFailed(EventArgs e)
        {
            if (ConnectingFailed != null)
                ConnectingFailed(this, e);
        }

        private void OnConnectingSuccessed(EventArgs e)
        {
            if (ConnectingSuccessed != null)
                ConnectingSuccessed(this, e);
        }

        private void OnServerDisconnected(ServerDisconnectedEventArgs e)
        {
            if (ServerDisconnected != null)
                ServerDisconnected(this, e);
        }

        private void OnDisconnectedFromServer(ClientDisconnectedEventArgs e)
        {
            if (DisconnectedFromServer != null)
                DisconnectedFromServer(this, e);
        }

        private void OnDataReceived(DataReceivedEventArgs e)
        {
            if (DataReceived != null)
                DataReceived(this, e);
        }

        private void OnDataSent(DataSentEventArgs e)
        {
            if (DataSent != null)
                DataSent(this, e);
        }

        private IPAddress _ipServer;
        private int _port;
        private Socket _socket;
        private int _id;
        private NetworkStream _networkStream;
        private Semaphore _semaphor = new Semaphore(1, 1);
    }
}
