using System;
using SharpDX.WIC;
using SharpDX;
using DXFormHandler.Controller.Bitmap;
using System.Drawing.Imaging;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace DXFormHandler.Models
{
    class GameObject
    {
        /*
          . . . (0,0)_____T_____ (0,n) ->
                     |         |    L < R
                     L         R    B < T
                     |         |
          . . . . . ._____B_____ 
                (n,0)            (n,n)
                    |
                    V
        */

        public GameObject(string name, string texturePath, Size2 size, Position pos, WindowRenderTarget renderTarget)
        {
            this.ObjectName = name;
            this.size = size;

            ObjectRectangle = new RawRectangleF(pos.XPos - size.Width  / 2,
                                                pos.YPox - size.Height / 2,
                                                pos.XPos + size.Width  / 2,
                                                pos.YPox + size.Height / 2);
            ObjectRectangleName = new RawRectangleF(ObjectRectangle.Left + size.Width / 4, 
                                                    ObjectRectangle.Top - 20, 
                                                    ObjectRectangle.Right - size.Width / 4, 
                                                    ObjectRectangle.Top - 1);

            SetTexture(texturePath, renderTarget);
        }

        public Transform ObjectTransform { get; set; }
        public readonly string ObjectName;

        public SharpDX.Direct2D1.Bitmap ObjectBitmap;

        public RawRectangleF ObjectRectangle;
        public RawRectangleF ObjectRectangleName;

        private readonly Size2 size;

        public float jumpPower = 10;
        public float Weight = 1.2f;
        public float Speed = 1.5f;

        private int layer;

        public void MoveBottom(float dist)
        {
            float rdist = dist * Weight;
            ObjectRectangle.Bottom += rdist;
            ObjectRectangle.Top += rdist;

            ObjectRectangleName.Bottom += rdist;
            ObjectRectangleName.Top += rdist;
        }

        public void MoveRight(float dist)
        {
            float rdist = dist * Speed;
            ObjectRectangle.Left += rdist;
            ObjectRectangle.Right += rdist;

            ObjectRectangleName.Left += rdist;
            ObjectRectangleName.Right += rdist;
        }

        public void SetTexture(string texturePath, WindowRenderTarget target)
        {
            BitmapHandler bmpHandler = new BitmapHandler();
            ImagingFactory factory = new ImagingFactory();

            var bmps = bmpHandler.LoadBMPSFromFile(texturePath, factory, ImageFormat.Bmp);
            FormatConverter formatConverter = new FormatConverter(factory);
            formatConverter.Initialize(bmps, SharpDX.WIC.PixelFormat.Format32bppPBGRA, BitmapDitherType.Spiral8x8, null, 0f, BitmapPaletteType.MedianCut);
            ObjectBitmap = SharpDX.Direct2D1.Bitmap.FromWicBitmap(target, formatConverter);
        }
    }
}
