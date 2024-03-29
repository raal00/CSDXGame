﻿using System;
using System.Collections.Generic;
using System.Linq;

using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Windows;

using DXFormHandler.Models;
using System.Diagnostics;
using DXFormHandler.Models.Styles;

namespace DXFormHandler.Controller
{
    public class GameForm
    {
        public GameForm(string formTitle)
        {
            gameFormTitle = formTitle;

            initForm();

            Console.WriteLine("Форма создана");

            initObjects();

            Console.WriteLine("Объекты созданы");

        }


        #region GAME FORM FIELDS
        private readonly string gameFormTitle;

        // FORM
        protected RenderForm mainRenderForm;
        protected MapStyle mapStyle;
        public WindowRenderTarget RenderTarget = null;  // DROP GRAPHIC HERE -> DRAW
        private SharpDX.Direct2D1.Factory Factory = null; //

        // SCENE BRUSHES
        private SharpDX.Mathematics.Interop.RawColor4 mainFormBackground = new SharpDX.Mathematics.Interop.RawColor4(255, 255, 255, 1);
        
        private SolidColorBrush blackBrush;
        private SolidColorBrush greenBrush;
        private SolidColorBrush yellowBrush;

        private RenderLoop.RenderCallback callback;
        private FPSModel fpsModel;
        protected TextFormat textFormat;

        //FPS
        private double fps = 0;
        private Stopwatch gameClock;
        private double gameTime = 0;
        private int gameFrameCount = 0;
        private bool pause;

        // TEXT BOXES
        SharpDX.Mathematics.Interop.RawRectangleF fpsTextBox;
        SharpDX.Mathematics.Interop.RawRectangleF TitleTextBox;

        // FORM PROPS
        private RenderTargetProperties rndTargetProperties;
        private HwndRenderTargetProperties hwndTargetProperties;
        #endregion

        #region GAME FORM METHODS

        public void GamePauseMode(bool pause)
        {
            this.pause = pause;
        }

        public void StartRender()
        {
            callback = new RenderLoop.RenderCallback(Render);
            RenderLoop.Run(mainRenderForm, callback);
        }

        /// <summary>
        /// начальная инициализация формы, игровых часов и цветов
        /// </summary>
        protected virtual void initForm()
        {
            mainRenderForm = new RenderForm(gameFormTitle);
            Factory = new SharpDX.Direct2D1.Factory(SharpDX.Direct2D1.FactoryType.SingleThreaded);

            fpsModel = new FPSModel()
            {
                FPS = 60
            };

            fpsTextBox = new SharpDX.Mathematics.Interop.RawRectangleF(4, 32, 255, 4);
            TitleTextBox = new SharpDX.Mathematics.Interop.RawRectangleF(4, 4, 255, 4);

            textFormat = new TextFormat(new SharpDX.DirectWrite.Factory(), "Tahoma", 15) { TextAlignment = TextAlignment.Leading, ParagraphAlignment = ParagraphAlignment.Center };
            mainRenderForm.Width = FormStyle.Width;
            mainRenderForm.Height = FormStyle.Height;
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

            blackBrush = new SolidColorBrush(RenderTarget, SharpDX.Color.Black);
            greenBrush = new SolidColorBrush(RenderTarget, SharpDX.Color.Green);
            yellowBrush = new SolidColorBrush(RenderTarget, SharpDX.Color.YellowGreen);

            mainRenderForm.Show();
        }

        protected virtual void initObjects() { }


        private void Render()
        {
            if (pause) return;

            RenderTarget.BeginDraw();
            RenderTarget.Clear(mainFormBackground);

            // START

            RenderTarget.DrawText("GameForm is working", textFormat, TitleTextBox, blackBrush);
             
            GameLogic();
            if (FormStyle.ShowFPS) RenderTarget.DrawText($"{(int)fps} fps", textFormat, fpsTextBox, yellowBrush);
            
            // END

            RenderTarget.EndDraw();


            if (FormStyle.ShowFPS)
            {
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

        public virtual void GameLogic()
        {

        }
        #endregion

    }
}
