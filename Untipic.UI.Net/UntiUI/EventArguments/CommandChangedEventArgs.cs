using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Untipic.EventArguments
{
    public class CommandChangedEventArgs : EventArgs
    {
        public CommandChangedEventArgs(DrawPadTools.DrawPadCommand command)
        {
            Command = command;
        }

        public DrawPadTools.DrawPadCommand Command { get; set; }
    }

    public delegate void CommandChangedEventHandler(Object sender, CommandChangedEventArgs e);
}
