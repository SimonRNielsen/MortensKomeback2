using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortensKomeback2
{
    internal class Area : GameObject
    {
        #region fields
        //private int spriteID; //Which sprite is going to be used

        #endregion

        #region Properties

        #endregion

        #region Constructor
        public Area(Vector2 placement, int areaArray /*int xPosition, int yPosition*/)
        {
            this.position = placement;
            this.layer = 0f;
            this.scale = 1f;
            /*this.position.X = xPosition;
            this.position.Y = yPosition;*/
            this.health = 1;
            this.sprites = GameWorld.animationSprites["areaStart"];
            this.sprite = sprites[areaArray];
            //this.spriteID = spriteID;
        }

        #endregion

        #region Method
        public override void LoadContent(ContentManager content)
        {
            
        }

        public override void OnCollision(GameObject gameObject)
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            
        }
        #endregion
    }
    //internal class Area
    //{
    //}
    //internal class Area : GameObject
    //{
    //    #region fields
    //    //private int spriteID; //Which sprite is going to be used

    //    #endregion

    //    #region Properties

    //    #endregion

    //    #region Constructor
    //    public Area(int areaArray, int xPosition, int yPosition)
    //    {
    //        this.layer = 0f;
    //        this.scale = 1f;
    //        this.position.X = xPosition;
    //        this.position.Y = yPosition;
    //        this.health = 1;
    //        this.sprites = GameWorld.animationSprites["areaStart"];
    //        this.sprite = sprites[areaArray];
    //        //this.spriteID = spriteID;
    //    }

    //    #endregion

    //    #region Method
    //    public override void LoadContent(ContentManager content)
    //    {
            
    //    }

    //    public override void OnCollision(GameObject gameObject)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void Update(GameTime gameTime)
    //    {
            
    //    }
    //    #endregion
    //}
}
