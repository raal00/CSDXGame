using SharpDX;
using SharpDX.Direct2D1;
using System;

namespace DXFormHandler.Models
{
    public class FormColors
    {
        public FormColors(RenderTarget target)
        {
            redBrush = new SolidColorBrush(target, SharpDX.Color.Red);
            blackBrush = new SolidColorBrush(target, SharpDX.Color.Black);
            greenBrush = new SolidColorBrush(target, SharpDX.Color.Green);
            blueBrush = new SolidColorBrush(target, SharpDX.Color.Blue);
        }

        public enum BrushesEnum
        {
            RED = 0,
            BLACK = 1,
            GREEN = 2,
            BLUE = 3
        }

        public SolidColorBrush getBrush(BrushesEnum brushesEnum)
        {
            switch (brushesEnum)
            {
                case BrushesEnum.BLACK:
                    return blackBrush;
                case BrushesEnum.BLUE:
                    return blueBrush;
                case BrushesEnum.GREEN:
                    return greenBrush;
                case BrushesEnum.RED:
                    return redBrush;
                default: return null;
            }
        }

        public SolidColorBrush getBrush(int brushesEnum)
        {
            switch (brushesEnum)
            {
                case 1:
                    return blackBrush;
                case 2:
                    return blueBrush;
                case 3:
                    return greenBrush;
                case 4:
                    return redBrush;
                default: return null;
            }
        }

        public SolidColorBrush redBrush;
        public SolidColorBrush blackBrush;
        public SolidColorBrush greenBrush;
        public SolidColorBrush blueBrush;
    }
}
