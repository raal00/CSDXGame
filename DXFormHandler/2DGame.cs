using DXFormHandler.Controller.Bitmap;
using DXFormHandler.Models;
using SharpDX.Direct2D1.Effects;
using SharpDX.WIC;
using SharpDX.Mathematics.Interop;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using DXFormHandler.Models.Objects;

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

            GameForm.AddObject(new GameObject("1", "C:/Users/Ivan/Downloads/1.png", new SharpDX.Size2(128, 128), new Position(521, 521), GameForm.RenderTarget) { tag = ObjectTypeEnum.NPC });
            GameForm.AddObject(new GameObject("2", "C:/Users/Ivan/Downloads/1.png", new SharpDX.Size2(128, 128), new Position(400, 600), GameForm.RenderTarget) { tag = ObjectTypeEnum.NPC });
            GameForm.AddObject(new GameObject("3", "C:/Users/Ivan/Downloads/1.png", new SharpDX.Size2(128, 128), new Position(555, 544), GameForm.RenderTarget) { tag = ObjectTypeEnum.ControlledNPC });

            GameForm.StartRender();
            GameForm.GamePauseMode(true);
        }

        public void Play()
        {
            InitForm();
        }
    }
}
