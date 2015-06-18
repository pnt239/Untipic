// ////////////////////////////////////////////////////////////////////////////
//
//  $RCSfile: MessageListBox.cs,v $
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
//  Changed by R. Lelieveld, SimVA GmbH.
//
// ////////////////////////////////////////////////////////////////////////////

using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Untipic.UI.UntiUI.Extensions.AccountListBox
{
	/// <summary>
	/// 
	/// </summary>
	public class AccountListBox : ResizableListBox 
	{
		private const int	MainTextOffset = 30;
        private readonly Font _headingFont;
		private System.ComponentModel.IContainer _components = null;
        private int _thumbImageSize = 48;

		/// <summary>
		/// Constructor.
		/// </summary>
		public AccountListBox()
		{	
			InitializeComponent();		
			_headingFont = new Font(Font, FontStyle.Bold);
			MeasureItem += MeasureItemHandler;
		}


	    /// <summary>
		/// Windows-Init.
		/// </summary>
		private void InitializeComponent()
		{
            SuspendLayout();
            ResumeLayout(false);

		}

        public int ThumbImageSize
        {
            get { return _thumbImageSize; }
            set { _thumbImageSize = value; }
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( _components != null )
					_components.Dispose();
			}

			_headingFont.Dispose();

			base.Dispose( disposing );
		}

		#region overrides
		protected override void OnDrawItem( DrawItemEventArgs e)
		{
			e.DrawBackground();
			e.DrawFocusRectangle();
		    Rectangle bounds = e.Bounds;
			Color textColor = SystemColors.ControlText;

			var item = Items[e.Index];

            // Pre-process before paint
		    var padding = 5;
		    var imageLeft = bounds.Left + padding;
		    var imageTop = bounds.Top + 5;
		    var headerLeft = imageLeft + _thumbImageSize + padding;
		    var headerTop = imageTop;
            var textLeft = headerLeft;
            var textTop = headerTop + Font.Height;

			//draw selected item background
			if(e.State == DrawItemState.Selected)
			{
				using ( Brush b = new SolidBrush(SystemColors.Highlight) )
				{
					// Fill background;
					e.Graphics.FillRectangle(b, e.Bounds);
				}	
				textColor = SystemColors.HighlightText;
			}

			//draw image
		    if (item.ThumbImage != null)
		    {
                e.Graphics.DrawImage(item.ThumbImage, imageLeft, imageTop, _thumbImageSize, _thumbImageSize);
		    }

			using(var textBrush = new SolidBrush(textColor))
			{
				//draw Headline
				e.Graphics.DrawString(
					item.LineHeader,
					_headingFont,
					textBrush,
                    headerLeft,
                    headerTop);

				//draw main text
				int LinesFilled=0,
					CharsFitted=0,
					top;

				// Draw layout, 2 times the offset (left & right)
				Size oneLine = new Size( this.Width - MainTextOffset*2, this.Font.Height);

				StringBuilder sbTextToDraw = new StringBuilder( item.MessageText);
				string strLineToDraw;
				int index1 = 0,
					index2, index2New;
                top = textTop;

				while ( sbTextToDraw.Length > 0)
				{
					// Break string into more lines when an end-of-line character is found
					if ( ( index2 = sbTextToDraw.ToString().IndexOf( '\n')) > 0)
					{
						strLineToDraw = sbTextToDraw.ToString( index1, index2-index1);
						index2New = index2 + 1;
					}
					else
					{
						index2 = sbTextToDraw.Length;
						index2New = index2;
						strLineToDraw = sbTextToDraw.ToString();
					}

					e.Graphics.MeasureString(
						strLineToDraw,
						this.Font,
						oneLine,
						StringFormat.GenericDefault,
						out CharsFitted,
						out LinesFilled);

					// There's no knowledge about words, so just don't split words up if possible
					if ( CharsFitted < index2)
					{
						int index = strLineToDraw.LastIndexOf(' ', CharsFitted-1, CharsFitted);
						if ( index != -1)
							index2New = index + 1;
						else
							index2New = CharsFitted;
						strLineToDraw = sbTextToDraw.ToString( index1, index2New-index1);
					}

					// Draw the text
					e.Graphics.DrawString(
						strLineToDraw,
						this.Font,
						textBrush,
                        textLeft,
						top);

					// Adjust top
					top += this.Font.Height;

					// Next line
					sbTextToDraw = sbTextToDraw.Remove( index1, index2New);
				}

				sbTextToDraw = null;
				strLineToDraw = null;
			}
		}

		
		private void MeasureItemHandler(object sender, MeasureItemEventArgs e)
		{
		    ParseMessageEventArgs item;
			item =  (ParseMessageEventArgs) Items[e.Index];
			int LinesFilled, CharsFitted;
			
			// Draw layout, 2 times the offset (left & right)
			Size sz = new Size( this.Width - MainTextOffset*2, this.Font.Height);

			StringBuilder sbTextToDraw = new StringBuilder( item.MessageText);
			string strLineToDraw;
			int index1 = 0,
				index2,
				index2New,
				lines = 0;

			while ( sbTextToDraw.Length > 0)
			{
				// Break string into more lines when an end-of-line character is found
				if ( ( index2 = sbTextToDraw.ToString().IndexOf( '\n')) > 0)
				{
					strLineToDraw = sbTextToDraw.ToString( index1, index2-index1);
					index2New = index2 + 1;
				}
				else
				{
					index2 = sbTextToDraw.Length;
					index2New = index2;
					strLineToDraw = sbTextToDraw.ToString();
				}

				e.Graphics.MeasureString(
					strLineToDraw,
					Font,
					sz,
					StringFormat.GenericDefault,
					out CharsFitted,
					out LinesFilled);

				// There's no knowledge about words, so just don't split words up if possible
				if ( CharsFitted < index2)
				{
					int index = strLineToDraw.LastIndexOf(' ', CharsFitted-1, CharsFitted);
					if ( index != -1)
						index2New = index + 1;
					else
						index2New = CharsFitted;
				}

				lines += LinesFilled;
				sbTextToDraw = sbTextToDraw.Remove( index1, index2New);
			}

			sbTextToDraw = null;
			strLineToDraw = null;

			var mainTextHeight = lines * Font.Height;

            e.ItemHeight = _thumbImageSize + mainTextHeight + 4;
			e.ItemWidth = sz.Width;
		}
		#endregion
	}
}
