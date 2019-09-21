using DXFormHandler.Controller.Bitmap;
using DXFormHandler.Models;
using SharpDX.Direct2D1.Effects;
using SharpDX.WIC;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace DXFormHandler
{
    public class _2DGame
    {
        public _2DGame()
        {
            
        }
        
        MainForm GameForm;

        public void InitForm()
        {
            FormStyle.ShowFPS = true;
            GameForm = new MainForm("Test form");
            GameForm.GamePauseMode(true);
        }

        public void Play()
        {
            InitForm();
            FormStyle.ShowFPS = true;
        }
    }

     class MainForm : DXFormHandler.Controller.GameForm
     {
        public FormColors colors = null;
        
        public MainForm(string formTitle) : base(formTitle)
        {
            
        }

        protected override void initForm()
        {
            base.initForm();

            /// INIT ALL STYLES
            colors = new FormColors(RenderTarget);
            /// ...
        }

        BitmapHandler a = new BitmapHandler();
        
        public override void GameLogic()
        {
            base.GameLogic();
            ImagingFactory factory = new ImagingFactory();
            var bmps  = a.LoadBMPSFromFile("D:/Code/front/card.jpg", factory, ImageFormat.Bmp);
            FormatConverter formatConverter = new FormatConverter(factory);
            formatConverter.Initialize(bmps, SharpDX.WIC.PixelFormat.Format32bppPBGRA, BitmapDitherType.Spiral8x8, null, 0f, BitmapPaletteType.MedianCut);
            SharpDX.Direct2D1.Bitmap bitmap = SharpDX.Direct2D1.Bitmap.FromWicBitmap(RenderTarget,formatConverter);

            RenderTarget.DrawLine(new SharpDX.Mathematics.Interop.RawVector2(1, new Random().Next(700)),
                                  new SharpDX.Mathematics.Interop.RawVector2(new Random().Next(700), 1), colors.getBrush(new Random().Next(1, 4)));
            RenderTarget.DrawBitmap(bitmap, 1, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        } 
     }
}
