using System.Drawing;

namespace Untipic.Presentation
{
    public interface IFontManager
    {
        Font GetFont(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit);
    }
}
