using System;
using System.Drawing;

namespace Untipic.Presentation
{
    public interface IFontManager
    {
        String DefaultFont { get; }

        String DefaultLightFont { get; }

        Font GetFont(String family, float emSize, FontStyle style, GraphicsUnit unit);
    }
}
