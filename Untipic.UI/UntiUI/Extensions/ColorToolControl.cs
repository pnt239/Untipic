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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Untipic.UI.UntiUI.Extensions
{
    /// <summary>
    /// Use for adjust size and select color to draw or fill the shape
    /// </summary>
    public class ColorToolControl : Control
    {
        private sealed class TrackBarSync
        {
            public TrackBarSync(UntiTrackBar mytrackbar, Control assistant)
            {
                _trackbars = new List<UntiTrackBar>();
                _mytrackbar = mytrackbar;
                _mytrackbar.ValueChanged += _mytrackbar_ValueChanged;
                _assistant = assistant;
            }

            public event EventHandler ValueChanged = null;

            public int Value
            {
                get { return _mytrackbar.Value; }
            }

            public void Add(UntiTrackBar trackbar)
            {
                _trackbars.Add(trackbar);
            }

            private void OnValueChanged(EventArgs e)
            {
                if (ValueChanged != null)
                    ValueChanged(this, e);
            }

            void _mytrackbar_ValueChanged(object sender, EventArgs e)
            {
                _assistant.Text = ConvertWidthToString(_mytrackbar.Value);
                foreach (var trb in _trackbars)
                    trb.Value = _mytrackbar.Value;

                OnValueChanged(e);
            }

            private readonly List<UntiTrackBar> _trackbars;
            private readonly UntiTrackBar _mytrackbar;
            private readonly Control _assistant;
        }

        public ColorToolControl(bool fillMode = false)
        {
            _selectedColor = Color.Black;
            _selectedWidth = 2F;
            _selectedDash = DashStyle.Solid;

            InitializeComponent(fillMode);
        }

        public ColorToolControl(Color color, float width, DashStyle dash, bool fillMode = false)
        {
            _selectedColor = color;
            _selectedWidth = width;
            _selectedDash = dash;

            InitializeComponent(fillMode);
        }

        /// <summary>
        /// Gets or sets the selected color.
        /// </summary>
        /// <value>
        /// The selected color.
        /// </value>
        public Color SelectedColor
        {
            get { return _selectedColor; }
            set { _selectedColor = value; }
        }

        /// <summary>
        /// Gets or sets the selected width.
        /// </summary>
        /// <value>
        /// The selected width.
        /// </value>
        public float SelectedWidth
        {
            get { return _selectedWidth; }
            set { _selectedWidth = value; }
        }

        /// <summary>
        /// Gets or sets the selected dash.
        /// </summary>
        /// <value>
        /// The selected dash.
        /// </value>
        public DashStyle SelectedDash
        {
            get { return _selectedDash; }
            set { _selectedDash = value; }
        }

        public event EventHandler ColorSelected = null;

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var p1 = new Point(2, _llbSwitchAdvance.Top + _llbSwitchAdvance.Height);
            var p2 = new Point(2 + _llbSwitchAdvance.Width, _llbSwitchAdvance.Top + _llbSwitchAdvance.Height);

            using (var b = new SolidBrush(Color.FromArgb(0xcc, 0xcc, 0xcc)))
            using (var p = new Pen(b, 1F))
            {
                e.Graphics.DrawLine(p, p1, p2);

                p2.Y = p1.Y += _isBasic ? 120 : 190;
                e.Graphics.DrawLine(p, p1, p2);
            }
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitializeComponent(bool fillMode)
        {
            // Some fill must initialize before begin
            _colorTable = new[]
            {
                Color.FromArgb(0xff, 0, 0), Color.FromArgb(0xff, 0x80, 0x17),
                Color.FromArgb(0xff, 0xf7, 0x0), Color.FromArgb(0, 0xe8, 0),
                Color.FromArgb(0x19, 0x77, 0xff), Color.FromArgb(0xfc, 0x0c, 0x59),
                Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0, 0, 0)
            };

            _isBasic = true;

            _mpnMain = new MultiPanel();
            // Basic initialize
            _pgeBasic = new MultiPanelPage();
            _tlpBasic = new TableLayoutPanel();
            _llbSwitchAdvance = new LinkLabel();
            _tlpBasicColorTable = new TableLayoutPanel();

            _csbColor = new ColorSelectorButton[8];

            _tlpBasicSizeBar = new TableLayoutPanel();
            _tkbBasicSize = new UntiTrackBar();
            _lbBasicSize = new Label();
            _btnNoFillBasic = new UntiButton();
            // Advance initialize
            _pgeAdvance = new MultiPanelPage();
            _tlpAdvance = new TableLayoutPanel();
            _llbSwitchBasic = new LinkLabel();
            _tlpColorSelector = new TableLayoutPanel();
            _clrWheel = new ColorWheel();
            _colorEditor = new ColorEditor();
            _colorEditorManager = new ColorEditorManager();
            _cbxPenStyle = new ComboBox();
            _tlpAdvanceSizeBar = new TableLayoutPanel();
            _tkbAdvanceSize = new UntiTrackBar();
            _txtSize = new TextBox();
            _btnNoFillAdvance = new UntiButton();

            _mpnMain.SuspendLayout();
            // Basic suspend layout
            _pgeBasic.SuspendLayout();
            _tlpBasic.SuspendLayout();
            _tlpBasicColorTable.SuspendLayout();
            if (!fillMode)
                _tlpBasicSizeBar.SuspendLayout();
            // Advance suspend layout
            _pgeAdvance.SuspendLayout();
            _tlpAdvance.SuspendLayout();
            _tlpColorSelector.SuspendLayout();

            if (!fillMode)
                _tlpAdvanceSizeBar.SuspendLayout();

            SuspendLayout();
            // 
            // mpnMain
            // 
            _mpnMain.BackColor = Color.Transparent;
            _mpnMain.Controls.Add(_pgeBasic);
            _mpnMain.Controls.Add(_pgeAdvance);
            _mpnMain.Dock = DockStyle.Fill;
            _mpnMain.Location = new Point(0, 0);
            _mpnMain.Margin = new Padding(3, 4, 3, 4);
            _mpnMain.Name = "_mpnMain";
            _mpnMain.SelectedPage = _pgeBasic;
            _mpnMain.Size = new Size(210, 198);
            _mpnMain.TabIndex = 0;
            // 
            // pgeBasic
            // 
            _pgeBasic.BackColor = Color.Transparent;
            _pgeBasic.Controls.Add(_tlpBasic);
            _pgeBasic.Dock = DockStyle.Fill;
            _pgeBasic.Location = new Point(0, 0);
            _pgeBasic.Margin = new Padding(3, 4, 3, 4);
            _pgeBasic.Name = "_pgeBasic";
            _pgeBasic.Size = new Size(210, 198);
            _pgeBasic.TabIndex = 0;
            _pgeBasic.Text = @"Basic";
            // 
            // tlpBasic
            // 
            _tlpBasic.BackColor = Color.Transparent;
            _tlpBasic.ColumnCount = 1;
            _tlpBasic.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpBasic.Controls.Add(_llbSwitchAdvance, 0, 0);
            _tlpBasic.Controls.Add(_tlpBasicColorTable, 0, 1);
            if (fillMode)
                _tlpBasic.Controls.Add(_btnNoFillBasic, 0, 2);
            else
                _tlpBasic.Controls.Add(_tlpBasicSizeBar, 0, 2);
            _tlpBasic.Dock = DockStyle.Fill;
            _tlpBasic.Location = new Point(0, 0);
            _tlpBasic.Margin = new Padding(3, 4, 3, 4);
            _tlpBasic.Name = "_tlpBasic";
            _tlpBasic.RowCount = 3;
            _tlpBasic.RowStyles.Add(new RowStyle());
            _tlpBasic.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            _tlpBasic.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            _tlpBasic.Size = new Size(210, 198);
            _tlpBasic.TabIndex = 0;
            // 
            // llbSwitchAdvance
            // 
            _llbSwitchAdvance.LinkColor = Color.FromArgb(1, 123, 205);
            _llbSwitchAdvance.Location = new Point(3, 0);
            _llbSwitchAdvance.Name = @"_llbSwitchAdvance";
            _llbSwitchAdvance.Size = new Size(204, 24);
            _llbSwitchAdvance.TabIndex = 0;
            _llbSwitchAdvance.TabStop = true;
            _llbSwitchAdvance.Text = @"Advance";
            _llbSwitchAdvance.TextAlign = ContentAlignment.MiddleCenter;
            _llbSwitchAdvance.Click += _llbSwitchAdvance_Click;
            // 
            // tlpBasicColorTable
            // 
            _tlpBasicColorTable.ColumnCount = 4;
            _tlpBasicColorTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            _tlpBasicColorTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            _tlpBasicColorTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            _tlpBasicColorTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            _tlpBasicColorTable.Dock = DockStyle.Fill;
            _tlpBasicColorTable.Location = new Point(10, 34);
            _tlpBasicColorTable.Margin = new Padding(10);
            _tlpBasicColorTable.Name = "_tlpBasicColorTable";
            _tlpBasicColorTable.RowCount = 2;
            _tlpBasicColorTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            _tlpBasicColorTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            _tlpBasicColorTable.Size = new Size(190, 100);
            _tlpBasicColorTable.TabIndex = 1;


            for (int i=0; i < 2; i++)
                for (int j = 0; j < 4; j++)
                {
                    // 
                    // colorSelectorButton
                    // 
                    int id = i*4 + j;

                    _csbColor[id] = new ColorSelectorButton
                    {
                        BackColor = _colorTable[id],
                        Dock = DockStyle.Fill,
                        Location = new Point(10, 10),
                        Margin = new Padding(10),
                        Name = string.Format("csbColor{0}", id),
                        Size = new Size(27, 30),
                        TabIndex = id,
                        Text = string.Format("Color{0}", id),
                        UseVisualStyleBackColor = true,
                    };
                    _csbColor[id].Click += ColorSizeControl_Click;

                    _tlpBasicColorTable.Controls.Add(_csbColor[id], j, i);
                }
            if (fillMode)
            {
                //
                // btnNoFillBasic
                //
                _btnNoFillBasic.BackColor = Color.White;
                _btnNoFillBasic.Dock = DockStyle.Fill;
                _btnNoFillBasic.HoverColor = Color.FromArgb(230, 230, 230);
                _btnNoFillBasic.Location = new Point(15, 230);
                _btnNoFillBasic.Margin = new Padding(15);
                _btnNoFillBasic.Name = "_btnNoFillBasic";
                _btnNoFillBasic.PressColor = Color.FromArgb(77, 77, 77);
                _btnNoFillBasic.Size = new Size(180, 30);
                _btnNoFillBasic.TabIndex = 4;
                _btnNoFillBasic.Text = @"No Fill";
                _btnNoFillBasic.UseVisualStyleBackColor = false;
                _btnNoFillBasic.Click += NoFillButton_Click;
            }
            else
            {
                // 
                // tlpBasicSizeBar
                // 
                _tlpBasicSizeBar.ColumnCount = 2;
                _tlpBasicSizeBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                _tlpBasicSizeBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
                _tlpBasicSizeBar.Controls.Add(_tkbBasicSize, 0, 0);
                _tlpBasicSizeBar.Controls.Add(_lbBasicSize, 1, 0);
                _tlpBasicSizeBar.Dock = DockStyle.Fill;
                _tlpBasicSizeBar.Location = new Point(15, 159);
                _tlpBasicSizeBar.Margin = new Padding(15);
                _tlpBasicSizeBar.Name = "_tlpBasicSizeBar";
                _tlpBasicSizeBar.RowCount = 1;
                _tlpBasicSizeBar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                _tlpBasicSizeBar.Size = new Size(180, 24);
                _tlpBasicSizeBar.TabIndex = 2;
                // 
                // tkbSize
                // 
                _tkbBasicSize.Dock = DockStyle.Fill;
                _tkbBasicSize.Location = new Point(3, 3);
                _tkbBasicSize.Name = @"_tkbBasicSize";
                _tkbBasicSize.Size = new Size(124, 18);
                _tkbBasicSize.TabIndex = 0;
                _tkbBasicSize.Text = @"UntiTrackBar1";
                // synchronize trackbar mutualy
                var synTrkBasic = new TrackBarSync(_tkbBasicSize, _lbBasicSize);
                synTrkBasic.Add(_tkbAdvanceSize);
                synTrkBasic.ValueChanged += synTrackBar_ValueChanged;

                _tkbBasicSize.Value = (int) _selectedWidth;

                // 
                // lbSize
                // 
                _lbBasicSize.Dock = DockStyle.Fill;
                _lbBasicSize.Location = new Point(133, 0);
                _lbBasicSize.Name = @"_lbBasicSize";
                _lbBasicSize.Size = new Size(44, 24);
                _lbBasicSize.TabIndex = 1;
                _lbBasicSize.Text = ConvertWidthToString(_selectedWidth);
                _lbBasicSize.TextAlign = ContentAlignment.MiddleCenter;
            }
            // 
            // _pgeAdvance
            // 
            _pgeAdvance.Controls.Add(_tlpAdvance);
            _pgeAdvance.Dock = DockStyle.Fill;
            _pgeAdvance.Location = new Point(0, 0);
            _pgeAdvance.Margin = new Padding(3, 4, 3, 4);
            _pgeAdvance.Name = "_pgeAdvance";
            _pgeAdvance.Size = new Size(210, 275 + 26);
            _pgeAdvance.TabIndex = 1;
            _pgeAdvance.Text = @"_pgeAdvance";
            // 
            // _tlpAdvance
            // 
            _tlpAdvance.BackColor = Color.Transparent;
            _tlpAdvance.ColumnCount = 1;
            _tlpAdvance.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpAdvance.Controls.Add(_llbSwitchBasic, 0, 0);
            _tlpAdvance.Controls.Add(_tlpColorSelector, 0, 1);
            if (fillMode)
                _tlpAdvance.Controls.Add(_btnNoFillAdvance, 0, 2);
            else
            {
                _tlpAdvance.Controls.Add(_tlpAdvanceSizeBar, 0, 2);
                _tlpAdvance.Controls.Add(_cbxPenStyle, 0, 3);
            }
            _tlpAdvance.Dock = DockStyle.Fill;
            _tlpAdvance.Location = new Point(0, 0);
            _tlpAdvance.Margin = new Padding(3, 4, 3, 4);
            _tlpAdvance.Name = "_tlpAdvance";
            _tlpAdvance.RowCount = 4;
            _tlpAdvance.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            _tlpAdvance.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpAdvance.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            if (!fillMode)
                _tlpAdvance.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));

            _tlpAdvance.Size = new Size(210, 275 + 26);
            _tlpAdvance.TabIndex = 1;
            // 
            // _llbSwitchBasic
            // 
            _llbSwitchBasic.Dock = DockStyle.Fill;
            _llbSwitchBasic.LinkColor = Color.FromArgb(1, 123, 205);
            _llbSwitchBasic.Location = new Point(3, 0);
            _llbSwitchBasic.Name = "_llbSwitchBasic";
            _llbSwitchBasic.Size = new Size(204, 25);
            _llbSwitchBasic.TabIndex = 0;
            _llbSwitchBasic.TabStop = true;
            _llbSwitchBasic.Text = @"Basic";
            _llbSwitchBasic.TextAlign = ContentAlignment.MiddleCenter;
            _llbSwitchBasic.Click += _llbSwitchBasic_Click;
            // 
            // _tlpColorSelector
            // 
            _tlpColorSelector.ColumnCount = 1;
            _tlpColorSelector.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpColorSelector.Controls.Add(_clrWheel, 0, 0);
            _tlpColorSelector.Controls.Add(_colorEditor, 0, 1);
            _tlpColorSelector.Dock = DockStyle.Fill;
            _tlpColorSelector.Location = new Point(3, 28);
            _tlpColorSelector.Name = "_tlpColorSelector";
            _tlpColorSelector.RowCount = 2;
            _tlpColorSelector.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpColorSelector.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            _tlpColorSelector.Size = new Size(204, 184);
            _tlpColorSelector.TabIndex = 3;
            // 
            // _clrWheel
            // 
            _clrWheel.Color = _selectedColor;
            _clrWheel.Dock = DockStyle.Fill;
            _clrWheel.Location = new Point(3, 3);
            _clrWheel.Name = "_clrWheel";
            _clrWheel.Size = new Size(198, 138);
            _clrWheel.TabIndex = 0;
            
            // 
            // _colorEditor
            // 
            _colorEditor.Color = _selectedColor;
            _colorEditor.Dock = DockStyle.Fill;
            _colorEditor.TabIndex = 1;

            // 
            // colorEditorManager
            // 
            _colorEditorManager.ColorEditor = _colorEditor;
            _colorEditorManager.ColorWheel = _clrWheel;
            _colorEditorManager.ColorChanged += ColorEditorManager_ColorChanged;

            if (fillMode)
            {
                //
                // btnNoFillAdvance
                //
                _btnNoFillAdvance.BackColor = Color.White;
                _btnNoFillAdvance.Dock = DockStyle.Fill;
                _btnNoFillAdvance.HoverColor = Color.FromArgb(230, 230, 230);
                _btnNoFillAdvance.Location = new Point(15, 230);
                _btnNoFillAdvance.Margin = new Padding(15);
                _btnNoFillAdvance.Name = "_btnNoFillAdvance";
                _btnNoFillAdvance.PressColor = Color.FromArgb(77, 77, 77);
                _btnNoFillAdvance.Size = new Size(180, 30);
                _btnNoFillAdvance.TabIndex = 4;
                _btnNoFillAdvance.Text = @"No Fill";
                _btnNoFillAdvance.UseVisualStyleBackColor = false;
                _btnNoFillAdvance.Click += NoFillButton_Click;
            }
            else
            {
                // 
                // _tlpAdvanceSizeBar
                // 
                _tlpAdvanceSizeBar.ColumnCount = 2;
                _tlpAdvanceSizeBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                _tlpAdvanceSizeBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
                _tlpAdvanceSizeBar.Controls.Add(_tkbAdvanceSize, 0, 0);
                _tlpAdvanceSizeBar.Controls.Add(_txtSize, 1, 0);
                _tlpAdvanceSizeBar.Dock = DockStyle.Fill;
                _tlpAdvanceSizeBar.Location = new Point(15, 230);
                _tlpAdvanceSizeBar.Margin = new Padding(15);
                _tlpAdvanceSizeBar.Name = "_tlpAdvanceSizeBar";
                _tlpAdvanceSizeBar.RowCount = 1;
                _tlpAdvanceSizeBar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                _tlpAdvanceSizeBar.Size = new Size(180, 30);
                _tlpAdvanceSizeBar.TabIndex = 2;
                // 
                // _tkbAdvanceSize
                // 
                _tkbAdvanceSize.Dock = DockStyle.Fill;
                _tkbAdvanceSize.Location = new Point(3, 3);
                _tkbAdvanceSize.Name = "_tkbAdvanceSize";
                _tkbAdvanceSize.Size = new Size(124, 24);
                _tkbAdvanceSize.TabIndex = 0;
                _tkbAdvanceSize.Text = @"tkbAdvanceSize";
                // synchronize trackbar mutualy
                var synTrkAdvance = new TrackBarSync(_tkbAdvanceSize, _txtSize);
                synTrkAdvance.Add(_tkbBasicSize);
                synTrkAdvance.ValueChanged += synTrackBar_ValueChanged;

                _tkbAdvanceSize.Value = (int) _selectedWidth;

                // 
                // _txtSize
                // 
                _txtSize.Dock = DockStyle.Fill;
                _txtSize.Location = new Point(133, 3);
                _txtSize.Name = "_txtSize";
                _txtSize.Text = ConvertWidthToString(_selectedWidth);
                _txtSize.Size = new Size(44, 25);
                _txtSize.TabIndex = 1;
                _txtSize.CausesValidation = true;
                _txtSize.Validating += _txtSize_Validating;
                //
                // _cbxPenStyle
                //
                _cbxPenStyle.Dock = DockStyle.Fill;
                _cbxPenStyle.FormattingEnabled = true;
                _cbxPenStyle.DataSource = Enum.GetValues(typeof(DashStyle));
                _cbxPenStyle.Location = new Point(3, 4);
                _cbxPenStyle.Margin = new Padding(15, 0, 15, 0);
                _cbxPenStyle.Name = "_cbxPenStyle";
                _cbxPenStyle.Size = new Size(172, 25);
                _cbxPenStyle.TabIndex = 2;
                _cbxPenStyle.SelectedIndex = 0;
                _cbxPenStyle.SelectedIndexChanged += PenStyle_SelectedIndexChanged;
            }
            // 
            // This Control
            // 
            BackColor = Color.White;
            Controls.Add(_mpnMain);
            Font = new Font("Segoe UI Light", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(3, 4, 3, 4);
            Name = "ColorToolControl";
            Size = new Size(210, 198);
            _mpnMain.ResumeLayout(false);
            // Basic Resume Layout
            _pgeBasic.ResumeLayout(false);
            _tlpBasic.ResumeLayout(false);
            _tlpBasicColorTable.ResumeLayout(false);
            if (!fillMode)
                _tlpBasicSizeBar.ResumeLayout(false);
            // Advance Resume Layout
            _pgeAdvance.ResumeLayout(false);
            _tlpAdvance.ResumeLayout(false);
            _tlpColorSelector.ResumeLayout(false);
            if (!fillMode)
            {
                _tlpAdvanceSizeBar.ResumeLayout(false);
                _tlpAdvanceSizeBar.PerformLayout();
            }
            ResumeLayout(false);
        }

        /// <summary>
        /// Change into advance mode.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void _llbSwitchBasic_Click(object sender, EventArgs e)
        {
            _isBasic = !_isBasic;
            Size = new Size(210, 198);
            _mpnMain.SelectedPage = _pgeBasic;
        }

        /// <summary>
        /// Change into basic mode.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void _llbSwitchAdvance_Click(object sender, EventArgs e)
        {
            _isBasic = !_isBasic;
            Size = new Size(210, 275+28);
            _mpnMain.SelectedPage = _pgeAdvance;
        }

        /// <summary>
        /// Advance color changed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void ColorEditorManager_ColorChanged(object sender, EventArgs e)
        {
            _selectedColor = _colorEditorManager.Color;
        }

        /// <summary>
        /// Change selected color when color button in table is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ColorSizeControl_Click(object sender, EventArgs e)
        {
            var ctrl = sender as Control;
            if (ctrl == null) return;
            _selectedColor = ctrl.BackColor;

            // Fire event ColorSelected
            FireEventColorSelected();
        }

        /// <summary>
        /// Change selected color when no fill button is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void NoFillButton_Click(object sender, EventArgs e)
        {
            _selectedColor = Color.Transparent;

            // Fire event ColorSelected
            FireEventColorSelected();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the _cbxPenStyle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        void PenStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedDash = (DashStyle)_cbxPenStyle.SelectedItem;
        }

        /// <summary>
        /// Synchronize all value of controls which invoke with main trackbar.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void synTrackBar_ValueChanged(object sender, EventArgs e)
        {
            var trackbarSync = sender as TrackBarSync;
            if (trackbarSync != null) SelectedWidth = trackbarSync.Value;
        }

        /// <summary>
        /// Check size entered is correct format.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        void _txtSize_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string errorMsg;
            if (!CheckSizeInput(_txtSize.Text, out errorMsg))
            {
                // Cancel the event and select the text to be corrected by the user.
                e.Cancel = true;
                _txtSize.Select(0, _txtSize.Text.Length - 3);

                // Set the ErrorProvider error with the text to display.  
                //this.errorProvider1.SetError(textBox1, errorMsg);
            }
        }

        /// <summary>
        /// Checks size enter is valid. Can be converted into integer
        /// </summary>
        /// <param name="size">The text of txtSize. Eg: "2 px"</param>
        /// <param name="errorMessage">The message is fired when catch an error</param>
        /// <returns></returns>
        private bool CheckSizeInput(string size, out string errorMessage)
        {
            var regex = new Regex(@"\d{1,3} px");
            if (regex.IsMatch(size))
            {
                errorMessage = "";
                return true;
            }

            errorMessage = "Width of pen must enter correctly! (eg: \"2 px\")";
            return false;
        }

        /// <summary>
        /// Converts the width value to string with format.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <returns></returns>
        private static string ConvertWidthToString(float width)
        {
            var w = (int)width;
            return string.Format("{0} px", w);
        }


        /// <summary>
        /// Fires the event color selected.
        /// </summary>
        private void FireEventColorSelected()
        {
            if (ColorSelected != null)
                ColorSelected(this, EventArgs.Empty);
        }

        private Color _selectedColor;
        private float _selectedWidth;
        private DashStyle _selectedDash;

        private Color[] _colorTable;
        private bool _isBasic;

        private MultiPanel _mpnMain;
        private MultiPanelPage _pgeBasic;
        private MultiPanelPage _pgeAdvance;
        // Basic Page
        private TableLayoutPanel _tlpBasic;
        private LinkLabel _llbSwitchAdvance;
        private TableLayoutPanel _tlpBasicColorTable;
        private ColorSelectorButton[] _csbColor;
        private TableLayoutPanel _tlpBasicSizeBar;
        private UntiTrackBar _tkbBasicSize;
        private Label _lbBasicSize;
        private UntiButton _btnNoFillBasic;
        // Advance Page
        private TableLayoutPanel _tlpAdvance;
        private LinkLabel _llbSwitchBasic;
        private TableLayoutPanel _tlpColorSelector;
        private ColorWheel _clrWheel;
        private ColorEditor _colorEditor;
        private ColorEditorManager _colorEditorManager;
        private ComboBox _cbxPenStyle;
        private TableLayoutPanel _tlpAdvanceSizeBar;
        private UntiTrackBar _tkbAdvanceSize;
        private TextBox _txtSize;
        private UntiButton _btnNoFillAdvance;
    }
}
