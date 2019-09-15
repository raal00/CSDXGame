using System;
using System.Threading.Tasks;

namespace DXFormHandler
{
    public class _2DGame
    {
        public _2DGame()
        {
            
        }

        DXFormHandler.Models.FormStyle FormStyle;
        DXFormHandler.Controller.MainDXForm DXForm;

        public void InitForm()
        {
            FormStyle = new Models.FormStyle()
            {
                Width = 1280,
                Height = 720

            };
        }

        public void Play()
        {
            InitForm();
            DXForm = new DXFormHandler.Controller.MainDXForm(FormStyle);
        }
    }
}
