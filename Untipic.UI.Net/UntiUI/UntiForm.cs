#region Copyright (c) 2013 Pham Ngoc Thanh, https://github.com/panoti/DADHMT_LTW/
/**
 * MetroUI - Windows Modern UI for .NET WinForms applications
 * Copyright (c) 2014 Pham Ngoc Thanh, https://github.com/panoti/DADHMT_LTW/
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation the rights to use, copy, 
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 */
#endregion

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Untipic.Presentation;
using Untipic.UI.Net.WinApi;

namespace Untipic.UI.Net.UntiUI
{
    public class UntiForm : Form
    {
        private enum WindowButtons
        {
            Close = 0,
            Maximize = 1,
            Minimize = 2,
        }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UntiForm"/> class.
        /// </summary>
        public UntiForm()
        {
            // Init default field's value
            _theme = new ThemeManager();

            _borderColor = Color.FromArgb(0xcc, 0xcc, 0xcc);
            _borderWidth = BORDER_WIDTH;
            _formIconPosition = new Point(_borderWidth + 5, _borderWidth + 5);
            _formCaptionPosition = new PointF(_formIconPosition.X + Icon.Size.Width + 5, _formIconPosition.Y + 6f);
            _borderStyle = UntiBorderStyle.Sizable;

            // Set serveral option for paint
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |  // <-- prevents size handle artifacts
                ControlStyles.UserPaint, true);
            
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
        }
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            // Add window buttons
            if (ControlBox && _borderStyle != UntiBorderStyle.None)
            {
                AddWindowButton(WindowButtons.Close);
                if (MaximizeBox) AddWindowButton(WindowButtons.Maximize);
                if (MinimizeBox) AddWindowButton(WindowButtons.Minimize);
            }

            base.OnLoad(e);
        }

        #region Properties

        /// <summary>
        /// The form border style
        /// </summary>
        /// <value>
        /// The border style.
        /// </value>
        public UntiBorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set { _borderStyle = value; }
        }

        public int BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; }
        }

        public ThemeManager Theme { get { return _theme; } }

        /// <summary>
        /// Gets or sets padding within the control.
        /// </summary>
        /// <returns>A <see cref="T:System.Windows.Forms.Padding" /> representing the control's internal spacing characteristics.</returns>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public new Padding Padding
        {
            get { return base.Padding; }
            set
            {
                value.Top = _borderStyle == UntiBorderStyle.None
                    ? Math.Max(value.Top, _borderWidth)
                    : Math.Max(value.Top, CAPTION_HEIGHT + _borderWidth);
                base.Padding = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets the internal spacing, in pixels, of the contents of a control.
        /// </summary>
        /// <returns>A <see cref="T:System.Windows.Forms.Padding" /> that represents the internal spacing of the contents of a control.</returns>
        protected override Padding DefaultPadding
        {
            get
            {
                if (_borderStyle == UntiBorderStyle.None)
                    return new Padding(_borderWidth, _borderWidth, _borderWidth, _borderWidth);

                return new Padding(_borderWidth, _borderWidth + CAPTION_HEIGHT, _borderWidth, _borderWidth);
            }
        }

        #endregion

        #region Paint Methods

        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try // without ControlStyles.AllPaintingInWmPaint, we need our own error handling
            {
                // Clear all by white
                e.Graphics.Clear(_theme.FormBackColor);

                // Draw border
                float iborder = _borderWidth*2;
                using (var b = new LinearGradientBrush(new Point(0, 0), new Point(Width, Height),
                    _borderColor, Color.FromArgb(0xa4, 0xa4, 0xa4)))
                using (var p = new Pen(b, iborder))
                    e.Graphics.DrawRectangle(p, new Rectangle(0, 0, Width, Height));

                if (_borderStyle != UntiBorderStyle.None)
                {
                    // Draw Icon
                    e.Graphics.DrawIcon(Icon, _formIconPosition.X, _formIconPosition.Y);

                    // Draw Caption
                    using (var b = new SolidBrush(Color.Black))
                    {
                        var f = _theme.FormTitleFont;
                        e.Graphics.DrawString(Text, f, b, _formCaptionPosition);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Invalidate();
            }
        }

        #endregion

        #region Management Methods

        /// <summary>
        /// </summary>
        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
        protected override void WndProc(ref Message m)
        {
            // Not process in design mode
            if (DesignMode)
            {
                base.WndProc(ref m);
                return;
            }

            switch (m.Msg)
            {
                // See more in page 272, Windows Forms Programmng in C#, Chris Sells
                case Messages.WM_NCHITTEST:
                {
                    // Hit test a point
                    var ht = HitTestNca(m.HWnd, m.WParam, m.LParam);
                    if (ht != HitTest.HTCLIENT)
                    {
                        m.Result = (IntPtr)ht;
                        return;
                    }
                }
                break;

                // Prevent fullscreen because form border is none
                // Converted form http://www.codeproject.com/Articles/107994/Taskbar-with-Window-Maximized-and-WindowState-to-N
                case Messages.WM_GETMINMAXINFO:
                {
                    WmGetMinMaxInfo(m.HWnd, m.LParam);
                }
                break;

                case Messages.WM_SIZING:
                    OnWmSizing(m.WParam, m.LParam);
                    break;
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// Sent to a window in order to determine what part of the window corresponds to a particular screen coordinate. 
        /// </summary>
        /// <param name="hwnd">A handle to the window procedure that received the message.</param>
        /// <param name="wparam">This parameter is not used.</param>
        /// <param name="lparam">The low-order word specifies the x-coordinate of the cursor. The coordinate is relative to the upper-left corner of the screen.
        /// <br/>The high-order word specifies the y-coordinate of the cursor. The coordinate is relative to the upper-left corner of the screen.
        /// </param>
        /// <returns>The return value of the DefWindowProc function is one of the following values, indicating the position of the cursor hot spot</returns>
        private HitTest HitTestNca(IntPtr hwnd, IntPtr wparam, IntPtr lparam)
        {
            Point pc = PointToClient(new Point((int)lparam));

            // Prepair init value for compare
            var captionRec = new Rectangle(_borderWidth, _borderWidth, Width - _borderWidth * 2, CAPTION_HEIGHT);
            var topLeftRec = new Rectangle(0, 0, _borderWidth, _borderWidth);
            var topRec = new Rectangle(_borderWidth, 0, captionRec.Width, _borderWidth);
            var topRightRec = new Rectangle(topRec.Width + _borderWidth, 0, _borderWidth, _borderWidth);
            var rightRec = new Rectangle(topRec.Width + _borderWidth, _borderWidth, _borderWidth, Height - _borderWidth*2);
            var bottomRightRec = new Rectangle(topRec.Width + _borderWidth, rightRec.Height + _borderWidth, 
                _borderWidth, _borderWidth);
            var bottomRec = new Rectangle(_borderWidth, rightRec.Height + _borderWidth, topRec.Width, _borderWidth);
            var bottomLeftRec = new Rectangle(0, rightRec.Height + _borderWidth, _borderWidth, _borderWidth);
            var leftRec = new Rectangle(0, _borderWidth, _borderWidth, rightRec.Height);

            var sysmenuRec = new Rectangle(_formIconPosition, Icon.Size);

            if (_borderStyle == UntiBorderStyle.Sizable)
            {
                if (topLeftRec.Contains(pc))
                    return HitTest.HTTOPLEFT;
                if (topRec.Contains(pc))
                    return HitTest.HTTOP;
                if (topRightRec.Contains(pc))
                    return HitTest.HTTOPRIGHT;
                if (rightRec.Contains(pc))
                    return HitTest.HTRIGHT;
                if (bottomRightRec.Contains(pc))
                    return HitTest.HTBOTTOMRIGHT;
                if (bottomRec.Contains(pc))
                    return HitTest.HTBOTTOM;
                if (bottomLeftRec.Contains(pc))
                    return HitTest.HTBOTTOMLEFT;
                if (leftRec.Contains(pc))
                    return HitTest.HTLEFT;
            }

            if (sysmenuRec.Contains(pc))
                return HitTest.HTSYSMENU;

            if (captionRec.Contains(pc))
                return HitTest.HTCAPTION;

            return HitTest.HTCLIENT;
        }

        /// <summary>
        /// Wms the get minimum maximum information.
        /// </summary>
        /// <param name="hwnd">>A handle to the window procedure that received the message.</param>
        /// <param name="lParam">The lparameter.</param>
        private void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            var mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            var s = Screen.FromHandle(hwnd);
            mmi.MaxSize = s.WorkingArea.Size;
            mmi.MaxPosition.X = Math.Abs(s.WorkingArea.Left - s.Bounds.Left);
            mmi.MaxPosition.Y = Math.Abs(s.WorkingArea.Top - s.Bounds.Top);

            _minTrackSize.Width = Math.Max(_minTrackSize.Width, mmi.MinTrackSize.Width);
            _minTrackSize.Height = Math.Max(_minTrackSize.Height, mmi.MinTrackSize.Height);
            _minTrackSize = SizeFromClientSize(_minTrackSize);

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        private void OnWmSizing(IntPtr wParam, IntPtr lParam)
        {
            var rc = (RECT)Marshal.PtrToStructure(lParam, typeof(RECT));
            rc.Width = Math.Max(rc.Width, _minTrackSize.Width);
            rc.Height = Math.Max(rc.Height, _minTrackSize.Height);
            Marshal.StructureToPtr(rc, lParam, true);
        }
        #endregion

        #region Private Menthod

        private string GetButtonText(WindowButtons button)
        {
            switch (button)
            {
                case WindowButtons.Close: return "r";
                case WindowButtons.Minimize: return "0";
                case WindowButtons.Maximize: return WindowState == FormWindowState.Normal ? "1" : "2";
            }
            throw new InvalidOperationException();
        }

        private void WindowButton_Click(object sender, EventArgs e)
        {
            var btn = sender as UntiButton;
            if (btn == null) return;
            switch ((WindowButtons)btn.Tag)
            {
                case WindowButtons.Close:
                    Close();
                    return;
                case WindowButtons.Minimize:
                    WindowState = FormWindowState.Minimized;
                    return;
                case WindowButtons.Maximize:
                    WindowState = (WindowState == FormWindowState.Normal) ? FormWindowState.Maximized : FormWindowState.Normal;
                    btn.Text = GetButtonText(WindowButtons.Maximize);
                    return;
            }
            throw new InvalidOperationException();
        }

        private void AddWindowButton(WindowButtons button)
        {
            if (_windowButtons[(int)button] != null) throw new InvalidOperationException();

            var btnPostion = new Point {Y = _borderWidth};
            if ((int) button - 1 < 0)
            {
                btnPostion.X = Width - _borderWidth - WINBUTTON_WIDTH;
            }
            else
            {
                btnPostion.X = _windowButtons[(int) button - 1].Location.X - WINBUTTON_WIDTH;
            }

            var newButton = new UntiButton
            {
                Text = GetButtonText(button),
                Font = new Font("Webdings", 9.25f),
                Tag = button,
                Location = btnPostion,
                Size = new Size(WINBUTTON_WIDTH, WINBUTTON_WIDTH),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            if (button == WindowButtons.Close)
            {
                newButton.HoverColor = Color.FromArgb(0xe0, 0x43, 0x43);
                newButton.PressColor = Color.FromArgb(0x99, 0x3d, 0x3d);
            }

            newButton.Click += WindowButton_Click;
            Controls.Add(newButton);
            _windowButtons[(int)button] = newButton;
        }
        #endregion

        #region Constant Values

        /// <summary>
        /// The border width
        /// </summary>
        private const int BORDER_WIDTH = 5;

        /// <summary>
        /// The caption height
        /// </summary>
        private const int CAPTION_HEIGHT = 60;

        private const int WINBUTTON_WIDTH = 35;

        #endregion

        #region Privated Field

        /// <summary>
        /// The theme manager
        /// </summary>
        private ThemeManager _theme;

        /// <summary>
        /// The border color
        /// </summary>
        private Color _borderColor;

        /// <summary>
        /// The border width
        /// </summary>
        private int _borderWidth;

        /// <summary>
        /// The position of form's icon 
        /// </summary>
        private Point _formIconPosition;

        /// <summary>
        /// The position of form's caption 
        /// </summary>
        private PointF _formCaptionPosition;

        /// <summary>
        /// The form border style
        /// </summary>
        private UntiBorderStyle _borderStyle;

        /// <summary>
        /// The window buttons (include close, mini, maxi buttons)
        /// </summary>
        private readonly UntiButton[] _windowButtons = new UntiButton[3];

        /// <summary>
        /// The min track size
        /// </summary>
        private Size _minTrackSize;
        #endregion
    }
}
