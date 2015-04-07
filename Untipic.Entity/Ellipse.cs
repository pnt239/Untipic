namespace Untipic.Entity
{
    public class Ellipse : ShapeBase
    {
        /// <summary>
        /// Gets the type of the shape.
        /// </summary>
        /// <returns></returns>
        public override ShapeType GetShapeType()
        {
            return ShapeType.Ellipse;
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
            var shape = new Ellipse
            {
                Location = Location,
                Size = Size,
                DrawMethod = DrawMethod,
                OutlineColor = OutlineColor,
                OutlineWidth = OutlineWidth,
                FillColor = FillColor
            };
            return shape;
        }
    }
}
