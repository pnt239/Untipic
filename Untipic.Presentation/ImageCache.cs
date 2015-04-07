using System.Drawing;
using System.IO;

namespace Untipic.Presentation
{
    public class ImageCache
    {
        public ImageCache(ShapeDrawer shapeDrawer, Page page, int width, int height)
        {
            _shapeDrawer = shapeDrawer;
            _page = page;

            _page.ImageBuffer = new Bitmap(width, height);
        }

        public void Render(Graphics g)
        {
            using (var graph = Graphics.FromImage(_page.ImageBuffer))
            {
                graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graph.Clear(Color.White);
                foreach (var obj in _page.DrawingObjects)
                    if (obj.GetObjectType() == DrawingObjectType.Shape)
                    {
                        var shape = (ShapeBase)obj;
                        _shapeDrawer.Draw(shape, graph);
                    }
            }
            g.DrawImageUnscaled(_page.ImageBuffer, 0, 0);
        }

        public void SaveFile(Stream stream, System.Drawing.Imaging.ImageFormat format)
        {
            _page.ImageBuffer.Save(stream, format);
        }

        private ShapeDrawer _shapeDrawer;
        private Page _page;
    }
}
