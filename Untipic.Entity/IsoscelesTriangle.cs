namespace Untipic.Entity
{
    public class IsoscelesTriangle : PolygonBase
    {
        public IsoscelesTriangle()
        {
            IsClosedFigure = true;

            Vertices.Add();
            Vertices.Add();
            Vertices.Add();
        }

        public override ShapeType GetShapeType()
        {
            return ShapeType.IsoscelesTriangle;
        }

        public override ShapeBase Clone()
        {
            var shape = new IsoscelesTriangle
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
