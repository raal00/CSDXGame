using DXFormHandler.Models.Styles;
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

        public override void Move(Vector2 moveVector2)
        {
            base.Move(moveVector2);
        }

        public override void ReSize(int delta)
        {
            base.ReSize(delta);
        }

        public override void SetRectangle()
        {
            ObjectRectangle.Bottom = ObjectPosition.YPos + ObjectSize.Height / 2;
            ObjectRectangle.Top = ObjectPosition.YPos - ObjectSize.Height / 2;

            ObjectRectangle.Left = ObjectPosition.XPos - ObjectSize.Width / 2;
            ObjectRectangle.Right = ObjectPosition.XPos + ObjectSize.Width / 2;
        }

    }
}
