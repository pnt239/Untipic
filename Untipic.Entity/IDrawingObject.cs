using System.Drawing;

namespace Untipic.Entity
{
    public enum DrawingObjectType
    {
        Shape = 0,
        Text,
        Image
    }

    public interface IDrawingObject 
    {
        PointF Location { get; set; }

        SizeF Size { get; set; }

        DrawingObjectType GetObjectType();
    }
}
