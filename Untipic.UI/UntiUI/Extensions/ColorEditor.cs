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
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Untipic.Presentation;

namespace Untipic.UI.UntiUI.Extensions
{
    public class ColorEditor : Control, IColorEditor
    {
        public ColorEditor()
        {
            Initialize();
        }

        /// <summary>
        /// Occurs when the Color property value changes
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler ColorChanged;

        /// <summary>
        /// Gets or sets the component color.
        /// </summary>
        /// <value>The component color.</value>
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "0, 0, 0")]
        public virtual Color Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;

                    if (!LockUpdates)
                    {
                        LockUpdates = true;
                        HslColor = new HslColor(value);
                        LockUpdates = false;
                        UpdateFields();
                    }
                    else
                    {
                        OnColorChanged(EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the component color as a HSL structure.
        /// </summary>
        /// <value>The component color.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual HslColor HslColor
        {
            get { return _hslColor; }
            set
            {
                if (HslColor != value)
                {
                    _hslColor = value;

                    if (!LockUpdates)
                    {
                        LockUpdates = true;
                        Color = value.ToRgbColor();
                        LockUpdates = false;
                        UpdateFields();
                    }
                    else
                    {
                        OnColorChanged(EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether input changes should be processed.
        /// </summary>
        /// <value><c>true</c> if input changes should be processed; otherwise, <c>false</c>.</value>
        protected bool LockUpdates { get; set; }

        /// <summary>
        /// Raises the <see cref="ColorChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnColorChanged(EventArgs e)
        {
            UpdateFields();

            var handler = ColorChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Updates the editing field values.
        /// </summary>
        protected virtual void UpdateFields()
        {
            if (!LockUpdates)
            {
                try
                {
                    LockUpdates = true;

                    // Hex
                    if (_ckbHex.CheckState == CheckState.Checked)
                    {
                        _txtColor.Text = string.Format("#{0:X2}{1:X2}{2:X2}", Color.R, Color.G, Color.B);
                    }
                    else if (_ckbHex.CheckState == CheckState.Unchecked)
                    {
                        _txtColor.Text = string.Format("{0}, {1}, {2}", Color.R, Color.G, Color.B);
                    }
                }
                finally
                {
                    LockUpdates = false;
                }
            }
        }

        private void Initialize()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            _tlpMain = new TableLayoutPanel();
            _ckbHex = new CheckBox();
            _txtColor = new TextBox();

            _tlpMain.SuspendLayout();
            SuspendLayout();

            // 
            // _tlpMain
            // 
            _tlpMain.ColumnCount = 2;
            _tlpMain.ColumnStyles.Add(new ColumnStyle());
            _tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpMain.Controls.Add(_ckbHex, 0, 0);
            _tlpMain.Controls.Add(_txtColor, 1, 0);
            _tlpMain.Dock = DockStyle.Fill;
            _tlpMain.Location = new Point(3, 147);
            _tlpMain.Name = "_tlpColorInput";
            _tlpMain.RowCount = 1;
            _tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpMain.Size = new Size(198, 34);
            _tlpMain.TabIndex = 1;
            // 
            // _ckbHex
            // 
            _ckbHex.AutoSize = true;
            _ckbHex.Location = new Point(3, 3);
            _ckbHex.Name = "_ckbHex";
            _ckbHex.Size = new Size(48, 21);
            _ckbHex.TabIndex = 0;
            _ckbHex.Text = @"Hex";
            _ckbHex.UseVisualStyleBackColor = true;
            // 
            // _txtColor
            // 
            _txtColor.Dock = DockStyle.Fill;
            _txtColor.Location = new Point(57, 3);
            _txtColor.Name = "_txtColor";
            _txtColor.Size = new Size(138, 25);
            _txtColor.TabIndex = 1;
            _txtColor.TextChanged += TxtColor_TextChanged;

            //
            // This Control
            //
            BackColor = Color.Transparent;
            Controls.Add(_tlpMain);
            Font = new Font("Segoe UI Light", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(3, 4, 3, 4);
            Name = "ColorEditor";

            _tlpMain.ResumeLayout(false);
            _tlpMain.PerformLayout();
            ResumeLayout(false);
        }

        private void TxtColor_TextChanged(object sender, EventArgs e)
        {
            if (!LockUpdates)
            {
                LockUpdates = true;
                // Process text color
                var regex = _ckbHex.CheckState == CheckState.Checked
                    ? new Regex(@"#[A-F0-9]{2}[A-F0-9]{2}[A-F0-9]{2}")
                    : new Regex(@"\d{1,3}, \d{1,3}, \d{1,3}");
                if (regex.IsMatch(_txtColor.Text))
                {
                    int r, g, b;

                    if (_txtColor.Text.StartsWith("#"))
                    {
                        r =  Convert.ToInt32(_txtColor.Text.Substring(1, 2), 16);
                        g = Convert.ToInt32(_txtColor.Text.Substring(3, 2), 16);
                        b = Convert.ToInt32(_txtColor.Text.Substring(5, 2), 16);
                    }
                    else
                    {
                        var parts = _txtColor.Text.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                        r = int.Parse(parts[0]);
                        g = int.Parse(parts[1]);
                        b = int.Parse(parts[2]);
                    }

                    Color = Color.FromArgb(r, g, b);
                    HslColor = new HslColor(_color);
                }
                LockUpdates = false;
            }
        }

        private TableLayoutPanel _tlpMain;
        private CheckBox _ckbHex;
        private TextBox _txtColor;

        private Color _color;
        private HslColor _hslColor;
    }
}
