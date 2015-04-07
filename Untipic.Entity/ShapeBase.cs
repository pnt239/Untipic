using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Untipic.Entity
{
    public enum ShapeType
    {
        Line = 0,
        Bezier,
        IsoscelesTriangle,
        Oblong,
        Polygon,
        Ellipse,
        FreePencil
    }

    public enum DrawMethod
    {
        ByDragDrop = 0,
        ByClick
    }

    public abstract class ShapeBase : IDrawingObject
    {
        protected ShapeBase()
        {
            _vertices = new VertexCollection();

            DrawMethod = DrawMethod.ByDragDrop;
            CanResize = true;
            CanMove = true;
            Location = new PointF();
            Size = new SizeF();
            OutlineColor = Color.Black;
            OutlineWidth = 1F;
            FillColor = Color.Transparent;
        }

        /// <summary>
        /// Gets or sets the vertices.
        /// </summary>
        /// <value>
        /// The vertices.
        /// </value>
        public VertexCollection Vertices
        {
            get { return _vertices; } 
            set { _vertices = value; }
        }

        /// <summary>
        /// Gets or sets the position of the shape on paint area.
        /// </summary>
        /// <value>
        /// The position of the shape on paint area.
        /// </value>
        public PointF Location { get; set; }

        /// <summary>
        /// Gets or sets the size of shape.
        /// </summary>
        /// <value>
        /// The size of shape.
        /// </value>
        public SizeF Size { get; set; }

        public DrawingObjectType GetObjectType()
        {
            return DrawingObjectType.Shape;
        }

        /// <summary>
        /// Gets or sets the color of the outline.
        /// </summary>
        /// <value>
        /// The color of the outline.
        /// </value>
        public Color OutlineColor { get; set; }

        /// <summary>
        /// Gets or sets the width of the ountline.
        /// </summary>
        /// <value>
        /// The width of the ountline.
        /// </value>
        public float OutlineWidth { get; set; }

        /// <summary>
        /// Gets or sets the outline dash.
        /// </summary>
        /// <value>
        /// The outline dash.
        /// </value>
        public DashStyle OutlineDash { get; set; }

        /// <summary>
        /// Gets or sets the color of the fill.
        /// </summary>
        /// <value>
        /// The color of the fill.
        /// </value>
        public Color FillColor { get; set; }

        /// <summary>
        /// Gets or sets the draw method.
        /// </summary>
        /// <value>
        /// The draw method.
        /// </value>
        public DrawMethod DrawMethod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can resize.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can resize; otherwise, <c>false</c>.
        /// </value>
        public Boolean CanResize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can move.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can move; otherwise, <c>false</c>.
        /// </value>
        public Boolean CanMove { get; set; }

        /// <summary>
        /// Gets the type of the shape.
        /// </summary>
        /// <returns></returns>
        public abstract ShapeType GetShapeType();

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>The same shape</returns>
        public abstract ShapeBase Clone();

        private VertexCollection _vertices;
    }
}
