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
        private Random randomAction;
        private int enemyAction;
        bool blocking = false;
        string enemyActionText;
        string playerActionText;
        Texture2D[] playerDefaultSpriteArray;

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
            GameWorld.PlayerInstance.SpriteEffectIndex = 0;
            playerDefaultSpriteArray = GameWorld.PlayerInstance.Sprites;

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
            randomAction = new Random();
            enemyActionText = "";
            playerActionText = "";

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
            //The enemies action is also chosen here at random
            if (!playerActionOngoing && !enemyActionOngoing)
            {
                HandleInput();
                chosenAction = HandleInput();
                if (chosenAction > 0)
                { playerActionOngoing = true; }
                enemyAction = randomAction.Next(1, 5);
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
                spriteBatch.DrawString(standardFont, "1) " + attackText, battlefieldDialogue.Position - new Vector2((float)(820), 90), textBoxColor, 0, textOrigin, textScale, SpriteEffects.None, layer + 0.1f);
                spriteBatch.DrawString(standardFont, "2) " + blockText, battlefieldDialogue.Position - new Vector2((float)(820), 50), textBoxColor, 0, textOrigin, textScale, SpriteEffects.None, layer + 0.1f);
                spriteBatch.DrawString(standardFont, "3) " + healText, battlefieldDialogue.Position - new Vector2((float)(820), 10), textBoxColor, 0, textOrigin, textScale, SpriteEffects.None, layer + 0.1f);
            }
            if (playerActionOngoing)
            {
                spriteBatch.DrawString(standardFont, playerActionText, battlefieldDialogue.Position - new Vector2((float)(820), 130), Color.Black, 0, textOrigin, textScale, SpriteEffects.None, layer + 0.1f);
            }
            if (enemyActionOngoing)
            {
                spriteBatch.DrawString(standardFont, enemyActionText, battlefieldDialogue.Position - new Vector2((float)(820), 130), Color.Black, 0, textOrigin, textScale, SpriteEffects.None, layer + 0.1f);
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
                        MeleeAttack(gameTime);
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
                    GameWorld.PlayerInstance.Sprites = GameWorld.animationSprites["crusaderAnimArray"]; //TODO : Insert right sprite array. Like Magic heal. 
                    Heal(gameTime);
                    break;

                default:
                    break;
            }
        }


        private void MeleeAttack(GameTime gameTime)
        {
            playerActionText = "You are attacking the enemy!";
            //Animate
            GameWorld.PlayerInstance.Animation(gameTime);

            //Move
            if ((GameWorld.PlayerInstance.Position.X <= this.Position.X + 300) && (move == 1))
                GameWorld.PlayerInstance.Position += new Vector2(5f, 0);
            else if ((GameWorld.PlayerInstance.Position.X >= this.Position.X + 300) && (move == 1))
            {
                move = 2;
                int currentDamage = GameWorld.PlayerInstance.Damage + playerDamageBonus;
                battlefieldEnemies[0].Health -= currentDamage;
                playerActionText = $"You dealt {currentDamage} to the enemy's health!";

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
            {
                GameWorld.PlayerInstance.Position += new Vector2(5f, 0);
                playerActionText = "You are attacking the enemy from range!";
            }
            else if ((GameWorld.PlayerInstance.Position.X >= this.Position.X + 300) && (move == 1))
            {
                move = 2;
                int currentDamage = GameWorld.PlayerInstance.Damage + playerDamageBonus;
                battlefieldEnemies[0].Health -= currentDamage;
                playerActionText = $"You dealt {currentDamage} to the enemy's health!";

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
            blocking = true;
            //Move
            if ((GameWorld.PlayerInstance.Position.X <= this.Position.X - 200) && (move == 1))
            {
                playerActionText = "You are trying to block the enemy's attack!";
                GameWorld.PlayerInstance.Position += new Vector2(5f, 0);
            }
            else if ((GameWorld.PlayerInstance.Position.X >= this.Position.X - 200) && (move == 1))
            {
                move = 2;
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
            //Animate
            //Sound
            //Calculate defense
        }

        private void Evade()
        {
            blocking = true;
            if ((GameWorld.PlayerInstance.Position.X <= this.Position.X - 200) && (move == 1))
            {
                playerActionText = "You are trying to evade the enemy's attack!";
                GameWorld.PlayerInstance.Position += new Vector2(5f, 0);
            }
            else if ((GameWorld.PlayerInstance.Position.X >= this.Position.X + -200) && (move == 1))
            {
                move = 2;
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
            //Move
            //Animate
            //Sound
            //Calculate defense

        }

        private void Heal(GameTime gameTime)
        {
            GameWorld.PlayerInstance.Animation(gameTime);

            if ((GameWorld.PlayerInstance.Position.X <= this.Position.X - 400) && (move == 1))
            {
                playerActionText = "You are trying to heal yourself...";
                GameWorld.PlayerInstance.Position += new Vector2(5f, 0);
            }
            else if ((GameWorld.PlayerInstance.Position.X >= this.Position.X + -400) && (move == 1))
            {
                move = 2;
                if (GameWorld.PlayerInstance.Health < 100)
                {
                    GameWorld.PlayerInstance.Health += 10;
                    playerActionText = "You heal yourself for 10 health!";
                }
                else
                {
                    playerActionText = "You are already at full health!";
                }
            }
            else if ((GameWorld.PlayerInstance.Position.X > this.Position.X - 700) && (move == 2))
            {
                GameWorld.PlayerInstance.Position -= new Vector2(5f, 0);

            }
            else
            {
                playerActionOngoing = false;
                GameWorld.PlayerInstance.Sprites = playerDefaultSpriteArray; 
                GameWorld.PlayerInstance.Sprite = GameWorld.PlayerInstance.Sprites[0];
                if (battlefieldEnemies.Count > 0)
                {
                    enemyActionOngoing = true;
                }
                else
                { WinBattle(); }
                move = 1;
            }
            //Animate
            //Sound
            //Calculate heal
            //Heal
        }

        private void EnemyAction()
        {
            switch (enemyAction)
            {
                case 1:
                case 2:
                case 3:
                    {
                        if ((battlefieldEnemies[0].Position.X >= this.Position.X - 300) && (move == 1))
                        {
                            enemyActionText = "The enemy is attacking you!";
                            battlefieldEnemies[0].Position -= new Vector2(5f, 0);
                        }
                        else if ((battlefieldEnemies[0].Position.X <= this.Position.X - 300) && (move == 1))
                        {
                            move = 2;
                            if (blocking)
                            {
                                int currentDamage = battlefieldEnemies[0].Damage - playerDamageReductionBonus - 5;
                                if (currentDamage >= 0)
                                {
                                    GameWorld.PlayerInstance.Health -= currentDamage;
                                    enemyActionText = $"The enemy dealt {currentDamage} damage! You negated 5 damage!";
                                }
                                else
                                {
                                    enemyActionText = $"The enemy dealt no damage! You negated 5 damage!";
                                }
                                blocking = false;
                            }
                            else
                            {
                                int currentDamage = battlefieldEnemies[0].Damage - playerDamageReductionBonus;
                                if (currentDamage >= 0)
                                {
                                    GameWorld.PlayerInstance.Health -= currentDamage;
                                    enemyActionText = $"The enemy dealt {currentDamage} damage!";
                                }
                                else
                                    enemyActionText = $"The enemy dealt no damage!";
                            }
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
                        break;
                    }
                case 4:
                    {
                        if ((battlefieldEnemies[0].Position.X >= this.Position.X + 300) && (move == 1))
                        {
                            battlefieldEnemies[0].Position -= new Vector2(5f, 0);
                            enemyActionText = "The enemy is healing itself... ";
                        }
                        else if ((battlefieldEnemies[0].Position.X <= this.Position.X + 300) && (move == 1))
                        {
                            move = 2;
                            if (battlefieldEnemies[0].Health < 100)//TODO: use maxhealth here!
                            {
                                battlefieldEnemies[0].Health += 5;
                                enemyActionText = "It heals itself for 5 health!";
                            }
                            else
                            {
                                enemyActionText = "The enemy is already at full health!";
                            }

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
                        break;
                    }

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
