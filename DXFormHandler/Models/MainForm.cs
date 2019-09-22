using DXFormHandler.Models;
using System;
using System.Collections.Generic;
using DXFormHandler.Models.Objects;

namespace DXFormHandler
{
    class MainForm : DXFormHandler.Controller.GameForm
     {
        public MainForm(string formTitle) : base(formTitle)
        {

        }

        #region Require gameObjects
        private int deltaX;
        private int deltaY;

        private GameObject hero;

        private Position endPoint = new Position(255,255);
        private float delX;
        private float K;

        private Position heroScreenPosition;
        private Vector2 PositionChange = new Vector2() { XMove = 0, YMove = 0 };

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
            mainRenderForm.KeyDown += CSGameKeyClick;
            mainRenderForm.SizeChanged += CSGameWindowSizeChange;
            mainRenderForm.MouseClick += CSGameUserClick;
        }

        private void CSGameUserClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            endPoint = new Position(e.X, e.Y);
            delX = e.X - hero.ObjectPosition.XPos;
            K = delX / (e.Y - hero.ObjectPosition.YPos);
            PositionChange = new Vector2() { XMove = delX / 100, YMove = delX / 100f / K };
            Console.WriteLine($"Move from {hero.ObjectPosition.XPos}x{hero.ObjectPosition.YPos} to {e.X}x{e.Y}");
        }

        private void CSGameWindowSizeChange(object sender, EventArgs e)
        {
            deltaX = mainRenderForm.Width / 15;
            deltaY = mainRenderForm.Height / 15;
            Console.WriteLine("\tScreen:\t" + mainRenderForm.Width + "x" + mainRenderForm.Height);
            Console.WriteLine("\tDeltaPos\t" + deltaX + "x" + deltaY );
        }


        private void CSGameKeyClick(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            System.Windows.Forms.Keys key = e.KeyCode;
            
            switch (key)
            {
                case System.Windows.Forms.Keys.Right:

                    if (VisibleMap.ObjectPosition.XPos < VisibleMap.RightBorder)
                    {
                        PositionChange.XMove = 1;
                        hero.Move(PositionChange);
                        
                        if (hero.ObjectPosition.XPos >= heroScreenPosition.XPos + deltaX)
                        {
                            PositionChange.XMove = deltaX;
                            VisibleMap.Move(PositionChange);

                            PositionChange.XMove = -deltaX;
                            MoveObjects(PositionChange);

                        }
                        PositionChange.XMove = 0;
                    }
                    break;

                case System.Windows.Forms.Keys.Left:

                    if (VisibleMap.ObjectPosition.XPos > VisibleMap.LeftBorder)
                    {
                        PositionChange.XMove = -1;
                        hero.Move(PositionChange);

                        if (hero.ObjectPosition.XPos <= heroScreenPosition.XPos - deltaX)
                        {
                            PositionChange.XMove = -deltaX;
                            VisibleMap.Move(PositionChange);
                            PositionChange.XMove = deltaX;
                            MoveObjects(PositionChange);
                        }
                        PositionChange.XMove = 0;
                    }
                    break;

                case System.Windows.Forms.Keys.Up:

                    if (VisibleMap.ObjectPosition.YPos > VisibleMap.UpBorder)
                    {
                        PositionChange.YMove = -1;
                        hero.Move(PositionChange);

                        if (hero.ObjectPosition.YPos <= heroScreenPosition.YPos - deltaY)
                        {
                            PositionChange.YMove = -deltaY;
                            VisibleMap.Move(PositionChange);
                            PositionChange.YMove = deltaY;
                            MoveObjects(PositionChange);
                        }
                        PositionChange.YMove = 0;
                    }
                    break;

                case System.Windows.Forms.Keys.Down:

                    if (VisibleMap.ObjectPosition.YPos < VisibleMap.DownBorder)
                    {
                        PositionChange.YMove = 1;
                        hero.Move(PositionChange);

                        if (hero.ObjectPosition.YPos >= heroScreenPosition.YPos + deltaY)
                        {
                            PositionChange.YMove = deltaY;
                            VisibleMap.Move(PositionChange);
                            PositionChange.YMove = -deltaY;
                            MoveObjects(PositionChange);
                        }
                        PositionChange.YMove = 0;

                    }
                    break;

                default:
                    break;
            }

            LastKey = key;
        }

        protected override void initObjects()
        {
            base.initObjects();
            mapStyle = new Models.Styles.MapStyle(4000,4000);
            deltaX = mainRenderForm.Width / 15;
            deltaY = mainRenderForm.Height / 15;

            /// INIT ALL GAMEOBJECTS

            // Screen positions
            // (0,0)    (0,n)
            // 
            // (m, 0)   (m,n)

            hero = new GameObject("Your name", "C:/Users/Ivan/Downloads/1.png", new SharpDX.Size2(128, 128), new Position(mainRenderForm.Width/2, mainRenderForm.Height / 2), RenderTarget);
            hero.tag = ObjectTypeEnum.Hero;
            heroScreenPosition = new Position(mainRenderForm.Width / 2, mainRenderForm.Height / 2);
            hero.Speed = 3;
            VisibleMap = new Background("1", "C:/Users/Ivan/Downloads/Terrain.png", new SharpDX.Size2(mapStyle.MapWidht, mapStyle.MapHeight), new Position(mapStyle.MapWidht / 2, mapStyle.MapHeight / 2), RenderTarget);
            VisibleMap.tag = ObjectTypeEnum.Terrain;
            VisibleMap.LeftBorder = mapStyle.MapHeight / 2 + 1;
            VisibleMap.RightBorder = mapStyle.MapWidht + (int)heroScreenPosition.XPos;
            VisibleMap.UpBorder = mapStyle.MapWidht / 2 + 1;
            VisibleMap.DownBorder = mapStyle.MapWidht + (int)heroScreenPosition.YPos;

            VisibleMap.Speed = 3;
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

        Vector2 npcMove = new Vector2();
        public override void GameLogic()
        {
            #region LOOP
            base.GameLogic();

            RenderTarget.DrawBitmap(VisibleMap.ObjectBitmap, VisibleMap.ObjectRectangle, 1, SharpDX.Direct2D1.BitmapInterpolationMode.NearestNeighbor);

            if (Math.Abs(endPoint.XPos - hero.ObjectPosition.XPos) > 10 && Math.Abs(endPoint.YPos - hero.ObjectPosition.YPos) > 10)
            {
               //MoveObjects(new Vector2() { XMove = -PositionChange.XMove, YMove = -PositionChange.YMove });
                hero.Move(PositionChange);
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
                // MOVE X
                if (controlledNpc.ObjectPosition.XPos >= hero.ObjectPosition.XPos + NpcDistance)
                {
                    npcMove.XMove = -1;
                    controlledNpc.Move(npcMove);
                }
                else if (controlledNpc.ObjectPosition.XPos <= hero.ObjectPosition.XPos - NpcDistance/2)
                {
                    npcMove.XMove = 1;
                    controlledNpc.Move(npcMove);
                }
                else { }

                if (controlledNpc.ObjectPosition.YPos >= hero.ObjectPosition.YPos + NpcDistance)
                {
                    npcMove.YMove = -1;
                    controlledNpc.Move(npcMove);
                }
                else if (controlledNpc.ObjectPosition.YPos <= hero.ObjectPosition.YPos - NpcDistance/2)
                {
                    npcMove.YMove = 1;
                    controlledNpc.Move(npcMove);
                }
                else { }

                npcMove.YMove = 0;
                npcMove.XMove = 0;

                RenderTarget.DrawBitmap(controlledNpc.ObjectBitmap, controlledNpc.ObjectRectangle, 1, SharpDX.Direct2D1.BitmapInterpolationMode.NearestNeighbor);
                RenderTarget.DrawText(controlledNpc.ObjectName, this.textFormat, controlledNpc.ObjectRectangleName, new FormColors(RenderTarget).redBrush);
            }
            // DRAW ENVIRONMENT
            foreach (var env in NPCs)
            {
                RenderTarget.DrawBitmap(env.ObjectBitmap, env.ObjectRectangle, 1, SharpDX.Direct2D1.BitmapInterpolationMode.NearestNeighbor);
            }

            #endregion
        }

        private void MoveObjects(Vector2 vector)
        {
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
    }
}
