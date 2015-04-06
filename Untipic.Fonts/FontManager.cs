using System;
using System.Drawing;
using Untipic.Presentation;

namespace Untipic.Fonts
{
    public class FontManager : IFontManager
    {
        private const string OpenSansRegular = "Open Sans";
        private const string OpenSansLight = "Open Sans Light";
        private const string OpenSansBold = "Open Sans Bold";

        public string DefaultFont
        {
            get { return OpenSansRegular; }
        }

        public string DefaultLightFont
        {
            get { return OpenSansLight; }
        }

        public Font GetFont(String family, float emSize, FontStyle style, GraphicsUnit unit)
        {
            throw new NotImplementedException();
        }
    }
}
