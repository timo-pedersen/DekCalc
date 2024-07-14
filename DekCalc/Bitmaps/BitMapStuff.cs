using System;
using System.Collections.Generic;
using System.Drawing;
using G = System.Drawing.Graphics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imaging = System.Windows.Media.Imaging;
using System.Drawing.Drawing2D;

namespace DekCalc.Bitmaps
{
    internal class BitMapStuff
    {
        private Bitmap? _btImg;
        private G? _g;

        public Bitmap? Bitmap => _btImg;
        public G? G => _g;
        public Imaging.BitmapImage ImageSource => BitmapToImageSource(_btImg);
        public Color BgColor { get; set; }

        internal BitMapStuff(int width, int height, Color? bgColor = null) 
        {
            BgColor = bgColor ?? Color.White;
            Update(width, height);

        }

        internal void Clear()
        {
            _g.Clear(BgColor);
        }

        internal void Update(int width, int height)
        {
            if (_btImg != null)
                _btImg.Dispose();
            
            if (_g != null)
                _g.Dispose();

            _btImg = new Bitmap(width, height);
            _g = G.FromImage(_btImg);
            _g.Clear(BgColor);
            _g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            _g.SmoothingMode = SmoothingMode.AntiAlias;
        }

        internal static Imaging.BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                Imaging.BitmapImage bitmapimage = new Imaging.BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = Imaging.BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
