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
    public partial class MainGui :  UntiForm
    {
        private Panel _currentPanel;
        private Panel _lastPanel;
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

            tsbNew.Tag = panNew;
            tsbOpen.Tag = panOpen;
            //tsbSave.Tag = panSave;
            //tsbSaveAs.Tag = panSave;

            SwitchPanel(panDraw);
        }

        private void SwitchPanel(Panel panel)
        {
            if (_currentPanel != null)
                _currentPanel.Visible = false;

            bool havingLine = (bool)panel.Tag;

            panel.Width += panel.Location.X;
            panel.Height += panel.Location.Y - (havingLine ? 2 : 0);
            panel.Location = new Point(0, havingLine ? 2 : 0);
            panel.Visible = true;

            _lastPanel = _currentPanel;
            _currentPanel = panel;
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
    }
}
