using System.Drawing;

namespace Untipic.Business.Action
{
    public class MouseMoveAction : IAction
    {
        public MouseMoveAction()
        {
            Visible = false;
            RePaint = true;
            IsToAll = true;
        }

        public UserInfo User { get; set; }

        public Point Location { get; set; }

        public int ReceiverId { get; set; }
        public int SenderId { get; set; }
        public bool Visible { get; set; }

        public bool RePaint { get; set; }

        public bool IsToAll { get; set; }

        public ActionType GetActionType()
        {
            return ActionType.MouseMove;
        }

        //public byte[] ToBytes()
        //{
        //    throw new NotImplementedException();
        //}

        public void Execute()
        {
            User.MouseLocation = Location;
        }

        //public void UnExecute()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
