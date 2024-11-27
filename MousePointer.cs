using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MortensKomeback2
{
    internal class MousePointer
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Returns a 1x1 pixel CollisionBox at the tip of the mousepointer
        /// </summary>
        public Rectangle CollisionBox
        {
            get { return new Rectangle((int)GameWorld.MousePosition.X, (int)GameWorld.MousePosition.Y, 1, 1); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a MousePointer with the intent of enabling "collision" with Button-class objects
        /// </summary>
        /// <param name="graphics">Needed for translating precise location of mouse in comparison to game</param>
        public MousePointer()
        {

        }

        #endregion

        #region Methods

        #endregion
    }
}