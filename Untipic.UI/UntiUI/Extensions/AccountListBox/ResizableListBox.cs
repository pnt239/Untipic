// ////////////////////////////////////////////////////////////////////////////
//
//  $RCSfile: ResizableListBox.cs,v $
//
//  $Revision: 1.2 $
//
//  Last change:
//    $Author: Robert $
//    $Date: 2004/07/28 10:42:08 $
//
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
//
//  Original Code from Christian Tratz (via www.codeproject.com).
//  Changed by R. Lelieveld, SimVA GmbH.
//
// ////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace Untipic.UI.UntiUI.Extensions.AccountListBox
{
	/// <summary>
	/// This is our base class. It tries to mimick the behaviour of a standard ListBox. It's
	/// not complete in that respect, but should do it for most cases.
	/// This class alone does NOT implement resizable listbox entries. It just provides the base
	/// for an inheriting class.
	/// </summary>
	public class ResizableListBox : Panel
	{
		//our data containers - exposed via properties
		private readonly ListBoxList	_items = new ListBoxList();
		private readonly ArrayList	_selectedItems = new ArrayList();
		private readonly ArrayList	_selectedItemIndices = new ArrayList();

		//just for internal use
		private bool		_ctrlPressed;
		private bool		_allowMultiSelect = true;

		/// <summary>
		/// The ctor.
		/// </summary>
		public ResizableListBox()
		{	
			// We are going to do all of the painting so better 
			// setup the control to use double buffering
			SetStyle(ControlStyles.AllPaintingInWmPaint|ControlStyles.ResizeRedraw|
				ControlStyles.Opaque|ControlStyles.UserPaint|ControlStyles.DoubleBuffer, true);


			//set some defaults			
			BackColor = Color.White;
			AutoScroll = true;
			HScroll = false;

			_items.OnItemInserted += ItemInserted;
		}

		#region Events
		
		public event EventHandler SelectedIndexChanged;		
		public event MeasureItemEventHandler MeasureItem;
		public event DrawItemEventHandler DrawItem;
		
		#endregion

		#region Overrides
		/// <summary>
		/// Handle the Ctrl-Key for multiple selections
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			_ctrlPressed = e.Control;
		}


		/// <summary>
		/// Handle the Ctrl-Key for multiple selections
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			_ctrlPressed = e.Control;
		}


		/// <summary>
		/// Here we implement the selection handling of the listbox
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			//call base implementation
			base.OnMouseDown(e);
			
			//make sure we receive key events
			Focus();

			if(e.Button != MouseButtons.Left) 
				return;

			//determine which item was clicked
			int index = ItemHitTest(e.X,e.Y);

			if(index < 0)
				return;
						
			if((_ctrlPressed) && _allowMultiSelect)
			{
				if(_selectedItemIndices.Contains(index))
				{
					RemoveSelectedItem(index);
				}
				else
				{
					AddSelectedItem(index);
				}
			}
			else
			{
				if((_selectedItemIndices.Contains(index)) && (_selectedItemIndices.Count == 1))
					return;

				_selectedItemIndices.Clear();
				_selectedItems.Clear();

				AddSelectedItem(index);
			}

			Invalidate();
		
		}

		
		/// <summary>
		/// This method is the core worker method. It calls the OnMeasureItem and OnDrawItem methods.
		/// The OnMeasureItem method is called *everytime* the OnDrawItem method is called. Not just once
		/// like in the original ListBox.
		/// </summary>
		/// <param name="pe"></param>
		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
			
			Graphics g = pe.Graphics;
			Rectangle bounds = new Rectangle();
			int posScrollY = AutoScrollPosition.Y;
			int height = 0;

			//clear background
			using ( Brush b = new SolidBrush(BackColor) )
			{
				// Fill background;
				g.FillRectangle(b, ClientRectangle);
			}			

			//draw our items
			int i = 0;
			int iItemHeight;
			while ( i < _items.Count)
			{
				// measure only when neccesary
				if ( !_items.Info(i).HeightValid)
				{
					MeasureItemEventArgs miea = new MeasureItemEventArgs( g, i);
					OnMeasureItem( miea);
				}
				iItemHeight = _items.Info(i).Height;

				if ( ( posScrollY + iItemHeight >= 0) && posScrollY < ClientRectangle.Height)
				{
					bounds.Location = new Point( AutoScrollPosition.X, posScrollY);
					bounds.Size = new Size( ClientRectangle.Right, iItemHeight);

					//and draw
					DrawItemState state = (_selectedItemIndices.Contains(i)) ? DrawItemState.Selected : DrawItemState.Default;
					DrawItemEventArgs diea = new DrawItemEventArgs(
						g,
						Font,
						bounds,
						i,
						state,
						ForeColor,
						BackColor);
					OnDrawItem(diea);
				}

				posScrollY += iItemHeight;
				height += iItemHeight;
				i++;
			}

			AutoScrollMinSize = new Size( Width - 30, height);
		}


		/// <summary>
		/// The listbox changed size. Update all items, their heights just became invalid.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			for ( int i = 0; i < _items.Count; i++)
				_items.Info(i).HeightValid = false;
			base.OnResize (e);
		}

		#endregion

		#region Private Methods
		/// <summary>
		/// Tests which item the user has clicked.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns>The index of the clicked item</returns>
		private int ItemHitTest(int x, int y)
		{
			var posY = AutoScrollPosition.Y;
			for(var i=0; i< _items.Count; i++)
			{
				posY += _items.Info(i).Height;

				if(y < posY)
				{
					return i;
				}
			}

			return -1;
		}


		/// <summary>
		/// Adds an item to the selected item ArrayList and fires the appropriate event.
		/// </summary>
		/// <param name="index"></param>
		private void AddSelectedItem(int index)
		{
			if(index == -1)
			{
				_selectedItemIndices.Clear();
				_selectedItems.Clear();
			}
			else
			{
				_selectedItemIndices.Add(index);
				_selectedItems.Add(_items[index]);
			}

			OnSelectedIndexChanged(new EventArgs());
		}

		
		/// <summary>
		/// Removes an item from the selected item ArrayList
		/// </summary>
		/// <param name="index"></param>
		private void RemoveSelectedItem(int index)
		{
			_selectedItemIndices.Remove(index);
			_selectedItems.Remove(_items[index]);
			OnSelectedIndexChanged(new EventArgs());
		}

		#endregion

		#region Properties		

		public ListBoxList Items
		{
			get { return _items; }
		}

		public object SelectedItem
		{
			get
			{
			    if(_selectedItems.Count > 0)
					return _selectedItems[0];
			    return null;
			}
		    set 
			{
				int pos = _items.IndexOf(value);
				if(pos >= 0)
				{
					//clear list
					_selectedItemIndices.Clear();
					_selectedItems.Clear();
					
					//add item
					AddSelectedItem(pos);					
				}
			}
		}

		public ArrayList SelectedItems
		{
			get { return _selectedItems; }
		}

		public int SelectedIndex
		{
			get
			{
			    if(_selectedItemIndices.Count > 0)
					return (int)_selectedItemIndices[0];
			    return -1;
			}
		    set
			{
				if((value < _items.Count) && (value >= -1))
					AddSelectedItem(value);
			}
		}

		public ArrayList SelectedItemIndices
		{
			get { return _selectedItemIndices; }
		}

		#endregion

		#region Virtual Methods
		/// <summary>
		/// This is just a standard implementation without any resize-logic.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDrawItem( DrawItemEventArgs e)
		{
			e.DrawBackground();
			e.DrawFocusRectangle();			
		
			Rectangle bounds = e.Bounds;
			Color textColor = SystemColors.ControlText;
					
			
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
			
			using (SolidBrush textBrush = new SolidBrush(textColor))
			{

				//draw item the standard way
				e.Graphics.DrawString(_items[e.Index].ToString(), Font, textBrush, bounds.Left , bounds.Top);
			}

			//fire event
			if(DrawItem != null)
				DrawItem(this, e);
			
		}


		/// <summary>
		/// Just a standard implementation. Subscribe to the event in a derived class to implement your logic
		/// to resize the items.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMeasureItem(MeasureItemEventArgs e)
		{
			//preset Height
			e.ItemHeight = Font.Height;

			if(MeasureItem != null)
				MeasureItem(this, e);

			// set the height
			_items.Info( e.Index).Height = e.ItemHeight;
		}

		protected virtual void OnSelectedIndexChanged(EventArgs e)
		{
			if(SelectedIndexChanged != null)
				SelectedIndexChanged(this, e);
		}

		#endregion


		/// <summary>
		/// An Item is inserted.
		/// </summary>
		/// <param name="index"></param>
		private void ItemInserted( int index)
		{
			// Adjust selected item indices
			for ( int i = 0; i < _selectedItemIndices.Count; i++)
			{
				int selIndex = (int)_selectedItemIndices[i];
				if ( selIndex >= index)
					_selectedItemIndices[i] = selIndex+1;
			}

			// Adjust the autoscrollposition, so that the newly added item is shown.
			//this.AutoScrollPosition = new Point( 0, 0);
		}

	}
}
