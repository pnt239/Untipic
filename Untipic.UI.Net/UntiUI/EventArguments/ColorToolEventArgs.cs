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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Untipic.EventArguments
{
    public class ColorToolEventArgs : EventArgs
    {
        public ColorToolEventArgs(Color color, float width)
        {
            Color = color;
            Width = width;
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// The color of outline/fill
        /// </summary>
        private Color _color;

        /// <summary>
        /// The width of outline
        /// </summary>
        private float _width;
    }
}
