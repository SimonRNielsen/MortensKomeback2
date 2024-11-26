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

        #endregion

        #region properti

        #endregion

        #region constructor
        public Enemy(GraphicsDeviceManager _graphics)
        {
            this.speed = 300;
            graphics = _graphics;
        }

        #endregion

        #region method

        public override void LoadContent(ContentManager content)
        {
            this.Sprite = content.Load<Texture2D>("Sprites\\Charactor\\goose0"); //Only a test sprite of Morten
        }

        public override void OnCollision(GameObject gameObject)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            Movement(gameTime);
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

            if (position.X >= graphics.PreferredBackBufferWidth - (Sprite.Width*2))
            {
                direction = true;
            }
            if (position.X <= -(graphics.PreferredBackBufferWidth - (Sprite.Width * 2)))
            {
                direction = false;
            }

            //if (position.X > graphics.PreferredBackBufferWidth / 2 && sw)
            //{
            //    position -= (velocity * speed * deltaTime);
            //    if (position.X <= (graphics.PreferredBackBufferWidth / 2))
            //    { sw = false; }
            //}
            //else if (position.X > -(graphics.PreferredBackBufferWidth / 2) && !sw)
            //{
            //    position += (velocity * speed * deltaTime);
            //    if (position.X <= -(graphics.PreferredBackBufferWidth / 2))
            //    { sw = true; }
            //}
        }

        public override void Animation(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
