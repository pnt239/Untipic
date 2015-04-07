using System;

namespace Untipic.UI.UntiUI.EventArguments
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
