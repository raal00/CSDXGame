namespace DXFormHandler.Models
{
    public class Rotation
    {
        public float RotationX;
        public float RotationY;
    }

    public class Position
    {
        public Position(float x, float y)
        {
            XPos = x;
            YPos = y;
        }

        public float XPos;
        public float YPos;
    }
}
