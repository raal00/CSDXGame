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


        /// <summary>
        /// Текстура земли должна двигаться противоположно ее координте
        /// </summary>
        /// <param name="moveVector2"></param>
        public override void Move(Vector2 moveVector2)
        {
            base.Move(moveVector2);
        }
        public override void ReSize(int delta)
        {
            base.ReSize(delta);
        }

    }
}
