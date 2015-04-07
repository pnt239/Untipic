using System.Drawing;

namespace Untipic.Business.Action
{
    public class AddVertexAction : IAction
    {
        public AddVertexAction(DrawingControl control)
        {
            _control = control;
            Visible = false;
            RePaint = true;
            IsToAll = true;
        }

        public Point Location { get; set; }

        public int ReceiverId { get; set; }
        public int SenderId { get; set; }
        public bool Visible { get; set; }
        public bool RePaint { get; set; }
        public bool IsToAll { get; set; }

        public ActionType GetActionType()
        {
            return ActionType.AddVertex;
        }

        public void Execute()
        {
            _control.CreateVertext(Location);
            _control.Visible = true;
        }

        private DrawingControl _control;
    }
}
