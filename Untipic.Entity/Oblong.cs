namespace Untipic.Entity
{
    public class Oblong : PolygonBase
    {
        public Oblong()
        {
            IsClosedFigure = true;

            Vertices.Add();
            Vertices.Add();
            Vertices.Add();
            Vertices.Add();
        }

        public override ShapeType GetShapeType()
        {
            return ShapeType.Oblong;
        }

        public override ShapeBase Clone()
        {
            var shape = new Oblong
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
