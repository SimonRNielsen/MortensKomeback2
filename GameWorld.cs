using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;

namespace MortensKomeback2
{
    public class GameWorld : Game
    {
        #region Fields

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private static Camera2D camera;
        private static Vector2 mousePosition;
        private bool escape;
        private static bool leftMouseButtonClick;
        private static bool rightMouseButtonClick;
        private static bool closeMenu = false;
        private static bool menuActive;
        private static bool exitGame = false;
        private static bool restart = false;
        private static bool battleActive = false;
        private static bool dialogue = false;
        private bool mortenLives = true;
        private static MousePointer mousePointer = new MousePointer();
        private static List<Menu> menu = new List<Menu>();
        private List<GameObject> gameObjects = new List<GameObject>();
        public static List<GameObject> newGameObjects = new List<GameObject>();
        public static List<Item> playerInventory = new List<Item>();
        public static List<Item> equippedPlayerInventory = new List<Item>();
        public static List<Item> hiddenItems = new List<Item>();
        public static Dictionary<string, Texture2D> commonSprites = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D[]> animationSprites = new Dictionary<string, Texture2D[]>();
        public static Dictionary<string, SoundEffect> commonSounds = new Dictionary<string, SoundEffect>();
        public static Dictionary<string, Song> backgroundMusic = new Dictionary<string, Song>();
        public static SpriteFont mortensKomebackFont;
        private static Color grayGoose = new Color(209, 208, 206);
        private List<Area> area51 = new List<Area>();
        private static Player playerInstance;
        public static List<NPC> nPCs = new List<NPC>();
        private static Random random = new Random();
        public static string currentTrack = null;

        #endregion Fields

        #region Properties

        public static Camera2D Camera { get => camera; set => camera = value; }
        public static Vector2 MousePosition { get => mousePosition; }
        public static bool LeftMouseButtonClick { get => leftMouseButtonClick; }
        public static bool RightMouseButtonClick { get => rightMouseButtonClick; }
        public static bool CloseMenu { get => closeMenu; set => closeMenu = value; }
        public static bool MenuActive { get => menuActive; }
        public static bool Dialogue { get => dialogue; set => dialogue = value; }
        internal static Player PlayerInstance { get => playerInstance; set => playerInstance = value; }
        public static bool BattleActive { get => battleActive; set => battleActive = value; }
        public static Color GrayGoose { get => grayGoose; }
        public static Random Random { get => random; }

        #endregion Properties

        #region Constructor

        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        #endregion

        #region Methods


        protected override void Initialize()
        {

            // TODO: Add your initialization logic here

            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            Camera = new Camera2D(GraphicsDevice, Vector2.Zero);

            //Preloading of all assets
            #region Assets

            mortensKomebackFont = Content.Load<SpriteFont>("mortalKombatFont");
            LoadCommonSprites();
            LoadAnimationArrays();
            LoadCommonSounds();
            LoadBackgroundSongs();

            #endregion Assets
            #region Items & Player

            hiddenItems.Add(new QuestItem(1, false, new Vector2(-205, -1080 * 10)));
            hiddenItems.Add(new QuestItem(0, false, new Vector2(random.Next(-601, 600), (-1080 * 12) + random.Next(-401, 400))));
            playerInventory.Add(new QuestItem(1, true, new Vector2(-10000, -10000)));
            playerInventory.Add(new QuestItem(1, true, new Vector2(-10000, -10000)));
            hiddenItems.Add(new QuestItem(3, false, new Vector2(random.Next(-601, 600), (1080 * 2) + random.Next(-401, 400)))); //Monks bible
            hiddenItems.Add(new QuestItem(4, false, new Vector2(random.Next(-601, 600), (-1080 * 6) + random.Next(-401, 400)))); //Nuns rosary

            menu.Add(new Menu(Camera.Position, 3));

            PlayerInstance = new Player(PlayerClass.Crusader, FindNPCLocation()); //Using it as a reference to get the players position
            newGameObjects.Add(PlayerInstance);

            #endregion Items & Player
            #region GUI

            newGameObjects.Add(new HealthBar(0.55f, 1));

            #endregion GUI
            #region Obstacles

            newGameObjects.Add(new AvSurface(200, 0)); //Sæt til igen
            newGameObjects.Add(new Obstacle(500, 0, "stone"));
            newGameObjects.Add(new Obstacle(-400, 00, "hole"));
            
            //Firepits in Bossroom
            //newGameObjects.Add(new Obstacle(650, (1080 * 7)-150, "firepit"));
            //newGameObjects.Add(new Obstacle(650, (1080 * 7)+150, "firepit"));
            //newGameObjects.Add(new Obstacle(-650, (1080 * 7)-150, "firepit"));
            //newGameObjects.Add(new Obstacle(-650, (1080 * 7)+150, "firepit"));


            //pews on right side in main room
            newGameObjects.Add(new Obstacle(535, 800, "pew"));
            newGameObjects.Add(new Obstacle(535, 800 + 350, "pew"));
            newGameObjects.Add(new Obstacle(535, 800 + 350*2, "pew"));
            newGameObjects.Add(new Obstacle(535, 800 + 350*3, "pew"));
            newGameObjects.Add(new Obstacle(535, 800 + 350*4, "pew"));
            newGameObjects.Add(new Obstacle(535, 800 + 350*5, "pew"));
            newGameObjects.Add(new Obstacle(535, 800 + 350*6, "pew"));
            newGameObjects.Add(new Obstacle(535, 800 + 350*7, "pew"));

            //pews on left side in main room
            newGameObjects.Add(new Obstacle(-535, 800, "leftPew"));
            newGameObjects.Add(new Obstacle(-535, 800 + 350, "leftPew"));
            newGameObjects.Add(new Obstacle(-535, 800 + 350 * 2, "leftPew"));
            newGameObjects.Add(new Obstacle(-535, 800 + 350 * 3, "leftPew"));
            newGameObjects.Add(new Obstacle(-535, 800 + 350 * 4, "leftPew"));
            newGameObjects.Add(new Obstacle(-535, 800 + 350 * 5, "leftPew"));
            newGameObjects.Add(new Obstacle(-535, 800 + 350 * 6, "leftPew"));
            newGameObjects.Add(new Obstacle(-535, 800 + 350 * 7, "leftPew"));

            #endregion Obstacles
            #region Areas

            newGameObjects.Add(new Area(new Vector2(0, 0), 1, "Room1"));       //main room
            newGameObjects.Add(new Area(new Vector2(0, 1080), 2, "Room1a"));    //main room
            newGameObjects.Add(new Area(new Vector2(0, 1080 * 2), 3, "Room1b"));    //main room
            newGameObjects.Add(new Area(new Vector2(0, 1080 * 3), 4, "Room1c"));  //main room
            newGameObjects.Add(new Area(new Vector2(0, 1080 * 5), 0, "Room8"));  // våbenhus - enemies
            newGameObjects.Add(new Area(new Vector2(0, 1080 * 7), 0, "Room9"));  // Bossfight
            newGameObjects.Add(new Area(new Vector2(0, 1080 * 9), 0, "Room10"));  // Scepter

            newGameObjects.Add(new Area(new Vector2(0, -1080 * 2), 0, "Room2"));          //ventre side, rum 2, nonne
            newGameObjects.Add(new Area(new Vector2(0, -1080 * 4), 0, "Room3"));     //ventre side, rum 3 enemies
            newGameObjects.Add(new Area(new Vector2(0, -1080 * 6), 0, "Room4"));     //ventre side, rum 4 enemies
            newGameObjects.Add(new Area(new Vector2(0, -1080 * 8), 0, "Room5"));   //ventre side, rum 5, item


            newGameObjects.Add(new Area(new Vector2(0, -1080 * 10), 0, "Room6"));      //højre side, rum 1, munk
            newGameObjects.Add(new Area(new Vector2(0, -1080 * 12), 0, "Room7"));      //højre side, rum 2, secret + item

            #endregion Areas
            #region Doors

            float leftSide = -820;
            float rigthSide = 820;
            float topSide = -430;
            float bottomSide = 430;

            float leftTele = -700;
            float rigthTele = 700;

            newGameObjects.Add(new Door(leftSide, 0, DoorTypes.Closed, DoorRotation.Left, new Vector2(rigthTele, -1080 * 2))); //1.2
            newGameObjects.Add(new Door(rigthSide, -1080 * 2, DoorTypes.Open, DoorRotation.Right, new Vector2(leftTele, 0))); //2.1

            newGameObjects.Add(new Door(leftSide, -1080 * 2, DoorTypes.Closed, DoorRotation.Left, new Vector2(rigthTele, -1080 * 4))); //2.3
            newGameObjects.Add(new Door(rigthSide, -1080 * 4, DoorTypes.Open, DoorRotation.Right, new Vector2(leftTele, -1080 * 2))); //3.2

            newGameObjects.Add(new Door(leftSide, -1080 * 4, DoorTypes.Closed, DoorRotation.Left, new Vector2(rigthTele, -1080 * 6))); //3.4
            newGameObjects.Add(new Door(rigthSide, -1080 * 6, DoorTypes.Open, DoorRotation.Right, new Vector2(leftTele, -1080 * 4))); //4.3

            newGameObjects.Add(new Door(0, -1080 * 4 + bottomSide, DoorTypes.Closed, DoorRotation.Bottom, new Vector2(0, -1080 * 8 + topSide + 120))); //3.5
            newGameObjects.Add(new Door(0, -1080 * 8 + topSide, DoorTypes.Closed, DoorRotation.Top, new Vector2(0, -1080 * 4 + bottomSide - 120))); //5.3

            newGameObjects.Add(new Door(rigthSide, 0, DoorTypes.Closed, DoorRotation.Right, new Vector2(leftTele, -1080 * 10))); //1.6
            newGameObjects.Add(new Door(leftSide, -1080 * 10, DoorTypes.Open, DoorRotation.Left, new Vector2(rigthTele, 0))); //6.1

            newGameObjects.Add(new Door(540, -1080 * 10 + topSide + 55, DoorTypes.Secret, DoorRotation.Top, new Vector2(leftSide / 2 - 50, -1080 * 12 + bottomSide - 150))); //6.7
            newGameObjects.Add(new Door(-540, -1080 * 12 + bottomSide - 55, DoorTypes.Secret, DoorRotation.Bottom, new Vector2(rigthSide / 2 + 100, -1080 * 10 + topSide + 150))); //7.6

            newGameObjects.Add(new Door(0, bottomSide + 1080 * 3, DoorTypes.Locked, DoorRotation.Bottom, new Vector2(0, 1080 * 5 + topSide + 120))); //1C.8
            newGameObjects.Add(new Door(0, topSide + 1080 * 5, DoorTypes.Open, DoorRotation.Top, new Vector2(0, 1080 * 3 + bottomSide - 120))); //8.1C

            newGameObjects.Add(new Door(0, bottomSide + 1080 * 5, DoorTypes.Locked, DoorRotation.Bottom, new Vector2(0, 1080 * 7 + topSide + 120))); //8.9
            newGameObjects.Add(new Door(0, topSide + 1080 * 7, DoorTypes.Open, DoorRotation.Top, new Vector2(0, 1080 * 5 + bottomSide - 120))); //9.8

            newGameObjects.Add(new Door(0, bottomSide + 1080 * 7, DoorTypes.Locked, DoorRotation.Bottom, new Vector2(0, 1080 * 9 + topSide + 120))); //9.10
            newGameObjects.Add(new Door(0, topSide + 1080 * 9, DoorTypes.Open, DoorRotation.Top, new Vector2(0, 1080 * 7 + bottomSide - 120))); //10.9

            #endregion Doors
            #region environment
            newGameObjects.Add(new Environment(new Vector2(0, 2000), 0, 1.4f));      //carpet


            #endregion environment
            #region NPCs & Enemies

            gameObjects.Add(new Boss(new Vector2(0, 1080 * 7)));
            newGameObjects.Add(new NPC(0, 0, new Vector2(0, - 1080 * 10))); //monk
            newGameObjects.Add(new NPC(1, 1, new Vector2(0, -1080 * 2))); //nun
            newGameObjects.Add(new Enemy(new Vector2(random.Next(-601, 600), 1030 * 1)));
            newGameObjects.Add(new Enemy(new Vector2(random.Next(-601, 600), 1030 * 3)));
            newGameObjects.Add(new Enemy(new Vector2(random.Next(-601, 600), 1080 * -4)));
            newGameObjects.Add(new Enemy(new Vector2(random.Next(-601, 600), 1080 * -8)));
            newGameObjects.Add(new Enemy(new Vector2(random.Next(-601, 600), 1080 * -8)));
            newGameObjects.Add(new Enemy(new Vector2(random.Next(-601, 600), 1080 * 5)));
            newGameObjects.Add(new Enemy(new Vector2(random.Next(-601, 600), 1080 * 5)));
            newGameObjects.Add(new Enemy(new Vector2(random.Next(-601, 600), 1080 * 5)));

            #endregion NPCs & Enemies

            base.Initialize();

        }


        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

        }

        protected override void Update(GameTime gameTime)
        {

            #region Exit & Restart

            if (exitGame)
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !escape)
            {
                if (menuActive && !DetectInOutro())
                    closeMenu = true;
                else if (DetectInOutro()) { }
                else
                    newGameObjects.Add(new Menu(Camera.Position, 4));
                escape = true;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Escape))
                escape = false;

            //Restart logic
            if (restart)
                Restart();

            #endregion Exit & Restart
            #region Win logic

            if (playerInventory.Find(popeSceptre => popeSceptre.ItemName == "Popes sceptre") != null && playerInstance.InRoom == "Room10" && !battleActive)
                menu.Add(new Menu(Camera.Position, 1));

            #endregion Win logic
            #region References

            if (area51.Count == 0)
                foreach (GameObject gameObject in gameObjects)
                    if (gameObject is Area)
                        area51.Add(gameObject as Area);

            if (nPCs.Count == 0)
                nPCs = FindNPCLocation();

            PlayerInRoom();

            #endregion References

            // TODO: Add your update logic here

            #region Mouse logic

            var mouseState = Mouse.GetState();
            mousePosition = new Vector2((int)(mouseState.X / Camera.Zoom) - (int)((float)_graphics.PreferredBackBufferWidth / 2 / Camera.Zoom) + (int)Camera.Position.X, (int)(mouseState.Y / Camera.Zoom) - (int)((float)_graphics.PreferredBackBufferHeight / 2 / Camera.Zoom) + (int)Camera.Position.Y);
            leftMouseButtonClick = mouseState.LeftButton == ButtonState.Pressed;
            rightMouseButtonClick = mouseState.RightButton == ButtonState.Pressed;

            #endregion Mouse logic
            #region Loss logic

            mortenLives = false;
            if (playerInstance.IsAlive || DetectInOutro())
                mortenLives = true;
            if (!mortenLives)
                newGameObjects.Add(new Menu(Camera.Position, 2));

            #endregion Loss logic
            #region Main game-loop

            //Updates gameObjects and collision
            bool bossExists = false;
            foreach (GameObject gameObject in gameObjects)
            {
                //Pause-logic
                if (!menuActive && !battleActive && !dialogue)
                    gameObject.Update(gameTime);
                else if (dialogue && gameObject is Dialogue)
                    gameObject.Update(gameTime);
                else if (battleActive && (gameObject is BattleField || gameObject is HealthBar) && !menuActive)
                    gameObject.Update(gameTime);
                if (!menuActive && !battleActive && dialogue && gameObject is Player && Keyboard.GetState().IsKeyDown(Keys.Enter))
                    gameObject.Update(gameTime);


                foreach (GameObject other in gameObjects)
                {
                    if (gameObject is Player)
                    {
                        if (other is AvSurface || other is Obstacle || other is Door || other is Enemy)
                        {
                            gameObject.CheckCollision(other);
                            other.CheckCollision(gameObject);
                        }
                    }

                    if (gameObject is Enemy)
                    {
                        if (gameObject is Boss)
                            bossExists = true;
                        if (other is Obstacle)
                        {
                            gameObject.CheckCollision(other);
                            other.CheckCollision(gameObject);
                        }

                    }

                    if (gameObject is Area)
                        if (other is Player)
                            if ((gameObject as Area).Room == (other as Player).InRoom)
                                (gameObject as Area).CheckCollision(other);
                }
            }
            //Gives player the main quest item after defeating the boss
            if (!bossExists && playerInventory.Find(popeSceptre => popeSceptre.ItemName == "Popes sceptre") == null && !battleActive && playerInstance.InRoom == "Room9")
            {
                playerInventory.Add(new QuestItem(2, true, new Vector2(-10000, -10000)));
                commonSounds["equipItem"].Play();
                newGameObjects.Add(new Dialogue(new Vector2(Camera.Position.X, Camera.Position.Y + 320), new NPC(2, 0, new Vector2(-10000, -10000))));
            }
            //if (playerInstance.InRoom == "Room9" && !bossExists)
              //  battleActive = false;

            //Removes objects
            gameObjects.RemoveAll(obj => obj.IsAlive == false);

            #endregion Main game-loop
            #region Pray logic

            //Search & Pray logic
            foreach (Item item in hiddenItems)
                item.Update(gameTime);
            hiddenItems.RemoveAll(found => found.IsPickedUp == true);

            #endregion Pray logic
            #region Menu & inventory loop

            //Menu logic
            foreach (Menu menuItem in menu)
            {
                if (BattleActive && (menuItem.IsInventory || menuItem.IsInOutro))
                {
                    continue;
                }

                menuActive = true;
                menuItem.Update(gameTime);
                if (menuItem is Button)
                {
                    (menuItem as Button).CheckCollision(mousePointer);
                }

            }
            if (rightMouseButtonClick && !(BattleActive))
                mousePointer.RightClickEvent();
            if (closeMenu || menu.Count == 0)
            {
                menu.Clear();
                menuActive = false;
                closeMenu = false;
            }
            if (DetectInventory())
                mousePointer.MouseOver();
            menu.RemoveAll(menuItem => menuItem.ButtonObsolete == true);
            playerInventory.RemoveAll(useable => useable.IsUsed == true);

            #endregion Menu & inventory loop
            #region Spawning

            //"Spawns" new items
            foreach (GameObject newGameObject in newGameObjects)
            {
                newGameObject.LoadContent(Content);
                if (newGameObject is Item)
                    hiddenItems.Add(newGameObject as Item);
                else if (newGameObject is Menu || newGameObject is Button)
                    menu.Add(newGameObject as Menu);
                else
                    gameObjects.Add(newGameObject);
            }
            newGameObjects.Clear();

            #endregion Spawning

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

#if DEBUG
            bool disableCollisionDrawing = Keyboard.GetState().IsKeyDown(Keys.Space);
#endif
            // TODO: Add your drawing code here
            _spriteBatch.Begin(transformMatrix: Camera.GetTransformation(), samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            foreach (GameObject gameObject in gameObjects)
            {

                gameObject.Draw(_spriteBatch);
#if DEBUG
                #region Draw CollisionBox
                if (disableCollisionDrawing)
                {
                    if (!(gameObject is BattleField))
                    { DrawCollisionBox(gameObject); }
                    if (gameObject is Area)
                    {
                        DrawLeftCollisionBox(gameObject);
                        DrawRightCollisionBox(gameObject);
                        DrawBottomCollisionBox(gameObject);
                        DrawTopCollisionBox(gameObject);
                    }
                }
                #endregion Draw CollisionBox
#endif

            }
            #region Draw items
            if (DetectInventory())
            {
                foreach (Item item in playerInventory)
                {
                    item.Draw(_spriteBatch);
                    if (disableCollisionDrawing)
                        DrawCollisionBox(item);
                    item.Update(gameTime);
                }

                foreach (Item item in equippedPlayerInventory)
                {
                    item.Draw(_spriteBatch);
                    if (disableCollisionDrawing)
                        DrawCollisionBox(item);
                    item.Update(gameTime);
                }
            }

            foreach (Item item in hiddenItems)
            {
                if (item.IsFound)
                    item.Draw(_spriteBatch);
            }

            foreach (Menu menuItem in menu)
            {
                menuItem.Draw(_spriteBatch);
            }

            #endregion Draw items
#if DEBUG
            _spriteBatch.Draw(commonSprites["collisionTexture"], mousePointer.CollisionBox, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1f);
#endif

            _spriteBatch.End();

            base.Draw(gameTime);
        }

#if DEBUG
        #region DrawCollisionBoxes
        private void DrawCollisionBox(GameObject gameObject)
        {
            if (!(gameObject is Area))
            {

                Color color = Color.Red;
                Rectangle collisionBox = gameObject.CollisionBox;
                Rectangle topLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);
                Rectangle bottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);
                Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);
                Rectangle leftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);

                if (gameObject is Item)
                    color = Color.Purple;

                _spriteBatch.Draw(commonSprites["collisionTexture"], topLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
                _spriteBatch.Draw(commonSprites["collisionTexture"], bottomLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
                _spriteBatch.Draw(commonSprites["collisionTexture"], rightLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
                _spriteBatch.Draw(commonSprites["collisionTexture"], leftLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
            }
        }
        private void DrawLeftCollisionBox(GameObject gameObject)
        {
            if (gameObject is Area)
            {

                Color color = Color.Red;
                Rectangle collisionBox = (gameObject as Area).LeftCollisionBox;
                Rectangle topLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);
                Rectangle bottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);
                Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);
                Rectangle leftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);

                _spriteBatch.Draw(commonSprites["collisionTexture"], topLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
                _spriteBatch.Draw(commonSprites["collisionTexture"], bottomLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
                _spriteBatch.Draw(commonSprites["collisionTexture"], rightLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
                _spriteBatch.Draw(commonSprites["collisionTexture"], leftLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);

            }
        }
        private void DrawRightCollisionBox(GameObject gameObject)
        {
            if (gameObject is Area)
            {

                Color color = Color.Red;
                Rectangle collisionBox = (gameObject as Area).RightCollisionBox;
                Rectangle topLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);
                Rectangle bottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);
                Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);
                Rectangle leftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);

                _spriteBatch.Draw(commonSprites["collisionTexture"], topLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
                _spriteBatch.Draw(commonSprites["collisionTexture"], bottomLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
                _spriteBatch.Draw(commonSprites["collisionTexture"], rightLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
                _spriteBatch.Draw(commonSprites["collisionTexture"], leftLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);

            }
        }
        private void DrawTopCollisionBox(GameObject gameObject)
        {
            if (gameObject is Area)
            {

                Color color = Color.Red;
                Rectangle collisionBox = (gameObject as Area).TopCollisionBox;
                Rectangle topLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);
                Rectangle bottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);
                Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);
                Rectangle leftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);

                _spriteBatch.Draw(commonSprites["collisionTexture"], topLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
                _spriteBatch.Draw(commonSprites["collisionTexture"], bottomLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
                _spriteBatch.Draw(commonSprites["collisionTexture"], rightLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
                _spriteBatch.Draw(commonSprites["collisionTexture"], leftLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);

            }
        }
        private void DrawBottomCollisionBox(GameObject gameObject)
        {
            if (gameObject is Area)
            {

                Color color = Color.Red;
                Rectangle collisionBox = (gameObject as Area).BottomCollisionBox;
                Rectangle topLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);
                Rectangle bottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);
                Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);
                Rectangle leftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);

                _spriteBatch.Draw(commonSprites["collisionTexture"], topLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
                _spriteBatch.Draw(commonSprites["collisionTexture"], bottomLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
                _spriteBatch.Draw(commonSprites["collisionTexture"], rightLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
                _spriteBatch.Draw(commonSprites["collisionTexture"], leftLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);

            }
        }
        #endregion DrawCollisionBoxes
#endif
        #region Sprites & sound
        /// <summary>
        /// Loads sprites into the "commonSprites" Dictionary
        /// </summary>
        private void LoadCommonSprites()
        {

#if DEBUG
            Texture2D collisionTexture = Content.Load<Texture2D>("Sprites\\DEBUG\\pixel");
            commonSprites.Add("collisionTexture", collisionTexture);
#endif
            #region Obstacles

            Texture2D stone = Content.Load<Texture2D>("Sprites\\Obstacle\\stone"); //Stone
            Texture2D hole = Content.Load<Texture2D>("Sprites\\Obstacle\\hole"); //Hole
            Texture2D magic = Content.Load<Texture2D>("Sprites\\Obstacle\\magic"); //magic missile
            Texture2D magicHeal = Content.Load<Texture2D>("Sprites\\Obstacle\\magicHeal"); //magic heal
            Texture2D egg = Content.Load<Texture2D>("Sprites\\Obstacle\\egg"); //magic heal
            Texture2D pew = Content.Load<Texture2D>("Sprites\\area\\pew"); //pew
            Texture2D leftPew = Content.Load<Texture2D>("Sprites\\area\\leftPew"); //pew left
            Texture2D pewPew = Content.Load<Texture2D>("Sprites\\area\\pewPew"); //pew left

            commonSprites.Add("stone", stone);
            commonSprites.Add("hole", hole);
            commonSprites.Add("magic", magic);
            commonSprites.Add("magicHeal", magicHeal);
            commonSprites.Add("egg", egg);
            commonSprites.Add("pew", pew);
            commonSprites.Add("leftPew", leftPew);
            commonSprites.Add("pewPew", pewPew);

            #endregion Obstacles
            #region Items

            Texture2D quest = Content.Load<Texture2D>("Sprites\\Item\\questItemPlaceholder");
            Texture2D mainHand = Content.Load<Texture2D>("Sprites\\Item\\mainHandPlaceholder");
            Texture2D offHand = Content.Load<Texture2D>("Sprites\\Item\\offHandPlaceholder");
            Texture2D torso = Content.Load<Texture2D>("Sprites\\Item\\torsoPlaceholder");
            Texture2D feet = Content.Load<Texture2D>("Sprites\\Item\\feetPlaceholder");
            Texture2D healItem = Content.Load<Texture2D>("Sprites\\Item\\torsoPlaceholder");
            Texture2D blink = Content.Load<Texture2D>("Sprites\\Item\\blinkPlaceholder");
            Texture2D mitre = Content.Load<Texture2D>("Sprites\\Item\\mitre");
            Texture2D helm = Content.Load<Texture2D>("Sprites\\Item\\helm");
            Texture2D sword = Content.Load<Texture2D>("Sprites\\Item\\sword");
            Texture2D key = Content.Load<Texture2D>("Sprites\\Item\\key");
            Texture2D shield = Content.Load<Texture2D>("Sprites\\Item\\shield");
            Texture2D rosary = Content.Load<Texture2D>("Sprites\\Item\\rosary");
            Texture2D staff = Content.Load<Texture2D>("Sprites\\Item\\staff");
            Texture2D potion = Content.Load<Texture2D>("Sprites\\Item\\potion");
            Texture2D scepter = Content.Load<Texture2D>("Sprites\\Item\\scepter");
            Texture2D boots = Content.Load<Texture2D>("Sprites\\Item\\boots");
            Texture2D robe = Content.Load<Texture2D>("Sprites\\Item\\robe");
            Texture2D sling = Content.Load<Texture2D>("Sprites\\Item\\sling");
            Texture2D bible = Content.Load<Texture2D>("Sprites\\Item\\bible");

            commonSprites.Add("questItem", quest);
            commonSprites.Add("mainHandItem", mainHand);
            commonSprites.Add("offHandItem", offHand);
            commonSprites.Add("torsoItem", torso);
            commonSprites.Add("feetItem", feet);
            commonSprites.Add("healItem", healItem);
            commonSprites.Add("blink", blink);
            commonSprites.Add("mitre", mitre);
            commonSprites.Add("helm", helm);
            commonSprites.Add("sword", sword);
            commonSprites.Add("key", key);
            commonSprites.Add("shield", shield);
            commonSprites.Add("rosary", rosary);
            commonSprites.Add("staff", staff);
            commonSprites.Add("potion", potion);
            commonSprites.Add("scepter", scepter);
            commonSprites.Add("boots", boots);
            commonSprites.Add("robe", robe);
            commonSprites.Add("sling", sling);
            commonSprites.Add("bible", bible);


            #endregion Items
            #region GUI

            Texture2D heartSprite = Content.Load<Texture2D>("Sprites\\GUI\\heartSprite");
            Texture2D weaponClassSprite = Content.Load<Texture2D>("Sprites\\GUI\\weaponClassSprite");
            Texture2D questRosarySprite = Content.Load<Texture2D>("Sprites\\GUI\\questRosarySprite");
            Texture2D questKey1Sprite = Content.Load<Texture2D>("Sprites\\GUI\\questKey1Sprite");
            Texture2D questKey2Sprite = Content.Load<Texture2D>("Sprites\\GUI\\questKey2Sprite");
            Texture2D questBibleSprite = Content.Load<Texture2D>("Sprites\\GUI\\questBibleSprite");
            Texture2D dialogueBox = Content.Load<Texture2D>("Sprites\\GUI\\dialogueBox");
            Texture2D healthBarBlack = Content.Load<Texture2D>("Sprites\\GUI\\HealthBar\\healthBarBlack");
            Texture2D healthBarRed = Content.Load<Texture2D>("Sprites\\GUI\\HealthBar\\healthBarRed");

            commonSprites.Add("heartSprite", heartSprite);
            commonSprites.Add("weaponClassSprite", weaponClassSprite);
            commonSprites.Add("questRosarySprite", questRosarySprite);
            commonSprites.Add("questKey1Sprite", questKey1Sprite);
            commonSprites.Add("questKey2Sprite", questKey2Sprite);
            commonSprites.Add("questBibleSprite", questBibleSprite);
            commonSprites.Add("dialogueBox", dialogueBox);
            commonSprites.Add("healthBarBlack", healthBarBlack);
            commonSprites.Add("healthBarRed", healthBarRed);

            #endregion GUI
            #region Menu & Button

            Texture2D menuButton = Content.Load<Texture2D>("Sprites\\Menu\\menuButton");
            Texture2D button = Content.Load<Texture2D>("Sprites\\Menu\\button");
            Texture2D introScreen = Content.Load<Texture2D>("Sprites\\Menu\\introScreen");
            Texture2D winScreen = Content.Load<Texture2D>("Sprites\\Menu\\winScreen");
            Texture2D loseScreen = Content.Load<Texture2D>("Sprites\\Menu\\loseScreen");
            Texture2D inventoryScreen = Content.Load<Texture2D>("Sprites\\Menu\\inventory");
            Texture2D statPanel = Content.Load<Texture2D>("Sprites\\Menu\\statPanel");
            Texture2D pauseScreen = Content.Load<Texture2D>("Sprites\\Menu\\pauseScreen");
            Texture2D characterScreen = Content.Load<Texture2D>("Sprites\\Menu\\chooseCharacter");

            commonSprites.Add("menuButton", menuButton);
            commonSprites.Add("button", button);
            commonSprites.Add("introScreen", introScreen);
            commonSprites.Add("winScreen", winScreen);
            commonSprites.Add("loseScreen", loseScreen);
            commonSprites.Add("inventory", inventoryScreen);
            commonSprites.Add("statPanel", statPanel);
            commonSprites.Add("pauseScreen", pauseScreen);
            commonSprites.Add("characterScreen", characterScreen);

            #endregion Menu & Button
            #region NPC
            Texture2D nunNPC = Content.Load<Texture2D>("Sprites\\Charactor\\nunNPC");
            Texture2D nunNPCrosary = Content.Load<Texture2D>("Sprites\\Charactor\\nunNPCrosary");
            commonSprites.Add("nunNPC", nunNPC);
            commonSprites.Add("nunNPCrosary", nunNPCrosary);

            Texture2D monkNPC = Content.Load<Texture2D>("Sprites\\Charactor\\monkNPC");
            Texture2D monkNPCbible = Content.Load<Texture2D>("Sprites\\Charactor\\monkNPCbible");
            commonSprites.Add("monkNPC", monkNPC);
            commonSprites.Add("monkNPCbible", monkNPCbible);

            Texture2D talkPrompt = Content.Load<Texture2D>("Sprites\\Charactor\\talk");
            commonSprites.Add("talkPrompt", talkPrompt);
            #endregion NPC
        }

        /// <summary>
        /// Loads animation arrays into the "animationSprites" Dictionary
        /// </summary>
        private void LoadAnimationArrays()
        {

            #region Areas

            Texture2D[] areaArray = new Texture2D[6]
            {
            Content.Load<Texture2D>("Sprites\\area\\room_single"),
            Content.Load<Texture2D>("Sprites\\area\\bigRoom1"),
            Content.Load<Texture2D>("Sprites\\area\\bigRoom2"),
            Content.Load<Texture2D>("Sprites\\area\\bigRoom3"),
            Content.Load<Texture2D>("Sprites\\area\\bigRoom4"), 
            Content.Load<Texture2D>("Sprites\\area\\bossRoom")
            };
            animationSprites.Add("areaStart", areaArray);

            #endregion Areas
            #region Doors

            Texture2D[] doorArray = new Texture2D[4] //rooms
            {
            Content.Load<Texture2D>("Sprites\\area\\doorClosed_shadow"), //Closed door
            Content.Load<Texture2D>("Sprites\\area\\doorOpen_shadow"),
            Content.Load<Texture2D>("Sprites\\area\\doorLocked"), //locked door
            Content.Load<Texture2D>("Sprites\\area\\sercretBricks"), //secret door
            };
            animationSprites.Add("doorStart", doorArray);

            #endregion Doors
            #region environment

            Texture2D[] environment = new Texture2D[1] //rooms
            {
            Content.Load<Texture2D>("Sprites\\area\\carpet"), //Carpet
            };
            animationSprites.Add("environment", environment);

            #endregion environment
            #region Morten

            Texture2D[] bishop = new Texture2D[5];
            for (int i = 0; i < 5; i++)
            {
                bishop[i] = Content.Load<Texture2D>("Sprites\\Charactor\\mortenBishop" + i);
            }
            animationSprites.Add("bishop", bishop);

            Texture2D[] monk = new Texture2D[4];
            for (int i = 0; i < 4; i++)
            {
                monk[i] = Content.Load<Texture2D>("Sprites\\Charactor\\mortenMonk" + i);
            }
            animationSprites.Add("monk", monk);

            Texture2D[] crusader = new Texture2D[5];
            for (int i = 0; i < 5; i++)
            {
                crusader[i] = Content.Load<Texture2D>("Sprites\\Charactor\\mortenCrusader" + i);
            }
            animationSprites.Add("crusader", crusader);

            #endregion Morten
            #region Goose

            Texture2D[] gooseSprites = new Texture2D[8];
            for (int i = 0; i < 8; i++)
            {
                gooseSprites[i] = Content.Load<Texture2D>("Sprites\\Charactor\\gooseWalk" + i);
            }
            animationSprites.Add("WalkingGoose", gooseSprites);

            #region Aggro Goose
            Texture2D[] aggroGooseSprites = new Texture2D[8];
            for (int i = 0; i < 8; i++)
            {
                aggroGooseSprites[i] = Content.Load<Texture2D>("Sprites\\Charactor\\aggro" + i);
            }
            animationSprites.Add("AggroGoose", aggroGooseSprites);
            #endregion Aggro Goose
            #region Goosifer

            Texture2D[] goosifer = new Texture2D[3];
            for (int i = 0; i < 3; i++)
            {
                goosifer[i] = Content.Load<Texture2D>("Sprites\\Charactor\\goosifer" + i);
            }
            animationSprites.Add("goosifer", goosifer);

            #endregion Goosifer
            #endregion Goose

            #region Obstacles

            Texture2D[] firepit = new Texture2D[4];
            firepit[0] = Content.Load<Texture2D>("Sprites\\Obstacle\\firepit");
            firepit[1] = Content.Load<Texture2D>("Sprites\\Obstacle\\firepit0");
            firepit[2] = Content.Load<Texture2D>("Sprites\\Obstacle\\firepit0");
            firepit[3] = Content.Load<Texture2D>("Sprites\\Obstacle\\firepit");

            animationSprites.Add("firepit", firepit);



            #endregion Obstacles

        }

        /// <summary>
        /// Loads "SoundEffects" into the "commonSounds" Dictionary
        /// </summary>
        private void LoadCommonSounds()
        {

            #region Player

            SoundEffect playerAv = Content.Load<SoundEffect>("Sounds\\SoundEffects\\Player\\morten_Av");
            SoundEffect playerWalk1 = Content.Load<SoundEffect>("Sounds\\SoundEffects\\Player\\walkSound");
            SoundEffect playerWalk2 = Content.Load<SoundEffect>("Sounds\\SoundEffects\\Player\\walkSound2");

            commonSounds.Add("playerAv", playerAv);
            commonSounds.Add("playerWalk1", playerWalk1);
            commonSounds.Add("playerWalk2", playerWalk2);

            #endregion Player
            #region Menu

            SoundEffect equipItem = Content.Load<SoundEffect>("Sounds\\SoundEffects\\Menu\\powerUp_Sound");

            commonSounds.Add("equipItem", equipItem);

            #endregion Menu
            #region Enemy

            SoundEffect aggroGoose = Content.Load<SoundEffect>("Sounds\\SoundEffects\\Enemy\\gooseSound_Short");

            commonSounds.Add("aggroGoose", aggroGoose);

            #endregion Enemy
        


        }

        /// <summary>
        /// Loads "songs" into the "backgroundMusic" Dictionary
        /// </summary>
        private void LoadBackgroundSongs()
        {
            #region Organ music
            Song bgMusic = Content.Load<Song>("Sounds\\Music\\bgMusic");
            backgroundMusic.Add("bgMusic", bgMusic);

            Song battleMusic = Content.Load<Song>("Sounds\\Music\\battleMusic");
            backgroundMusic.Add("battleMusic", battleMusic);

         
            PlayMusic(1);

            #endregion Organ Music



        }
        #endregion
        #region Functionality
        /// <summary>
        /// Marks sub-menu items as obsolete, which are then deleted after having been read
        /// </summary>
        public static void MarkMenuItemsObsolete()
        {

            menu.FindAll(button => button is Button).ConvertAll(button => (Button)button).FindAll(button => button.ItemButton == true).ForEach(Button => Button.ButtonObsolete = true);

        }

        /// <summary>
        /// Detects if the there are any buttons active, in this case a right-click menu which consists of only buttons
        /// </summary>
        /// <returns>true if any "ItemButtons", otherwise false</returns>
        public static bool DetectRightClickMenu()
        {

            bool exists = false;
            foreach (Menu menuItem in menu)
                if (menuItem is Button)
                    if ((menuItem as Button).ItemButton)
                        exists = true;
            return exists;

        }

        /// <summary>
        /// Detects if inventory menu specificly is in existence
        /// </summary>
        /// <returns>true if inventory exists otherwise false</returns>
        public static bool DetectInventory()
        {

            bool inventoryOpen = false;
            foreach (Menu menuItem in menu)
                if (menuItem.IsMenu)
                    if (menuItem.IsInventory)
                        inventoryOpen = true;
            return inventoryOpen;

        }

        /// <summary>
        /// Detects if Intro or Outro menu specificly is in existence
        /// </summary>
        /// <returns>true if inventory exists otherwise false</returns>
        public static bool DetectInOutro()
        {

            bool inOutroOpen = false;
            foreach (Menu menuItem in menu)
                if (menuItem.IsMenu)
                    if (menuItem.IsInOutro)
                        inOutroOpen = true;
            return inOutroOpen;

        }

        /// <summary>
        /// Closes the game on next pass of "Update"
        /// </summary>
        public static void ExitGame()
        {

            exitGame = true;

        }

        /// <summary>
        /// Clears all menus and runs GameWorlds "Initialize" to simulate a fresh start
        /// </summary>
        private void Restart()
        {

            menu.Clear();
            gameObjects.Clear();
            newGameObjects.Clear();
            playerInventory.Clear();
            equippedPlayerInventory.Clear();
            commonSprites.Clear();
            animationSprites.Clear();
            commonSounds.Clear();
            backgroundMusic.Clear();
            hiddenItems.Clear();
            playerInstance = null;
            Initialize();
            restart = false;

        }

        /// <summary>
        /// Sets a bool that initializes the restart process
        /// </summary>
        public static void InitiateRestart()
        {

            restart = true;

        }

        /// <summary>
        /// Gives start parameters (currently none)
        /// </summary>
        public static void StartGame()
        {

            newGameObjects.Add(new Menu(Camera.Position, 5));

        }

        /// <summary>
        /// Function to make a list for Player of NPCs to interact with
        /// </summary>
        /// <returns>List with NPC references</returns>
        private List<NPC> FindNPCLocation()
        {

            return gameObjects.FindAll(npc => npc is NPC).ConvertAll(npc => (NPC)npc);

        }

        /// <summary>
        /// Used to locate a healing item
        /// </summary>
        /// <returns>Healing Item</returns>
        public static Item FindHealingItem()
        {

            return playerInventory.FindAll(questItem => questItem is QuestItem).ConvertAll(questItem => (QuestItem)questItem).Find(questItem => questItem.HealItem == true);

        }

        /// <summary>
        /// Updates the name of which room the player is in by calculating which room is closest
        /// </summary>
        private void PlayerInRoom()
        {

            float closestRoom = 2000;
            foreach (Area area in area51)
            {
                float distanceToPlayer = Vector2.Distance(playerInstance.Position, area.Position);
                if (distanceToPlayer < closestRoom)
                {
                    playerInstance.InRoom = area.Room;
                    closestRoom = distanceToPlayer;
                }
            }

        }
        /// <summary>
        /// Method for switching between music
        /// </summary>
        /// <param name="typeMusic"></param>
        public static void PlayMusic(int typeMusic)
        {
            
            switch (typeMusic)
            { 
                case 1: //backround music
                    MediaPlayer.Play(backgroundMusic["bgMusic"]);
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Volume = 0.15f;
                    break;
                case 2: //battle music
                    MediaPlayer.Play(backgroundMusic["battleMusic"]);
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Volume = 0.15f;
                    break;
            }

       
        }

        #endregion Functionality
        #endregion Methods
    }
}
