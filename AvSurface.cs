using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortensKomeback2
{
    internal class AvSurface : Obstacle
    {
        #region field
        private float timeElapsed;
        private int currentIndex;
        #endregion

        #region properti

        #endregion

        #region constructor
        public AvSurface(int xPosition, int yPosition)
        {
            this.fps = 1f;
            this.position = new Vector2(xPosition, yPosition);
        }
        #endregion

        #region method
        public override void LoadContent(ContentManager content)
        {
            sprites = GameWorld.animationSprites["firepit"];
            
            this.Sprite = sprites[0];
        }

        public override void Update(GameTime gameTime)
        {
            Animation(gameTime);
        }

        public void Animation(GameTime gameTime)
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

        #endregion
    }
}
