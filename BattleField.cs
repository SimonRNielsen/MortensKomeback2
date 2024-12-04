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
        public static List<Enemy> battlefieldEnemies = new List<Enemy>();
        GraphicsDevice graphicsDevice;
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
        private SpriteFont standardFont;
        private Dialogue battlefieldDialogue;
        private Vector2 textOrigin;
        private float textScale;
        private string attackText;
        private string blockText;
        private string healText;
        private bool battleWon = false;

        #endregion

        #region Properties

        #endregion

        #region Constructor
        public BattleField(Enemy enemy)
        {

            battlefieldEnemies.Add(enemy);
            this.Position = new Vector2(0, 1080 * 10);
            GameWorld.Camera.Position = this.Position;
            //this.graphicsDevice = graphicsDevice;
            playerOriginPosition = GameWorld.PlayerInstance.Position;
            GameWorld.PlayerInstance.Position = new Vector2(this.Position.X - 700, this.Position.Y); ;
            enemy.Position = new Vector2(this.Position.X + 700, this.Position.Y);
            enemy.SpriteEffectIndex = 0;
            chosenAction = 0;
            battlefieldDialogue = new Dialogue(new Vector2(this.Position.X, this.Position.Y + 320));
            GameWorld.newGameObjects.Add(battlefieldDialogue);      //Dialogue box visual
            foreach (Item i in GameWorld.equippedPlayerInventory)
            {
                playerDamageBonus += i.DamageBonus;
                playerDamageReductionBonus += i.DamageReductionBonus;
            }
            textOrigin = Vector2.Zero;
            textScale = 2f;

        }

        #endregion

        #region Methods
        public override void LoadContent(ContentManager content)
        {
            textBoxColor = Color.Beige;
            standardFont = content.Load<SpriteFont>("standardFont");
            switch (GameWorld.PlayerInstance.PlayerClass)
            {
                case (PlayerClass)1:
                    attackText = "Melee attack!";
                    blockText = "Block";
                    healText = "Heal (drink goose blood)";
                    break;
                case (PlayerClass)2:
                    attackText = "Ranged attack!";
                    blockText = "Block";
                    healText = "Heal (drink goose blood)";
                    break;
                case (PlayerClass)3:
                    attackText = "Magic attack!";
                    blockText = "Evade";
                    healText = "Heal (holy magic)";
                    break;
            }


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
                TakeAction(chosenAction, GameWorld.PlayerInstance.PlayerClass, gameTime);
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
            if (!playerActionOngoing && !enemyActionOngoing)
            {
                spriteBatch.DrawString(standardFont, "Choose your action for this battle round!", battlefieldDialogue.Position - new Vector2((float)(820), 130), Color.Black, 0, textOrigin, textScale, SpriteEffects.None, layer + 0.1f);
                spriteBatch.DrawString(standardFont, "1) " +attackText, battlefieldDialogue.Position - new Vector2((float)(820), 90), textBoxColor, 0, textOrigin, textScale, SpriteEffects.None, layer + 0.1f);
                spriteBatch.DrawString(standardFont, "2) " +blockText, battlefieldDialogue.Position - new Vector2((float)(820), 50), textBoxColor, 0, textOrigin, textScale, SpriteEffects.None, layer + 0.1f);
                spriteBatch.DrawString(standardFont, "3) " + healText, battlefieldDialogue.Position - new Vector2((float)(820), 10), textBoxColor, 0, textOrigin, textScale, SpriteEffects.None, layer + 0.1f);
            }
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
                GameWorld.PlayerInstance.Position = playerOriginPosition;
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
            GameWorld.PlayerInstance.Position += new Vector2(0, 3);

            //Move
            //Animate
            //Sound
            //Calculate hit
            //Calculate damage

        }

        private void RangedAttack(GameTime gameTime)
        {
            //Animate
            GameWorld.PlayerInstance.Animation(gameTime);

            //Move
            if ((GameWorld.PlayerInstance.Position.X <= this.Position.X + 300) && (move == 1))
                GameWorld.PlayerInstance.Position += new Vector2(5f, 0);
            else if ((GameWorld.PlayerInstance.Position.X >= this.Position.X + 300) && (move == 1))
            {
                move = 2;
                battlefieldEnemies[0].Health -= (GameWorld.PlayerInstance.Damage + playerDamageBonus);

            }
            else if ((GameWorld.PlayerInstance.Position.X > this.Position.X - 700) && (move == 2))
            {
                GameWorld.PlayerInstance.Position -= new Vector2(5f, 0);
            }
            else
            {
                playerActionOngoing = false;
                if (battlefieldEnemies.Count > 0)
                {
                    enemyActionOngoing = true;
                }
                else
                { WinBattle(); }
                move = 1;
            }



            //Sound
            //Calculate hit
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
            GameWorld.PlayerInstance.Position += new Vector2(3, 0);

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
                GameWorld.PlayerInstance.Health -= (battlefieldEnemies[0].Damage - playerDamageReductionBonus);
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
            battleWon = true;
        }


        #endregion
    }
}
