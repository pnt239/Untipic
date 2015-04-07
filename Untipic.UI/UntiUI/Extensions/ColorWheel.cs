﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Untipic.Presentation;

#if USEEXTERNALCYOTEKLIBS
using Cyotek.Drawing;

#endif

namespace Untipic.UI.UntiUI.Extensions
{
  // Cyotek Color Picker controls library
  // Copyright © 2013-2014 Cyotek.
  // http://cyotek.com/blog/tag/colorpicker

  // Licensed under the MIT License. See colorpicker-license.txt for the full text.

  // If you use this code in your applications, donations or attribution are welcome

  [DefaultProperty("Color")]
  [DefaultEvent("ColorChanged")]
  public class ColorWheel : Control, IColorEditor
  {
    #region Instance Fields

    private Brush _brush;

    private PointF _centerPoint;

    private Color _color;

    private int _colorStep;

    private bool _dragStartedWithinWheel;

    private HslColor _hslColor;

    private int _largeChange;

    private float _radius;

    private int _selectionSize;

    private int _smallChange;

    private int _updateCount;

    #endregion

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorWheel"/> class.
    /// </summary>
    public ColorWheel()
    {
      this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Selectable | ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, true);
      this.Color = Color.Black;
      this.ColorStep = 4;
      this.SelectionSize = 10;
      this.SmallChange = 1;
      this.LargeChange = 5;
      this.SelectionGlyph = this.CreateSelectionGlyph();
    }

    #endregion

    #region Events

    /// <summary>
    /// Occurs when the Color property value changes
    /// </summary>
    [Category("Property Changed")]
    public event EventHandler ColorChanged;

    /// <summary>
    /// Occurs when the ColorStep property value changes
    /// </summary>
    [Category("Property Changed")]
    public event EventHandler ColorStepChanged;

    /// <summary>
    /// Occurs when the HslColor property value changes
    /// </summary>
    [Category("Property Changed")]
    public event EventHandler HslColorChanged;

    /// <summary>
    /// Occurs when the LargeChange property value changes
    /// </summary>
    [Category("Property Changed")]
    public event EventHandler LargeChangeChanged;

    /// <summary>
    /// Occurs when the SelectionSize property value changes
    /// </summary>
    [Category("Property Changed")]
    public event EventHandler SelectionSizeChanged;

    /// <summary>
    /// Occurs when the SmallChange property value changes
    /// </summary>
    [Category("Property Changed")]
    public event EventHandler SmallChangeChanged;

    #endregion

    #region Overridden Properties

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Font Font
    {
      get { return base.Font; }
      set { base.Font = value; }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Color ForeColor
    {
      get { return base.ForeColor; }
      set { base.ForeColor = value; }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override string Text
    {
      get { return base.Text; }
      set { base.Text = value; }
    }

    #endregion

    #region Overridden Methods

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" /> and its child controls and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (_brush != null)
        {
          _brush.Dispose();
        }

        if (this.SelectionGlyph != null)
        {
          this.SelectionGlyph.Dispose();
        }
      }

      base.Dispose(disposing);
    }

    /// <summary>
    /// Determines whether the specified key is a regular input key or a special key that requires preprocessing.
    /// </summary>
    /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values.</param>
    /// <returns>true if the specified key is a regular input key; otherwise, false.</returns>
    protected override bool IsInputKey(Keys keyData)
    {
      bool result;

      if ((keyData & Keys.Left) == Keys.Left || (keyData & Keys.Up) == Keys.Up || (keyData & Keys.Down) == Keys.Down || (keyData & Keys.Right) == Keys.Right || (keyData & Keys.PageUp) == Keys.PageUp || (keyData & Keys.PageDown) == Keys.PageDown)
      {
        result = true;
      }
      else
      {
        result = base.IsInputKey(keyData);
      }

      return result;
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnGotFocus(EventArgs e)
    {
      base.OnGotFocus(e);

      this.Invalidate();
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.KeyDown" /> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
    protected override void OnKeyDown(KeyEventArgs e)
    {
      HslColor color;
      double hue;
      int step;

      color = this.HslColor;
      hue = color.H;

      step = e.Shift ? this.LargeChange : this.SmallChange;

      switch (e.KeyCode)
      {
        case Keys.Right:
        case Keys.Up:
          hue += step;
          break;
        case Keys.Left:
        case Keys.Down:
          hue -= step;
          break;
        case Keys.PageUp:
          hue += this.LargeChange;
          break;
        case Keys.PageDown:
          hue -= this.LargeChange;
          break;
      }

      if (hue >= 360)
      {
        hue = 0;
      }
      if (hue < 0)
      {
        hue = 359;
      }

      if (hue != color.H)
      {
        color.H = hue;

        // As the Color and HslColor properties update each other, need to temporarily disable this and manually set both
        // otherwise the wheel "sticks" due to imprecise conversion from RGB to HSL
        this.LockUpdates = true;
        this.Color = color.ToRgbColor();
        this.HslColor = color;
        this.LockUpdates = false;

        e.Handled = true;
      }

      base.OnKeyDown(e);
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.LostFocus" /> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnLostFocus(EventArgs e)
    {
      base.OnLostFocus(e);

      this.Invalidate();
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
    protected override void OnMouseDown(MouseEventArgs e)
    {
      base.OnMouseDown(e);

      if (!this.Focused && this.TabStop)
      {
        this.Focus();
      }

      if (e.Button == MouseButtons.Left && this.IsPointInWheel(e.Location))
      {
        _dragStartedWithinWheel = true;
        this.SetColor(e.Location);
      }
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);

      if (e.Button == MouseButtons.Left && _dragStartedWithinWheel)
      {
        this.SetColor(e.Location);
      }
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"/> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data. </param>
    protected override void OnMouseUp(MouseEventArgs e)
    {
      base.OnMouseUp(e);

      _dragStartedWithinWheel = false;
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.PaddingChanged" /> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnPaddingChanged(EventArgs e)
    {
      base.OnPaddingChanged(e);

      this.RefreshWheel();
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);

      if (this.AllowPainting)
      {
        base.OnPaintBackground(e); // HACK: Easiest way of supporting things like BackgroundImage, BackgroundImageLayout etc

        // if the parent is using a transparent color, it's likely to be something like a TabPage in a tab control
        // so we'll draw the parent background instead, to avoid having an ugly solid color
        if (this.BackgroundImage == null && this.Parent != null && (this.BackColor == this.Parent.BackColor || this.Parent.BackColor.A != 255))
        {
          ButtonRenderer.DrawParentBackground(e.Graphics, this.DisplayRectangle, this);
        }

        if (_brush != null)
        {
          e.Graphics.FillPie(_brush, this.ClientRectangle, 0, 360);
        }
        // HACK: smooth out the edge of the wheel.
        // https://github.com/cyotek/Cyotek.Windows.Forms.ColorPicker/issues/1 - the linked source doesn't do this hack yet draws with a smoother edge
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (Pen pen = new Pen(this.BackColor, 2))
        {
          e.Graphics.DrawEllipse(pen, new RectangleF(_centerPoint.X - _radius, _centerPoint.Y - _radius, _radius * 2, _radius * 2));
        }

        this.PaintCurrentColor(e);
      }
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);

      this.RefreshWheel();
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets the component color.
    /// </summary>
    /// <value>The component color.</value>
    [Category("Appearance")]
    [DefaultValue(typeof(Color), "Black")]
    public virtual Color Color
    {
      get { return _color; }
      set
      {
        if (this.Color != value)
        {
          _color = value;

          this.OnColorChanged(EventArgs.Empty);
        }
      }
    }

    /// <summary>
    /// Gets or sets the increment for rendering the color wheel.
    /// </summary>
    /// <value>The color step.</value>
    /// <exception cref="System.ArgumentOutOfRangeException">Value must be between 1 and 359</exception>
    [Category("Appearance")]
    [DefaultValue(4)]
    public virtual int ColorStep
    {
      get { return _colorStep; }
      set
      {
        if (value < 1 || value > 359)
        {
          throw new ArgumentOutOfRangeException("value", value, "Value must be between 1 and 359");
        }

        if (this.ColorStep != value)
        {
          _colorStep = value;

          this.OnColorStepChanged(EventArgs.Empty);
        }
      }
    }

    /// <summary>
    /// Gets or sets the component color.
    /// </summary>
    /// <value>The component color.</value>
    [Category("Appearance")]
    [DefaultValue(typeof(HslColor), "0, 0, 0")]
    [Browsable(false) /* disable editing until I write a proper type convertor */]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual HslColor HslColor
    {
      get { return _hslColor; }
      set
      {
        if (this.HslColor != value)
        {
          _hslColor = value;

          this.OnHslColorChanged(EventArgs.Empty);
        }
      }
    }

    /// <summary>
    /// Gets or sets a value to be added to or subtracted from the <see cref="Color"/> property when the wheel selection is moved a large distance.
    /// </summary>
    /// <value>A numeric value. The default value is 5.</value>
    [Category("Behavior")]
    [DefaultValue(5)]
    public virtual int LargeChange
    {
      get { return _largeChange; }
      set
      {
        if (this.LargeChange != value)
        {
          _largeChange = value;

          this.OnLargeChangeChanged(EventArgs.Empty);
        }
      }
    }

    /// <summary>
    /// Gets or sets the size of the selection handle.
    /// </summary>
    /// <value>The size of the selection handle.</value>
    [Category("Appearance")]
    [DefaultValue(10)]
    public virtual int SelectionSize
    {
      get { return _selectionSize; }
      set
      {
        if (this.SelectionSize != value)
        {
          _selectionSize = value;

          this.OnSelectionSizeChanged(EventArgs.Empty);
        }
      }
    }

    /// <summary>
    /// Gets or sets a value to be added to or subtracted from the <see cref="Color"/> property when the wheel selection is moved a small distance.
    /// </summary>
    /// <value>A numeric value. The default value is 1.</value>
    [Category("Behavior")]
    [DefaultValue(1)]
    public virtual int SmallChange
    {
      get { return _smallChange; }
      set
      {
        if (this.SmallChange != value)
        {
          _smallChange = value;

          this.OnSmallChangeChanged(EventArgs.Empty);
        }
      }
    }

    #endregion

    #region Protected Properties

    /// <summary>
    ///   Gets a value indicating whether painting of the control is allowed.
    /// </summary>
    /// <value>
    ///   <c>true</c> if painting of the control is allowed; otherwise, <c>false</c>.
    /// </value>
    protected virtual bool AllowPainting
    {
      get { return _updateCount == 0; }
    }

    protected Color[] Colors { get; set; }

    protected bool LockUpdates { get; set; }

    protected PointF[] Points { get; set; }

    protected Image SelectionGlyph { get; set; }

    #endregion

    #region Public Members

    /// <summary>
    ///   Disables any redrawing of the image box
    /// </summary>
    public virtual void BeginUpdate()
    {
      _updateCount++;
    }

    /// <summary>
    ///   Enables the redrawing of the image box
    /// </summary>
    public virtual void EndUpdate()
    {
      if (_updateCount > 0)
      {
        _updateCount--;
      }

      if (this.AllowPainting)
      {
        this.Invalidate();
      }
    }

    #endregion

    #region Protected Members

    /// <summary>
    /// Calculates wheel attributes.
    /// </summary>
    protected virtual void CalculateWheel()
    {
      List<PointF> points;
      List<Color> colors;

      points = new List<PointF>();
      colors = new List<Color>();

      // Only define the points if the control is above a minimum size, otherwise if it's too small, you get an "out of memory" exceptions (of all things) when creating the brush
      if (this.ClientSize.Width > 16 && this.ClientSize.Height > 16)
      {
        int w;
        int h;

        w = this.ClientSize.Width;
        h = this.ClientSize.Height;

        _centerPoint = new PointF(w / 2.0F, h / 2.0F);
        _radius = this.GetRadius(_centerPoint);

        for (double angle = 0; angle < 360; angle += this.ColorStep)
        {
          double angleR;
          PointF location;

          angleR = angle * (Math.PI / 180);
          location = this.GetColorLocation(angleR, _radius);

          points.Add(location);
          colors.Add(new HslColor(angle, 1, 0.5).ToRgbColor());
        }
      }

      this.Points = points.ToArray();
      this.Colors = colors.ToArray();
    }

    /// <summary>
    /// Creates the gradient brush used to paint the wheel.
    /// </summary>
    protected virtual Brush CreateGradientBrush()
    {
      Brush result;

      if (this.Points.Length != 0 && this.Points.Length == this.Colors.Length)
      {
        result = new PathGradientBrush(this.Points, WrapMode.Clamp)
                 {
                   CenterPoint = _centerPoint,
                   CenterColor = Color.White,
                   SurroundColors = this.Colors
                 };
      }
      else
      {
        result = null;
      }

      return result;
    }

    /// <summary>
    /// Creates the selection glyph.
    /// </summary>
    protected virtual Image CreateSelectionGlyph()
    {
      Image image;
      int halfSize;

      halfSize = this.SelectionSize / 2;
      image = new Bitmap(this.SelectionSize + 1, this.SelectionSize + 1, PixelFormat.Format32bppArgb);

      using (Graphics g = Graphics.FromImage(image))
      {
        Point[] diamondOuter;

        diamondOuter = new[]
                       {
                         new Point(halfSize, 0), new Point(this.SelectionSize, halfSize), new Point(halfSize, this.SelectionSize), new Point(0, halfSize)
                       };

        g.FillPolygon(SystemBrushes.Control, diamondOuter);
        g.DrawPolygon(SystemPens.ControlDark, diamondOuter);

        using (Pen pen = new Pen(Color.FromArgb(128, SystemColors.ControlDark)))
        {
          g.DrawLine(pen, halfSize, 1, this.SelectionSize - 1, halfSize);
          g.DrawLine(pen, halfSize, 2, this.SelectionSize - 2, halfSize);
          g.DrawLine(pen, halfSize, this.SelectionSize - 1, this.SelectionSize - 2, halfSize + 1);
          g.DrawLine(pen, halfSize, this.SelectionSize - 2, this.SelectionSize - 3, halfSize + 1);
        }

        using (Pen pen = new Pen(Color.FromArgb(196, SystemColors.ControlLightLight)))
        {
          g.DrawLine(pen, halfSize, this.SelectionSize - 1, 1, halfSize);
        }

        g.DrawLine(SystemPens.ControlLightLight, 1, halfSize, halfSize, 1);
      }

      return image;
    }

    /// <summary>
    /// Gets the point within the wheel representing the source color.
    /// </summary>
    /// <param name="color">The color.</param>
    protected PointF GetColorLocation(Color color)
    {
      return this.GetColorLocation(new HslColor(color));
    }

    /// <summary>
    /// Gets the point within the wheel representing the source color.
    /// </summary>
    /// <param name="color">The color.</param>
    protected virtual PointF GetColorLocation(HslColor color)
    {
      double angle;
      double radius;

      angle = color.H * Math.PI / 180;
      ;
      radius = _radius * color.S;

      return this.GetColorLocation(angle, radius);
    }

    protected PointF GetColorLocation(double angleR, double radius)
    {
      double x;
      double y;

      x = this.Padding.Left + _centerPoint.X + Math.Cos(angleR) * radius;
      y = this.Padding.Top + _centerPoint.Y - Math.Sin(angleR) * radius;

      return new PointF((float)x, (float)y);
    }

    protected float GetRadius(PointF centerPoint)
    {
      return Math.Min(centerPoint.X, centerPoint.Y) - (Math.Max(this.Padding.Horizontal, this.Padding.Vertical) + (this.SelectionSize / 2));
    }

    /// <summary>
    /// Determines whether the specified point is within the bounds of the color wheel.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <returns><c>true</c> if the specified point is within the bounds of the color wheel; otherwise, <c>false</c>.</returns>
    protected bool IsPointInWheel(Point point)
    {
      PointF normalized;

      // http://my.safaribooksonline.com/book/programming/csharp/9780672331985/graphics-with-windows-forms-and-gdiplus/ch17lev1sec21

      normalized = new PointF(point.X - _centerPoint.X, point.Y - _centerPoint.Y);

      return (normalized.X * normalized.X + normalized.Y * normalized.Y) <= (_radius * _radius);
    }

    /// <summary>
    /// Raises the <see cref="ColorChanged" /> event.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    protected virtual void OnColorChanged(EventArgs e)
    {
      EventHandler handler;

      if (!this.LockUpdates)
      {
        this.HslColor = new HslColor(this.Color);
      }
      this.Refresh();

      handler = this.ColorChanged;

      if (handler != null)
      {
        handler(this, e);
      }
    }

    /// <summary>
    /// Raises the <see cref="ColorStepChanged" /> event.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    protected virtual void OnColorStepChanged(EventArgs e)
    {
      EventHandler handler;

      this.RefreshWheel();

      handler = this.ColorStepChanged;

      if (handler != null)
      {
        handler(this, e);
      }
    }

    /// <summary>
    /// Raises the <see cref="HslColorChanged" /> event.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    protected virtual void OnHslColorChanged(EventArgs e)
    {
      EventHandler handler;

      if (!this.LockUpdates)
      {
        this.Color = this.HslColor.ToRgbColor();
      }
      this.Invalidate();

      handler = this.HslColorChanged;

      if (handler != null)
      {
        handler(this, e);
      }
    }

    /// <summary>
    /// Raises the <see cref="LargeChangeChanged" /> event.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    protected virtual void OnLargeChangeChanged(EventArgs e)
    {
      EventHandler handler;

      handler = this.LargeChangeChanged;

      if (handler != null)
      {
        handler(this, e);
      }
    }

    /// <summary>
    /// Raises the <see cref="SelectionSizeChanged" /> event.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    protected virtual void OnSelectionSizeChanged(EventArgs e)
    {
      EventHandler handler;

      if (this.SelectionGlyph != null)
      {
        this.SelectionGlyph.Dispose();
      }

      this.SelectionGlyph = this.CreateSelectionGlyph();
      this.RefreshWheel();

      handler = this.SelectionSizeChanged;

      if (handler != null)
      {
        handler(this, e);
      }
    }

    /// <summary>
    /// Raises the <see cref="SmallChangeChanged" /> event.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    protected virtual void OnSmallChangeChanged(EventArgs e)
    {
      EventHandler handler;

      handler = this.SmallChangeChanged;

      if (handler != null)
      {
        handler(this, e);
      }
    }

    protected virtual void PaintCurrentColor(PaintEventArgs e)
    {
      if (!this.Color.IsEmpty)
      {
        PointF location;

        location = this.GetColorLocation(this.HslColor);

        if (!float.IsNaN(location.X) && !float.IsNaN(location.Y))
        {
          int x;
          int y;

          x = (int)location.X - (this.SelectionSize / 2);
          y = (int)location.Y - (this.SelectionSize / 2);

          if (this.SelectionGlyph == null)
          {
            e.Graphics.DrawRectangle(Pens.Black, x, y, this.SelectionSize, this.SelectionSize);
          }
          else
          {
            e.Graphics.DrawImage(this.SelectionGlyph, x, y);
          }

          if (this.Focused)
          {
            ControlPaint.DrawFocusRectangle(e.Graphics, new Rectangle(x - 1, y - 1, this.SelectionSize + 2, this.SelectionSize + 2));
          }
        }
      }
    }

    protected virtual void SetColor(Point point)
    {
      double dx;
      double dy;
      double angle;
      double distance;
      double saturation;

      dx = Math.Abs(point.X - _centerPoint.X - this.Padding.Left);
      dy = Math.Abs(point.Y - _centerPoint.Y - this.Padding.Top);
      angle = Math.Atan(dy / dx) / Math.PI * 180;
      distance = Math.Pow((Math.Pow(dx, 2) + (Math.Pow(dy, 2))), 0.5);
      saturation = distance / _radius;

      if (distance < 6)
      {
        saturation = 0; // snap to center
      }

      if (point.X < _centerPoint.X)
      {
        angle = 180 - angle;
      }
      if (point.Y > _centerPoint.Y)
      {
        angle = 360 - angle;
      }

      this.LockUpdates = true;
      this.HslColor = new HslColor(angle, saturation, 0.5);
      this.Color = this.HslColor.ToRgbColor();
      this.LockUpdates = false;
    }

    #endregion

    #region Private Members

    /// <summary>
    /// Refreshes the wheel attributes and then repaints the control
    /// </summary>
    private void RefreshWheel()
    {
      if (_brush != null)
      {
        _brush.Dispose();
      }

      this.CalculateWheel();
      _brush = this.CreateGradientBrush();
      this.Invalidate();
    }

    #endregion
  }
}
