using System;

namespace DXFormHandler.Models.Styles
{
    public class MapStyle
    {
        public MapStyle(int w, int h)
        {
            this.MapWidht = w;
            this.MapHeight = h;
        }

        public int MapWidht;
        public int MapHeight;
    }
}
