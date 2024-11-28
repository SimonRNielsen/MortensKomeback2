using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using System.Reflection.Metadata;

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
        private List<GameObject> gameObjects = new List<GameObject>();
        public static List<GameObject> newGameObjects = new List<GameObject>();
        public static List<Item> playerInventory = new List<Item>();
        public static List<Item> equippedPlayerInventory = new List<Item>();
        public static Dictionary<string, Texture2D> commonSprites = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D[]> animationSprites = new Dictionary<string, Texture2D[]>();
        public static Dictionary<string, SoundEffect> commonSounds = new Dictionary<string, SoundEffect>();
        public static Dictionary<string, Song> backgroundMusic = new Dictionary<string, Song>();
        public static Texture2D[] areaArray;


        private static Player playerInstance;

        #endregion

        #region Properties

        public static Camera2D Camera { get => camera; set => camera = value; }
        public static Vector2 MousePosition { get => mousePosition; }
        public static bool LeftMouseButtonClick { get => leftMouseButtonClick; }
        public static bool RightMouseButtonClick { get => rightMouseButtonClick; }
        internal static Player PlayerInstance { get => playerInstance; private set => playerInstance = value; }

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

            LoadCommonSprites();
            LoadAnimationArrays();
            LoadCommonSounds();
            LoadBackgroundSongs();

            newGameObjects.Add(new MainHandItem(2));

            PlayerInstance = new Player(PlayerClass.Bishop); //Using it as a reference to get the players position
            newGameObjects.Add(PlayerInstance);
            newGameObjects.Add(new Enemy(_graphics));
            newGameObjects.Add(new Area(0, 0, 0));
            newGameObjects.Add(new Area(1, 2000, 2000));

            #region obstacle
            newGameObjects.Add(new AvSurface(200, 0));
            newGameObjects.Add(new Obstacle(-200, 0));
            #endregion

            base.Initialize();

            //gameObjects.Add(new GUI());
        }



        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var mouseState = Mouse.GetState();

            mousePosition = new Vector2((int)(mouseState.X / Camera.Zoom) - (int)((float)_graphics.PreferredBackBufferWidth / 2 / Camera.Zoom) + (int)Camera.Position.X, (int)(mouseState.Y / Camera.Zoom) - (int)((float)_graphics.PreferredBackBufferHeight / 2 / Camera.Zoom) + 20 + (int)Camera.Position.Y);
            leftMouseButtonClick = mouseState.LeftButton == ButtonState.Pressed;
            rightMouseButtonClick = mouseState.RightButton == ButtonState.Pressed;

            //Updates gameObjects and collision
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);

                foreach (GameObject other in gameObjects)
                {
                    if (gameObject is Player)
                    {
                        if (other is AvSurface || other is Obstacle)
                        {
                            gameObject.CheckCollision(other);
                            other.CheckCollision(gameObject);
                        }
                    }

                    if (gameObject is Enemy)
                    {
                        if (other is AvSurface || other is Obstacle)
                        {
                            gameObject.CheckCollision(other);
                            other.CheckCollision(gameObject);
                        }
                        
                    }
                }
            }

            //"Spawns" new items
            foreach (GameObject newGameObject in newGameObjects)
            {
                newGameObject.LoadContent(Content);
                if (newGameObject is Item)
                    playerInventory.Add(newGameObject as Item);
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

#if DEBUG
                DrawCollisionBox(item);
#endif

            }


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
#endif

        /// <summary>
        /// Loads sprites into the "commonSprites" Dictionary
        /// </summary>
        private void LoadCommonSprites()
        {

#if DEBUG
            Texture2D collisionTexture = Content.Load<Texture2D>("Sprites\\DEBUG\\pixel");
#endif

            Texture2D quest = Content.Load<Texture2D>("Sprites\\Item\\questItemPlaceholder");
            Texture2D mainHand = Content.Load<Texture2D>("Sprites\\Item\\mainHandPlaceholder");
            Texture2D offHand = Content.Load<Texture2D>("Sprites\\Item\\offHandPlaceholder");
            Texture2D torso = Content.Load<Texture2D>("Sprites\\Item\\torsoPlaceholder");
            Texture2D feet = Content.Load<Texture2D>("Sprites\\Item\\feetPlaceholder");
            Texture2D stone = Content.Load<Texture2D>("Sprites\\Obstacle\\stone"); //Stone


            commonSprites.Add("questItem", quest);
            commonSprites.Add("mainHandItem", mainHand);
            commonSprites.Add("offHandItem", offHand);
            commonSprites.Add("torsoItem", torso);
            commonSprites.Add("feetItem", feet);
            commonSprites.Add("stone", stone);

#if DEBUG
            commonSprites.Add("collisionTexture", collisionTexture);
#endif

        }

        /// <summary>
        /// Loads animation arrays into the "animationSprites" Dictionary
        /// </summary>
        private void LoadAnimationArrays()
        {

            areaArray = new Texture2D[5]
            {
            Content.Load<Texture2D>("Sprites\\area\\roomUdkast"),
            Content.Load<Texture2D>("Sprites\\area\\bigRoom1"),
            Content.Load<Texture2D>("Sprites\\area\\bigRoom2"),
            Content.Load<Texture2D>("Sprites\\area\\bigRoom3"),
            Content.Load<Texture2D>("Sprites\\area\\bigRoom4"),
            };
            animationSprites.Add("areaStart", areaArray);

            #region Morten

            #region Bishop
            Texture2D[] bishop = new Texture2D[4];
            for (int i = 0; i < 4; i++)
            {
                bishop[i] = Content.Load<Texture2D>("Sprites\\Charactor\\helligMortenHvid" + i);
            }
            animationSprites.Add("BishopMorten", bishop);

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
        #endregion
    }
}
