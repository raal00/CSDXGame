using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXFormHandler.Models
{
    class Background : GameObject
    {

        public Background(string name, string texturePath, Size2 size, Position pos, WindowRenderTarget renderTarget) 
                                : base (name, texturePath, size, pos, renderTarget)
        {

        }


        public int LeftBorder;
        public int RightBorder;
        public int UpBorder;
        public int DownBorder;

        /// <summary>
        /// Текстура земли должна двигаться противоположно ее координте
        /// </summary>
        /// <param name="moveVector2"></param>
        public override void Move(Vector2 moveVector2)
        {
            if (moveVector2.XMove != 0)
            {
                float move = moveVector2.XMove * Speed;
                ObjectRectangle.Left -= move;
                ObjectRectangle.Right -= move;

                ObjectPosition.XPos += move;
            }
            if (moveVector2.YMove != 0)
            {
                float move = moveVector2.YMove * Speed;
                ObjectRectangle.Bottom -= move;
                ObjectRectangle.Top -= move;

                ObjectPosition.YPos += move;
            }
        }

    }
}
