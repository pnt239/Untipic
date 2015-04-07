using System.Collections.Generic;

namespace Untipic.Business.Action
{
    public class RemoveUserAction : IAction
    {
        public RemoveUserAction()
        {
            _list = new Dictionary<int, UserInfo>();
            _sendList = new Dictionary<int, Queue<IAction>>();
            Visible = false;
            RePaint = false;
            IsToAll = false;
        }

        public RemoveUserAction(Dictionary<int, UserInfo> list, Dictionary<int, Queue<IAction>> sendList)
            : this()
        {
            _list = list;
            _sendList = sendList;
        }

        public UserInfo User { get; set; }

        public int ReceiverId { get; set; }
        public int SenderId { get; set; }
        public bool Visible { get; set; }
        public bool RePaint { get; set; }
        public bool IsToAll { get; set; }
        public ActionType GetActionType()
        {
            return ActionType.RemoveUser;
        }

        public void Execute()
        {
            _list.Remove(User.Id);
            _sendList.Remove(User.Id);
        }

        private readonly Dictionary<int, UserInfo> _list;
        private readonly Dictionary<int, Queue<IAction>> _sendList;
    }
}
