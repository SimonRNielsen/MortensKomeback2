using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

        
        #endregion

        #region Properties

        #endregion

        #region Constructor
        public BattleField(Player player, Enemy enemy, GraphicsDevice graphicsDevice)
        {
            battlefieldPlayers[0] = player;
            battlefieldEnemies.Add(enemy);
            this.graphicsDevice = graphicsDevice;
        }

        #endregion

        #region Methods
        public override void LoadContent(ContentManager content)
        {
            /// Create the single-pixel texture
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });


            textBox = new Rectangle(0, 0, 500, 50);
            /// Set a default color for the smallRectangle
            textBoxColor = Color.White;
        }

        public override void OnCollision(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            int chosenAction = HandleInput();
            TakeAction(chosenAction, battlefieldPlayers[0].PlayerClass);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixel, textBox, textBoxColor);
        }

        private int HandleInput()
        {
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
                    if(playerClass == (PlayerClass)1)
                    {
                        MeleeAttack();
                    }
                    else
                    {
                        RangedAttack();
                    }
                    break;
                case 2:
                    if(playerClass == (PlayerClass)1)
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
