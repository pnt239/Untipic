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

using System.Drawing;
using System.Drawing.Drawing2D;

namespace Untipic.Presentation
{
    /// <summary>
    /// 
    /// </summary>
    [System.Flags]
    public enum Corners
    {
        None = 0,
        TopLeft = 1,
        TopRight = 2,
        BottomLeft = 4,
        BottomRight = 8,
        All = TopLeft | TopRight | BottomLeft | BottomRight
    }

    /// <summary>
    /// Utility methods for draw or create shape.
    /// </summary>
    public class Drawer
    {
        /// <summary>
        /// Creates the leaf shape path
        /// </summary>
        /// <param name="rec">The boundary of leaf shape</param>
        /// <param name="round">The round edge of leaf</param>
        /// <returns>Then GraphicsPath of leaf shape</returns>
        public static GraphicsPath CreateLeaf(Rectangle rec, float round)
        {
            var path = new GraphicsPath();
            path.StartFigure();
            path.AddLine(rec.X, rec.Y + rec.Height, rec.X, rec.Y + round);
            path.AddArc(rec.X, rec.Y, round, round, 180F, 90F);
            path.AddLine(rec.X + round, rec.Y, rec.X + rec.Width, rec.Y);
            path.AddLine(rec.X + rec.Width, rec.Y, rec.X + rec.Width, rec.Y + rec.Height - round);
            path.AddArc(rec.X + rec.Width - round, rec.Y + rec.Height - round, round, round, 0F, 90F);
            path.CloseFigure();

            return path;
        }

        /// <summary>
        /// Draw a rounds the rectangle.
        /// </summary>
        /// <param name="r">The bound.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="corners">The corners.</param>
        /// <remarks>Source: http://dotnetrix.co.uk/button.htm </remarks>
        /// <returns></returns>
        public static GraphicsPath RoundRectangle(Rectangle r, int radius, Corners corners)
        {
            //Make sure the Path fits inside the rectangle
            r.Width -= 1;
            r.Height -= 1;

            //Scale the radius if it's too large to fit.
            if (radius > (r.Width))
                radius = r.Width;
            if (radius > (r.Height))
                radius = r.Height;

            var path = new GraphicsPath();
            path.StartFigure();
            if (radius <= 0)
                path.AddRectangle(r);
            else
            {
                if ((corners & Corners.TopLeft) == Corners.TopLeft)
                    path.AddArc(r.Left, r.Top, radius, radius, 180F, 90F);
                else
                    path.AddLine(r.Left, r.Top, r.Left, r.Top);

                if ((corners & Corners.TopRight) == Corners.TopRight)
                    path.AddArc(r.Right - radius, r.Top, radius, radius, 270F, 90F);
                else
                    path.AddLine(r.Right, r.Top, r.Right, r.Top);

                if ((corners & Corners.BottomRight) == Corners.BottomRight)
                    path.AddArc(r.Right - radius, r.Bottom - radius, radius, radius, 0, 90F);
                else
                    path.AddLine(r.Right, r.Bottom, r.Right, r.Bottom);

                if ((corners & Corners.BottomLeft) == Corners.BottomLeft)
                    path.AddArc(r.Left, r.Bottom - radius, radius, radius, 90F, 90F);
                else
                    path.AddLine(r.Left, r.Bottom, r.Left, r.Bottom);
            }
            
            path.CloseFigure();

            return path;
        }

        /// <summary>
        /// Rounds the image.
        /// </summary>
        /// <param name="startImage">The start image.</param>
        /// <param name="cornerRadius">The corner radius.</param>
        /// <param name="backgroundColor">Color of the background.</param>
        /// <remarks>Source: http://stackoverflow.com/questions/1758762/how-to-create-image-with-rounded-corners-in-c </remarks>
        /// <returns></returns>
        public static Image RoundImage(Image startImage, int cornerRadius, Color backgroundColor)
        {
            cornerRadius *= 2;
            var roundedImage = new Bitmap(startImage.Width, startImage.Height);
            Graphics g = Graphics.FromImage(roundedImage);
            g.Clear(backgroundColor);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Brush brush = new TextureBrush(startImage);
            var gp = new GraphicsPath();
            gp.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90);
            gp.AddArc(0 + roundedImage.Width - cornerRadius, 0, cornerRadius, cornerRadius, 270, 90);
            gp.AddArc(0 + roundedImage.Width - cornerRadius, 0 + roundedImage.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            gp.AddArc(0, 0 + roundedImage.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            g.FillPath(brush, gp);
            g.Dispose();
            return roundedImage;
        }
    }
}
