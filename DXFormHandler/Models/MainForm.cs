using DXFormHandler.Models;
using System;
using System.Collections.Generic;
using DXFormHandler.Models.Objects;
using DXFormHandler.Models.Styles;

namespace DXFormHandler
{
    class MainForm : DXFormHandler.Controller.GameForm
     {
        public MainForm(string formTitle) : base(formTitle)
        {

        }

        #region Require gameObjects

        private GameObject hero;

        private Position endPoint = new Position(255,255);
        private bool isHeroesMoving = false;
        private float delX;
        private float K;

        private Position heroScreenPosition;

        private Vector2 PositionChange = new Vector2() { XMove = 0, YMove = 0 };
        private Vector2 objectsMoving = new Vector2() { XMove = 0, YMove = 0 };
        private int cameraSpeed = 2;

        private bool isCameraMoving = false;
        private bool isHorizCameraMoving = false;
        private bool isVerticCameraMoving = false;


        private Background VisibleMap;
        #endregion

        public List<GameObject> NPCs = new List<GameObject>();
        public List<GameObject> ControlledNPCs = new List<GameObject>();
        public List<GameObject> Environments = new List<GameObject>();

        System.Windows.Forms.Keys LastKey = System.Windows.Forms.Keys.Escape;
        public int NpcDistance = 225;

        public FormColors colors = null;
        
        
        

        public void AddObject(GameObject _object)
        {
            if (_object.tag == default(ObjectTypeEnum))
            {
                Console.WriteLine($"Не удалось добавить объект {_object.ObjectName}! Не указан явно тег");
            }
            switch (_object.tag)
            {
                case ObjectTypeEnum.NPC:
                    NPCs.Add(_object);
                    break;
                case ObjectTypeEnum.ControlledNPC:
                    ControlledNPCs.Add(_object);
                    break;
                case ObjectTypeEnum.Environment:
                    Environments.Add(_object);
                    break;
            }
        }

        private void SetBinds()
        {
            // board keys
            mainRenderForm.KeyDown += CSGameKeyClick;

            // form
            mainRenderForm.SizeChanged += CSGameWindowSizeChange;

            // mouse
            mainRenderForm.MouseClick += CSGameUserClick;
            mainRenderForm.MouseMove += CSGameMouseMove;
            mainRenderForm.MouseWheel += CSGameMouseWheel;
        }

        private void CSGameMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
                if(e.Delta > 0 && ZoomModel.MapZoom < ZoomModel.ZoomMax)
                    ZoomModel.MapZoom += 0.01f;
                else if (e.Delta < 0 && ZoomModel.MapZoom > ZoomModel.ZoomMin)
                    ZoomModel.MapZoom -= 0.01f;

                changeSize();
        }

        #region Events
        private void CSGameMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.X > mainRenderForm.Width - 100)
            {
                objectsMoving.XMove = -cameraSpeed;
                isHorizCameraMoving = true;
            }
            else if (e.X < 100)
            {
                objectsMoving.XMove = cameraSpeed;
                isHorizCameraMoving = true;
            }
            else
            {
                objectsMoving.XMove = 0;
                isHorizCameraMoving = false;
            }

            if (e.Y > mainRenderForm.Height - 100)
            {
                objectsMoving.YMove = -cameraSpeed;
                isVerticCameraMoving = true;
            }
            else if (e.Y < 100)
            {
                objectsMoving.YMove = cameraSpeed;
                isVerticCameraMoving = true;
            }
            else
            {
                objectsMoving.YMove = 0;
                isVerticCameraMoving = false;
            }
            isCameraMoving = isHorizCameraMoving || isVerticCameraMoving;
        }

        private void CSGameUserClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            endPoint = new Position(e.X, e.Y);
            delX = e.X - hero.ObjectPosition.XPos;
            K = delX / (e.Y - hero.ObjectPosition.YPos);
            PositionChange = new Vector2() { XMove = delX / 100, YMove = delX / 100f / K };
            isHeroesMoving = true;
        }

        private void CSGameWindowSizeChange(object sender, EventArgs e)
        {
            
        }





        private void CSGameKeyClick(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            System.Windows.Forms.Keys key = e.KeyCode;
            

            LastKey = key;
        }

        #endregion

        protected override void initObjects()
        {
            base.initObjects();
            mapStyle = new Models.Styles.MapStyle(1280 * 3, 720 * 3);
            
            /// INIT ALL GAMEOBJECTS

            // Screen positions
            // (0,0)    (0,n)
            // 
            // (m, 0)   (m,n)

            hero = new GameObject("Your name", GameStrings.ObjectImagesPath+"Hero_1.png", new SharpDX.Size2(NPCsize.Width, NPCsize.Height), new Position(mainRenderForm.Width/2, mainRenderForm.Height / 2), RenderTarget);
            hero.tag = ObjectTypeEnum.Hero;
            heroScreenPosition = new Position(mainRenderForm.Width / 2, mainRenderForm.Height / 2);

            VisibleMap = new Background("1", GameStrings.ObjectImagesPath+"Terrain.png", new SharpDX.Size2(mapStyle.MapWidht, mapStyle.MapHeight), new Position(mapStyle.MapWidht / 2, mapStyle.MapHeight / 2), RenderTarget);
            VisibleMap.tag = ObjectTypeEnum.Terrain;
            
        }


        protected override void initForm()
        {
            base.initForm();
            SetBinds();

            /// INIT ALL STYLES
            colors = new FormColors(RenderTarget);
            /// ...
            /// 
            
        }

        public override void GameLogic()
        {
            #region LOOP
            base.GameLogic();

            if (isCameraMoving)
            {
                MoveObjects(objectsMoving);
            }

            RenderTarget.DrawBitmap(VisibleMap.ObjectBitmap, VisibleMap.ObjectRectangle, 1, SharpDX.Direct2D1.BitmapInterpolationMode.NearestNeighbor);

            if (isHeroesMoving)
            {
                if ((Math.Abs(endPoint.XPos - hero.ObjectPosition.XPos) > (NPCsize.Width * ZoomModel.MapZoom)) || (Math.Abs(endPoint.YPos - hero.ObjectPosition.YPos) > (NPCsize.Height * ZoomModel.MapZoom)))
                {
                    hero.Move(PositionChange);
                }
                else
                {
                    isHeroesMoving = false;
                }
            }

            RenderTarget.DrawBitmap(hero.ObjectBitmap, hero.ObjectRectangle, 1, SharpDX.Direct2D1.BitmapInterpolationMode.NearestNeighbor);
            RenderTarget.DrawText(hero.ObjectName, this.textFormat, hero.ObjectRectangleName, new FormColors(RenderTarget).redBrush);

            // DRAW NPCs
            foreach (var npc in NPCs)
            {
                RenderTarget.DrawBitmap(npc.ObjectBitmap, npc.ObjectRectangle, 1, SharpDX.Direct2D1.BitmapInterpolationMode.NearestNeighbor);
                RenderTarget.DrawText(npc.ObjectName, this.textFormat, npc.ObjectRectangleName, new FormColors(RenderTarget).redBrush);
            }
            // DRAW ControlledNPCs
            foreach (var controlledNpc in ControlledNPCs)
            {
                RenderTarget.DrawBitmap(controlledNpc.ObjectBitmap, controlledNpc.ObjectRectangle, 1, SharpDX.Direct2D1.BitmapInterpolationMode.NearestNeighbor);
                RenderTarget.DrawText(controlledNpc.ObjectName, this.textFormat, controlledNpc.ObjectRectangleName, new FormColors(RenderTarget).redBrush);
            }
            // DRAW ENVIRONMENT
            foreach (var env in Environments)
            {
                RenderTarget.DrawBitmap(env.ObjectBitmap, env.ObjectRectangle, 1, SharpDX.Direct2D1.BitmapInterpolationMode.NearestNeighbor);
            }

            #endregion
        }

        private void MoveObjects(Vector2 vector)
        {
            VisibleMap.Move(vector);
            endPoint.XPos += vector.XMove;
            endPoint.YPos += vector.YMove;
            foreach (var npc in NPCs)
            {
                npc.Move(vector);
            }
            foreach (var controllednpc in ControlledNPCs)
            {
                controllednpc.Move(vector);
            }
            foreach (var env in Environments)
            {
                env.Move(vector);
            }
            hero.Move(vector);
        }
        private void changeSize()
        {
            VisibleMap.ReSize();

            foreach (var npc in NPCs)
            {
                npc.ReSize();
            }
            foreach (var controllednpc in ControlledNPCs)
            {
                controllednpc.ReSize();
            }
            foreach (var env in Environments)
            {
                env.ReSize();
            }
            hero.ReSize();
        }
    }
}
