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
        private bool direction = false;
        private float timeElapsed;
        private int curretIndex;

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
        }

        #endregion

        #region method

        public override void LoadContent(ContentManager content)
        {
            sprites = new Texture2D[8];

            for (int i = 0; i < 8; i++)
            {
                sprites[i] = content.Load<Texture2D>("Sprites\\Charactor\\gooseWalk" + i);
            }

            this.Sprite = sprites[0];
        }

        public override void OnCollision(GameObject gameObject)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
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
                position -= (velocity * speed * deltaTime);
            }
            else
            {
            position += (velocity * speed * deltaTime);
            }

            if (position.X >= graphics.PreferredBackBufferWidth - Sprite.Width*3.5f)
            {
                direction = true;
            }
            if (position.X <= -(graphics.PreferredBackBufferWidth - Sprite.Width*3.5f))
            {
                direction = false;
            }

        }

        public override void Animation(GameTime gameTime)
        {
            //Adding the time which has passed since the last update
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            curretIndex = (int)(timeElapsed * fps);

            sprite = sprites[curretIndex];

            //Restart the animation
            if (curretIndex >= sprites.Length - 1)
            {
                timeElapsed = 0;
                curretIndex = 0;
            }
        }

        #endregion
    }

}
