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

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Untipic.Presentation;

namespace Untipic.UI.UntiUI.Extensions
{
    /// <summary>
    /// Color will return when click at button
    /// </summary>
    public class ColorSelectorButton : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSelectorButton"/> class.
        /// </summary>
        public ColorSelectorButton()
        {
            Initialize();
        }

        /// <summary>
        /// Default size of control
        /// </summary>
        protected override Size DefaultSize
        {
            get { return new Size(24, 24); }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            RecreatePath();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.BackColorChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);

            _borderColor = new HslColor(BackColor);
            _borderColor.L -= 0.1F;
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnPaint(System.Windows.Forms.PaintEventArgs)" /> event.
        /// </summary>
        /// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            pevent.Graphics.Clear(Color.White);

            using (var b = new SolidBrush(_borderColor))
            using (var p = new Pen(b, 2F))
            {
                using (var b1 = new SolidBrush(BackColor))
                    pevent.Graphics.FillPath(b1, _pathLeaf);
                pevent.Graphics.DrawPath(p, _pathLeaf);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _borderColor.L -= 0.1F;
            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _borderColor.L += 0.1F;
            Invalidate();

            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            // Set serveral option for paint
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint, true);

            _borderColor = new HslColor(BackColor);
            _borderColor.L -= 0.1F;

            RecreatePath();
        }

        private void RecreatePath()
        {
            var rec = new Rectangle(1, 1, Size.Width - 2, Size.Height - 2);
            _pathLeaf = Drawer.CreateLeaf(rec, CalculateRound(rec));
            //Region = new Region(_pathLeaf);
        }

        private float CalculateRound(Rectangle rec)
        {
            return 2*rec.Height*0.2F;
        }

        /// <summary>
        /// The leaf bound of button
        /// </summary>
        private GraphicsPath _pathLeaf;

        private HslColor _borderColor;
    }
}
