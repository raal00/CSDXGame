using DXFormHandler.Controller.Bitmap;
using DXFormHandler.Models;
using SharpDX.Direct2D1.Effects;
using SharpDX.WIC;
using SharpDX.Mathematics.Interop;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        GameObject obj1;
        GameObject obj2;
        GameObject hero;

        public List<GameObject> objects = new List<GameObject>();
        System.Windows.Forms.Keys LastKey = System.Windows.Forms.Keys.Escape;

        public FormColors colors = null;
        private int groundPos;
        
        public MainForm(string formTitle) : base(formTitle)
        {
            
        }

        private void SetBinds()
        {
            mainRenderForm.KeyDown += CSGameKeyClick;
            mainRenderForm.KeyUp += CSGameKeyUp;
        }

        private void CSGameKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            hero.Speed = 1;
        }

        private void CSGameKeyClick(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            System.Windows.Forms.Keys key = e.KeyCode;
            
            switch (key)
            {
                case System.Windows.Forms.Keys.Right:
                    if (LastKey == key) hero.Speed += 0.1f;
                    hero.MoveRight(1);
                    break;
                case System.Windows.Forms.Keys.Left:
                    if (LastKey == key) hero.Speed += 0.1f;
                    hero.MoveRight(-1);
                    break;
                case System.Windows.Forms.Keys.Space:
                    hero.MoveBottom(hero.jumpPower);
                    if (LastKey == System.Windows.Forms.Keys.Left)
                        hero.MoveRight(-2 * hero.Speed);
                    else
                        hero.MoveRight(2 * hero.Speed);
                    break;
                default:
                    break;
            }
            LastKey = key;
        }

        protected override void initObjects()
        {
            base.initObjects();
            SetBinds();
            groundPos = mainRenderForm.Height - 100;
            /// INIT ALL GAMEOBJECTS

            // Screen positions
            // (0,0)    (0,n)
            // 
            // (m, 0)   (m,n)



            obj1 = new GameObject("Player 1", "D:/Code/hero1.jpg", new SharpDX.Size2(128, 128), new Position(300,100), RenderTarget);
            obj2 = new GameObject("Player 2", "D:/Code/hero1.jpg", new SharpDX.Size2(128, 128), new Position(600, 50), RenderTarget);
            hero = new GameObject("Your name", "D:/Code/hero1.jpg", new SharpDX.Size2(128, 128), new Position(500, 500), RenderTarget);

            obj1.Weight = 3;
            hero.Weight = 2;
            hero.Speed = 1;
            hero.jumpPower = -mainRenderForm.Height / 10;

            objects.Add(obj1);
            objects.Add(obj2);
            objects.Add(hero);
        }


        protected override void initForm()
        {
            base.initForm();

            /// INIT ALL STYLES
            colors = new FormColors(RenderTarget);
            /// ...
            /// 
            
        }
        
        public override void GameLogic()
        {
            base.GameLogic();

            foreach (var obj in objects)
            {
                // 
                
                // Gravity
                if (obj.ObjectRectangle.Bottom < groundPos)
                {
                    obj.MoveBottom(1f); // 1f * weight
                }
            }

            RenderTarget.DrawBitmap(obj1.ObjectBitmap, obj1.ObjectRectangle, 1, SharpDX.Direct2D1.BitmapInterpolationMode.NearestNeighbor);
            RenderTarget.DrawText(obj1.ObjectName, this.textFormat, obj1.ObjectRectangleName, new FormColors(RenderTarget).redBrush);

            RenderTarget.DrawBitmap(obj2.ObjectBitmap, obj2.ObjectRectangle, 1, SharpDX.Direct2D1.BitmapInterpolationMode.NearestNeighbor);
            RenderTarget.DrawText(obj2.ObjectName, this.textFormat, obj2.ObjectRectangleName, new FormColors(RenderTarget).redBrush);

            RenderTarget.DrawBitmap(hero.ObjectBitmap, hero.ObjectRectangle, 1, SharpDX.Direct2D1.BitmapInterpolationMode.NearestNeighbor);
            RenderTarget.DrawText(hero.ObjectName, this.textFormat, hero.ObjectRectangleName, new FormColors(RenderTarget).redBrush);

        }
    }
}
