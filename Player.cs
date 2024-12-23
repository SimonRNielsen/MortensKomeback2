﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MortensKomeback2
{
    internal class Player : Character
    {
        #region field
        private PlayerClass playerClass;
        private float timeElapsed;
        private int currentIndex;
        private bool praying;
        private bool interact;
        private bool inventory;
        private bool healing;
        private byte interactRange = 133;
        private List<NPC> nPCList;
        private int limitedHeals = 5;
        private int maxHealth = 100;
        private int healthBonus;
        private string inRoom;
        private float invulnerable = 1f;
        private float invulnerableTimer;
        private bool invulnerability;
        private float playDuration;
        private float playDurationTimer = 0.5f;
        private bool playFirst = false;
        private float speedBonus;
        private bool closeDialog = false;
        private bool enter = false;
        private bool battlefieldDamage = false;

        private string helpText = "K - Keybindings";
        private string keyBindings = "E - Interact with items or NPCs \nI - Inventory \nP - Pray \nEnter - Close Dialogue \nRight-click on items to equip \nESC to close menu, pause and exit";
        private bool showKeybinding = false;

        /// <summary>
        /// Bool to change the spriteEffectIndex so the player face the direction is walking 
        /// </summary>
        private bool direction = true;


        #endregion

        #region properti
        internal PlayerClass PlayerClass { get => playerClass; set => playerClass = value; }

        public int MaxHealth { get => maxHealth; }
        public int HealthBonus { get => healthBonus; set => healthBonus = value; }
        public string InRoom { get => inRoom; set => inRoom = value; }
        public Vector2 Velocity { get => velocity; set => velocity = value; }
        public bool CloseDialog { get => closeDialog; }
        public float SpeedBonus
        {
            get
            {
                return 1 + (speedBonus / 100);
            }
            set => speedBonus = value;
        }

        #endregion

        #region constructor
        public Player(PlayerClass playerClass, List<NPC> nPCs)
        {
            this.speed = 550; //Not sure what speed should be
            this.health = 100; //Not sure what health should be
            this.Fps = 10f;
            this.playerClass = playerClass;
            nPCList = nPCs;
            layer = 0.25f;
            if (GameWorld.PlayerInstance != null)
            {
                GameWorld.hiddenItems.Add(new MainHandItem(playerClass, new Vector2(GameWorld.Random.Next(-601, 600), (-1080 * 8) + GameWorld.Random.Next(-401, 400)), false, false));
                GameWorld.hiddenItems.Add(new MainHandItem(playerClass, new Vector2(GameWorld.Random.Next(-601, 600), (-1080 * 12) + GameWorld.Random.Next(-401, 400)), true, false));
                GameWorld.hiddenItems.Add(new OffHandItem(playerClass, new Vector2(GameWorld.Random.Next(-601, 600), (-1080 * 8) + GameWorld.Random.Next(-401, 400)), false, false));
                GameWorld.hiddenItems.Add(new OffHandItem(playerClass, new Vector2(GameWorld.Random.Next(-601, 600), (-1080 * 12) + GameWorld.Random.Next(-401, 400)), true, false));
                GameWorld.hiddenItems.Add(new TorsoSlotItem(playerClass, false, new Vector2(GameWorld.Random.Next(-601, 600), (-1080 * 6) + GameWorld.Random.Next(-401, 400))));
                GameWorld.hiddenItems.Add(new FeetSlotItem(playerClass, true, new Vector2(-400, -100)));
            }
            Damage = 10;
        }

        #endregion

        #region method
        /// <summary>
        /// Loading the different content for the different kind of PlayerClass
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content)
        {
            Sprites = new Texture2D[4];

            switch (PlayerClass)
            {
                case PlayerClass.Monk:
                    Sprites = GameWorld.animationSprites["monk"];
                    break;
                case PlayerClass.Crusader:
                    Sprites = GameWorld.animationSprites["crusader"];
                    break;
                case PlayerClass.Bishop:
                    Sprites = GameWorld.animationSprites["bishop"];
                    break;
            }

            //Start sprite
            this.Sprite = Sprites[0];
        }

        /// <summary>
        /// What the different outcome is for the player when it's colliding with a gameObject
        /// </summary>
        /// <param name="gameObject">A gameObject</param>
        public override void OnCollision(GameObject gameObject)
        {

            if (gameObject is AvSurface && !invulnerability)
            {
                //Reduse the players health when waking through
                TakeEnvironmentDamage(); //Not sure if it should be 10
            }

            if (gameObject is Obstacle)
            {
                int moveAway = 30; //How much the player is bouncing back after colliding 

                if (CollisionBox.Intersects(gameObject.CollisionBox))

                    if (this.CollisionBox.Y < gameObject.CollisionBox.Y) //Checking if the player is left to the obstacle
                    {

                        if (this.CollisionBox.X < gameObject.CollisionBox.X) //Checking if the player is o  top of the obstacle
                        {
                            this.position.X = this.position.X - moveAway; //Moving higher up
                        }
                        else
                        {
                            this.position.X = this.position.X + moveAway; //Moving down
                        }

                        this.position.Y = this.position.Y - moveAway; //Moving further to the left
                    }

                if (this.CollisionBox.Y > gameObject.CollisionBox.Y) //The same but to the right
                {
                    if (this.CollisionBox.X < gameObject.CollisionBox.X)
                    {
                        this.position.X = this.position.X - moveAway;
                    }
                    else
                    {
                        this.position.X = this.position.X + moveAway;
                    }

                    this.position.Y = this.position.Y + moveAway;
                }

            }
            if (gameObject is Enemy && !(gameObject is Boss))
            {
                if (GameWorld.BattleActive == false)
                {
                    GameWorld.BattleActive = true;
                    GameWorld.newGameObjects.Add(new BattleField(gameObject as Enemy));
                    GameWorld.PlayMusic(2); //Plays battlemusic

                }
            }
            if (gameObject is Boss)
            {
                (gameObject as Boss).StartDialogue = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            closeDialog = false;
            float npcInRange = interactRange * SpeedBonus;
            foreach (NPC nPC in nPCList)
                if (Vector2.Distance(nPC.Position, position) < npcInRange)
                    nPC.TextBubble = true;
            if (GameWorld.equippedPlayerInventory.Find(boots => boots is FeetSlotItem) != null)
                speed = 550f * (SpeedBonus);
            else
                speed = 550f;
            Movement(gameTime);
            HandleInput();
            Animation(gameTime);
            playDuration += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (velocity != Vector2.Zero && playDuration > playDurationTimer)
            {
                playDuration = 0;
                if (!playFirst)
                {
                    GameWorld.commonSounds["playerWalk2"].Play();
                    playFirst = true;
                }
                else
                {
                    GameWorld.commonSounds["playerWalk1"].Play();
                    playFirst = false;
                }
            }
            base.Update(gameTime);
            invulnerableTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (invulnerableTimer > invulnerable)
            {
                invulnerability = false;
            }
        }

        /// <summary>
        /// Handle the players keyboard input
        /// </summary>
        public void HandleInput()
        {
            //Reseting the velocity so Morten will stand still when there is no key pressed down
            velocity = Vector2.Zero;

            //Get the keyboard state
            KeyboardState keyState = Keyboard.GetState();

            //Player moves up when pressed W
            if (keyState.IsKeyDown(Keys.W))
            {
                velocity += new Vector2(0, -1);
            }

            //Player moves down when pressed S
            if (keyState.IsKeyDown(Keys.S))
            {
                velocity += new Vector2(0, 1);
            }

            //Player moves left when pressed A
            if (keyState.IsKeyDown(Keys.A))
            {
                velocity += new Vector2(-1, 0);
                this.SpriteEffectIndex = 1;
                direction = true;
            }

            //Player moves right when pressed D
            if (keyState.IsKeyDown(Keys.D))
            {
                velocity += new Vector2(1, 0);
                this.SpriteEffectIndex = 0;
                direction = false;
            }

            //Normalizing the velocity so if the player press more than one key the velocity will grow greater and greater 
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }

            //The player is Pray
            if (keyState.IsKeyDown(Keys.P) && !praying)
            {
                Pray((byte)(interactRange * SpeedBonus));
                praying = true;
            }

            if (keyState.IsKeyUp(Keys.P))
                praying = false;

            //Interact 
            if (keyState.IsKeyDown(Keys.E) && !interact)
            {
                Interact((byte)(interactRange * SpeedBonus));
                interact = true;
            }

            if (keyState.IsKeyUp(Keys.E))
                interact = false;

            //Inventory
            if (keyState.IsKeyDown(Keys.I) && !inventory && !GameWorld.DetectInventory())
            {
                GameWorld.newGameObjects.Add(new Menu(GameWorld.Camera.Position, 0));
                inventory = true;
            }
            else if (keyState.IsKeyDown(Keys.I) && !inventory && GameWorld.DetectInventory())
            {
                GameWorld.CloseMenu = true;
                inventory = true;
            }

            if (keyState.IsKeyUp(Keys.I))
                inventory = false;


            //Heal
            if (keyState.IsKeyDown(Keys.H) && !healing)
            {
                Heal();
                healing = true;
            }

            if (keyState.IsKeyUp(Keys.H))
                healing = false;

            //K for Keybindings
            if (keyState.IsKeyDown(Keys.K))
            {
                showKeybinding = true;
            }

            if (keyState.IsKeyUp(Keys.K))
            {
                showKeybinding = false;

            }

            if (keyState.IsKeyDown(Keys.Enter) && !enter)
            {
                enter = true;
                closeDialog = true;
            }

            if (keyState.IsKeyUp(Keys.Enter))
                enter = false;

        }

        /// <summary>
        /// The players movement calculated a the product of velocity, speed and deltaTime
        /// </summary>
        /// <param name="gameTime">A GameTime</param>
        public override void Movement(GameTime gameTime)
        {
            //Calculating the deltatime which is the time that has passed since the last frame
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //The player is moving by the result of HandleInput and deltaTime
            position += (velocity * speed * deltaTime);
        }

        /// <summary>
        /// Making an animation of the sprites
        /// </summary>
        /// <param name="gameTime">A GameTime</param>
        public override void Animation(GameTime gameTime)
        {
            //Restart the animation
            if (currentIndex >= Sprites.Length - 1)
            {
                timeElapsed = 0;
                currentIndex = 0;
            }
            //If the velocity is equal to Vect2.Zero there will not be any animation
            if (velocity == Vector2.Zero)
            {
                return;
            }

            //Adding the time which has passed since the last update
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            currentIndex = (int)(timeElapsed * Fps);

            sprite = Sprites[currentIndex];

        }

        /// <summary>
        /// Flips a bool on all items within a certain distance from the Player (currently set to 300 pixels)
        /// </summary>
        /// <param name="range">Determines the radius for which the Player "interacts with items nearby</param>
        private void Pray(byte range)
        {

            foreach (Item item in GameWorld.hiddenItems)
            {
                float distance = Vector2.Distance(position, item.Position);
                if (distance < (range * 3) && distance > (-range * 3))
                    item.IsFound = true;
            }
            GameWorld.commonSounds["playerHeal"].Play();

        }

        /// <summary>
        /// Makes the Player do a certain interaction with Item/NPC class depending on what it is
        /// </summary>
        /// <param name="range">Determines the radius for which the Player "interacts with items nearby</param>
        private void Interact(byte range)
        {

            bool nPCNearby = false;
            float distance;

            foreach (NPC nPC in nPCList)
            {
                distance = Vector2.Distance(nPC.Position, position);
                if (distance < range && distance > -range)
                {
                    nPCNearby = true;
                    GameWorld.newGameObjects.Add(new Dialogue(new Vector2(GameWorld.Camera.Position.X, GameWorld.Camera.Position.Y + 320), nPC));
                }
                if (nPCNearby)
                    break;
            }

            if (!nPCNearby)
            {
                bool playSound = false;
                foreach (Item item in GameWorld.hiddenItems)
                {
                    distance = Vector2.Distance(position, item.Position);
                    if (distance < range && distance > -range)
                    {
                        item.IsPickedUp = true;
                        item.IsFound = false;
                        if (item.Sprite.Name.Contains("blink"))
                            item.Sprite = item.StandardSprite;
                        GameWorld.playerInventory.Add(item);
                        playSound = true;
                    }
                }
                if (playSound == true)
                    GameWorld.commonSounds["equipItem"].Play();
            }

        }

        /// <summary>
        /// Attempts to find a key in the players inventory
        /// </summary>
        /// <returns>Instance of a key or null when no key is found</returns>
        public static QuestItem FindKey()
        {
            var possibleKeys = GameWorld.playerInventory.FindAll(i => i is QuestItem);
            QuestItem key = (QuestItem)possibleKeys.Find(i => (i as QuestItem).IsKey == true);
            return key;
        }

        /// <summary>
        /// Handles Player taking environment damage
        /// </summary>
        private void TakeEnvironmentDamage()
        {
            Health -= 10;
            invulnerableTimer = 0;
            invulnerability = true;
            GameWorld.commonSounds["playerAv"].Play();

        }

        /// <summary>
        /// Performs a healing action for Player to recover missing health
        /// </summary>
        public bool Heal()
        {

            int healAmount = 25;
            Item healingItem = GameWorld.FindHealingItem();

            if (!(health == maxHealth + healthBonus))
                if (playerClass == PlayerClass.Bishop && limitedHeals > 0 || playerClass == PlayerClass.Bishop && GameWorld.BattleActive)
                {
                    Health += healAmount + 25;
                    if (!GameWorld.BattleActive)
                        limitedHeals--;
                    if (!GameWorld.BattleActive)
                        GameWorld.commonSounds["playerHeal"].Play();
                    return true;
                }
                else if (healingItem != null)
                {
                    Health += healAmount;
                    healingItem.IsUsed = true;
                    if (!GameWorld.BattleActive)
                        GameWorld.commonSounds["playerHeal"].Play();
                    return true;
                }

            return false;

        }


        /// <summary>
        /// Overrides to give a damage "effect" on Player
        /// </summary>
        /// <param name="spriteBatch">Drawing tool</param>
        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);
            if (invulnerability || battlefieldDamage)
                spriteBatch.Draw(Sprite, Position, null, new Color(255, 0, 0) * 0.4f, rotation, new Vector2(Sprite.Width / 2, Sprite.Height / 2), scale, objectSpriteEffects[spriteEffectIndex], layer + 0.1f);

            //draw text
            if (showKeybinding == true)
            {
                spriteBatch.DrawString(GameWorld.mortensKomebackFont, keyBindings, new Vector2(Position.X, Position.Y - 100), Color.White, 0f, new Vector2(100, 100), 2f, SpriteEffects.None, 1f);

            }
            else
            {
                spriteBatch.DrawString(GameWorld.mortensKomebackFont, "K - Keybindings", new Vector2(GameWorld.Camera.Position.X - 620, GameWorld.Camera.Position.Y - 300), Color.White, 0f, new Vector2(100, 100), 1.2f, SpriteEffects.None, 0.66f);

            }


        }

        public void DamageAnimation(bool animation)
        {
            battlefieldDamage = animation;
        }

        #endregion
    }
}
