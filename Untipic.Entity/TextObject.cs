using System.Drawing;

namespace Untipic.Entity
{
    public class TextObject : IDrawingObject
    {
        public PointF Location { get; set; }

        public SizeF Size { get; set; }

        public DrawingObjectType GetObjectType()
        {
            return DrawingObjectType.Text;
        }
    }
}
