namespace Untipic.Entity
{
    public class Polygon : PolygonBase
    {
        public Polygon()
        {
            CanResize = false;
            CanMove = false;
        }
        /// <summary>
        /// Gets the type of the shape.
        /// </summary>
        /// <returns></returns>
        public override ShapeType GetShapeType()
        {
            return ShapeType.Polygon;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>
        /// The same shape
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override ShapeBase Clone()
        {
            var shape = new Polygon
            {
                Location = Location,
                Size = Size,
                DrawMethod = DrawMethod,
                OutlineColor = OutlineColor,
                OutlineWidth = OutlineWidth,
                FillColor = FillColor,
                IsClosedFigure = IsClosedFigure,
                Vertices = (VertexCollection)Vertices.Clone()
            };
            return shape;
        }
    }
}
