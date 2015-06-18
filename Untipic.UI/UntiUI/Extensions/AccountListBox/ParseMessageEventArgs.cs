// ////////////////////////////////////////////////////////////////////////////
//
//  $RCSfile: ParseMessageEventArgs.cs,v $
//
//  $Revision: 1.1.1.1 $
//
//  Last change:
//    $Author: Robert $
//    $Date: 2004/06/22 09:56:47 $
//
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
//
//  Original Code from Christian Tratz (via www.codeproject.com).
//  Changed by R. Lelieveld, SimVA GmbH, Pham Ngoc Thanh
//
// ////////////////////////////////////////////////////////////////////////////

using System.Drawing;

namespace Untipic.UI.UntiUI.Extensions.AccountListBox
{
	/// <summary>
	/// 
	/// </summary>
	public class ParseMessageEventArgs : System.EventArgs
	{
		private string _messageHeader;
		private string _messageText;
		private string _parseSource;
	    private Image _thumbImage;

	    public ParseMessageEventArgs()
	    {
	    }

		public ParseMessageEventArgs(string messageHeader, string messageText) : this()
		{		
			_messageHeader = messageHeader;
			_messageText = messageText;
            _thumbImage = null;
		}

		public ParseMessageEventArgs(string lineHeader, string messageText, string source) : this(lineHeader,messageText)
		{		
			_parseSource = source;			
		}

		public string MessageText
		{
			get { return _messageText; }
			set { _messageText = value; }
		}

		public string Source
		{
			get { return _parseSource; }
			set { _parseSource = value; }
		}

		public string LineHeader
		{
			get { return _messageHeader; }
			set { _messageHeader = value; }
		}

		public Image ThumbImage
		{
            get { return _thumbImage; }
            set { _thumbImage = value; }
		}
	}
}
