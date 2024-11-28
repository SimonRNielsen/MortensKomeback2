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
        /// <summary>
        /// The construction of AvSurface where it's placed by enteret input
        /// </summary>
        /// <param name="xPosition">X position</param>
        /// <param name="yPosition">Y position</param>
        public AvSurface(int xPosition, int yPosition) : base(xPosition, yPosition)
        {
            this.fps = 15;
            this.position = new Vector2(xPosition, yPosition);
            
            sprites = GameWorld.animationSprites["firepit"];
            this.Sprite = sprites[0]; 
        }
        #endregion

        #region method
        public override void LoadContent(ContentManager content)
        {
        }

        public override void Update(GameTime gameTime)
        {
            Animation(gameTime);
        }

        /// <summary>
        /// Making an animation of the sprites
        /// </summary>
        /// <param name="gameTime">A GameTime</param>
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
