using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MortensKomeback2
{
    public class GameWorld : Game
    {
        #region Fields

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private static Camera2D camera;
        private static Vector2 mousePosition;
        private static bool leftMouseButtonClick;
        private static bool rightMouseButtonClick;
        private static bool closeMenu = false;
        private static bool menuActive;
        private static bool exitGame = false;
        private static bool restart = false;
        private static MousePointer mousePointer;
        private static List<Menu> menu = new List<Menu>();
        private List<GameObject> gameObjects = new List<GameObject>();
        public static List<GameObject> newGameObjects = new List<GameObject>();
        public static List<Item> playerInventory = new List<Item>();
        public static List<Item> equippedPlayerInventory = new List<Item>();
        public static Dictionary<string, Texture2D> commonSprites = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D[]> animationSprites = new Dictionary<string, Texture2D[]>();
        public static Dictionary<string, SoundEffect> commonSounds = new Dictionary<string, SoundEffect>();
        public static Dictionary<string, Song> backgroundMusic = new Dictionary<string, Song>();
        public static SpriteFont mortensKomebackFont;

        #endregion

        #region Properties

        public static Camera2D Camera { get => camera; set => camera = value; }
        public static Vector2 MousePosition { get => mousePosition; }
        public static bool LeftMouseButtonClick { get => leftMouseButtonClick; }
        public static bool RightMouseButtonClick { get => rightMouseButtonClick; }
        public static bool CloseMenu { get => closeMenu; set => closeMenu = value; }
        public static bool MenuActive { get => menuActive; }

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
            mousePointer = new MousePointer();

            //Preloading of all assets
            mortensKomebackFont = Content.Load<SpriteFont>("mortalKombatFont");
            LoadCommonSprites();
            LoadAnimationArrays();
            LoadCommonSounds();
            LoadBackgroundSongs();

            newGameObjects.Add(new MainHandItem(2, Vector2.Zero));
            //newGameObjects.Add(new Button(Vector2.Zero, 0));

            newGameObjects.Add(new Player());
            newGameObjects.Add(new Enemy());

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || exitGame)
                Exit();

            // TODO: Add your update logic here

            if (restart)
                Restart();

            var mouseState = Mouse.GetState();

            mousePosition = new Vector2((int)(mouseState.X / Camera.Zoom) - (int)((float)_graphics.PreferredBackBufferWidth / 2 / Camera.Zoom) + (int)Camera.Position.X, (int)(mouseState.Y / Camera.Zoom) - (int)((float)_graphics.PreferredBackBufferHeight / 2 / Camera.Zoom) + 20 + (int)Camera.Position.Y);
            leftMouseButtonClick = mouseState.LeftButton == ButtonState.Pressed;
            rightMouseButtonClick = mouseState.RightButton == ButtonState.Pressed;

            //Updates gameObjects
            foreach (GameObject gameObject in gameObjects)
            {
                //Pause-logic
                if (!menuActive)
                    gameObject.Update(gameTime);
                //Måske skrotte nedenstående?
                else if (menuActive && gameObject is Player)
                    gameObject.Update(gameTime);

            }

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
#if DEBUG
            if (menu.Count == 0 && menuActive)
                closeMenu = true;
#endif
            if (closeMenu)
            {
                menu.Clear();
                menuActive = false;
                closeMenu = false;
            }
            menu.RemoveAll(menuItem => (menuItem as Button).ButtonObsolete == true);

            //"Spawns" new items
            foreach (GameObject newGameObject in newGameObjects)
            {
                newGameObject.LoadContent(Content);
                if (newGameObject is Item)
                    playerInventory.Add(newGameObject as Item);
                else if (newGameObject is Menu || newGameObject is Button)
                    menu.Add(newGameObject as Menu);
                else
                    gameObjects.Add(newGameObject);
            }
            newGameObjects.Clear();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(transformMatrix: Camera.GetTransformation(), samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            foreach (GameObject gameObject in gameObjects)
            {

                gameObject.Draw(_spriteBatch);
#if DEBUG
                DrawCollisionBox(gameObject);
#endif

            }


            foreach (Item item in playerInventory)
            {
                item.Draw(_spriteBatch);
                DrawCollisionBox(item);
            }

            foreach (Item item in equippedPlayerInventory)
            {
                item.Draw(_spriteBatch);
                DrawCollisionBox(item);
            }


            foreach (Menu menuItem in menu)
            {
                menuItem.Draw(_spriteBatch);
            }

#if DEBUG
            DrawMouseCollisionBox();
#endif

            _spriteBatch.End();

            base.Draw(gameTime);
        }


#if DEBUG
        private void DrawCollisionBox(GameObject gameObject)
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

        private void DrawMouseCollisionBox()
        {
            Color color = Color.Red;
            Rectangle collisionBox = mousePointer.CollisionBox;
            Rectangle topLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);
            Rectangle bottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);
            Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);
            Rectangle leftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);

            _spriteBatch.Draw(commonSprites["collisionTexture"], topLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
            _spriteBatch.Draw(commonSprites["collisionTexture"], bottomLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
            _spriteBatch.Draw(commonSprites["collisionTexture"], rightLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
            _spriteBatch.Draw(commonSprites["collisionTexture"], leftLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
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

            commonSprites.Add("questItem", quest);
            commonSprites.Add("mainHandItem", mainHand);
            commonSprites.Add("offHandItem", offHand);
            commonSprites.Add("torsoItem", torso);
            commonSprites.Add("feetItem", feet);

            Texture2D menuButton = Content.Load<Texture2D>("Sprites\\Menu\\menuButton");
            Texture2D button = Content.Load<Texture2D>("Sprites\\Menu\\button");
            Texture2D introScreen = Content.Load<Texture2D>("Sprites\\Menu\\introScreen");
            Texture2D winScreen = Content.Load<Texture2D>("Sprites\\Menu\\winScreen");
            Texture2D loseScreen = Content.Load<Texture2D>("Sprites\\Menu\\loseScreen");
            //Texture2D inventoryScreen = Content.Load<Texture2D>("Sprites\\Menu\\inventoryScreen");

            commonSprites.Add("menuButton", menuButton);
            commonSprites.Add("button", button);
            commonSprites.Add("introScreen", introScreen);
            commonSprites.Add("winScreen", winScreen);
            commonSprites.Add("loseScreen", loseScreen);
            //commonSprites.Add("inventory", inventoryScreen);

        }

        /// <summary>
        /// Loads animation arrays into the "animationSprites" Dictionary
        /// </summary>
        private void LoadAnimationArrays()
        {



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


        public static void MarkMenuItemsObsolete()
        {
            foreach (Menu menuItem in menu)
            {
                if (menuItem is Button)
                    if ((menuItem as Button).ItemButton)
                        (menuItem as Button).ButtonObsolete = true;
            }
        }


        public static bool DetectRightClickMenu()
        {
            bool exists = false;
            foreach (Menu menuItem in menu)
                if (menuItem is Button)
                    if ((menuItem as Button).ItemButton)
                        exists = true;
            return exists;
        }


        public static bool DetectInventory()
        {
            bool inventoryOpen = false;
            foreach (Menu menuItem in menu)
                if (menuItem.IsMenu)
                    if (menuItem.IsInventory)
                        inventoryOpen = true;
            return inventoryOpen;
        }


        public static void ExitGame()
        {
            exitGame = true;
        }


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
            Initialize();
            restart = false;
        }


        public static void InitiateRestart()
        {
            restart = true;
        }


        public static void StartGame()
        {
            //Start game logic here
        }

        #endregion
    }
}
