using System;
using Untipic.UI.UntiUI.DrawPad;

namespace Untipic.UI.UntiUI.EventArguments
{
    public class CommandChangedEventArgs : EventArgs
    {
        public CommandChangedEventArgs(DrawPadCommand command)
        {
            Command = command;
        }

        public DrawPadCommand Command { get; set; }
    }

    public delegate void CommandChangedEventHandler(Object sender, CommandChangedEventArgs e);
}
