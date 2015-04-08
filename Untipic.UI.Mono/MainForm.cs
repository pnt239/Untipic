using System;
using Gtk;

namespace Untipic.UI.Mono
{
	public partial class MainForm : Gtk.Window
	{
		public MainForm () :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}
	}
}

