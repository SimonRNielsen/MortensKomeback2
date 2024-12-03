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
        private int chosenAction;
        bool keysAreLifted;
        float actionTimer = 0;
        float actionTimerDuration = 5f;
        bool playerActionOngoing = false;
        bool enemyActionOngoing = false;
        private int move = 1;
        private int playerDamageReductionBonus = 0;
        private int playerDamageBonus = 0;


        #endregion

        #region Properties

        #endregion

        #region Constructor
        public BattleField(Player player, Enemy enemy)
        {
            battlefieldPlayers[0] = player;
            battlefieldEnemies.Add(enemy);
            this.Position = new Vector2(0, 1080 * 10);
            GameWorld.Camera.Position = this.Position;
            //this.graphicsDevice = graphicsDevice;
            playerOriginPosition = player.Position;
            player.Position = new Vector2(this.Position.X - 700, this.Position.Y); ;
            enemy.Position = new Vector2(this.Position.X + 700, this.Position.Y);
            enemy.SpriteEffectIndex = 0;
            chosenAction = 0;
            GameWorld.newGameObjects.Add(new Dialogue(new Vector2(0, this.Position.Y + 320)));      //Dialogue box visual
            foreach (Item i in GameWorld.equippedPlayerInventory)
            {
                playerDamageBonus += i.DamageBonus;
                playerDamageReductionBonus += i.DamageReductionBonus;
            }
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
            //Action logic: if there is no acton going on, player should be able to choose actions   
            if (!playerActionOngoing && !enemyActionOngoing)
            {
                HandleInput();
                chosenAction = HandleInput();
                if (chosenAction > 0)
                { playerActionOngoing = true; }
            }
            //If the action is happening, the player shouldn't be able to do anything but watch the anctions onfold
            if (playerActionOngoing /*&& actionTimer < actionTimerDuration*/)
            {
                TakeAction(chosenAction, battlefieldPlayers[0].PlayerClass, gameTime);
                //actionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (enemyActionOngoing)
            {
                EnemyAction();
            }
            foreach (Enemy e in battlefieldEnemies)
            {
                if (battlefieldEnemies[0].Health <= 0)
                {
                    battlefieldEnemies[0].IsAlive = false;
                }
            }
                battlefieldEnemies.RemoveAll(enemy => enemy.IsAlive == false);
            //When the timer is up, the action has ended.
            //else
            //{
            //    actionTimer = 0;
            //    actionOngoing = false;
            //}
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

            if (keyState.IsKeyDown(Keys.D1) || keyState.IsKeyDown(Keys.NumPad1) && keysAreLifted)
            {
                keysAreLifted = false;
                return 1;
            }

            if (keyState.IsKeyDown(Keys.D2) || keyState.IsKeyDown(Keys.NumPad2) && keysAreLifted)
            {
                keysAreLifted = false;
                return 2;
            }

            if (keyState.IsKeyDown(Keys.D3) || keyState.IsKeyDown(Keys.NumPad3) && keysAreLifted)
            {
                keysAreLifted = false;
                return 3;
            }

            if (keyState.IsKeyUp(Keys.NumPad1)
                && keyState.IsKeyUp(Keys.NumPad2)
                && keyState.IsKeyUp(Keys.NumPad3)
                && keyState.IsKeyUp(Keys.D1)
                && keyState.IsKeyUp(Keys.D2)
                && keyState.IsKeyUp(Keys.D3))
            {
                keysAreLifted = true;
            }

            if (keyState.IsKeyDown(Keys.Q))
            {
                battlefieldPlayers[0].Position = playerOriginPosition;
                GameWorld.Camera.Position = Vector2.Zero;
                GameWorld.BattleActive = false;
                IsAlive = false;
            }
            return 0;
        }

        /// <summary>
        /// This method makes the action happen that the player and enemy has decided
        /// </summary>
        private void TakeAction(int chosenAction, PlayerClass playerClass, GameTime gameTime)
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
                        RangedAttack(gameTime);
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
            battlefieldPlayers[0].Position += new Vector2(0, 3);

            //Move
            //Animate
            //Sound
            //Calculate hit
            //Calculate damage

        }

        private void RangedAttack(GameTime gameTime)
        {
            //Animate
            battlefieldPlayers[0].Animation(gameTime);

            //Move
            if ((battlefieldPlayers[0].Position.X <= this.Position.X + 700) && (move == 1))
                battlefieldPlayers[0].Position += new Vector2(5f, 0);
            else if ((battlefieldPlayers[0].Position.X >= this.Position.X + 700) && (move == 1))
            {
                move = 2;
                battlefieldEnemies[0].Health -= (battlefieldPlayers[0].Damage + playerDamageBonus);

            }
            else if ((battlefieldPlayers[0].Position.X > this.Position.X - 700) && (move == 2))
            {
                battlefieldPlayers[0].Position -= new Vector2(5f, 0);
            }
            else
            {
                playerActionOngoing = false;
                if (battlefieldEnemies.Count >0)
                {
                    enemyActionOngoing = true;
                }
                else
                { WinBattle(); }
                move = 1;
            }



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
            battlefieldPlayers[0].Position += new Vector2(3, 0);

            //Animate
            //Sound
            //Calculate heal
            //Heal
        }

        private void EnemyAction()
        {
            if ((battlefieldEnemies[0].Position.X >= this.Position.X - 700) && (move == 1))
                battlefieldEnemies[0].Position -= new Vector2(5f, 0);
            else if ((battlefieldEnemies[0].Position.X <= this.Position.X - 500) && (move == 1))
            {
                move = 2;
                battlefieldPlayers[0].Health -= (battlefieldEnemies[0].Damage - playerDamageReductionBonus);
            }
            else if ((battlefieldEnemies[0].Position.X < this.Position.X + 700) && (move == 2))
            {
                battlefieldEnemies[0].Position += new Vector2(5f, 0);
            }
            else
            {
                enemyActionOngoing = false;
                move = 1;
            }
        }

        /// <summary>
        /// When an enemy is defeated, the battle is won. Show specific text, and lets the player return form the battle. 
        /// </summary>
        private void WinBattle()
        {

        }


        #endregion
    }
}
