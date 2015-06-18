#region Copyright (c) 2013 Pham Ngoc Thanh, https://github.com/panoti/DADHMT_LTW/
/**
 * UntiUI - Windows Modern UI for .NET WinForms applications
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
    ///// <summary>
    ///// A ToolStripButton that can display a size and color picker.
    ///// </summary>
    //public class ToolStripOutlineButton : UntiToolStripDropDownButton
    //{
    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="ToolStripOutlineButton"/> class.
    //    /// </summary>
    //    public ToolStripOutlineButton()
    //    {
    //        // Initialize
    //        OutlineWidth = 2;
    //        OutlineColor = Color.Black;
    //        OutlineDash = DashStyle.Solid;
    //        base.Image = GenerateThumbWidthColor(OutlineColor, OutlineWidth, OutlineDash);
    //        // Create size and color picker
    //        _control = new ColorToolControl(OutlineColor, OutlineWidth, OutlineDash);
    //        _control.ColorSelected += control_ColorSelected;

    //        // Add to dropdown list
    //        var dropdown = new ToolStripDropDown();
    //        dropdown.Items.Add(new ToolStripControlHost(_control));

    //        DropDown = dropdown;
    //    }

    //    public event EventHandler OutlineChanged = null;

    //    /// <summary>
    //    /// Gets or sets the width of the outline.
    //    /// </summary>
    //    /// <value>
    //    /// The width of the outline.
    //    /// </value>
    //    public float OutlineWidth { get; set; }

    //    /// <summary>
    //    /// Gets or sets the color of the outline shape.
    //    /// </summary>
    //    /// <value>
    //    /// The color of the outline shape.
    //    /// </value>
    //    public Color OutlineColor { get; set; }

    //    /// <summary>
    //    /// Gets or sets the outline style.
    //    /// </summary>
    //    /// <value>
    //    /// The outline style.
    //    /// </value>
    //    public DashStyle OutlineDash { get; set; }

    //    /// <summary>
    //    /// Gets the screen coordinates, in pixels, of the upper-left corner of the <see cref="T:System.Windows.Forms.ToolStripDropDownItem" />.
    //    /// </summary>
    //    protected override Point DropDownLocation
    //    {
    //        get
    //        {
    //            var dropdownLocation = base.DropDownLocation;
    //            dropdownLocation.X += 10;
    //            return dropdownLocation;
    //        }
    //    }

    //    protected override void OnDropDownClosed(System.EventArgs e)
    //    {
    //        OutlineWidth = _control.SelectedWidth;
    //        OutlineColor = _control.SelectedColor;
    //        OutlineDash = _control.SelectedDash;
    //        base.Image = GenerateThumbWidthColor(OutlineColor, OutlineWidth, OutlineDash);

    //        base.OnDropDownClosed(e);
    //        OnOutlineChanged();
    //    }

    //    private void OnOutlineChanged()
    //    {
    //        if (OutlineChanged != null)
    //            OutlineChanged(this, EventArgs.Empty);
    //    }

    //    private void control_ColorSelected(object sender, System.EventArgs e)
    //    {
    //        DropDown.Close();
    //        OnOutlineChanged();
    //    }

    //    private Image GenerateThumbWidthColor(Color color, float width, DashStyle dash)
    //    {
    //        Image img = new Bitmap(48, 48);

    //        // Draw sample shape
    //        using (var g = Graphics.FromImage(img))
    //        {
    //            g.SmoothingMode = SmoothingMode.AntiAlias;
    //            g.Clear(Color.Transparent);

    //            bool isNoOutline = (Math.Abs(width) < 1.0e-15f);
    //            if (isNoOutline)
    //            {
    //                color = Color.FromArgb(0xcc, 0xcc, 0xcc);
    //                width = 2;
    //            }
    //            using (var b = new SolidBrush(color))
    //            using (var p = new Pen(b, width))
    //            {
    //                p.DashStyle = dash;
    //                g.DrawPath(p, Drawer.CreateLeaf(new Rectangle(5, 9, 38, 30), 20F));
    //            }

    //            if (isNoOutline)
    //                using (var p = new Pen(Color.Red, 5F))
    //                    g.DrawLine(p, 5, 9, 5 + 38, 9 + 30);
    //        }
    //        return img;
    //    }

    //    private readonly ColorToolControl _control;
    //}

    /// <summary>
    /// A ToolStripButton that can display a size and color picker.
    /// </summary>
    public class ToolStripOutlineButton : UntiToolStripButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolStripOutlineButton"/> class.
        /// </summary>
        public ToolStripOutlineButton()
        {
            // Initialize
            _outlineWidth = 2;
            _outlineColor = Color.Black;
            _outlineDash = DashStyle.Solid;
            base.Image = GenerateThumbWidthColor(_outlineColor, _outlineWidth, _outlineDash);
        }

        public event EventHandler OutlineChanged = null;

        /// <summary>
        /// Gets or sets the width of the outline.
        /// </summary>
        /// <value>
        /// The width of the outline.
        /// </value>
        public float OutlineWidth
        {
            get { return _outlineWidth; }
            set
            {
                _outlineWidth = value;
                OnOutlineChanged();
            }
        }

        /// <summary>
        /// Gets or sets the color of the outline shape.
        /// </summary>
        /// <value>
        /// The color of the outline shape.
        /// </value>
        public Color OutlineColor {
            get { return _outlineColor; }
            set
            {
                _outlineColor = value;
                OnOutlineChanged();
            }
        }

        /// <summary>
        /// Gets or sets the outline style.
        /// </summary>
        /// <value>
        /// The outline style.
        /// </value>
        public DashStyle OutlineDash { 
            get { return _outlineDash; }
            set
            {
                _outlineDash = value;
                OnOutlineChanged();
            }
        }

        private void OnOutlineChanged()
        {
            base.Image = GenerateThumbWidthColor(_outlineColor, _outlineWidth, _outlineDash);

            if (OutlineChanged != null)
                OutlineChanged(this, EventArgs.Empty);
        }

        private Image GenerateThumbWidthColor(Color color, float width, DashStyle dash)
        {
            Image img = new Bitmap(48, 48);

            // Draw sample shape
            using (var g = Graphics.FromImage(img))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                bool isNoOutline = (Math.Abs(width) < 1.0e-15f);
                if (isNoOutline)
                {
                    color = Color.FromArgb(0xcc, 0xcc, 0xcc);
                    width = 2;
                }
                using (var b = new SolidBrush(color))
                using (var p = new Pen(b, width))
                {
                    p.DashStyle = dash;
                    g.DrawPath(p, Drawer.CreateLeaf(new Rectangle(5, 9, 38, 30), 20F));
                }

                if (isNoOutline)
                    using (var p = new Pen(Color.Red, 5F))
                        g.DrawLine(p, 5, 9, 5 + 38, 9 + 30);
            }
            return img;
        }

        private Color _outlineColor;
        private float _outlineWidth;
        private DashStyle _outlineDash;
    }
}
