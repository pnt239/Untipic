using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Untipic.DrawPadTools
{
    public class CursorTool
    {
        public static Stream GetResourceStream(string resourceName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(
                    "Untipic.Cursors." + resourceName);
        }

        public static Cursor GetSelectionCursor()
        {
            return (new Cursor(GetResourceStream("Selection.cur")));
        }

        public static Cursor GetDirectSelectionCursor()
        {
            return (new Cursor(GetResourceStream("DirectSelection.cur"))); 
        }

        public static Cursor GetEditorCursor()
        {
            return Cursors.IBeam;
        }

        public static Cursor GetShapeCursor()
        {
            return Cursors.Cross;
        }

        public static Cursor GetBrushCursor()
        {
            return (new Cursor(GetResourceStream("Brush.cur"))); 
        }

        public static Cursor GetEraserCursor()
        {
            return (new Cursor(GetResourceStream("Brush.cur"))); 
        }

        public static Cursor GetBucketCursor()
        {
            return Cursors.Default;
        }

        public static Cursor GetCropCursor()
        {
            return GetSelectionCursor();
        }
    }
}
