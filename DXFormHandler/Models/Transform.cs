using System;

namespace DXFormHandler.Models
{
    public class Transform
    {
        public float RotationX;
        public float RotationY;
    }

    public class Position
    {
        public Position(int x, int y)
        {
            XPos = x;
            YPox = y;
        }

        public int XPos;
        public int YPox;
    }
}
