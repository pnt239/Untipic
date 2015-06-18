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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Untipic.Business;
using Untipic.Entity;
using Untipic.Presentation;
using Untipic.UI.UntiUI.EventArguments;

namespace Untipic.UI.UntiUI.DrawPad
{
    public partial class DrawPad : UserControl
    {
        public DrawPad()
        {
            InitializeComponent();

            Zoom = 1;

            _shapeDrawer = new ShapeDrawer();
            _drawingControl = new DrawingControl();
            _drawingControl.SetShapDrawer(_shapeDrawer);
            _drawingControl.ShapeCreated += DrawingControl_ShapeCreated;

            _currentCommand = DrawPadCommand.None;
            _currentShape = null;

            _outlineWidth = 2F;
            _outlineColor = Color.Black;
            _outlineDash = DashStyle.Solid;
            _fillColor = Color.Transparent;

            CreateNewPage(21F, 29.7F, MessureUnit.Cm, 37.62F);
        }

        public event MouseEventHandler GdiMouseMove = null;
        public event MouseEventHandler GdiAddedVertex = null;
        public event EventHandler GdiControlBoxLoad = null;
        public event EventHandler GdiControlBoxUpdated = null;
        public event CommandChangedEventHandler CommandChanged = null;
        public event ShapeCreatedEventHandler ShapeCreated = null;
        

        public ShapeDrawer ShapeDrawer
        { 
            get { return _shapeDrawer; }
        }

        public DrawingControl DrawingControl
        {
            get { return _drawingControl; }
        }

        public DrawPadCommand CurrentCommand
        {
            get { return _currentCommand; }
            set
            {
                _currentCommand = value;
                OnCommandChanged(new CommandChangedEventArgs(_currentCommand));
            }
        }

        public float Resolution { get; set; }

        public float Zoom { get; set; }

        public int ViewportWidth { get; set; }

        public int ViewportHeith { get; set; }

        public float OutlineWidth
        {
            get { return _outlineWidth; }
            set { _outlineWidth = value; }
        }

        public Color OutlineColor
        {
            get { return _outlineColor; }
            set { _outlineColor = value; }
        }

        public DashStyle OutlineDash
        {
            get { return _outlineDash; }
            set { _outlineDash = value; }
        }

        public Color FillColor
        {
            get { return _fillColor; }
            set { _fillColor = value; }
        }

        public Page Page { get { return _page; } }

        public event PaintEventHandler GdiPaint = null;

        public void ChangeTool(CommandObject command)
        {
            ShapeBase shape;
            switch (_currentCommand = command.Command)
            {
                case DrawPadCommand.Selection:
                    gdiArea.Cursor = _currentCursor = CursorTool.GetSelectionCursor();
                    break;
                case DrawPadCommand.DirectSelection:
                    gdiArea.Cursor = _currentCursor = CursorTool.GetDirectSelectionCursor();
                    break;
                case DrawPadCommand.DrawText:
                    gdiArea.Cursor = _currentCursor = CursorTool.GetEditorCursor();
                    break;
                case DrawPadCommand.DrawShape:
                    shape = command.Reserve as ShapeBase;
                    if (shape != null) _currentShape = shape.Clone();
                    gdiArea.Cursor = _currentCursor = CursorTool.GetShapeCursor();
                    // init in drawing control
                    _drawingControl.ControlMode = ControlMode.CreateShape;
                    _drawingControl.LoadShape(_currentShape);
                    // fire event for app manament
                    if (GdiControlBoxLoad != null)
                        GdiControlBoxLoad(this, EventArgs.Empty);
                    break;
                case DrawPadCommand.Brush:
                    shape = command.Reserve as ShapeBase;
                    if (shape != null) LoadCurrentShape(shape.Clone());

                    gdiArea.Cursor = _currentCursor = CursorTool.GetBrushCursor();

                    // init in drawing control
                    shape.OutlineWidth = _outlineWidth;
                    shape.OutlineColor = _outlineColor;
                    shape.OutlineDash = _outlineDash;
                    _drawingControl.ControlMode = ControlMode.CreateShape;
                    _drawingControl.LoadShape(_currentShape, true);
                    break;
                case DrawPadCommand.Eraser:
                    shape = command.Reserve as ShapeBase;
                    if (shape != null) LoadCurrentShape(shape.Clone());

                    gdiArea.Cursor = _currentCursor = CursorTool.GetEraserCursor();

                    _currentShape.OutlineColor = Color.White;
                    _drawingControl.ControlMode = ControlMode.CreateShape;
                    _drawingControl.LoadShape(_currentShape, true);
                    break;
                case DrawPadCommand.Bucket:
                    gdiArea.Cursor = _currentCursor = CursorTool.GetBucketCursor();
                    break;
                case DrawPadCommand.Crop:
                    gdiArea.Cursor = _currentCursor = CursorTool.GetCropCursor();
                    break;
            }
        }

        public void CreateNewPage(float winWidth, float winHeight, MessureUnit unit, float resolution)
        {
            _page = new Page(winWidth, winHeight, unit);
            Resolution = resolution;
            ViewportWidth = (int) WinToView(winWidth);
            ViewportHeith = (int) WinToView(winHeight);
            SetViewSize(ViewportWidth, ViewportHeith);

            _imageCache = new ImageCache(_shapeDrawer, _page, ViewportWidth, ViewportHeith);
        }

        public void SavePage(Stream stream, System.Drawing.Imaging.ImageFormat format)
        {
            _imageCache.SaveFile(stream, format);
        }

        public void SetViewSize(int width, int height)
        {
            //SuspendLayout();
            //tlpMain.SuspendLayout();

            //gdiArea.Size = size;
            tlpMain.RowStyles[1].Height = height;
            tlpMain.ColumnStyles[1].Width = width;
            //tlpMain.Width = size.Width + 40;
            //tlpMain.Height = size.Height + 40;

            AutoScrollMinSize = new Size(width + 40, height + 40);

            //tlpMain.ResumeLayout(false);
            //ResumeLayout(false);
        }

        public void RePaint()
        {
            gdiArea.Invalidate();
        }

        private void DrawPad_Load(object sender, EventArgs e)
        {
            ///
        }

        private void gdiArea_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.White);

            //DrawPage(e.Graphics);
            _imageCache.Render(e.Graphics);

            _drawingControl.Draw(e.Graphics);

            OnGdiPaint(sender, e);
        }

        private void gdiArea_Click(object sender, EventArgs e)
        {
            Focus();
        }

        private void gdiArea_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (_currentCommand)
                {
                    case DrawPadCommand.Brush:
                    case DrawPadCommand.Eraser:
                    case DrawPadCommand.DrawShape:
                        if (_currentShape.DrawMethod == DrawMethod.ByDragDrop)
                            _drawingControl.BeginCreateShape(e.Location);
                        break;
                    case DrawPadCommand.Selection:
                        if (_drawingControl.IsSelected)
                            if (!_drawingControl.BeginTranslation(e.Location))
                                OnCommandChanged(new CommandChangedEventArgs(DrawPadCommand.DrawShape));
                        break;

                }
            }
            gdiArea.Invalidate();
        }

        private void gdiArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Drag Mouse
                switch (_currentCommand)
                {
                    case DrawPadCommand.Brush:
                    case DrawPadCommand.Eraser:
                    case DrawPadCommand.DrawShape:
                        if (_currentShape.DrawMethod == DrawMethod.ByDragDrop && DrawingControl.IsInitializedShape)
                            _drawingControl.UpdateShape(e.Location);

                        if (GdiControlBoxUpdated != null)
                            GdiControlBoxUpdated(this, EventArgs.Empty);
                        break;
                    case DrawPadCommand.Selection:
                        if (_drawingControl.IsSelected)
                            _drawingControl.UpdateTranslation(e.Location);
                        break;
                }
            }
            gdiArea.Invalidate();

            if (_currentCommand == DrawPadCommand.DrawShape && _currentShape.DrawMethod == DrawMethod.ByClick)
            {
                _drawingControl.UpdateMouse(e.Location);
                gdiArea.Invalidate();
            }

            if (_drawingControl.IsSelected && _currentCommand == DrawPadCommand.Selection)
            {
                var hit = _drawingControl.GetHit(e.Location);
                if (hit != null)
                    gdiArea.Cursor = hit.Cursor;
                else
                    gdiArea.Cursor = _currentCursor;
            }
            //if (_drawingControl.IsSelected && !_hitMove)
            //{
            //    RecBoxBase box;
            //    if ((box = _drawingControl.HitTest(e.Location)) != null)
            //        gdiArea.Cursor = box.Cursor;
            //    else
            //        gdiArea.Cursor = _currentCursor;
            //}

            OnGdiMouseMove(e);
        }

        private void gdiArea_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (_currentCommand)
                {
                    case DrawPadCommand.Brush:
                    case DrawPadCommand.Eraser:
                    case DrawPadCommand.DrawShape:
                        if (_currentShape.DrawMethod == DrawMethod.ByDragDrop && _drawingControl.IsInitializedShape)
                        {
                            _drawingControl.EndCreateShape();
                            // Auto update command to selection
                            OnCommandChanged(new CommandChangedEventArgs(DrawPadCommand.Selection));
                        }
                        else if (_currentShape.DrawMethod == DrawMethod.ByClick)
                        {
                            if (!_drawingControl.CreateVertext(e.Location)) 
                            {
                                _drawingControl.EndCreateShape();
                                // Auto update command to selection
                                OnCommandChanged(new CommandChangedEventArgs(DrawPadCommand.Selection));
                            }
                            else
                                if (GdiAddedVertex != null) GdiAddedVertex(this, e);
                        }
                        break;
                    case DrawPadCommand.Selection:
                        if (_drawingControl.IsSelected)
                            _drawingControl.EndTranslation();
                        break;
                }
                gdiArea.Invalidate();
            } 
            else if (e.Button == MouseButtons.Right)
            {
                switch (_currentCommand)
                {
                    case DrawPadCommand.DrawShape:
                        if (_currentShape.DrawMethod == DrawMethod.ByClick)
                            _drawingControl.EndCreateShape();
                        break;
                }
                
                gdiArea.Invalidate();
            }
        }

        private void gdiArea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift)
                _drawingControl.IsRegularShape = true;
        }

        private void gdiArea_KeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Shift)
                _drawingControl.IsRegularShape = false;
        }

        private void DrawingControl_ShapeCreated(object sender, ShapeCreatedEventArgs e)
        {
            var shape = e.Shape;
            if (!_drawingControl.IsDirectlyUsing)
            {
                shape.FillColor = _fillColor;
                shape.OutlineWidth = _outlineWidth;
                shape.OutlineColor = _outlineColor;
                shape.OutlineDash = _outlineDash;
            }

            if (ShapeCreated != null)
                ShapeCreated(this, new ShapeCreatedEventArgs(shape));
            //_page.AddDrawingObject(shape);
        }

        protected virtual void OnGdiPaint(object sender, PaintEventArgs e)
        {
            if (GdiPaint != null)
                GdiPaint(sender, e);
        }

        private void OnGdiMouseMove(MouseEventArgs e)
        {
            if (GdiMouseMove != null)
                GdiMouseMove(this, e);
        }

        private void OnCommandChanged(CommandChangedEventArgs e)
        {
            if (CommandChanged != null)
                CommandChanged(this, e);
        }

        private void LoadCurrentShape(ShapeBase shape)
        {
            _currentShape = shape;
            _currentShape.OutlineWidth = _outlineWidth;
            _currentShape.OutlineColor = _outlineColor;
            _currentShape.FillColor = _fillColor;
        }

        private void DrawPage(Graphics g)
        {
            foreach (var obj in _page.DrawingObjects)
                if (obj.GetObjectType() == DrawingObjectType.Shape)
                {
                    var shape = (ShapeBase) obj;
                    _shapeDrawer.Draw(shape, g);
                }
        }

        private float WinToView(float value)
        {
            return RoundToInt(value*Resolution*Zoom);
        }

        private float RoundToInt(float value)
        {
            return (float)Math.Round(value);
        }

        private Page _page;

        private DrawingControl _drawingControl;
        private DrawPadCommand _currentCommand;
        private ShapeBase _currentShape;
        private Cursor _currentCursor;

        private ShapeDrawer _shapeDrawer;
        private ImageCache _imageCache;

        private float _outlineWidth;
        private Color _outlineColor;
        private DashStyle _outlineDash;
        private Color _fillColor;
    }
}
