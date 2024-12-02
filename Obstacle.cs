using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortensKomeback2
{
    internal class Obstacle : GameObject
    {
        #region field

        #endregion

        #region properties

        #endregion

        #region constructor
        /// <summary>
        /// The construction of AvSurface where it's placed by enteret input
        /// </summary>
        /// <param name="xPosition">X position</param>
        /// <param name="yPosition">Y position</param>
        public Obstacle(int xPosition, int yPosition)
        {
            this.position = new Vector2(xPosition, yPosition);
        }

        #endregion

        #region method
        public override void LoadContent(ContentManager content)
        {
            this.Sprite = GameWorld.commonSprites["stone"];
        }

        public override void OnCollision(GameObject gameObject)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
