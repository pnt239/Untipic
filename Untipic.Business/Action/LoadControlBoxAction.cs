using System.Drawing;
using Untipic.Entity;

namespace Untipic.Business.Action
{
    public class LoadControlBoxAction : IAction
    {
        public LoadControlBoxAction()
        {
            _control = null;

            Visible = false;
            RePaint = true;
            IsToAll = true;
        }

        public LoadControlBoxAction(DrawingControl control) : this()
        {
            _control = control;
        }

        public ShapeType ShapeType { get; set; }
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public int ReceiverId { get; set; }
        public int SenderId { get; set; }
        public bool Visible { get; set; }
        public bool RePaint { get; set; }
        public bool IsToAll { get; set; }
        public ActionType GetActionType()
        {
            return ActionType.LoadControlBox;
        }

        public void Execute()
        {
            _control.UpdateControl(ShapeType, StartPoint, EndPoint);
            _control.Visible = false;
        }

        private DrawingControl _control;
    }
}
