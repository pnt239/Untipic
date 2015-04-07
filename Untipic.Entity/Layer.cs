using System.Collections.Generic;

namespace Untipic.Entity
{
    public class Layer
    {
        public Layer()
        {
            _drawingObjects = new List<IDrawingObject>();
        }

        public void AddDrawingObject(IDrawingObject item)
        {
            _drawingObjects.Add(item);
        }

        private List<IDrawingObject> _drawingObjects;
    }
}
