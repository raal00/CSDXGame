using System;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using System.Diagnostics;

using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Windows;
using SharpDX.Mathematics;

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
            StartRender(isRendering);

            initForm();
            callback = new RenderLoop.RenderCallback(Render);
            RenderLoop.Run(mainRenderForm, callback);

            //showForm(mainForm);
        }

        private void MainRenderForm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            isRendering = !isRendering;
            StartRender(isRendering);
        }

        /// Fields
        /// 

        // FORM
        private readonly RenderForm mainRenderForm;

        private WindowRenderTarget RenderTarget = null;  // DROP GRAPHIC HERE -> DRAW
        private bool isRendering;
        private SharpDX.Direct2D1.Factory Factory = null; //

            // SCENE BRUSHES
        private SharpDX.Mathematics.Interop.RawColor4 mainFormBackground = new SharpDX.Mathematics.Interop.RawColor4(255,255,255,1);
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
        private Ellipse Ellipse;
        private readonly Random random = new Random();
        private System.Drawing.Point curPos = new System.Drawing.Point(200,200);

        /// Methods
        /// 

        public void StartRender(bool Render)
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
            Console.WriteLine(mainRenderForm.Width + " " + mainRenderForm.Height);
            if (!showFrame) return;

            gameFrameCount++;
            var ElapsedTime = (double)gameClock.ElapsedTicks / Stopwatch.Frequency;
            
            gameTime += ElapsedTime;

            if (gameTime >= 0.05f)
            {
                fps = gameFrameCount / gameTime;
                gameFrameCount = 0;
                gameTime = 0;
            }

            curPos.X += random.Next(-1, 2);
            curPos.Y += random.Next(-1, 2);

            Ellipse = new Ellipse(new SharpDX.Mathematics.Interop.RawVector2(curPos.X, curPos.Y), 50, 50);

            RenderTarget.BeginDraw();

            RenderTarget.Clear(mainFormBackground);

            RenderTarget.Transform = (Matrix3x2)Matrix.Identity;

            RenderTarget.DrawText("Working...", textFormat, TitleTextBox, blackBrush);
            RenderTarget.DrawText($"{fps} fps", textFormat, fpsTextBox, blackBrush);

            RenderTarget.DrawEllipse(Ellipse, redBrush);
            
            RenderTarget.EndDraw();

            gameClock.Restart();
        }
    }
}
