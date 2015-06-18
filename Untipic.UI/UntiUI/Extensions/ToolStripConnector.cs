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
using System.Windows.Forms;
using Untipic.UI.UntiUI.DrawPad;

namespace Untipic.UI.UntiUI.Extensions
{
    public class ToolStripConnector
    {
        public ToolStripConnector()
        {
            _actionList = new Dictionary<UntiToolStripButton, DrawPadAction>();
            _lastItem = null;
            _isToggle = false;
        }

        public ToolStripConnector(bool useToggle) : this()
        {
            _isToggle = useToggle;
        }

        public void Connect(UntiToolStripButton item, DrawPadAction action)
        {
            item.MouseDown += Item_MouseDown;
            if (item.IsDropDownButton)
                ((UntiToolStripDropDownButton)item).DropDownItemClicked += Item_DropDownItemClicked;
            _actionList.Add(item, action);
        }

        public void Connect(UntiToolStripButton item, CommandObject command, DrawPadAction action)
        {
            item.Tag = command;
            Connect(item, action);
        }

        public void Select(UntiToolStripButton item)
        {
            // Get command from control
            var command = item.Tag as CommandObject;
            if (command == null) return;

            var action = _actionList[item];
            if (action != null)
                action(command);

            // Toggle proccessing
            if (_lastItem != null)
                _lastItem.Checked = false;

            _lastItem = item;
            if (_lastItem != null) _lastItem.Checked = true;
        }

        private void Item_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Item_MouseDown(sender, e);
        }

        private void Item_MouseDown(object sender, EventArgs e)
        {
            // Get MetroToolStripButton
            var ctrl = sender as UntiToolStripButton;
            if (ctrl == null) return;

            // Get command from control
            var command = ctrl.Tag as CommandObject;
            if (command == null) return;

            // Do action
            var action = _actionList[ctrl];
            if (action != null)
                action(command);

            if (!_isToggle)
                return;

            // Toggle proccessing
            if (_lastItem != null)
                _lastItem.Checked = false;

            _lastItem = sender as UntiToolStripButton;
            if (_lastItem != null) _lastItem.Checked = true;
        }

        private readonly Dictionary<UntiToolStripButton, DrawPadAction> _actionList;
        private UntiToolStripButton _lastItem;
        private readonly bool _isToggle;
    }
}
