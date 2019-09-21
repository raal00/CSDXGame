using SharpDX.WIC;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXFormHandler.Controller.Bitmap
{
    public class BitmapHandler
    {
        public BitmapSource LoadBMPSFromFile(string path, ImagingFactory factory, ImageFormat format)
        {
            BitmapDecoder bitmapDecoder = new BitmapDecoder(
                    factory,
                    path,
                    SharpDX.WIC.DecodeOptions.CacheOnDemand);

            FormatConverter formatConverter = new FormatConverter(factory);
            formatConverter.Initialize(bitmapDecoder.GetFrame(0), 
                    SharpDX.WIC.PixelFormat.Format32bppBGRA, 
                    BitmapDitherType.DualSpiral8x8,
                    null,
                    0.5, 
                    BitmapPaletteType.Custom);
            return formatConverter;
        }
    }
}
