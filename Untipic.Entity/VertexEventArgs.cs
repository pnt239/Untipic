using System;

namespace Untipic.Entity
{
    public class VertexEventArgs : EventArgs
    {
        // Constructor: VertexEventArgs()
        // Initializes a new instance of the <see cref="VertexEventArgs" /> class.
        public VertexEventArgs(IVertex vertex)
        {
            if (vertex == null) throw new ArgumentNullException("vertex");
            _vertex = vertex;
        }

        // Property: Vertex
        // Gets the vertex associated with the event.
        public IVertex Vertex
        {
            get
            {
                return (_vertex);
            }
        }

        // Vertex associated with the event.
        private IVertex _vertex;
    }

    // Delegate: VertexEventHandler
    // Represents a method that will handle an event involving a vertex.
    public delegate void
    VertexEventHandler
    (
        Object sender,
        VertexEventArgs vertexEventArgs
    );
}
