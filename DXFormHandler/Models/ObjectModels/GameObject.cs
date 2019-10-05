using System;
using SharpDX.WIC;
using SharpDX;
using DXFormHandler.Controller.Bitmap;
using System.Drawing.Imaging;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using DXFormHandler.Models.Interfaces;
using DXFormHandler.Models.Objects;
using DXFormHandler.Models.Styles;

namespace DXFormHandler.Models
{
    class GameObject : IGameObject
    {

        public GameObject(string name, string texturePath, Size2 size, Position pos, WindowRenderTarget renderTarget)
        {
            this.ObjectName = name;
            this.ObjectPosition = pos;
            this.ObjectSize = size;

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


        #region Fields
        public Position ObjectPosition;
        public Size2    ObjectSize;

        public readonly string ObjectName;

        public SharpDX.Direct2D1.Bitmap ObjectBitmap;

        public RawRectangleF ObjectRectangle;

        public RawRectangleF ObjectRectangleName;

        public float Speed = 3f;

        public ObjectTypeEnum tag { get; set; }
        #endregion



        #region methods
        public virtual void ReSize(int delta)
        {
            ObjectSize.Width += delta;
            ObjectSize.Height += delta;

            SetRectangle();
        }

        public virtual void Move(Vector2 moveVector2)
        {
            if (moveVector2.XMove != 0)
            {
                float move = moveVector2.XMove * Speed;
                ObjectPosition.XPos += move;

                SetRectangle();
            }
            if (moveVector2.YMove != 0)
            {
                float move = moveVector2.YMove * Speed;
                ObjectPosition.YPos += move;

                SetRectangle();
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

        public virtual void SetRectangle()
        {
            ObjectRectangle.Bottom  = ObjectPosition.YPos + ObjectSize.Height / 2;
            ObjectRectangle.Top     = ObjectPosition.YPos - ObjectSize.Height / 2;

            ObjectRectangle.Left    = ObjectPosition.XPos - ObjectSize.Width / 2;
            ObjectRectangle.Right   = ObjectPosition.XPos + ObjectSize.Width / 2;


            ObjectRectangleName.Bottom  = ObjectRectangle.Top - 2;
            ObjectRectangleName.Top     = ObjectRectangle.Top - 20;

            ObjectRectangleName.Left =  ObjectPosition.XPos - ObjectSize.Width / 2;
            ObjectRectangleName.Right = ObjectPosition.XPos + ObjectSize.Width / 2;
        }
        #endregion
    }
}
