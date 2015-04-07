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
using System.Windows.Forms;
using Untipic.Entity;
using Untipic.UI.UntiUI.DrawPad;
using Untipic.UI.UntiUI.EventArguments;

namespace Untipic.UI.UntiUI.Extensions
{
    /// <summary>
    /// Use for select shape to draw
    /// </summary>
    public class ShapeSelectorControl : Control
    {
        public ShapeSelectorControl()
        {
            // Initialize
            Initialize();
        }

        /// <summary>
        /// Occurs when [shape selected].
        /// </summary>
        public event EventHandler ShapeSelected
        {
            add { Events.AddHandler(EventShapeSelected, value); }
            remove { Events.RemoveHandler(EventShapeSelected, value); }
        }

        /// <summary>
        /// Raises the <see cref="ShapeSelected" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ShapeToolEventArgs"/> instance containing the event data.</param>
        protected internal virtual void OnShapeSelected(ShapeToolEventArgs e)
        {
            var handler = (EventHandler)Events[EventShapeSelected];
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            //  Set size of control. A icon is 48x48 px
            Size = new Size(240, 48);
            var img = new Image[]
            {
                Properties.Resources.Line,
                Properties.Resources.Triangle,
                Properties.Resources.Quad,
                Properties.Resources.Polygon,
                Properties.Resources.Ellipse
            };

            var command = new[]
            {
                new CommandObject(DrawPadCommand.DrawShape,
                    new Line()),
                new CommandObject(DrawPadCommand.DrawShape,
                    new IsoscelesTriangle()),
                new CommandObject(DrawPadCommand.DrawShape,
                    new Oblong()),
                new CommandObject(DrawPadCommand.DrawShape,
                    new Polygon {DrawMethod = DrawMethod.ByClick}),
                new CommandObject(DrawPadCommand.DrawShape,
                    new Ellipse())
            };

            // Initialize control
            _lytMain = new TableLayoutPanel();
            _btnTools = new UntiButton[5]; // have 6 tools
            _lytMain.SuspendLayout();
            SuspendLayout();

            // 
            // btnTools
            // 
            for (int i = 0; i < 5; i++)
            {
                _btnTools[i] = new UntiButton
                {
                    Dock = DockStyle.Fill,
                    HoverColor = Color.FromArgb(230, 230, 230),
                    PressColor = Color.FromArgb(77, 77, 77),
                    Image = img[i],
                    Location = new Point(3, 3),
                    Size = new Size(42, 42),
                    TabIndex = i,
                    Text = String.Format("Tool{0}", i),
                    Name = String.Format("btnTool{0}", i),
                    Tag = command[i],
                    UseVisualStyleBackColor = true
                };

                _btnTools[i].Click += Tools_Click;
            }

            // 
            // lytMain
            // 
            _lytMain.ColumnCount = 5;
            for (int i = 0; i < 5; i++)
            {
                _lytMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 48F));
                _lytMain.Controls.Add(_btnTools[i], i, 0);
            }
            _lytMain.RowCount = 1;
            _lytMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _lytMain.Dock = DockStyle.Fill;
            _lytMain.Margin = new Padding(0);
            _lytMain.Location = new Point(2, 2);
            _lytMain.Size = new Size(240, 48);
            _lytMain.TabIndex = 3;
            _lytMain.Name = "lytMain";

            // 
            // This Control
            // 
            BackColor = Color.White;
            Controls.Add(_lytMain);
            _lytMain.ResumeLayout(false);
            ResumeLayout(false);
        }

        /// <summary>
        /// Handles the Click event of the Tools control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Tools_Click(object sender, EventArgs e)
        {
            var btn = (UntiButton) sender;
            OnShapeSelected(new ShapeToolEventArgs((CommandObject)btn.Tag));
        }

        /// <summary>
        /// The event shape selected
        /// </summary>
        private static readonly object EventShapeSelected = new object();

        /// <summary>
        /// The button tools
        /// </summary>
        private UntiButton[] _btnTools;

        /// <summary>
        /// The main table layout
        /// </summary>
        private TableLayoutPanel _lytMain;
    }
}
