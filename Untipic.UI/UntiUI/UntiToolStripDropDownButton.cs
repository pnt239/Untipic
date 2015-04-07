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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Untipic.UI.Net.UntiUI
{
    public class UntiToolStripDropDownButton : UntiToolStripButton
    {
        public UntiToolStripDropDownButton()
        {
            IsDropDownButton = true;
        }

        public event EventHandler DropDownOpening;
        public event EventHandler DropDownClosed;
        public event EventHandler DropDownOpened;
        public event ToolStripItemClickedEventHandler DropDownItemClicked;

        public ToolStripDropDown DropDown
        {
            get
            {
                if (_dropDown == null)
                {
                    DropDown = CreateDefaultDropDown();

                    if (Parent != null)
                    {
// ReSharper disable once PossibleNullReferenceException
                        _dropDown.ShowItemToolTips = Parent.ShowItemToolTips;
                    }
                }
                return _dropDown;
            }
            set
            {
                if (_dropDown != value) // prevent duplicate
                {
                    if (_dropDown != null) // Remove old handle event
                    {
                        _dropDown.Opened -= DropDown_Opened;
                        _dropDown.Closed -= DropDown_Closed;
                        _dropDown.ItemClicked -= DropDown_ItemClicked;
                    }

                    _dropDown = value;
                    _dropDown.OwnerItem = this;

                    if (_dropDown != null) // Add new handle event
                    {
                        _dropDown.Opened += DropDown_Opened;
                        _dropDown.Closed += DropDown_Closed;
                        _dropDown.ItemClicked += DropDown_ItemClicked;
                    }

                }
            }
        }

        [Browsable(false)]
        public virtual bool HasDropDownItems
        {
            get { return _dropDown != null && _dropDown.Items.Count != 0; }
        }

        protected virtual Point DropDownLocation
        {
            get
            {
                Point p;

                if (IsDropDownButton)
                {
                    p = Parent.PointToScreen(new Point(Bounds.Left, Bounds.Top - 1));
                    p.X += Bounds.Width;
                    p.Y += Bounds.Left;
                    return p;
                }
                p = new Point(Bounds.Left, Bounds.Bottom - 1);

                return Parent.PointToScreen(p);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            //base.OnMouseDown(e);
            //MessageBox.Show(this.Owner.Name);
            if (e.Button == MouseButtons.Left)
            {
                //Checked = true;

                if (DropDown.Visible)
                    HideDropDown(ToolStripDropDownCloseReason.ItemClicked);
                else
                    ShowDropDown();

                PerformClick();
            }
        }

        protected virtual void OnDropDownHide(EventArgs e)
        {
            Invalidate();
        }

        protected virtual void OnDropDownShow(EventArgs e)
        {
            if (DropDownOpening != null)
            {
                DropDownOpening(this, e);
            }
        }

        protected virtual void OnDropDownOpened(EventArgs e)
        {
            // only send the event if we're the thing that currently owns the DropDown.

            if (DropDown.OwnerItem == this)
            {
                if (DropDownOpened != null) DropDownOpened(this, e);
            }
        }

        protected virtual void OnDropDownClosed(EventArgs e)
        {
            // only send the event if we're the thing that currently owns the DropDown.
            Invalidate();

            if (DropDown.OwnerItem == this)
            {
                if (DropDownClosed != null) DropDownClosed(this, e);

                if (!DropDown.IsAutoGenerated)
                {
                    DropDown.OwnerItem = null;
                }
            }

        }

        protected virtual void OnDropDownItemClicked(ToolStripItemClickedEventArgs e)
        {
            // only send the event if we're the thing that currently owns the DropDown.

            if (DropDown.OwnerItem == this)
            {
                if (DropDownItemClicked != null) DropDownItemClicked(this, e);
            }
        }

        private void DropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            OnDropDownClosed(EventArgs.Empty);
        }

        private void DropDown_Opened(object sender, EventArgs e)
        {
            OnDropDownOpened(EventArgs.Empty);
        }

        private void DropDown_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            OnDropDownItemClicked(e);
        }

        private ToolStripDropDown CreateDefaultDropDown()
        {
            var tsdd = new ToolStripDropDownMenu {OwnerItem = this};
            return tsdd;
        }

        private void ShowDropDown()
        {
            // Don't go through this whole deal if
            // the DropDown is already visible
            if (DropDown.Visible)
                return;

            // Call this before the HasDropDownItems check to give
            // users a chance to handle it and add drop down items
            OnDropDownShow(EventArgs.Empty);

            if (!HasDropDownItems)
                return;

            Invalidate();
            DropDown.OwnerItem = this;
            DropDown.Show(DropDownLocation);
        }

        public void HideDropDown(ToolStripDropDownCloseReason reason)
        {
            if (_dropDown == null || !DropDown.Visible)
                return;

            // OnDropDownHide is called before actually closing DropDown
            OnDropDownHide(EventArgs.Empty);
            DropDown.Close(reason);
            Invalidate();
        }

        private ToolStripDropDown _dropDown;
    }
}
