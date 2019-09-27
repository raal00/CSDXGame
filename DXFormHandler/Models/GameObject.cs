using System;
using SharpDX.WIC;
using SharpDX;
using DXFormHandler.Controller.Bitmap;
using System.Drawing.Imaging;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using DXFormHandler.Models.Interfaces;
using DXFormHandler.Models.Objects;

namespace DXFormHandler.Models
{
    class GameObject : IGameObject
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
            this.ObjectPosition = pos;

            ObjectRectangle = new RawRectangleF(pos.XPos - size.Width / 2,
                                                pos.YPos - size.Height / 2,
                                                pos.XPos + size.Width / 2,
                                                pos.YPos + size.Height / 2);
            ObjectRectangleName = new RawRectangleF(ObjectRectangle.Left + size.Width / 4,
                                                    ObjectRectangle.Top - 20,
                                                    ObjectRectangle.Right - size.Width / 4,
                                                    ObjectRectangle.Top - 1);

            SetTexture(texturePath, renderTarget);
        }

        public Rotation ObjectRotation;
        public Position ObjectPosition;

        public readonly string ObjectName;

        public SharpDX.Direct2D1.Bitmap ObjectBitmap;

        public RawRectangleF ObjectRectangle;
        public RawRectangleF ObjectRectangleName;

        public float Speed = 3f;

        public ObjectTypeEnum tag { get; set; }

        public virtual void ReSize(int delta)
        {
            ObjectRectangle.Left += delta;
            ObjectRectangle.Right -= delta;
            ObjectRectangle.Top += delta;
            ObjectRectangle.Bottom -= delta;

            ObjectRectangleName.Left += delta;
            ObjectRectangleName.Right -= delta;
            ObjectRectangleName.Top += delta;
            ObjectRectangleName.Bottom -= delta;
        }

        public virtual void Move(Vector2 moveVector2)
        {
            if (moveVector2.XMove != 0)
            {
                float move = moveVector2.XMove * Speed;
                ObjectRectangle.Left += move;
                ObjectRectangle.Right += move;
                ObjectRectangleName.Left += move;
                ObjectRectangleName.Right += move;

                ObjectPosition.XPos += move;
            }
            if (moveVector2.YMove != 0)
            {
                float move = moveVector2.YMove * Speed;
                ObjectRectangle.Bottom += move;
                ObjectRectangle.Top += move;
                ObjectRectangleName.Bottom += move;
                ObjectRectangleName.Top += move;

                ObjectPosition.YPos += move;
            }
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
