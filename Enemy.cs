using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MortensKomeback2
{
    internal class Enemy : Character
    {
        #region field
        private float deltaTime;
        private float timeElapsed;
        private int currentIndex;
        private bool direction = true; //Bool to change the spriteEffectIndex so the player face the direction is walking
        private float playDuration;
        private float playDurationTimer = 1f;
        protected int maxHealth = 15;

        public float MaxHealth { get => maxHealth; }



        #endregion

        #region Properties

        #endregion

        #region constructor
        /// <summary>
        /// The construction of a enemy
        /// </summary>
        /// <param name="_graphics">A GraphicsDeviceManager</param>
        public Enemy(Vector2 placement)
        {
            this.speed = 300;
            this.health = maxHealth;
            this.fps = 7f;
            this.scale = 0.5f;
            layer = 0.25f;
            sprite = GameWorld.animationSprites["WalkingGoose"][0];
            this.Position = placement;
            this.Damage = 10;
        }


        #endregion

        #region method

        public override void LoadContent(ContentManager content)
        {
            Sprites = GameWorld.animationSprites["WalkingGoose"];
            this.Sprite = Sprites[0];
        }

        /// <summary>
        /// When the enemy is colliding with a Obstacle it will turn around and walk in that direction
        /// </summary>
        /// <param name="gameObject">A GameObject</param>
        public override void OnCollision(GameObject gameObject)
        {
            if (gameObject is Obstacle)
            {
                if (gameObject.CollisionBox.X + gameObject.CollisionBox.Width + 30 > this.CollisionBox.X + this.sprite.Width)
                {
                    this.direction = false;
                }
                else if (gameObject.CollisionBox.X - gameObject.CollisionBox.Width - 30 < this.CollisionBox.X)
                {
                    this.direction = true;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (DistanceToPlayer(GameWorld.PlayerInstance.Position) < 2203)
            {
                playDuration += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (playDuration > playDurationTimer && sprite.Name.Contains("aggro"))
                {
                    playDuration = 0f;
                    if (!(this is Boss))
                        GameWorld.commonSounds["aggroGoose"].Play();
                }
                if (DistanceToPlayer(GameWorld.PlayerInstance.Position) <= 300f) //If the player is with in 300 pixel the enemy will swift animation
                {
                    if (!(this is Boss))
                        Sprites = GameWorld.animationSprites["AggroGoose"];
                }
                else
                {
                    if (!(this is Boss))
                        Sprites = GameWorld.animationSprites["WalkingGoose"];
                }

                Movement(gameTime);
                Animation(gameTime);
                if (this.Health <= 0)
                { IsAlive = false; }
            }
        }

        public override void Movement(GameTime gameTime)
        {
            //Calculating the deltatime which is the time that has passed since the last frame
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            velocity = new Vector2(1, 0);

            //The enemys movement 
            if (direction)
            {
                position += (velocity * speed * deltaTime);
                this.SpriteEffectIndex = 1;
            }
            if (!direction)
            {
                position -= (velocity * speed * deltaTime);
                this.SpriteEffectIndex = 0;
            }

            if (position.X >= 710)
            {
                direction = false;
            }
            if (position.X <= -710)
            {
                direction = true;
            }

        }

        /// <summary>
        /// Making an animation of the sprites
        /// </summary>
        /// <param name="gameTime">A GameTime</param>
        public override void Animation(GameTime gameTime)
        {
            //Adding the time which has passed since the last update
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            currentIndex = (int)(timeElapsed * fps);

            sprite = Sprites[currentIndex];

            //Restart the animation
            if (currentIndex >= Sprites.Length - 1)
            {
                timeElapsed = 0;
                currentIndex = 0;
            }
        }

        public float DistanceToPlayer(Vector2 playerPosition)
        {
            return Vector2.Distance(this.position, playerPosition);
        }

        #endregion
    }

}
