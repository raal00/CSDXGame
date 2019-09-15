using System;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;

using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Windows;

using DXFormHandler.Models;

namespace DXFormHandler.Controller
{
    public class MainDXForm
    {
        public MainDXForm(FormStyle formStyle)
        {
            mainRenderForm = new RenderForm("Render DX form");
            mainRenderForm.MouseDoubleClick += MainRenderForm_MouseDoubleClick;
            Factory = new SharpDX.Direct2D1.Factory(SharpDX.Direct2D1.FactoryType.SingleThreaded);

            isRendering = true;
            fpsModel = new FPSModel()
            {
                FPS = 100
            };

            fpsTextBox = new SharpDX.Mathematics.Interop.RawRectangleF(4, 32, 255, 4);
            TitleTextBox = new SharpDX.Mathematics.Interop.RawRectangleF(4, 4, 255, 4);

            textFormat = new TextFormat(new SharpDX.DirectWrite.Factory(), "Calibri", 10) { TextAlignment = TextAlignment.Leading, ParagraphAlignment = ParagraphAlignment.Center };
            mainRenderForm.Width = formStyle.Width;
            mainRenderForm.Height = formStyle.Height;
            Pause(isRendering);

            initForm();
            callback = new RenderLoop.RenderCallback(Render);
            RenderLoop.Run(mainRenderForm, callback);

            //showForm(mainForm);
        }

        private void MainRenderForm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            isRendering = !isRendering;
            Pause(isRendering);
        }

        /// Fields
        /// 

        // FORM
        private readonly RenderForm mainRenderForm;

        private WindowRenderTarget RenderTarget = null;  // DROP GRAPHIC HERE -> DRAW
        private bool isRendering;
        private SharpDX.Direct2D1.Factory Factory = null; //

        // SCENE BRUSHES
        private SharpDX.Mathematics.Interop.RawColor4 mainFormBackground = new SharpDX.Mathematics.Interop.RawColor4(255, 255, 255, 1);
        private SolidColorBrush redBrush;
        private SolidColorBrush blueBrush;
        private SolidColorBrush blackBrush;
        private SolidColorBrush greenBrush;


        private RenderLoop.RenderCallback callback;
        private FPSModel fpsModel;
        private TextFormat textFormat;

        //FPS
        private double fps = 0;
        private Stopwatch gameClock;
        private double gameTime = 0;
        private int gameFrameCount = 0;
        private bool showFrame;

        // TEXT BOXES
        SharpDX.Mathematics.Interop.RawRectangleF fpsTextBox;
        SharpDX.Mathematics.Interop.RawRectangleF TitleTextBox;

        // FORM PROPS
        private RenderTargetProperties rndTargetProperties;
        private HwndRenderTargetProperties hwndTargetProperties;


        // SCENE TEST
        private readonly Random random = new Random();
        SolidColorBrush Brush;
        Ellipse[] ellipses = new Ellipse[3] {
            new Ellipse()
            {
                    Point = new SharpDX.Mathematics.Interop.RawVector2(200, 100),
                    RadiusX = 100,
                    RadiusY = 80
            },
            new Ellipse()
            {
                    Point = new SharpDX.Mathematics.Interop.RawVector2(150, 500),
                    RadiusX = 130,
                    RadiusY = 140
            },
            new Ellipse()
            {
                    Point = new SharpDX.Mathematics.Interop.RawVector2(1100, 400),
                    RadiusX = 70,
                    RadiusY = 70
            }
        };

        /// Methods
        /// 

        public void Pause(bool Render)
        {
            showFrame = Render;
        }

        /// <summary>
        /// начальная инициализация формы, игровых часов и цветов
        /// </summary>
        protected void initForm()
        {

            fps = fpsModel.FPS;
            gameClock = Stopwatch.StartNew();

            //
            rndTargetProperties = new RenderTargetProperties(new PixelFormat(Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied));
            hwndTargetProperties = new HwndRenderTargetProperties();

            //
            hwndTargetProperties.Hwnd = mainRenderForm.Handle;
            hwndTargetProperties.PixelSize = new Size2(mainRenderForm.ClientSize.Width, mainRenderForm.ClientSize.Height);
            hwndTargetProperties.PresentOptions = PresentOptions.None;

            RenderTarget = new WindowRenderTarget(Factory, rndTargetProperties, hwndTargetProperties);

            redBrush = new SolidColorBrush(RenderTarget, SharpDX.Color.Red);
            blackBrush = new SolidColorBrush(RenderTarget, SharpDX.Color.Black);
            greenBrush = new SolidColorBrush(RenderTarget, SharpDX.Color.Green);
            blueBrush = new SolidColorBrush(RenderTarget, SharpDX.Color.Blue);

            mainRenderForm.Show();
        }

        protected void showForm(Form form)
        {
            if (form == null) throw new ArgumentNullException();
            form.ShowDialog();
        }

        private void Render()
        {
            if (!showFrame) return;

            RenderTarget.BeginDraw();
            RenderTarget.Clear(mainFormBackground);

            // START

            RenderTarget.DrawText("Working...", textFormat, TitleTextBox, blackBrush);
            RenderTarget.DrawText($"{fps} fps", textFormat, fpsTextBox, blackBrush);

            for (int i = 0; i < 3; i++)
            {
                ellipses[i].Point.X += random.Next(0, 2);
                ellipses[i].Point.Y += random.Next(0, 2);

                Brush = blackBrush;

                if (ellipses[i].Point.X - ellipses[i].RadiusX <= 0) ellipses[i].RadiusX -= 50;
                if (ellipses[i].Point.X + ellipses[i].RadiusX >= mainRenderForm.Width) ellipses[i].RadiusX -= 50;
                if (ellipses[i].Point.Y - ellipses[i].RadiusY <= 0) ellipses[i].RadiusY -= 50;
                if (ellipses[i].Point.Y + ellipses[i].RadiusY >= mainRenderForm.Height) ellipses[i].RadiusY -= 50;

                if (ellipses[i].Point.X % 5 == 0)
                {
                    Brush = redBrush;
                }
                else if (ellipses[i].Point.X % 2 == 0)
                {
                    Brush = blueBrush;
                }
                else if (ellipses[i].Point.X % 3 == 0)
                {
                    Brush = greenBrush;
                }

                RenderTarget.DrawEllipse(ellipses[i], Brush);
            }

            // END

            RenderTarget.EndDraw();


            gameFrameCount++;
            var ElapsedTime = (double)gameClock.ElapsedTicks / Stopwatch.Frequency;
            gameTime += ElapsedTime;

            if (gameTime >= 1f)
            {
                fps = gameFrameCount / gameTime;
                gameFrameCount = 0;
                gameTime = 0;
            }
            gameClock.Restart();
        }
    }
}
