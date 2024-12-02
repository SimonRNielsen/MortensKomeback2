using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortensKomeback2
{
    internal class BattleField : GameObject
    {
        #region Fields
        public static Player[] battlefieldPlayers = new Player[1];
        public static List<Enemy> battlefieldEnemies = new List<Enemy>();
        private Rectangle textBox;
        GraphicsDevice graphicsDevice;
        // A single-pixel texture
        Texture2D pixel;
        Color textBoxColor;
        Vector2 playerOriginPosition;


        #endregion

        #region Properties

        #endregion

        #region Constructor
        public BattleField(Player player, Enemy enemy)
        {
            battlefieldPlayers[0] = player;
            battlefieldEnemies.Add(enemy);
            GameWorld.Camera.Position = new Vector2(2000, 0);
            this.Position = new Vector2(1500, 0);
            //this.graphicsDevice = graphicsDevice;
            playerOriginPosition = player.Position;
            player.Position = new Vector2(1200, 0); ;
            enemy.Position = new Vector2(2700, -200);
            enemy.SpriteEffectIndex = 0;
        }

        #endregion

        #region Methods
        public override void LoadContent(ContentManager content)
        {
            /// Create the single-pixel texture
            pixel = content.Load<Texture2D>("Sprites\\Menu\\button");


            textBox = new Rectangle(0, 0, 500, 50);
            /// Set a default color for the smallRectangle
            textBoxColor = Color.White;
        }

        public override void OnCollision(GameObject gameObject)
        {
            //Nothing will happen: the battlefiel shouldn't collide with anything. 
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Enemy e in battlefieldEnemies)
            {
                e.Animation(gameTime);
            }
            HandleInput();
            int chosenAction = HandleInput();
            TakeAction(chosenAction, battlefieldPlayers[0].PlayerClass);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixel, Position, textBox, textBoxColor, rotation, Vector2.Zero, scale, SpriteEffects.None, 1);
        }

        private int HandleInput()
        {
            //Reseting the velocity so Morten will stand still when there is no key pressed down
            velocity = Vector2.Zero;

            //Get the keyboard state
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Q))
            {
                battlefieldPlayers[0].Position = playerOriginPosition;
                GameWorld.Camera.Position = Vector2.Zero;
            }
            return 1;
        }

        /// <summary>
        /// This method makes the action happen that the player and enemy has decided
        /// </summary>
        private void TakeAction(int chosenAction, PlayerClass playerClass)
        {
            switch (chosenAction)
            {
                case 1:
                    if (playerClass == (PlayerClass)1)
                    {
                        MeleeAttack();
                    }
                    else
                    {
                        RangedAttack();
                    }
                    break;
                case 2:
                    if (playerClass == (PlayerClass)1)
                    {
                        Block();
                    }
                    else
                    {
                        Evade();
                    }
                    break;
                case 3:
                    Heal();
                    break;

                default:
                    break;
            }
        }


        private void MeleeAttack()
        {
            //Move
            //Animate
            //Sound
            //Calculate hit
            //Calculate damage

        }

        private void RangedAttack()
        {
            //Animate
            //Sound
            //Calculate hit
            //Calculate damage
            //Do damage
        }

        private void Block()
        {
            //Move
            //Animate
            //Sound
            //Calculate defense
        }

        private void Evade()
        {
            //Move
            //Animate
            //Sound
            //Calculate defense

        }

        private void Heal()
        {
            //Animate
            //Sound
            //Calculate heal
            //Heal
        }


        #endregion
    }
}
