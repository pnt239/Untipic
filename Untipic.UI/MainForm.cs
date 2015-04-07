using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Untipic.UI.Net
{
    public sealed partial class MainForm : Untipic.UI.Net.UntiUI.UntiForm
    {
        public MainForm()
        {
            InitializeComponent();
            Font = this.Theme.FormDefaultFont;
        }
    }
}
