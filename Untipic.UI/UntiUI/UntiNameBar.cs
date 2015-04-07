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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Untipic.Controls
{
    public partial class MetroNameBar : UserControl
    {
        public MetroNameBar()
        {
            InitializeComponent();

            _projectName = "Untitled";
            _isHover = false;
            txtName.Text = lbName.Text = _projectName;

            // Set serveral option for paint
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint, true);
        }

        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var y = (Height - txtName.Height) / 2;
            txtName.Top = y;

            lbName.Left = txtName.Left - 3;
            lbName.Top = y;

            btnYes.Top = y;
            btnYes.Left = txtName.Left + txtName.Width + 5;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(BackColor);

            if (_isHover)
            {
                var rec = new Rectangle(txtName.Left - 4, txtName.Top - 1, lbName.Width + 1, txtName.Height);
                using (var p = new Pen(Color.FromArgb(0xcc, 0xcc, 0xcc), 1F))
                    e.Graphics.DrawRectangle(p, rec);
            }
        }

        private void lbName_Click(object sender, EventArgs e)
        {
            SwitchVisiable();
            txtName.Focus();
        }

        private void lbName_MouseEnter(object sender, EventArgs e)
        {
            _isHover = true;
            Invalidate();
        }

        private void lbName_MouseLeave(object sender, EventArgs e)
        {
            _isHover = false;
            Invalidate();
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                SwitchVisiable();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            lbName.Text = _projectName = txtName.Text;
        }

        private void SwitchVisiable()
        {
            var curState = lbName.Visible;

            txtName.Visible = curState;
            btnYes.Visible = curState;
            lbName.Visible = !curState;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            SwitchVisiable();
        }

        private bool _isHover;
        private string _projectName;
    }
}
