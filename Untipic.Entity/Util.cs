using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace Untipic.Entity
{
    public enum MessureUnit
    {
        Cm = 0,
        Inch,
        Pixels
    }

    public class Util
    {
        public static RectangleF GetShapeBoundF(ShapeBase shape)
        {
            return new RectangleF(shape.Location, shape.Size);
        }

        public static Rectangle GetShapeBound(ShapeBase shape)
        {
            return new Rectangle(Point.Round(shape.Location), Size.Round(shape.Size));
        }

        public static double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public static void WriteColor(BinaryWriter writer, Color color)
        {
            writer.Write((byte)color.A);
            writer.Write((byte)color.R);
            writer.Write((byte)color.G);
            writer.Write((byte)color.B);
        }

        public static void WriteVertex(BinaryWriter writer, IVertex vertex)
        {
            writer.Write(vertex.X);
            writer.Write(vertex.Y);
        }

        public static void WritePoint(BinaryWriter writer, PointF point)
        {
            writer.Write(point.X);
            writer.Write(point.Y);
        }

        public static void WriteSize(BinaryWriter writer, SizeF size)
        {
            writer.Write(size.Width);
            writer.Write(size.Height);
        }

        public static Color ReadColor(BinaryReader reader)
        {
            byte a = reader.ReadByte();
            byte r = reader.ReadByte();
            byte g = reader.ReadByte();
            byte b = reader.ReadByte();
            return Color.FromArgb(a, r, g, b);
        }

        public static Vertex ReadVertex(BinaryReader reader)
        {
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            return new Vertex(x, y);
        }

        public static PointF ReadPoint(BinaryReader reader)
        {
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            return new PointF(x, y);
        }

        public static SizeF ReadSize(BinaryReader reader)
        {
            float w = reader.ReadSingle();
            float h = reader.ReadSingle();
            return new SizeF(w, h);
        }

        public static void SaveDrawingObject(BinaryWriter writer, IDrawingObject obj)
        {
            var type = (Int32)obj.GetObjectType();
            // write object type
            writer.Write(type);

            switch (obj.GetObjectType())
            {
                case DrawingObjectType.Shape:
                {
                    var shape = (ShapeBase) obj;
                    var shapeType = shape.GetShapeType();
                    // write shape type
                    writer.Write((Int32) shapeType);
                    // write shape location
                    WritePoint(writer, shape.Location);
                    // write shape size
                    WriteSize(writer, shape.Size);
                    // write shape outline color
                    WriteColor(writer, shape.OutlineColor);
                    // write shape outline width
                    writer.Write(shape.OutlineWidth);
                    // write shape outline dash
                    writer.Write((Int32) shape.OutlineDash);
                    // write shape fill color
                    WriteColor(writer, shape.FillColor);

                    if (shapeType != ShapeType.Ellipse)
                    {
                        // write count vertex
                        writer.Write((Int32)shape.Vertices.Count);
                        foreach (var vertex in shape.Vertices)
                            WriteVertex(writer, vertex);
                    }
                }
                    break;
            }
        }

        public static IDrawingObject ReadDrawingObject(BinaryReader reader)
        {
            // read object type
            IDrawingObject obj = null;
            int type = reader.ReadInt32();

            switch ((DrawingObjectType)type)
            {
                case DrawingObjectType.Shape:
                    {
                        // read shape type
                        int shapeType = reader.ReadInt32();
                        ShapeBase shape;

                        if ((ShapeType) shapeType == ShapeType.Ellipse)
                            shape = new Ellipse();
                        else
                            shape = new FreePencil();
                        //var shape = ShapeFactory.CreateShape((ShapeType) shapeType);
                        // write shape location
                        shape.Location = ReadPoint(reader);
                        // write shape size
                        shape.Size = ReadSize(reader);
                        // write shape outline color
                        shape.OutlineColor = ReadColor(reader);
                        // write shape outline width
                        shape.OutlineWidth = reader.ReadSingle();
                        // write shape outline dash
                        shape.OutlineDash = (DashStyle)reader.ReadInt32();
                        // write shape fill color
                        shape.FillColor = ReadColor(reader);

                        if (shape.GetShapeType() != ShapeType.Ellipse)
                        {
                            // write count vertex
                            var vcount = reader.ReadInt32();
                            for (int i = 0; i < vcount; i++)
                            {
                                Vertex v = ReadVertex(reader);
                                shape.Vertices.Add(v);
                            }
                        }

                        obj = shape;
                    }
                    break;
            }

            return obj;
        }
    }
}
