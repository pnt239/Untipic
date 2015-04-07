using System;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using Untipic.Business.Action;
using Untipic.Entity;

namespace Untipic.Business
{
    public class ActionFactory
    {
        public ActionFactory(AppManament manager)
        {
            _manager = manager;
        }

        public void SendAction(IAction action, NetworkStream stream)
        {
            // ActionType | Reciever UserId | ...
            WriteInt((int)action.GetActionType(), stream);
            WriteInt(action.SenderId, stream);
            WriteInt(action.ReceiverId, stream);

            switch (action.GetActionType())
            {
                case ActionType.MouseMove:
                    SendMouseMoveAction((MouseMoveAction)action, stream);
                    break;
                case ActionType.AddUser:
                    SendAddUserAction((AddUserAction)action, stream);
                    break;
                case ActionType.IdentifyUser:
                    break;
                case ActionType.LoadControlBox:
                    SendLoadControlBoxAction((LoadControlBoxAction) action, stream);
                    break;
                case ActionType.UpdateControlBox:
                    SendUpdateControlBoxAction((UpdateControlBoxAction) action, stream);
                    break;
                case ActionType.AddVertex:
                    SendAddVertexAction((AddVertexAction) action, stream);
                    break;
                case ActionType.RemoveUser:
                    SendRemoveUserAction((RemoveUserAction) action, stream);
                    break;
            }
        }

        public IAction GetAction(ActionType type, NetworkStream stream)
        {
            // Get user ID
            IAction action = null;
            int senderId = ReadInt(stream);
            int receiveId = ReadInt(stream);

            switch (type)
            {
                case ActionType.MouseMove:
                    action = GetMouseMoveAction(senderId, stream);
                    break;
                case ActionType.AddUser:
                    action = GetAddUserAction(senderId, stream);
                    break;
                case ActionType.IdentifyUser:
                    action = GetIdentifyAction(senderId, stream);
                    break;
                case ActionType.LoadControlBox:
                    action = GetLoadControlBoxAction(senderId, stream);
                    break;
                case ActionType.UpdateControlBox:
                    action = GetUpdateControlBoxAction(senderId, stream);
                    break;
                case ActionType.AddVertex:
                    action = GetAddVertexAction(senderId, stream);
                    break;
                case ActionType.RemoveUser:
                    action = GetRemoveUserAction(senderId, stream);
                    break;
            }

            if (action == null) return null;

            action.SenderId = senderId;
            action.ReceiverId = receiveId;

            return action;
        }

        private MouseMoveAction GetMouseMoveAction(int senderId, NetworkStream stream)
        {
            var action = new MouseMoveAction();
            action.User = _manager.ClientList[senderId];

            // Get X
            int x = ReadInt(stream);

            // Get Y
            int y = ReadInt(stream);

            // Set location
            action.Location = new Point(x, y);

            return action;
        }

        private AddUserAction GetAddUserAction(int senderId, NetworkStream stream)
        {
            // Get user ID
            int id = ReadInt(stream);

            // Get user name
            string name = ReadString(stream);

            var action = new AddUserAction(_manager.ClientList, _manager.SendList);
            action.User = new UserInfo(null, id, _manager.ShapeDrawer);
            action.User.Name = name;

            return action;
        }

        private IdentifyAction GetIdentifyAction(int senderId, NetworkStream stream)
        {
            var action = new IdentifyAction(_manager.Client);

            return action;
        }

        private LoadControlBoxAction GetLoadControlBoxAction(int senderId, NetworkStream stream)
        {
            var action = new LoadControlBoxAction(_manager.ClientList[senderId].ControlBox);
            action.ShapeType = (ShapeType)ReadInt(stream);
            var sx = ReadInt(stream);
            var sy = ReadInt(stream);
            action.StartPoint = new Point(sx, sy);

            var ex = ReadInt(stream);
            var ey = ReadInt(stream);
            action.EndPoint = new Point(ex, ey);

            return action;
        }

        private UpdateControlBoxAction GetUpdateControlBoxAction(int senderId, NetworkStream stream)
        {
            // ShapeType | Visible | Start Point | End Point
            var action = new UpdateControlBoxAction(_manager.ClientList[senderId].ControlBox);
            action.ShapeType = (ShapeType)ReadInt(stream);
            action.ControlVisible = ReadBool(stream);
            var sx = ReadInt(stream);
            var sy = ReadInt(stream);
            action.StartPoint = new Point(sx, sy);

            var ex = ReadInt(stream);
            var ey = ReadInt(stream);
            action.EndPoint = new Point(ex, ey);

            return action;
        }

        private AddVertexAction GetAddVertexAction(int senderId, NetworkStream stream)
        {
            // Location
            var action = new AddVertexAction(_manager.ClientList[senderId].ControlBox);
            var sx = ReadInt(stream);
            var sy = ReadInt(stream);
            action.Location = new Point(sx, sy);

            return action;
        }

        private RemoveUserAction GetRemoveUserAction(int senderId, NetworkStream stream)
        {
            // Id
            int id = ReadInt(stream);

            var action = new RemoveUserAction(_manager.ClientList, _manager.SendList);
            action.User = _manager.ClientList[id];

            return action;
        }

        private void SendMouseMoveAction(MouseMoveAction action, NetworkStream stream)
        {
            // x | y
            WriteInt(action.Location.X, stream);
            WriteInt(action.Location.Y, stream);
        }

        private void SendAddUserAction(AddUserAction action, NetworkStream stream)
        {
            WriteInt(action.User.Id, stream);
            WriteString(action.User.Name, stream);
        }

        private void SendLoadControlBoxAction(LoadControlBoxAction action, NetworkStream stream)
        {
            // ShapeType | start point | end point
            WriteInt((int)action.ShapeType, stream);
            WriteInt(action.StartPoint.X, stream);
            WriteInt(action.StartPoint.Y, stream);
            WriteInt(action.EndPoint.X, stream);
            WriteInt(action.EndPoint.Y, stream);
        }

        private void SendUpdateControlBoxAction(UpdateControlBoxAction action, NetworkStream stream)
        {
            // ShapeType | Visible | start point | end point
            WriteInt((int) action.ShapeType, stream);
            WriteBool(action.ControlVisible, stream);
            WriteInt(action.StartPoint.X, stream);
            WriteInt(action.StartPoint.Y, stream);
            WriteInt(action.EndPoint.X, stream);
            WriteInt(action.EndPoint.Y, stream);
        }

        private void SendAddVertexAction(AddVertexAction action, NetworkStream stream)
        {
            WriteInt(action.Location.X, stream);
            WriteInt(action.Location.Y, stream);
        }

        private void SendRemoveUserAction(RemoveUserAction action, NetworkStream stream)
        {
            // Id
            WriteInt(action.User.Id, stream);
        }

        private void WriteBool(bool b, NetworkStream stream)
        {
            WriteInt(b ? 1 : 0, stream);
        }

        private void WriteInt(int i, NetworkStream stream)
        {
            var buffer = new byte[4];
            buffer = BitConverter.GetBytes(i);
            stream.Write(buffer, 0, 4);
            stream.Flush();
        }

        private void WriteString(string str, NetworkStream stream)
        {
            byte[] strBuffer = Encoding.Unicode.GetBytes(str);
            var buffer = new byte[4];
            buffer = BitConverter.GetBytes(strBuffer.Length);
            // write string lenght
            stream.Write(buffer, 0, 4);
            stream.Flush();
            // write string data
            stream.Write(strBuffer, 0, strBuffer.Length);
            stream.Flush();
        }

        private bool ReadBool(NetworkStream stream)
        {
            var i = ReadInt(stream);
            if (i == 1)
                return true;
            return false;
        }

        private int ReadInt(NetworkStream stream)
        {
            var buffer = new byte[4];
            int nbyte = stream.Read(buffer, 0, 4);
            if (nbyte == 0) return 0;

            return BitConverter.ToInt32(buffer, 0);
        }

        private string ReadString(NetworkStream stream)
        {
            //Read the command's MetaData size.
            int metaDataSize = ReadInt(stream);

            //Read the command's Meta data.
            var buffer = new byte[metaDataSize];
            int nbyte = stream.Read(buffer, 0, metaDataSize);
            if (nbyte == 0)
                return "";

            return System.Text.Encoding.Unicode.GetString(buffer);
        }

        private AppManament _manager;
    }
}
