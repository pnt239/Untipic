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
        private const string Default = "Segoe UI";
        private const string DefaultLight = "Segoe UI Light";

        public string DefaultFont
        {
            get { return Default; }
        }

        public string DefaultLightFont
        {
            get { return DefaultLight; }
        }

        public Font GetFont(String family, float emSize, FontStyle style, GraphicsUnit unit)
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

        private Font _formTitleFont;
        private Font _formDefaultFont;

        public ThemeManager()
        {
            InitFontManager();
            InitColor();
            InitFont();
        }

        public Color FormBackColor { get { return _formBackColor; } }

        public Color FormForeColor { get { return _formForeColor; } }

        public Font FormTitleFont { get { return _formTitleFont; } }

        public Font FormDefaultFont { get { return _formDefaultFont; } }

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

        private void InitFont()
        {
            _formTitleFont = CreateFontDefaultLight(24f);
            _formDefaultFont = CreateFontDefault(12f);
        }

        private Font CreateFontDefault(float size)
        {
            return _fontManager.GetFont(_fontManager.DefaultFont, size, FontStyle.Regular, GraphicsUnit.Pixel);
        }

        private Font CreateFontDefaultLight(float size)
        {
            return _fontManager.GetFont(_fontManager.DefaultLightFont, size, FontStyle.Regular, GraphicsUnit.Pixel);
        }
    }
}
