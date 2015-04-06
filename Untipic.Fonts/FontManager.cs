using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using Untipic.Presentation;

namespace Untipic.Fonts
{
    public class FontManager : IFontManager
    {
        private const string OpenSansRegular = "Open Sans";
        private const string OpenSansLight = "Open Sans Light";
        private const string OpenSansBold = "Open Sans Bold";

        private readonly PrivateFontCollection fontCollection = new PrivateFontCollection();

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
            FontFamily fontFamily = GetFontFamily(family);
            return new Font(fontFamily, emSize, style, unit);
        }

        private FontFamily GetFontFamily(string familyName)
        {
            lock (fontCollection)
            {
                foreach (FontFamily fontFamily in fontCollection.Families)
                    if (fontFamily.Name == familyName) return fontFamily;

                string resourceName = GetType().Namespace + ".Resources." + familyName.Replace(' ', '_') + ".ttf";

                Stream fontStream = null;
                IntPtr data = IntPtr.Zero;
                try
                {
                    fontStream = GetType().Assembly.GetManifestResourceStream(resourceName);
                    int bytes = (int)fontStream.Length;
                    data = Marshal.AllocCoTaskMem(bytes);
                    byte[] fontdata = new byte[bytes];
                    fontStream.Read(fontdata, 0, bytes);
                    Marshal.Copy(fontdata, 0, data, bytes);
                    fontCollection.AddMemoryFont(data, bytes);
                    return fontCollection.Families[fontCollection.Families.Length - 1];
                }
                finally
                {
                    if (fontStream != null) fontStream.Dispose();
                    if (data != IntPtr.Zero) Marshal.FreeCoTaskMem(data);
                }
            }
        }
    }
}
