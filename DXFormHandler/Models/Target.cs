using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXFormHandler.Models
{
    class Target : GameObject
    {
        public Target(string name, string texturePath, Size2 size, Position pos, WindowRenderTarget renderTarget)
                                : base(name, texturePath, size, pos, renderTarget)
        {
        }

        public Position EndPosition;
        public Vector2 PositionChange = new Vector2() { XMove = 0, YMove = 0 };

        public float DeltaX;
        public float RouteTan;
        public bool isHeroesMoving = false;

        public void ChangePosition(Position endPosition)
        {
            Console.WriteLine(ObjectPosition.XPos + " " + ObjectPosition.YPos + "\n" + endPosition.XPos + " " + endPosition.YPos);
            this.EndPosition = endPosition;
            DeltaX = endPosition.XPos - ObjectPosition.XPos;
            RouteTan = DeltaX / (endPosition.YPos - ObjectPosition.YPos);

            PositionChange = new Vector2() { XMove = DeltaX / 100, YMove = DeltaX / 100f / RouteTan };
            isHeroesMoving = true;
        }
    }
}
