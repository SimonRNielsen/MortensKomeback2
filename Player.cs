using Microsoft.Xna.Framework;
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
        private int healthMax;
        private bool praying;
        private bool interact;
        private bool inventory;
        private bool healing;
        private byte interactRange = 100;
        private List<NPC> nPCList;
        private int limitedHeals = 5;
        private int maxHealth = 100;
        private int healthBonus;

        private bool searching;
        

        /// <summary>
        /// Bool to change the spriteEffectIndex so the player face the direction is walking 
        /// </summary>
        private bool direction = true;

        public int HealthMax { get => healthMax; set => healthMax = value; }

        #endregion

        #region properti

        public int MaxHealth { get => maxHealth; }
        public int HealthBonus { get => healthBonus; set => healthBonus = value; }

        #endregion

        #region constructor
        public Player(PlayerClass playerClass, List<NPC> nPCs)
        {
            //this.healthMax = health;
            this.speed = 600; //Not sure what health should be
            this.health = 100; //Not sure what health should be
            HealthMax = 100;
            this.fps = 2f;
            this.playerClass = playerClass;
            nPCList = nPCs;
            layer = 0.25f;
        }

        #endregion

        #region method
        /// <summary>
        /// Loading the different content for the different kind of PlayerClass
        /// </summary>
        /// <param name="content"></param>
        public override void LoadContent(ContentManager content)
        {
            sprites = new Texture2D[4];

            switch (playerClass)
            {
                case PlayerClass.Monk:
                    sprites = GameWorld.animationSprites["monk"];
                    break;
                case PlayerClass.Crusader:
                    sprites = GameWorld.animationSprites["crusader"];
                    break;
                case PlayerClass.Bishop:
                    sprites = GameWorld.animationSprites["bishop"];
                    break;
            }

            //Start sprite
            this.Sprite = sprites[0];
        }

        /// <summary>
        /// What the different outcome is for the player when it's colliding with a gameObject
        /// </summary>
        /// <param name="gameObject">A gameObject</param>
        public override void OnCollision(GameObject gameObject)
        {

            if (gameObject is AvSurface)
            {
                //Reduse the players health when waking through
                health = health - 10; //Not sure if it should be 10
            }

            if (gameObject is Obstacle)
            {
                int moveAway = 30; //How much the player is bouncing back after colliding 

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

        }

        public override void Update(GameTime gameTime)
        {
            GameWorld.Camera.Position = new Vector2(GameWorld.Camera.Position.X, position.Y);
            HandleInput();
            Movement(gameTime);
            Animation(gameTime);
            base.Update(gameTime);
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
                this.spriteEffectIndex = 1;
                direction = true;
            }

            //Player moves right when pressed D
            if (keyState.IsKeyDown(Keys.D))
            {
                velocity += new Vector2(1, 0);
                this.spriteEffectIndex = 0;
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
                Pray(interactRange);
                praying = true;
            }

            if (keyState.IsKeyUp(Keys.P))
                praying = false;

            if (keyState.IsKeyDown(Keys.E) && !interact)
            {
                Interact(interactRange);
                interact = true;
            }

            if (keyState.IsKeyUp(Keys.E))
                interact = false;

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

            if (keyState.IsKeyDown(Keys.H) && !healing)
            {
                Heal();
                healing = true;
            }

            if (keyState.IsKeyUp(Keys.H))
                healing = false;

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
            //If the velocity is equal to Vect2.Zero there will not be any animation
            if (velocity == Vector2.Zero)
            {
                return;
            }

            //Adding the time which has passed since the last update
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            currentIndex = (int)(timeElapsed * fps);

            sprite = sprites[currentIndex];

            //Restart the animation
            if (currentIndex >= sprites.Length - 1)
            {
                timeElapsed = 0;
                currentIndex = 0;
            }
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
                foreach (Item item in GameWorld.hiddenItems)
                {
                    distance = Vector2.Distance(position, item.Position);
                    if (distance < range && distance > -range)
                    {
                        item.IsPickedUp = true;
                        item.IsFound = false;
                        item.Sprite = item.StandardSprite;
                        GameWorld.playerInventory.Add(item);
                    }
                }
            }

        }

        /// <summary>
        /// Attempts to find a key in the players inventory
        /// </summary>
        /// <returns>Instance of a key or null when no key is found</returns>
        public static Item FindKey()
        {
            Item key = GameWorld.playerInventory.Find(i => (i as QuestItem).IsKey);

            return key;
        }

        /// <summary>
        /// Removes an item from the players inventory
        /// </summary>
        /// <param name="item">Item to be removed</param>
        public static void RemoveItem(Item item)
        {
            GameWorld.playerInventory.Remove(item);
        }




        /// <summary>
        /// Performs a healing action for Player to recover missing health
        /// </summary>
        public void Heal()
        {
            int healAmount = 25;
            Item healingItem = GameWorld.FindHealingItem();
            
            if (!(health == maxHealth + healthBonus))
                if (playerClass == PlayerClass.Bishop && limitedHeals > 0)
                {
                    Health = healAmount + 25;
                    //if (!GameWorld.BattleActive)                      HUSK AT INDKOMMENTERE IGEN!!!!!
                    limitedHeals--;
                }
                else if (healingItem != null)
                {
                    Health = healAmount;
                    healingItem.IsUsed = true;
                }
        }

        #endregion
    }
}
