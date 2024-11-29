using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortensKomeback2
{
    internal class Enemy : Character
    {
        #region field
        private GraphicsDeviceManager graphics;
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
        public Enemy(GraphicsDeviceManager _graphics)
        {
            this.speed = 300;
            graphics = _graphics;
            this.health = 100;
            this.fps = 7f;
            this.Position = new Vector2(0, -300);
        }

        #endregion

        #region method

        public override void LoadContent(ContentManager content)
        {
        }

        public override void OnCollision(GameObject gameObject)
        {
            if (gameObject is Player)
            {
                //Load fight 
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (DistanceToPlayer(GameWorld.PlayerInstance.Position) <= 800f)
            {
                sprites = GameWorld.animationSprites["AggroGoose"];
            }
            else
            {
                sprites = GameWorld.animationSprites["WalkingGoose"];
            }
            this.Sprite = sprites[0];

            Movement(gameTime);
            Animation(gameTime);
        }
        public override void Movement(GameTime gameTime)
        {
            //Calculating the deltatime which is the time that has passed since the last frame
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            velocity = new Vector2(1, 0);

            //The player is moving by the result of HandleInput and deltaTime
            if (direction)
            {
                position += (velocity * speed * deltaTime);
                this.SpriteEffectIndex = 1;
            }
            else
            {
                position -= (velocity * speed * deltaTime);
                this.SpriteEffectIndex = 0;
            }

            if (position.X >= graphics.PreferredBackBufferWidth - Sprite.Width * 3.5f)
            {
                direction = false;
            }
            if (position.X <= -(graphics.PreferredBackBufferWidth - Sprite.Width * 3.5f))
            {
                direction = true;
            }

        }

        public override void Animation(GameTime gameTime)
        {
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

        public float DistanceToPlayer(Vector2 playerPosition)
        {
            return Vector2.Distance(this.position, playerPosition);
        }

        #endregion
    }

}
