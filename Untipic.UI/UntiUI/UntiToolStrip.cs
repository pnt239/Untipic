#region Copyright (c) 2013 Pham Ngoc Thanh, https://github.com/panoti/DADHMT_LTW/
/**
 * MetroUI - Windows Modern UI for .NET WinForms applications
 * Copyright (c) 2014 Pham Ngoc Thanh, https://github.com/panoti/DADHMT_LTW/
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation the rights to use, copy, 
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 */
#endregion

using System.Drawing;
using System.Windows.Forms;

namespace Untipic.UI.UntiUI
{
    public class UntiToolStripRenderer : ToolStripProfessionalRenderer
    {
        public UntiToolStripRenderer()
        {
            Initialize();
        }

        public float BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; }
        }


        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            base.OnRenderToolStripBackground(e);

            using (var b = new SolidBrush(e.BackColor))
            {
                e.Graphics.FillRectangle(b, e.AffectedBounds);
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            //base.OnRenderToolStripBorder(e);

            if (e.ToolStrip.Dock == DockStyle.Fill)
                return;

            using (var b = new SolidBrush(Color.FromArgb(0xcc, 0xcc, 0xcc)))
            using (var p = new Pen(b, _borderWidth))
            {
                if (e.ToolStrip.Dock != DockStyle.Top)
                    e.Graphics.DrawLine(p, 0, 0, e.AffectedBounds.Right, 0);

                e.Graphics.DrawLine(p, e.AffectedBounds.Right - 1, 0, e.AffectedBounds.Right - 1, e.AffectedBounds.Bottom);
                e.Graphics.DrawLine(p, 0, e.AffectedBounds.Bottom - 1, e.AffectedBounds.Right, e.AffectedBounds.Bottom - 1);
                //e.Graphics.DrawRectangle(p, 0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
            }
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderButtonBackground(e);

            var g = e.Graphics;

            var bounds = new Rectangle(Point.Empty, e.Item.Size);

            Color backColor = Color.White;
            
            var button = e.Item as ToolStripButton;
            // Check button is null value
            if (button == null) return;

            if (button.Pressed || button.Checked)
            {
                backColor = Color.FromArgb(0x4d, 0x4d, 0x4d);
            }
            else if (button.Selected)
            {
                backColor = Color.FromArgb(0xe6, 0xe6, 0xe6);
            }

            using (Brush b = new SolidBrush(backColor))
            {
                g.FillRectangle(b, bounds);
            }
        }

        private void Initialize()
        {
            _borderWidth = 5;
            RoundedEdges = false;
        }

        private float _borderWidth;
    }

    public class MetroToolStrip : ToolStrip
    {
        public MetroToolStrip()
        {
            Renderer = new UntiToolStripRenderer();
        }
    }
}
