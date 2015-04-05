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
using System.Windows.Forms;

namespace Untipic.UI.Net.MetroUI
{
    public class MetroButton: Button
    {
        public MetroButton()
        {
            // Init default field's value
            HoverColor = Color.FromArgb(0xe6, 0xe6, 0xe6);
            PressColor = Color.FromArgb(0x4d, 0x4d, 0x4d);

            // Set serveral option for paint
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint, true);

        }

        public Color HoverColor
        {
            get { return _hoverColor; }
            set { _hoverColor = value; }
        }

        public Color PressColor
        {
            get { return _pressColor; }
            set { _pressColor = value; }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var back = GetEffectiveColor();

            var fore = (back.R > 128 && back.G > 128 && back.B > 128) ? ForeColor : Color.White;
            //var fore = ( ? ForeColor, ColorForm)
            pevent.Graphics.Clear(back);

            if (Image == null)
            TextRenderer.DrawText(pevent.Graphics, Text, Font, ClientRectangle,
                fore, back, 
                TextAlign.AsTextFormatFlags() | TextFormatFlags.EndEllipsis);

            if (Image != null)
                pevent.Graphics.DrawImage(Image, (Width - Image.Width) / 2, (Height - Image.Height) / 2, 
                    Image.Width, Image.Height);
            //using (var b = new SolidBrush(Color.FromArgb(255 - back.R, 255 - back.G, 255 - back.B)))
            //{
            //    var p = new PointF();
            //    p.Y = (float)(Height - Font.Height)/2;
            //    pevent.Graphics.DrawString(Text, Font, b, p);
            //}
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovered = true;
            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isPressed = true;
                Invalidate();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _isPressed = false;
            Invalidate();

            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovered = false;
            Invalidate();

            base.OnMouseLeave(e);
        }

        private Color GetEffectiveColor()
        {
            if (_isPressed)
                return _pressColor;
            if (_isHovered)
                return _hoverColor;

            return BackColor;
        }

        private bool _isHovered;
        private bool _isPressed;

        private Color _hoverColor;
        private Color _pressColor;
    }
}
