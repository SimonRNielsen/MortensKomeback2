using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace MortensKomeback2
{
    public class GameWorld : Game
    {
        #region Fields

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static Camera2D camera; //Set to public so it can change position when the player is teleporting
        private static Vector2 mousePosition;
        private bool escape;
        private static bool leftMouseButtonClick;
        private static bool rightMouseButtonClick;
        private static bool closeMenu = false;
        private static bool menuActive;
        private static bool exitGame = false;
        private static bool restart = false;
        private static bool dialogue = false;
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

        #endregion

        #region Properties

        public static Camera2D Camera { get => camera; set => camera = value; }
        public static Vector2 MousePosition { get => mousePosition; }
        public static bool LeftMouseButtonClick { get => leftMouseButtonClick; }
        public static bool RightMouseButtonClick { get => rightMouseButtonClick; }
        public static bool CloseMenu { get => closeMenu; set => closeMenu = value; }
        public static bool MenuActive { get => menuActive; }
        public static bool Dialogue { set => dialogue = value; }
        internal static Player PlayerInstance { get => playerInstance; private set => playerInstance = value; }
        public static Color GrayGoose { get => grayGoose; }
        public static Rectangle CurrentRoomBoundary { get; internal set; }

        SpriteFont font1;

        // The position to draw the text
        Vector2 fontPos;

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
            hiddenItems.Add(new QuestItem(0, false, Vector2.Zero));
            hiddenItems.Add(new QuestItem(1, false, Vector2.Zero));
            hiddenItems.Add(new QuestItem(1, false, Vector2.Zero));
            //newGameObjects.Add(new Dialogue(new Vector2(Camera.Position.X, Camera.Position.Y + 320), new NPC(2)));

            //menu.Add(new Menu(Camera.Position, 3));

            PlayerInstance = new Player(PlayerClass.Monk, FindNPCLocation()); //Using it as a reference to get the players position
            newGameObjects.Add(PlayerInstance);
            newGameObjects.Add(new Enemy(_graphics));


            #region area
            newGameObjects.Add(new Area(new Vector2(0, 0), 1, "Room1"));       //main room
            newGameObjects.Add(new Area(new Vector2(0, 1080), 2, "Room1a"));    //main room
            newGameObjects.Add(new Area(new Vector2(0, 2160), 3, "Room1b"));    //main room
            newGameObjects.Add(new Area(new Vector2(0, 1080 * 3), 4, "Room1c"));  //main room
            newGameObjects.Add(new Area(new Vector2(0, 1080 * 5), 0, "Room8"));  // våbenhus - enemies
            newGameObjects.Add(new Area(new Vector2(0, 1080 * 7), 0, "Room9"));  // puzzle
            newGameObjects.Add(new Area(new Vector2(0, 1080 * 9), 0, "Room10"));  // boss fight

            newGameObjects.Add(new Area(new Vector2(-3000, 0), 0, "Room2"));          //ventre side, rum 2, nonne
            newGameObjects.Add(new Area(new Vector2(-6000, 0), 0, "Room3"));     //ventre side, rum 3 enemies
            newGameObjects.Add(new Area(new Vector2(-6000, 1080 * 2), 0, "Room4"));     //ventre side, rum 4 enemies
            newGameObjects.Add(new Area(new Vector2(-9000, 0 * 4), 0, "Room5"));   //ventre side, rum 5, item


            newGameObjects.Add(new Area(new Vector2(3000, 0), 0, "Room6"));           //højre side, rum 1, munk
            newGameObjects.Add(new Area(new Vector2(3000, -2160), 0, "Room7"));      //højre side, rum 2, secret + item
            #endregion

            //#region GUI

            Texture2D barBG = GameWorld.commonSprites["healthBarBlack"];
            newGameObjects.Add(new HealthBar( barBG, 0.5f));
            Texture2D bar = GameWorld.commonSprites["healthBarRed"];
            newGameObjects.Add(new HealthBar( bar, 0.55f));

            //newGameObjects.Add(new GUI(new Vector2(-855, -400)));       //GUI, pauset ud pt
            //newGameObjects.Add(new Dialogue(new Vector2(0, 320)));      //Dialogue box visual
            #endregion

            //newGameObjects.Add(new Dialogue(new Vector2(0, 320)));      //Dialogue box visual

            #region obstacle
            newGameObjects.Add(new AvSurface(200, 0)); //Sæt til igen
            newGameObjects.Add(new Obstacle(500, 0));
            newGameObjects.Add(new Obstacle(-400, 00));

            #region doors
            newGameObjects.Add(new Door(0, -443 + 15, DoorTypes.Closed, new Vector2(-3000, 0))); // Teleports to left room 1
            newGameObjects.Add(new Door(-3000, -443 + 15, DoorTypes.Open, Vector2.Zero)); // Teleports to main room from left room 1
            newGameObjects.Add(new Door(2200, 0, DoorTypes.Open, Vector2.Zero)); // Teleports to main room from right room 1
            newGameObjects.Add(new Door(800, 0, DoorTypes.Open, new Vector2(3000, 0))); // Teleports to main room from right room 1

            #endregion

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

            if (restart)
                Restart();

            PlayerInRoom();

            var mouseState = Mouse.GetState();

            mousePosition = new Vector2((int)(mouseState.X / Camera.Zoom) - (int)((float)_graphics.PreferredBackBufferWidth / 2 / Camera.Zoom) + (int)Camera.Position.X, (int)(mouseState.Y / Camera.Zoom) - (int)((float)_graphics.PreferredBackBufferHeight / 2 / Camera.Zoom) + 20 + (int)Camera.Position.Y);
            leftMouseButtonClick = mouseState.LeftButton == ButtonState.Pressed;
            rightMouseButtonClick = mouseState.RightButton == ButtonState.Pressed;

            //Updates gameObjects and collision
            foreach (GameObject gameObject in gameObjects)
            {
                //Pause-logic
                if (!menuActive)
                    gameObject.Update(gameTime);
                //Måske skrotte nedenstående?
                else if (menuActive && gameObject is Player)
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

            gameObjects.RemoveAll(obj => obj.IsAlive == false);

            //Search & Pray logic
            foreach (Item item in hiddenItems)
                item.Update(gameTime);
            hiddenItems.RemoveAll(found => found.IsPickedUp == true);

            //Menu logic
            foreach (Menu menuItem in menu)
            {
                menuActive = true;
                menuItem.Update(gameTime);
                if (menuItem is Button)
                {
                    (menuItem as Button).CheckCollision(mousePointer);
                }
            }
            if (rightMouseButtonClick)
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

            if (area51.Count == 0)
                foreach (GameObject gameObject in gameObjects)
                    if (gameObject is Area)
                        area51.Add(gameObject as Area);
            

            newGameObjects.Clear();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);



            // TODO: Add your drawing code here
            _spriteBatch.Begin(transformMatrix: Camera.GetTransformation(), samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            foreach (GameObject gameObject in gameObjects)
            {

                gameObject.Draw(_spriteBatch);
#if DEBUG
                DrawCollisionBox(gameObject);
                if (gameObject is Area)
                {
                    DrawLeftCollisionBox(gameObject);
                    DrawRightCollisionBox(gameObject);
                    DrawBottomCollisionBox(gameObject);
                    DrawTopCollisionBox(gameObject);
                }
#endif

            }

            if (DetectInventory())
            {
                foreach (Item item in playerInventory)
                {
                    item.Draw(_spriteBatch);
                    DrawCollisionBox(item);
                    item.Update(gameTime);
                }

                foreach (Item item in equippedPlayerInventory)
                {
                    item.Draw(_spriteBatch);
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
#endif

        /// <summary>
        /// Loads sprites into the "commonSprites" Dictionary
        /// </summary>
        private void LoadCommonSprites()
        {

#if DEBUG
            Texture2D collisionTexture = Content.Load<Texture2D>("Sprites\\DEBUG\\pixel");
            commonSprites.Add("collisionTexture", collisionTexture);
#endif

            Texture2D quest = Content.Load<Texture2D>("Sprites\\Item\\questItemPlaceholder");
            Texture2D mainHand = Content.Load<Texture2D>("Sprites\\Item\\mainHandPlaceholder");
            Texture2D offHand = Content.Load<Texture2D>("Sprites\\Item\\offHandPlaceholder");
            Texture2D torso = Content.Load<Texture2D>("Sprites\\Item\\torsoPlaceholder");
            Texture2D feet = Content.Load<Texture2D>("Sprites\\Item\\feetPlaceholder");
            Texture2D healItem = Content.Load<Texture2D>("Sprites\\Item\\torsoPlaceholder");
            Texture2D blink = Content.Load<Texture2D>("Sprites\\Item\\blinkPlaceholder");

            Texture2D stone = Content.Load<Texture2D>("Sprites\\Obstacle\\stone"); //Stone
            Texture2D doorClosed = Content.Load<Texture2D>("Sprites\\Area\\doorClosed_shadow"); //door closed


            commonSprites.Add("questItem", quest);
            commonSprites.Add("mainHandItem", mainHand);
            commonSprites.Add("offHandItem", offHand);
            commonSprites.Add("torsoItem", torso);
            commonSprites.Add("feetItem", feet);
            commonSprites.Add("healItem", healItem);
            commonSprites.Add("blink", blink);
            //commonSprites.Add("feetItem", feet); Sat dobbelt ind, kan slettes 
            commonSprites.Add("stone", stone);

            //GUI
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

            Texture2D menuButton = Content.Load<Texture2D>("Sprites\\Menu\\menuButton");
            Texture2D button = Content.Load<Texture2D>("Sprites\\Menu\\button");
            Texture2D introScreen = Content.Load<Texture2D>("Sprites\\Menu\\introScreen");
            Texture2D winScreen = Content.Load<Texture2D>("Sprites\\Menu\\winScreen");
            Texture2D loseScreen = Content.Load<Texture2D>("Sprites\\Menu\\loseScreen");
            Texture2D inventoryScreen = Content.Load<Texture2D>("Sprites\\Menu\\inventory");
            Texture2D statPanel = Content.Load<Texture2D>("Sprites\\Menu\\statPanel");
            Texture2D pauseScreen = Content.Load<Texture2D>("Sprites\\Menu\\pauseScreen");

            commonSprites.Add("menuButton", menuButton);
            commonSprites.Add("button", button);
            commonSprites.Add("introScreen", introScreen);
            commonSprites.Add("winScreen", winScreen);
            commonSprites.Add("loseScreen", loseScreen);
            commonSprites.Add("inventory", inventoryScreen);
            commonSprites.Add("statPanel", statPanel);
            commonSprites.Add("pauseScreen", pauseScreen);
        }

        /// <summary>
        /// Loads animation arrays into the "animationSprites" Dictionary
        /// </summary>
        private void LoadAnimationArrays()
        {

            //#endregion

            Texture2D[] areaArray = new Texture2D[5] //rooms
            {
            Content.Load<Texture2D>("Sprites\\area\\room_single"),
            Content.Load<Texture2D>("Sprites\\area\\bigRoom1"),
            Content.Load<Texture2D>("Sprites\\area\\bigRoom2"),
            Content.Load<Texture2D>("Sprites\\area\\bigRoom3"),
            Content.Load<Texture2D>("Sprites\\area\\bigRoom4")
            };
            animationSprites.Add("areaStart", areaArray);

            Texture2D[] doorArray = new Texture2D[3] //rooms
         {
            Content.Load<Texture2D>("Sprites\\area\\doorClosed_shadow"), //Closed door
            Content.Load<Texture2D>("Sprites\\area\\doorOpen_shadow"), //Open door
            Content.Load<Texture2D>("Sprites\\area\\doorLocked") //locked door
         };
            animationSprites.Add("doorStart", doorArray);

            #region Morten

            Texture2D[] bishop = new Texture2D[4];
            for (int i = 0; i < 4; i++)
            {
                bishop[i] = Content.Load<Texture2D>("Sprites\\Charactor\\mortenBishop" + i);
            }
            animationSprites.Add("bishop", bishop);

            Texture2D[] monkAnimArray = new Texture2D[4];
            for (int i = 0; i < 4; i++)
            {
                monkAnimArray[i] = Content.Load<Texture2D>("Sprites\\Charactor\\mortenMonk" + i);
            }
            animationSprites.Add("monk", monk);

            Texture2D[] crusaderAnimArray = new Texture2D[4];
            for (int i = 0; i < 4; i++)
            {
                crusaderAnimArray[i] = Content.Load<Texture2D>("Sprites\\Charactor\\mortenCrusader" + i);
            }
            animationSprites.Add("crusader", crusader);

            #endregion

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



        }

        /// <summary>
        /// Loads "songs" into the "backgroundMusic" Dictionary
        /// </summary>
        private void LoadBackgroundSongs()
        {



        }

        /// <summary>
        /// Marks sub-menu items as obsolete, which are then deleted after having been read
        /// </summary>
        public static void MarkMenuItemsObsolete()
        {
            foreach (Menu menuItem in menu)
            {
                if (menuItem is Button)
                    if ((menuItem as Button).ItemButton)
                        (menuItem as Button).ButtonObsolete = true;
            }
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
            //Start game logic here
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list">list to be parsed for NPCs</param>
        /// <returns>List with NPC references</returns>
        private List<NPC> FindNPCLocation()
        {
            List<NPC> nPCs = gameObjects.FindAll(npc => npc is NPC).ConvertAll(npc => (NPC)npc);
            return nPCs;
        }

        /// <summary>
        /// Used to locate a healing item
        /// </summary>
        /// <returns>Healing Item</returns>
        public static Item FindHealingItem()
        {
            return playerInventory.FindAll(questItem => questItem is QuestItem).ConvertAll(questItem => (QuestItem)questItem).Find(questItem => questItem.HealItem == true);
        }


        public void UpdateCamera()
        {
            if (playerInstance != null)
            {
                // Center the camera on the player
                camera.Position = playerInstance.Position - new Vector2(1920 / 2, 1080 / 2);
            }
        }

        /// <summary>
        /// Updates the name of which room the player is in by calculating which room is closest
        /// </summary>
        private void PlayerInRoom()
        {
            float distance = 2000;
            foreach (Area area in area51)
            {
                float distanceToPlayer = Vector2.Distance(playerInstance.Position, area.Position);
                if (distanceToPlayer < distance)
                {
                    playerInstance.InRoom = area.Room;
                    distance = distanceToPlayer;
                }
            }
        }

        #region
        #endregion
    }
}
