using System;
using System.Drawing;

namespace Untipic.Entity
{
    public class Vertex : IVertex
    {
        const float EPSILON = 1.0e-15f;
        private float _x;
        private float _y;

        public Vertex()
        {
            _x = _y = 0;
        }

        public Vertex(float x, float y)
        {
            _x = x;
            _y = y;
        }

        public Vertex(PointF point)
        {
            _x = point.X;
            _y = point.Y;
        }

        public bool IsEmpty
        {
            get { return (Math.Abs(X) < EPSILON) && (Math.Abs(Y) < EPSILON); }
        }

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public Point ToPoint()
        {
            return new Point((int) Math.Round(_x), (int) Math.Round(_y));
        }

        public IVertex Clone()
        {
            return new Vertex(_x, _y);
        }
    }
}
