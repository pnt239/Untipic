using System.Collections.Generic;
using System.Drawing;

namespace Untipic.Entity
{
    public interface IVertexCollection : IList<IVertex>
    {
        // Method: Add()
        // Creates a vertex and adds it to the collection.
        IVertex Add();

        Point[] ToPoints();

        // Event: VertexAdded
        // Occurs when a vertex is added to the collection.
        event VertexEventHandler VertexAdded;

        // Event: VertexRemoved
        // Occurs when a vertex is removed from the collection.
        event VertexEventHandler VertexRemoved;

        // Method: Clone
        // Create a clone of this vertexs
        IVertexCollection Clone();
    }
}
