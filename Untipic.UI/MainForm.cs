using Untipic.UI.UntiUI;

namespace Untipic.UI
{
    public sealed partial class MainForm : UntiForm
    {
        public MainForm() : 
#if MONO
            base (true)
#else
            base(false)
#endif
        {
            InitializeComponent();
            Font = this.Theme.FormDefaultFont;
        }
    }
}
