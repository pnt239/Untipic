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
    public class ToolStripFillButton : UntiToolStripDropDownButton
    {
        public ToolStripFillButton()
        {
            FillColor = Color.Transparent;
            base.Image = GenerateThumbWidthColor(FillColor);

            // Create size and color picker
            _control = new ColorToolControl(FillColor, 0, DashStyle.Solid, true);
            _control.ColorSelected += control_ColorSelected;

            // Add to dropdown list
            var dropdown = new ToolStripDropDown();
            dropdown.Items.Add(new ToolStripControlHost(_control));

            DropDown = dropdown;
        }

        public event EventHandler FillChanged = null;

        public Color FillColor { get; set; }

        protected override void OnDropDownClosed(EventArgs e)
        {
            FillColor = _control.SelectedColor;
            Image = GenerateThumbWidthColor(FillColor);
            base.OnDropDownClosed(e);
            OnOutlineChanged();
        }

        protected override Point DropDownLocation
        {
            get
            {
                var dropdownLocation = base.DropDownLocation;
                dropdownLocation.X += 10;
                return dropdownLocation;
            }
        }

        private void OnOutlineChanged()
        {
            if (FillChanged != null)
                FillChanged(this, EventArgs.Empty);
        }

        private void control_ColorSelected(object sender, EventArgs e)
        {
            DropDown.Close();
            OnOutlineChanged();
        }

        private Image GenerateThumbWidthColor(Color colorfill)
        {
            Image img = new Bitmap(48, 48);

            // Draw sample shape
            using (var g = Graphics.FromImage(img))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                bool isNoFill = (colorfill.A == 0);
                if (isNoFill)
                    colorfill = Color.FromArgb(0xcc, 0xcc, 0xcc);

                using (var b = new SolidBrush(colorfill))
                {
                    var path = Drawer.CreateLeaf(new Rectangle(5, 9, 38, 30), 20F);
                    g.FillPath(b, path);

                    var borderColor = new HslColor(colorfill);
                    borderColor.L -= 0.1F;

                    using (var p = new Pen(borderColor.ToRgbColor(), 2F))
                        g.DrawPath(p, path);

                    if (isNoFill)
                    {
                        using (var p = new Pen(Color.Red, 5F))
                            g.DrawLine(p, 5, 9, 5 + 38, 9 + 30);
                    }
                }
            }
            return img;
        }

        private readonly ColorToolControl _control;
    }
}
