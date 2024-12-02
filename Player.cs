using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;

namespace MortensKomeback2
{
    internal class Player : Character
    {
        #region field
        private PlayerClass playerClass;
        private float timeElapsed;
        private int currentIndex;

        /// <summary>
        /// Bool to change the spriteEffectIndex so the player face the direction is walking 
        /// </summary>
        private bool direction = true;

        #endregion

        #region properti

        #endregion

        #region constructor
        public Player(PlayerClass playerClass)
        {
            this.speed = 600; //Not sure what health should be
            this.health = 100; //Not sure what health should be
            this.fps = 2f;
            this.playerClass = playerClass;
            //this.scale = 0.75f;
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
                case PlayerClass.Crusader:
                    break;
                case PlayerClass.Munk:
                    break;
                case PlayerClass.Bishop:
                    sprites = GameWorld.animationSprites["BishopMorten"];
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
            
            //if (gameObject is Door)
            //{
            // Open door to net area
            //}

            if (gameObject is Item)
            { 
                //Collect item
            }

            if (gameObject is AvSurface)
            {
                //Reduse the players health
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
            HandleInput();
            Movement(gameTime);
            Animation(gameTime);
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
            if (keyState.IsKeyDown(Keys.P))
            {

            }

            //Interact with item, door or NPC
            if (keyState.IsKeyDown(Keys.E))
            {

            }
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

        #endregion
    }
}
