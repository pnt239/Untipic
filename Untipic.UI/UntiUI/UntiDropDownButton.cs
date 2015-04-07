using System;
using System.Drawing;
using System.Windows.Forms;

namespace Untipic.UI.UntiUI
{
    public class UntiDropDownButton : UntiButton
    {
        public UntiDropDownButton()
        {
            Direction = DockStyle.Left;
        }

        public ToolStripDropDown DropDown { get; set; }

        public DockStyle Direction { get; set; }

        protected override void OnClick(EventArgs e)
        {
            if (DropDown != null)
                DropDown.Show(GetPostionDropDown());
            base.OnClick(e);
        }

        private Point GetPostionDropDown()
        {
            var p = Parent.PointToScreen(Location);
            switch (Direction)
            {
                case DockStyle.Left:
                    p.X -= DropDown.GetPreferredSize(Size.Empty).Width + 10;
                    break;
            }

            return p;
        }
    }
}
