namespace Untipic.Entity
{
    public class ShapeFactory
    {
        public static ShapeBase CreateShape(ShapeType shapeType)
        {
            switch (shapeType)
            {
                case ShapeType.Line:
                    return new Line();
                case ShapeType.IsoscelesTriangle:
                    return new IsoscelesTriangle();
                case ShapeType.Oblong:
                    return new Oblong();
                case ShapeType.Polygon:
                    return new Polygon();
                case ShapeType.Ellipse:
                    return new Ellipse();
            }
            return null;
        }
    }
}
