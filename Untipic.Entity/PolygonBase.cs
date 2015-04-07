using System;

namespace Untipic.Entity
{
    public abstract class PolygonBase : ShapeBase
    {
        protected PolygonBase() : base()
        {
            IsClosedFigure = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is closed figure.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is closed figure; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsClosedFigure { get; set; }
    }
}
