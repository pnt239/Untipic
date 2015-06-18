using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Untipic.UI.UntiUI;

namespace Untipic.UI
{
    public partial class MainGui : UntiForm
    {
        private Panel _currentPanel;
        private Panel _lastPanel;
        private Panel _currentSidePanel;

        public MainGui() :
#if MONO
           base(true)
#else
            base(false)
#endif
        {
            InitializeComponent();
            SubInitialize();
        }

        private void SubInitialize()
        {
            _lastPanel = null;
            _currentPanel = null;

            panStartScreen.Tag = true;
            panNew.Tag = true;
            panOpen.Tag = true;
            panMessage.Tag = false;
            panDraw.Tag = true;

            panLeftSideFont.Tag = tsbFont;
            panRightSideNavigation.Tag = tsbNavigation;
            panLeftSideOutline.Tag = tsbToolOutline;
            panLeftSideFill.Tag = tsbToolFill;

            tsbNew.Tag = panNew;
            tsbOpen.Tag = panOpen;
            //tsbSave.Tag = panSave;
            //tsbSaveAs.Tag = panSave;

            tsbFont.Tag = panLeftSideFont;
            tsbNavigation.Tag = panRightSideNavigation;
            tsbToolOutline.Tag = panLeftSideOutline;
            tsbToolFill.Tag = panLeftSideFill;

            SwitchPanel(panDraw);
            RelayoutSidePanel();
            DrawOutlineFillPanelSide();

            lsbAccount.Items.Add(new UntiUI.Extensions.AccountListBox.ParseMessageEventArgs("Thanh", "Test") { ThumbImage = Properties.Resources.User });
            lsbAccount.Items.Add(new UntiUI.Extensions.AccountListBox.ParseMessageEventArgs("Thanh", "Test") { ThumbImage = Properties.Resources.User });
        }

        private void SwitchPanel(Panel panel)
        {
            if (_currentPanel != null)
                _currentPanel.Visible = false;

            bool havingLine = (bool) panel.Tag;

            panel.Width += panel.Location.X;
            panel.Height += panel.Location.Y - (havingLine ? 2 : 0);
            panel.Location = new Point(0, havingLine ? 2 : 0);
            panel.Visible = true;

            _lastPanel = _currentPanel;
            _currentPanel = panel;
        }

        private void RelayoutSidePanel()
        {
            panLeftSideFont.Location = new Point(2, 0);
            panLeftSideFont.Height = panDrawContainer.Height;

            panLeftSideOutline.Location = new Point(2, 0);
            panLeftSideOutline.Height = panDrawContainer.Height;

            panLeftSideFill.Location = new Point(2, 0);
            panLeftSideFill.Height = panDrawContainer.Height;

            panRightSideNavigation.Width = 250;
            panRightSideNavigation.Height = panDrawContainer.Height;
            panRightSideNavigation.Location = new Point(panDrawContainer.Width - panRightSideNavigation.Width);

            drawPad.Location = new Point(0, 0);
            drawPad.Width = panDrawContainer.Width;
            drawPad.Height = panDrawContainer.Height;
        }

        private void DrawOutlineFillPanelSide()
        {
            // Some fill must initialize before begin
            Color[] colorTable = new[]
            {
                Color.FromArgb(0xff, 0, 0), Color.FromArgb(0xff, 0x80, 0x17),
                Color.FromArgb(0xff, 0xf7, 0x0), Color.FromArgb(0, 0xe8, 0),
                Color.FromArgb(0x19, 0x77, 0xff), Color.FromArgb(0xfc, 0x0c, 0x59),
                Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0, 0, 0)
            };

            UntiUI.Extensions.ColorSelectorButton[] csbColorOutline = new UntiUI.Extensions.ColorSelectorButton[8];

            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 4; j++)
                {
                    // 
                    // colorSelectorButton
                    // 
                    int id = i * 4 + j;

                    csbColorOutline[id] = new UntiUI.Extensions.ColorSelectorButton
                    {
                        BackColor = colorTable[id],
                        Dock = DockStyle.Fill,
                        Location = new Point(10, 10),
                        Margin = new Padding(10),
                        Name = string.Format("csbColorOutline{0}", id),
                        Size = new Size(27, 30),
                        TabIndex = id,
                        Text = string.Format("Color{0}", id),
                        UseVisualStyleBackColor = true,
                    };
                    csbColorOutline[id].Click += ColorSizeControlOutline_Click;

                    tlpColorTableOutline.Controls.Add(csbColorOutline[id], j, i);
                }

            UntiUI.Extensions.ColorSelectorButton[] csbColorFill = new UntiUI.Extensions.ColorSelectorButton[8];

            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 4; j++)
                {
                    // 
                    // colorSelectorButton
                    // 
                    int id = i * 4 + j;

                    csbColorFill[id] = new UntiUI.Extensions.ColorSelectorButton
                    {
                        BackColor = colorTable[id],
                        Dock = DockStyle.Fill,
                        Location = new Point(10, 10),
                        Margin = new Padding(10),
                        Name = string.Format("csbColorFill{0}", id),
                        Size = new Size(27, 30),
                        TabIndex = id,
                        Text = string.Format("Color{0}", id),
                        UseVisualStyleBackColor = true,
                    };
                    csbColorFill[id].Click += ColorSizeControlFill_Click;

                    tlpColorTableFill.Controls.Add(csbColorFill[id], j, i);
                }
        }

        private void BackToPrePanel()
        {
            SwitchPanel(_lastPanel);
        }

        private void tsbCreateSave_Click(object sender, EventArgs e)
        {
            var button = sender as ToolStripButton;
            if (button == null) return;

            var panel = (Panel) button.Tag;
            if (panel == null) return;

            SwitchPanel(panel);
        }

        private void btnCreateSaveCancel_Click(object sender, EventArgs e)
        {
            SwitchPanel(panStartScreen);
        }

        private void tsbOpenSidePanel_Click(object sender, EventArgs e)
        {
            var button = sender as ToolStripButton;
            if (button == null) return;

            var sidePanel = button.Tag as Panel;
            if (sidePanel == null) return;

            if (_currentSidePanel != null && _currentSidePanel.Name != sidePanel.Name)
            {
                var toolButon = _currentSidePanel.Tag as ToolStripButton;
                if (toolButon == null) return;

                _currentSidePanel.Visible = false;
                toolButon.Checked = false;
            }

            button.Checked = !button.Checked;
            sidePanel.Visible = !sidePanel.Visible;

            if ((_currentSidePanel == null) || (_currentSidePanel.Name != sidePanel.Name))
            {
                _currentSidePanel = sidePanel;
            }
        }

        private void btnFontSelect_Click(object sender, EventArgs e)
        {
            //
        }

        private void ColorSizeControlOutline_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ColorSizeControlFill_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
