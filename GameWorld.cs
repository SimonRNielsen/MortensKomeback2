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
        public static List<NPC> nPCs;
        private static Random random = new Random();

        #endregion

        #region Properties

        public static Camera2D Camera { get => camera; set => camera = value; }
        public static Vector2 MousePosition { get => mousePosition; }
        public static bool LeftMouseButtonClick { get => leftMouseButtonClick; }
        public static bool RightMouseButtonClick { get => rightMouseButtonClick; }
        public static bool CloseMenu { get => closeMenu; set => closeMenu = value; }
        public static bool MenuActive { get => menuActive; }
        public static bool Dialogue { set => dialogue = value; }
        internal static Player PlayerInstance { get => playerInstance; set => playerInstance = value; }
        public static bool BattleActive { get => battleActive; set => battleActive = value; }
        public static Color GrayGoose { get => grayGoose; }
        public static Random Random { get => random; }

        #endregion

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

            //_graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            Camera = new Camera2D(GraphicsDevice, Vector2.Zero);

            //Preloading of all assets
            mortensKomebackFont = Content.Load<SpriteFont>("mortalKombatFont");
            LoadCommonSprites();
            LoadAnimationArrays();
            LoadCommonSounds();
            LoadBackgroundSongs();

            hiddenItems.Add(new MainHandItem(PlayerClass.Monk, Vector2.Zero, false, false));
            playerInventory.Add(new TorsoSlotItem(PlayerClass.Monk, true, Vector2.Zero));
            playerInventory.Add(new QuestItem(1, false, Vector2.Zero));
            playerInventory.Add(new QuestItem(0, true, Vector2.Zero));
            hiddenItems.Add(new QuestItem(1, false, Vector2.Zero));
            hiddenItems.Add(new QuestItem(1, false, Vector2.Zero));

            menu.Add(new Menu(Camera.Position, 3));

            PlayerInstance = new Player(PlayerClass.Crusader, FindNPCLocation()); //Using it as a reference to get the players position
            newGameObjects.Add(PlayerInstance);
            newGameObjects.Add(new Enemy(_graphics));

            newGameObjects.Add(new HealthBar(0.5f, 1));

            //newGameObjects.Add(new Dialogue(new Vector2(0, 320)));      //Dialogue box visual

            #region obstacle
            newGameObjects.Add(new AvSurface(200, 0)); //Sæt til igen
            newGameObjects.Add(new Obstacle(500, 0));
            newGameObjects.Add(new Obstacle(-400, 00));
            #endregion

            #region area
            newGameObjects.Add(new Area(new Vector2(0, 0), 1, "Room1"));       //main room
            newGameObjects.Add(new Area(new Vector2(0, 1080), 2, "Room1a"));    //main room
            newGameObjects.Add(new Area(new Vector2(0, 1080 * 2), 3, "Room1b"));    //main room
            newGameObjects.Add(new Area(new Vector2(0, 1080 * 3), 4, "Room1c"));  //main room
            newGameObjects.Add(new Area(new Vector2(0, 1080 * 5), 0, "Room8"));  // våbenhus - enemies
            newGameObjects.Add(new Area(new Vector2(0, 1080 * 7), 0, "Room9"));  // puzzle
            newGameObjects.Add(new Area(new Vector2(0, 1080 * 9), 0, "Room10"));  // boss fight

            newGameObjects.Add(new Area(new Vector2(0, -1080 * 2), 0, "Room2"));          //ventre side, rum 2, nonne
            newGameObjects.Add(new Area(new Vector2(0, -1080 * 4), 0, "Room3"));     //ventre side, rum 3 enemies
            newGameObjects.Add(new Area(new Vector2(0, -1080 * 6), 0, "Room4"));     //ventre side, rum 4 enemies
            newGameObjects.Add(new Area(new Vector2(0, -1080 * 8), 0, "Room5"));   //ventre side, rum 5, item


            newGameObjects.Add(new Area(new Vector2(0, -1080 * 10), 0, "Room6"));           //højre side, rum 1, munk
            newGameObjects.Add(new Area(new Vector2(0, -1080 * 12), 0, "Room7"));      //højre side, rum 2, secret + item
            #endregion

            #region doors
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

            newGameObjects.Add(new Door(0, bottomSide + 1080 * 5, DoorTypes.Locked, DoorRotation.Bottom, new Vector2(0, 1080 * 7 + topSide + 120))); //Skal være locked 8.9
            newGameObjects.Add(new Door(0, topSide + 1080 * 7, DoorTypes.Open, DoorRotation.Top, new Vector2(0, 1080 * 5 + bottomSide - 120))); //9.8

            newGameObjects.Add(new Door(0, bottomSide + 1080 * 7, DoorTypes.Open, DoorRotation.Bottom, new Vector2(0, 1080 * 9 + topSide + 120))); //9.10
            newGameObjects.Add(new Door(0, topSide + 1080 * 9, DoorTypes.Open, DoorRotation.Top, new Vector2(0, 1080 * 7 + bottomSide - 120))); //10.9
            //#region GUI
            newGameObjects.Add(new HealthBar(0.55f, 1));

            #endregion

            #region obstacle

            newGameObjects.Add(new AvSurface(200, 0)); //Sæt til igen
            newGameObjects.Add(new Obstacle(500, 0));
            newGameObjects.Add(new Obstacle(-400, 00));
            newGameObjects.Add(new Obstacle(-400, 00));

            #endregion




            base.Initialize();

        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {

            //Exit logic
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

            // TODO: Add your update logic here

            //Restart logic
            if (restart)
                Restart();

            //Location logic
            PlayerInRoom();

            //Mouse input
            var mouseState = Mouse.GetState();
            mousePosition = new Vector2((int)(mouseState.X / Camera.Zoom) - (int)((float)_graphics.PreferredBackBufferWidth / 2 / Camera.Zoom) + (int)Camera.Position.X, (int)(mouseState.Y / Camera.Zoom) - (int)((float)_graphics.PreferredBackBufferHeight / 2 / Camera.Zoom) + 20 + (int)Camera.Position.Y);
            leftMouseButtonClick = mouseState.LeftButton == ButtonState.Pressed;
            rightMouseButtonClick = mouseState.RightButton == ButtonState.Pressed;

            //Loss logic
            mortenLives = false;
            if (playerInstance.IsAlive || DetectInOutro())
                mortenLives = true;
            if (!mortenLives)
                newGameObjects.Add(new Menu(Camera.Position, 2));

            //Updates gameObjects and collision
            foreach (GameObject gameObject in gameObjects)
            {
                //Pause-logic
                if (!menuActive && !battleActive)
                    gameObject.Update(gameTime);
                //Måske skrotte nedenstående?
                /*else if (menuActive && gameObject is Player)
                    gameObject.Update(gameTime); */
                else if (battleActive && gameObject is BattleField && !menuActive)
                    gameObject.Update(gameTime);


                foreach (GameObject other in gameObjects)
                {
                    if (gameObject is Player)
                    {
                        if (other is AvSurface || other is Obstacle || other is Door)
                        {
                            gameObject.CheckCollision(other);
                            other.CheckCollision(gameObject);
                        }
                    }

                    if (gameObject is Enemy)
                    {
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

            //Removes objects
            gameObjects.RemoveAll(obj => obj.IsAlive == false);
            //Search & Pray logic
            foreach (Item item in hiddenItems)
                item.Update(gameTime);
            hiddenItems.RemoveAll(found => found.IsPickedUp == true);

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


            //Test collision for batllefield
            foreach (GameObject p in gameObjects)
            {
                if (!(p is Player))
                { continue; }
                else
                    foreach (GameObject e in gameObjects)
                    {
                        if (!(e is Enemy))
                        { continue; }
                        else
                            p.CheckCollision(e);
                    }
            }


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

            //Creates a "map" for locating players position
            if (area51.Count == 0)
                foreach (GameObject gameObject in gameObjects)
                    if (gameObject is Area)
                        area51.Add(gameObject as Area);


            newGameObjects.Clear();

            if (nPCs == null)
                nPCs = FindNPCLocation();

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
#endif

            }

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
        #endregion
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

            commonSprites.Add("stone", stone);

            #endregion
            #region Items

            Texture2D quest = Content.Load<Texture2D>("Sprites\\Item\\questItemPlaceholder");
            Texture2D mainHand = Content.Load<Texture2D>("Sprites\\Item\\mainHandPlaceholder");
            Texture2D offHand = Content.Load<Texture2D>("Sprites\\Item\\offHandPlaceholder");
            Texture2D torso = Content.Load<Texture2D>("Sprites\\Item\\torsoPlaceholder");
            Texture2D feet = Content.Load<Texture2D>("Sprites\\Item\\feetPlaceholder");
            Texture2D healItem = Content.Load<Texture2D>("Sprites\\Item\\torsoPlaceholder");
            Texture2D blink = Content.Load<Texture2D>("Sprites\\Item\\blinkPlaceholder");
            Texture2D mitre = Content.Load<Texture2D>("Sprites\\Item\\mitre");

            commonSprites.Add("questItem", quest);
            commonSprites.Add("mainHandItem", mainHand);
            commonSprites.Add("offHandItem", offHand);
            commonSprites.Add("torsoItem", torso);
            commonSprites.Add("feetItem", feet);
            commonSprites.Add("healItem", healItem);
            commonSprites.Add("blink", blink);
            commonSprites.Add("mitre", mitre);

            #endregion
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

            #endregion
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

            #endregion
        }

        /// <summary>
        /// Loads animation arrays into the "animationSprites" Dictionary
        /// </summary>
        private void LoadAnimationArrays()
        {

            #region Areas

            Texture2D[] areaArray = new Texture2D[5]
            {
            Content.Load<Texture2D>("Sprites\\area\\room_single"),
            Content.Load<Texture2D>("Sprites\\area\\bigRoom1"),
            Content.Load<Texture2D>("Sprites\\area\\bigRoom2"),
            Content.Load<Texture2D>("Sprites\\area\\bigRoom3"),
            Content.Load<Texture2D>("Sprites\\area\\bigRoom4")
            };
            animationSprites.Add("areaStart", areaArray);
            #endregion
            #region Doors

            Texture2D[] doorArray = new Texture2D[4] //rooms
            {
            Content.Load<Texture2D>("Sprites\\area\\doorClosed_shadow"), //Closed door
            Content.Load<Texture2D>("Sprites\\area\\doorOpen_shadow"),
            Content.Load<Texture2D>("Sprites\\area\\doorLocked"), //locked door
            Content.Load<Texture2D>("Sprites\\area\\sercretBricks"), //secret door
            };
            animationSprites.Add("doorStart", doorArray);

            #endregion
            #region Morten

            Texture2D[] bishop = new Texture2D[4];
            for (int i = 0; i < 4; i++)
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

            Texture2D[] crusader = new Texture2D[4];
            for (int i = 0; i < 4; i++)
            {
                crusader[i] = Content.Load<Texture2D>("Sprites\\Charactor\\mortenCrusader" + i);
            }
            animationSprites.Add("crusader", crusader);

            #endregion


            #region goose

            Texture2D[] gooseSprites = new Texture2D[8];
            for (int i = 0; i < 8; i++)
            {
                gooseSprites[i] = Content.Load<Texture2D>("Sprites\\Charactor\\gooseWalk" + i);
            }
            animationSprites.Add("WalkingGoose", gooseSprites);

            #region aggro goose
            Texture2D[] aggroGooseSprites = new Texture2D[8];
            for (int i = 0; i < 8; i++)
            {
                aggroGooseSprites[i] = Content.Load<Texture2D>("Sprites\\Charactor\\aggro" + i);
            }
            animationSprites.Add("AggroGoose", aggroGooseSprites);
            #endregion
            #endregion
            #region obstalce

            Texture2D[] firepit = new Texture2D[4];
            firepit[0] = Content.Load<Texture2D>("Sprites\\Obstacle\\firepit");
            firepit[1] = Content.Load<Texture2D>("Sprites\\Obstacle\\firepit0");
            firepit[2] = Content.Load<Texture2D>("Sprites\\Obstacle\\firepit0");
            firepit[3] = Content.Load<Texture2D>("Sprites\\Obstacle\\firepit");
            animationSprites.Add("firepit", firepit);

            #endregion

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

            #endregion
            #region Menu

            SoundEffect equipItem = Content.Load<SoundEffect>("Sounds\\SoundEffects\\Menu\\powerUp_Sound");

            commonSounds.Add("equipItem", equipItem);

            #endregion
            #region Enemy

            SoundEffect aggroGoose = Content.Load<SoundEffect>("Sounds\\SoundEffects\\Enemy\\gooseSound_Short");

            commonSounds.Add("aggroGoose", aggroGoose);

            #endregion

        }

        /// <summary>
        /// Loads "songs" into the "backgroundMusic" Dictionary
        /// </summary>
        private void LoadBackgroundSongs()
        {



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

        /* 0 references so outcommented, also seems obsolete?
        public void UpdateCamera()
        {
            if (playerInstance != null)
            {
                // Center the camera on the player
                camera.Position = playerInstance.Position - new Vector2(1920 / 2, 1080 / 2);
            }
        }
        */

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
        #endregion
        #endregion
    }
}
