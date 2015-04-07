using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Untipic.Controls
{
    public class GdiArea : Control
    {
        public GdiArea()
        {
            base.BackColor = Color.White;
            base.Margin = new Padding(0);

            // Set serveral option for paint
            SetStyle(
                ControlStyles.Selectable | 
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint, true);
        }

        protected override Padding DefaultMargin
        {
            get
            {
                return new Padding(0);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);

            using (var p = new Pen(Color.FromArgb(198, 198, 198)))
                pevent.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
        }
    }
}
