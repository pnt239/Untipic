using System.Net.Sockets;
using Untipic.Entity;
using Untipic.Presentation;

namespace Untipic.Business
{

    public class UserInfo : UserInfoBase
    {
        public UserInfo(Socket socket, int id, ShapeDrawer shapedrawer)
        {
            Socket = socket;
            Id = id;
            Name = "Client " + id;

            ControlBox = new DrawingControl();
            ControlBox.SetShapDrawer(shapedrawer);
            ControlBox.ControlMode = ControlMode.CreateShape;
            ControlBox.Visible = false;
        }

        public DrawingControl ControlBox { get; set; }

        public Socket Socket { get; set; }

        public bool Disconnect()
        {
            if (Socket != null && Socket.Connected)
            {
                try
                {
                    Socket.Shutdown(SocketShutdown.Both);
                    Socket.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
    }
}
