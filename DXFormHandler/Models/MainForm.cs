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

        private Position endPoint = new Position(255, 255);
        
        private Vector2 CameraMoving = new Vector2() { XMove = 0, YMove = 0 };
        private int cameraSpeed = 2;

        private bool isCameraMoving = false;
        private bool isHorizCameraMoving = false;
        private bool isVerticCameraMoving = false;


        private Background VisibleMap;
        #endregion

        public List<GameObject> NPCs = new List<GameObject>();
        public List<Target> ControlledNPCs = new List<Target>();
        public List<GameObject> Environments = new List<GameObject>();

        System.Windows.Forms.Keys LastKey = System.Windows.Forms.Keys.Escape;

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
                    ControlledNPCs.Add(_object as Target);
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
            if (e.Delta > 0 && ZoomModel.MapZoom < ZoomModel.ZoomMax)
                ZoomModel.MapZoom += 0.05f;
            else if (e.Delta < 0 && ZoomModel.MapZoom > ZoomModel.ZoomMin)
                ZoomModel.MapZoom -= 0.05f;

            ZoomModel.DeltaZoom = e.Delta / 100;
            MapZoom();
        }

        #region Events
        private void CSGameMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.X > mainRenderForm.Width - 100)
            {
                CameraMoving.XMove = -cameraSpeed;
                isHorizCameraMoving = true;
            }
            else if (e.X < 100)
            {
                CameraMoving.XMove = cameraSpeed;
                isHorizCameraMoving = true;
            }
            else
            {
                CameraMoving.XMove = 0;
                isHorizCameraMoving = false;
            }

            if (e.Y > mainRenderForm.Height - 100)
            {
                CameraMoving.YMove = -cameraSpeed;
                isVerticCameraMoving = true;
            }
            else if (e.Y < 100)
            {
                CameraMoving.YMove = cameraSpeed;
                isVerticCameraMoving = true;
            }
            else
            {
                CameraMoving.YMove = 0;
                isVerticCameraMoving = false;
            }
            isCameraMoving = isHorizCameraMoving || isVerticCameraMoving;
        }

        private void CSGameUserClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            foreach (var target in ControlledNPCs)
            {
                target.Move(new Vector2() { XMove = e.X, YMove = e.Y });
            }
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

            VisibleMap = new Background("1", GameStrings.ObjectImagesPath + "Terrain.png", new SharpDX.Size2(mapStyle.MapWidht, mapStyle.MapHeight), new Position(mapStyle.MapWidht / 2, mapStyle.MapHeight / 2), RenderTarget);
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
                MoveObjects(CameraMoving);
            }

            RenderTarget.DrawBitmap(VisibleMap.ObjectBitmap, VisibleMap.ObjectRectangle, 1, SharpDX.Direct2D1.BitmapInterpolationMode.NearestNeighbor);

            foreach (var target in ControlledNPCs)
            {
                if (target.isHeroesMoving)
                {
                    if ((Math.Abs(target.EndPosition.XPos - target.ObjectPosition.XPos) > 60) || (Math.Abs(target.EndPosition.YPos - target.ObjectPosition.YPos) > 60))
                    {
                        target.Move(target.PositionChange);
                    }
                    else
                    {
                        target.isHeroesMoving = false;
                    }
                }

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
            }

            private void MapZoom()
            {
                VisibleMap.ReSize((int)ZoomModel.DeltaZoom);

                foreach (var npc in NPCs)
                {
                    npc.ReSize((int)ZoomModel.DeltaZoom);
                }
                foreach (var controllednpc in ControlledNPCs)
                {
                    controllednpc.ReSize((int)ZoomModel.DeltaZoom);
                }
                foreach (var env in Environments)
                {
                    env.ReSize((int)ZoomModel.DeltaZoom);
                }
            }
    }
}
