using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

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
        public Obstacle(int xPosition, int yPosition, string obstacleType)
        {
            this.position = new Vector2(xPosition, yPosition);
            switch (obstacleType.ToLower())
            {
                case "stone":
                    this.Sprite = GameWorld.commonSprites["stone"];
                    break;
                case "egg":
                    this.Sprite = GameWorld.commonSprites["egg"];
                    break;
                case "magic":
                    this.Sprite = GameWorld.commonSprites["magic"];
                    break;
                case "magicHeal":
                    this.Sprite = GameWorld.commonSprites["magicHeal"];
                    break;
                default:
                    this.Sprite = GameWorld.commonSprites["hole"];
                    break;


            }
        }

        #endregion

        #region method
        public override void LoadContent(ContentManager content)
        {
        }

        public override void OnCollision(GameObject gameObject)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        #endregion
    }
}
