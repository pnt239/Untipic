using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Untipic.Presentation
{
    public class DefaultFontManager : IFontManager
    {
        public Font GetFont(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit)
        {
            return new Font(family, emSize, style, unit);
        }
    }

    public class ThemeManager
    {
        private IFontManager _fontManager;
        private Color _formBackColor;
        private Color _formForeColor;

        private Color _buttonBackColor;
        private Color _buttonForeColor;
        private Color _buttonHoverColor;
        private Color _buttonNormalColor;
        private Color _buttonPressColor;
        private Color _buttonDisableColor;

        public ThemeManager()
        {
            InitFontManager();
            InitColor();
        }

        public Color FormBackColor { get { return _formBackColor; } }

        public Color FormForeColor { get { return _formForeColor; } }

        private void InitFontManager()
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom("Untipic.Fonts.dll");
                Type type = assembly.GetType("Untipic.Fonts.FontManager");
                if (type != null)
                {
                    _fontManager = (IFontManager)Activator.CreateInstance(type);
                    if (_fontManager != null)
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // ignore
            }
            _fontManager = new DefaultFontManager();
        }

        private void InitColor()
        {
            _formBackColor = Color.White;
            _formForeColor = Color.Black;


        }
    }
}
