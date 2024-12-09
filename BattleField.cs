using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MortensKomeback2
{
    internal class BattleField : GameObject
    {
        #region Fields
        private bool battleWon = false;
        private bool blocking = false;
        private bool enemyActionOngoing = false;
        private bool keysAreLifted;
        private bool playerActionOngoing = false;
        private Color textBodyColor;
        private Color textHeaderColor;
        private Dialogue battlefieldDialogue;
        private GraphicsDevice graphicsDevice;
        private float actionTimer = 0;
        private float actionTimerDuration = 2f;
        private float textScale;
        private int chosenAction;
        private int enemyAction;
        private int actionPhase = 0;
        private int playerDamageReductionBonus = 0;
        private int playerDamageBonus = 0;
        public static List<Enemy> battlefieldEnemies = new List<Enemy>();
        private Obstacle egg;
        private static Obstacle magic;
        private Random randomAction;
        private SpriteFont standardFont;
        private string attackText;
        private string blockText;
        private string enemyActionText;
        private string healText;
        private string playerActionText;
        private Texture2D[] playerDefaultSpriteArray;
        private Vector2 playerOriginPosition;
        private Vector2 textOrigin;

        #endregion

        #region Properties

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for the battelfield class. Adds the relevant enemy to a list, and sets the fields of battlefield class
        /// Also moves the position of the plaer and enemy instance, and adds relevant objects to gameObjects in GameWorld. 
        /// </summary>
        /// <param name="enemy">The enemy the player has to battle</param>
        public BattleField(Enemy enemy)
        {
            //Adds enemy to a list, so it easily can be referenced
            battlefieldEnemies.Add(enemy);

            //Positions are set:
            this.Position = new Vector2(0, 1080 * 10);
            GameWorld.Camera.Position = this.Position;
            playerOriginPosition = GameWorld.PlayerInstance.Position;
            GameWorld.PlayerInstance.Position = new Vector2(this.Position.X - 700, this.Position.Y); ;
            enemy.Position = new Vector2(this.Position.X + 700, this.Position.Y);

            //Dialogue box is added
            battlefieldDialogue = new Dialogue(new Vector2(this.Position.X, this.Position.Y + 320));
            GameWorld.newGameObjects.Add(battlefieldDialogue);

            //Sprites are set for enemy and player
            playerDefaultSpriteArray = GameWorld.PlayerInstance.Sprites;
            enemy.SpriteEffectIndex = 0;
            GameWorld.PlayerInstance.SpriteEffectIndex = 0;

            //Adds new objects to be used for ranged attacks and healing:
            magic = new Obstacle((int)(GameWorld.PlayerInstance.Position.X + 100), (int)GameWorld.PlayerInstance.Position.Y, "magic");

            egg = new Obstacle((int)(GameWorld.PlayerInstance.Position.X + 100), (int)GameWorld.PlayerInstance.Position.Y, "egg");

            //Calculates damage bonus and defence bonus, based on the objects the player is wearing.
            foreach (Item i in GameWorld.equippedPlayerInventory)
            {
                playerDamageBonus += i.DamageBonus;
                playerDamageReductionBonus += i.DamageReductionBonus;
            }

            //Sets chosen action to zero
            chosenAction = 0;

        }

        #endregion

        #region Methods
        /// <summary>
        /// Loads text based on playerClass and sets colors, fonts and size for text. 
        /// Ovverride of GameObjects LoadContent.
        /// </summary>
        /// <param name="content">ContentManager</param>
        public override void LoadContent(ContentManager content)
        {
            textBodyColor = GameWorld.GrayGoose;
            textHeaderColor = Color.Black;
            textOrigin = Vector2.Zero;
            textScale = 2f;
            standardFont = content.Load<SpriteFont>("standardFont");
            enemyActionText = "";
            playerActionText = "";
            switch (GameWorld.PlayerInstance.PlayerClass)
            {
                case (PlayerClass)1:
                    attackText = "Melee attack!";
                    blockText = "Block";
                    healText = "Heal (drink Blood of Geesus)";
                    break;
                case (PlayerClass)2:
                    attackText = "Ranged attack!";
                    blockText = "Evade";
                    healText = "Heal (drink Blood of Geesus)";
                    break;
                case (PlayerClass)3:
                    attackText = "Magic attack!";
                    blockText = "Evade";
                    healText = "Heal (Holy Magic)";
                    break;
            }


            //Instantiates randomAction
            randomAction = new Random();

        }

        public override void OnCollision(GameObject gameObject)
        {
            //Nothing will happen: the battlefield shouldn't collide with anything. 
        }

        /// <summary>
        /// Update function for battelfield. Should only be called when an batte is happening.
        /// While this is the case, Update methods for the other objects in gameObjects will not be called,
        /// so this Update, should include HandleInput and other methods where necessary. 
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Update(GameTime gameTime)
        {
            //The enemy should allways be animated. 
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
                if (chosenAction > 0 && !(battleWon))
                { playerActionOngoing = true; }
                enemyAction = randomAction.Next(1, 5);
            }
            //If the action is happening, the player shouldn't be able to do anything but watch the anctions unfold
            if (playerActionOngoing)
            {
                TakeAction(chosenAction, GameWorld.PlayerInstance.PlayerClass, gameTime);
            }
            else if (enemyActionOngoing)
            {
                EnemyAction();
            }
            //Controls if there are any enemies without health, and makes sure they are marked as not alive
            foreach (Enemy e in battlefieldEnemies)
            {
                if (battlefieldEnemies[0].Health <= 0)
                {
                    battlefieldEnemies[0].IsAlive = false;
                }
            }
            //Removes all dead enemies. 
            battlefieldEnemies.RemoveAll(enemy => enemy.IsAlive == false);

        }

        /// <summary>
        /// Draw method for Battlefield. As dialoguebox, enemies, and attack is handled by their own classes, this method
        /// only draws the relevant text to the screen, depending on if an what actions are happening. 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!playerActionOngoing && !enemyActionOngoing && !battleWon)
            {
                spriteBatch.DrawString(standardFont, "Choose your action for this battle round!", battlefieldDialogue.Position - new Vector2((float)(820), 130), textHeaderColor, 0, textOrigin, textScale, SpriteEffects.None, layer + 0.1f);
                spriteBatch.DrawString(standardFont, "1) " + attackText, battlefieldDialogue.Position - new Vector2((float)(820), 90), textBodyColor, 0, textOrigin, textScale, SpriteEffects.None, layer + 0.1f);
                spriteBatch.DrawString(standardFont, "2) " + blockText, battlefieldDialogue.Position - new Vector2((float)(820), 50), textBodyColor, 0, textOrigin, textScale, SpriteEffects.None, layer + 0.1f);
                spriteBatch.DrawString(standardFont, "3) " + healText, battlefieldDialogue.Position - new Vector2((float)(820), 10), textBodyColor, 0, textOrigin, textScale, SpriteEffects.None, layer + 0.1f);
            }
            if (playerActionOngoing)
            {
                spriteBatch.DrawString(standardFont, playerActionText, battlefieldDialogue.Position - new Vector2((float)(820), 130), textBodyColor, 0, textOrigin, textScale, SpriteEffects.None, layer + 0.1f);
            }
            if (enemyActionOngoing)
            {
                spriteBatch.DrawString(standardFont, enemyActionText, battlefieldDialogue.Position - new Vector2((float)(820), 130), textBodyColor, 0, textOrigin, textScale, SpriteEffects.None, layer + 0.1f);
            }
            if (battleWon)
            {
                spriteBatch.DrawString(standardFont, "You have won the battle!", battlefieldDialogue.Position - new Vector2((float)(820), 130), textHeaderColor, 0, textOrigin, textScale, SpriteEffects.None, layer + 0.1f);
                spriteBatch.DrawString(standardFont, "Press \"Entter\" to return...", battlefieldDialogue.Position - new Vector2((float)(820), 90), textBodyColor, 0, textOrigin, textScale, SpriteEffects.None, layer + 0.1f);

            }

        }

        /// <summary>
        /// Handles player input, when the player has to choose which action to take, and to exit the battle when
        /// has been won. 
        /// </summary>
        /// <returns></returns>
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

            if (battleWon && keyState.IsKeyDown(Keys.Enter))
            {
                GameWorld.PlayerInstance.Position = playerOriginPosition;
                GameWorld.Camera.Position = Vector2.Zero;
                GameWorld.BattleActive = false;
                IsAlive = false;
            }
            return 0;
        }

        /// <summary>
        /// This method makes the action happen that the player has chosen
        /// </summary>
        private void TakeAction(int chosenAction, PlayerClass playerClass, GameTime gameTime)
        {
            switch (chosenAction)
            {
                case 1:
                    if (playerClass == PlayerClass.Crusader)
                    {
                        MeleeAttack(gameTime);
                    }
                    else if (playerClass == PlayerClass.Bishop)
                    {
                        //If the player is a Bishop, it will shoot magic.
                        magic.IsAlive = true;
                        RangedAttack(gameTime, magic);
                    }
                    else
                    {
                        //If the player is a Monk, it will shoot an egg
                        egg.IsAlive = true;
                        RangedAttack(gameTime, egg);
                    }
                    break;
                case 2:
                    Block();
                    break;
                case 3:
                    Heal(gameTime);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Player melee attack, that it can use if the player is a crusader. 
        /// Moves the player and deals damage to the enemy
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        private void MeleeAttack(GameTime gameTime)
        {
            //Animate
            GameWorld.PlayerInstance.Animation(gameTime);

            BeginAction("player");

            //Move
            if ((GameWorld.PlayerInstance.Position.X <= this.Position.X + 300) && (actionPhase == 1))
            {
                GameWorld.PlayerInstance.Position += new Vector2(5f, 0);
                playerActionText = "You are attacking the enemy!";
            }
            else if ((GameWorld.PlayerInstance.Position.X >= this.Position.X + 300) && (actionPhase == 1))
            {
                actionPhase = 2;
                int currentDamage = GameWorld.PlayerInstance.Damage + playerDamageBonus;
                battlefieldEnemies[0].Health -= currentDamage;
                playerActionText = $"You dealt {currentDamage} to the enemy's health!";

            }
            else if ((GameWorld.PlayerInstance.Position.X > this.Position.X - 700) && (actionPhase == 2))
            {
                GameWorld.PlayerInstance.Position -= new Vector2(5f, 0);
            }
            else
            {
                EndAction("player");
            }

        }

        /// <summary>
        /// Player ranged attack, that it can use if it is a Bishop or Monk
        /// Adds an object to GameWorlds gameObjects list, moves it, removes it form the list
        /// and damages the enemy
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        /// <param name="projectile">The pojectile (egg or magic) that the player is shooting</param>
        private void RangedAttack(GameTime gameTime, Obstacle projectile)
        {
            //Animate
            GameWorld.PlayerInstance.Animation(gameTime);

            BeginAction("player");

            //Move
            if ((projectile.Position.X <= this.Position.X + 300) && (actionPhase == 1))
            {
                projectile.Position += new Vector2(5f, 0);
                projectile.Rotation += 0.05f;
                playerActionText = "You are attacking the enemy from range!";
            }
            else if ((projectile.Position.X >= this.Position.X + 300) && (actionPhase == 1))
            {
                actionPhase = 2;
                int currentDamage = GameWorld.PlayerInstance.Damage + playerDamageBonus;
                battlefieldEnemies[0].Health -= currentDamage;
                playerActionText = $"You dealt {currentDamage} to the enemy's health!";

            }
            else if (actionTimer < actionTimerDuration && actionPhase == 2)
            {
                actionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                projectile.Rotation += 0.1f;
            }
            else
            {
                actionTimer = 0;
                projectile.IsAlive = false;
                projectile.Position = new Vector2(GameWorld.PlayerInstance.Position.X + 100, GameWorld.PlayerInstance.Position.Y);
                EndAction("player");
            }



            //Sound

        }

        /// <summary>
        /// The players block action. It does this by setting blocking = true.  
        /// Int the enemyAction method the damage taken is reduced if blocking = true, and if is a crusader damages the enemy in return.
        /// </summary>
        private void Block()
        {
            blocking = true;

            BeginAction("player");
            //Move
            if ((GameWorld.PlayerInstance.Position.X >= this.Position.X - 900) && (actionPhase == 1))
            {
                if (GameWorld.PlayerInstance.PlayerClass == PlayerClass.Crusader)
                    playerActionText = "You are trying to block the enemy's attack!";
                else
                    playerActionText = "You are trying to evade the enemy's attack!";

                GameWorld.PlayerInstance.Position -= new Vector2(5f, 0);
            }
            else if ((GameWorld.PlayerInstance.Position.X <= this.Position.X - 900) && (actionPhase == 1))
            {
                actionPhase = 2;
            }
            else if ((GameWorld.PlayerInstance.Position.X < this.Position.X - 700) && (actionPhase == 2))
            {
                GameWorld.PlayerInstance.Position += new Vector2(5f, 0);
            }
            else
            {
                EndAction("player");
            }
            //Animate
            //Sound
            //Calculate defense
        }

        private void Evade()
        {
            blocking = true;

            BeginAction("player");

            if ((GameWorld.PlayerInstance.Position.X <= this.Position.X - 200) && (actionPhase == 1))
            {
                playerActionText = "You are trying to evade the enemy's attack!";
                GameWorld.PlayerInstance.Position += new Vector2(5f, 0);
            }
            else if ((GameWorld.PlayerInstance.Position.X >= this.Position.X + -200) && (actionPhase == 1))
            {
                actionPhase = 2;
            }
            else if ((GameWorld.PlayerInstance.Position.X > this.Position.X - 700) && (actionPhase == 2))
            {
                GameWorld.PlayerInstance.Position -= new Vector2(5f, 0);
            }
            else
            {
                EndAction("player");
            }
            //Move
            //Animate
            //Sound
            //Calculate defense

        }
        /// <summary>
        /// The Heal action. Users the players Heal() method from player class.
        /// Differentiates text depending on class, and thereby how much the player heals/ if healing items is neccessary.
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        private void Heal(GameTime gameTime)
        {
            GameWorld.PlayerInstance.Animation(gameTime);

            BeginAction("player");

            if ((GameWorld.PlayerInstance.Position.X <= this.Position.X - 400) && (actionPhase == 1))
            {
                playerActionText = "You are trying to heal yourself...";
                GameWorld.PlayerInstance.Position += new Vector2(5f, 0);
            }
            else if ((GameWorld.PlayerInstance.Position.X >= this.Position.X + -400) && (actionPhase == 1))
            {
                actionPhase = 2;
                if (GameWorld.PlayerInstance.Health < GameWorld.PlayerInstance.MaxHealth + GameWorld.PlayerInstance.HealthBonus)
                {
                    bool succes = GameWorld.PlayerInstance.Heal();
                    if (GameWorld.PlayerInstance.PlayerClass == PlayerClass.Bishop && succes)
                    {
                        playerActionText = "You heal yourself for 50 health!";
                    }
                    else if (succes)
                    {
                        playerActionText = "You heal yourself for 25 health!";
                    }
                    else if (succes == false)
                    {
                        playerActionText = "You don't have any healing items!";
                    }
                }
                else
                {
                    playerActionText = "You are already at full health!";
                }
            }
            else if ((GameWorld.PlayerInstance.Position.X > this.Position.X - 700) && (actionPhase == 2))
            {
                GameWorld.PlayerInstance.Position -= new Vector2(5f, 0);

            }
            else
            {
                EndAction("player");
            }
            //Animate
            //Sound
            //Calculate heal
            //Heal
        }

        /// <summary>
        /// The Enemy's action. There is 3/4 chance of attack: Moves the enemy and damages the player
        /// And 1/4 chance of healing the enemy.
        /// </summary>
        private void EnemyAction()
        {

            BeginAction("enemy");
            switch (enemyAction)
            {
                case 1:
                case 2:
                case 3:
                    {
                        if ((battlefieldEnemies[0].Position.X >= this.Position.X - 300) && (actionPhase == 1))
                        {
                            enemyActionText = "The enemy is attacking you!";
                            battlefieldEnemies[0].Position -= new Vector2(5f, 0);
                        }
                        else if ((battlefieldEnemies[0].Position.X <= this.Position.X - 300) && (actionPhase == 1))
                        {
                            actionPhase = 2;
                            if (blocking)
                            {
                                int currentDamage = battlefieldEnemies[0].Damage - playerDamageReductionBonus - 5;
                                if (currentDamage >= 0)
                                {
                                    GameWorld.PlayerInstance.Health -= currentDamage;
                                    enemyActionText = $"The enemy dealt {currentDamage} damage! Your action negated 5 damage!";
                                    if(GameWorld.PlayerInstance.PlayerClass == PlayerClass.Crusader)
                                    {
                                        battlefieldEnemies[0].Health -= 5;
                                        enemyActionText += "\n You also dealt 5 damage to the enemy's health by blocking!";
                                    }
                                }
                                else
                                {
                                    enemyActionText = $"The enemy dealt no damage! Your action negated 5 damage!";
                                    if (GameWorld.PlayerInstance.PlayerClass == PlayerClass.Crusader)
                                    {
                                        battlefieldEnemies[0].Health -= 5;
                                        enemyActionText += "\n You also dealt 5 damage to the enemy's health by blocking!";
                                    }

                                }
                                blocking = false;
                            }
                            else
                            {
                                int currentDamage = battlefieldEnemies[0].Damage - playerDamageReductionBonus;
                                if (currentDamage >= 0)
                                {
                                    GameWorld.PlayerInstance.Health -= currentDamage;
                                    enemyActionText = $"The enemy dealt {currentDamage} damage to your health!";
                                }
                                else
                                    enemyActionText = $"The enemy dealt no damage!";
                            }
                        }
                        else if ((battlefieldEnemies[0].Position.X < this.Position.X + 700) && (actionPhase == 2))
                        {
                            battlefieldEnemies[0].Position += new Vector2(5f, 0);
                        }
                        else
                        {
                            EndAction("enemy");
                        }
                        break;
                    }
                case 4:
                    {
                        if ((battlefieldEnemies[0].Position.X >= this.Position.X + 300) && (actionPhase == 1))
                        {
                            battlefieldEnemies[0].Position -= new Vector2(5f, 0);
                            enemyActionText = "The enemy is healing itself... ";
                        }
                        else if ((battlefieldEnemies[0].Position.X <= this.Position.X + 300) && (actionPhase == 1))
                        {
                            actionPhase = 2;
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
                        else if ((battlefieldEnemies[0].Position.X < this.Position.X + 700) && (actionPhase == 2))
                        {
                            battlefieldEnemies[0].Position += new Vector2(5f, 0);

                        }
                        else
                        {
                            EndAction("enemy");
                        }
                        break;
                    }

            }
        }

        /// <summary>
        /// Ends the action currently happening. And also checks if the enemies are defeated. 
        /// When an enemy is defeated, battleWon is set to be true. 
        /// </summary>
        private void EndAction(String actor)
        {
            if (actor.ToLower() == "player")
            {
                playerActionOngoing = false;
                if (battlefieldEnemies.Count > 0)
                {
                    enemyActionOngoing = true;
                }
                else
                {
                    battleWon = true;
                }
            }
            else if (actor.ToLower() == "enemy")
            {
                enemyActionOngoing = false;
            }
            actionPhase = 0;

        }
        /// <summary>
        /// Starts the action that is about to happen. Used for things that should only be handled ad the beginning of the action and not changed during. 
        /// </summary>
        /// <param name="actor"></param>
        private void BeginAction(String actor)
        {
            if(actionPhase == 0)
            { 
            if (actor.ToLower() == "player")
            {
              if(GameWorld.PlayerInstance.PlayerClass == PlayerClass.Bishop && chosenAction == 1)
                {
                        GameWorld.newGameObjects.Add(magic);
                }
              else if(GameWorld.PlayerInstance.PlayerClass == PlayerClass.Monk)
                {
                    GameWorld.newGameObjects.Add(egg);
                }
            }
            else if (actor.ToLower() == "enemy")
            {
               
            }
            actionPhase = 1;
            }
        }


        #endregion
    }
}
