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
        private bool topCollision;
        private bool bottomCollision;
        private bool leftCollision;
        private bool rightCollision;

        #endregion

        #region Properties

        public Rectangle LeftCollisionBox
        {
            get
            {
                if (leftCollision)
                {
                    return new Rectangle((int)Position.X - (sprite.Width / 2), (int)Position.Y - (sprite.Height / 2), 1, sprite.Height);
                }
                else
                    return new Rectangle();
            }
        }

        public Rectangle RightCollisionBox
        {
            get
            {
                if (rightCollision)
                {
                    return new Rectangle((int)Position.X + (sprite.Width / 2), (int)Position.Y - (sprite.Height / 2), 1, sprite.Height);
                }
                else
                    return new Rectangle();
            }
        }

        public Rectangle TopCollisionBox
        {
            get
            {
                if (topCollision)
                {
                    return new Rectangle((int)Position.X - (sprite.Width / 2), (int)Position.Y - (sprite.Height / 2), sprite.Width, 1);
                }
                else
                    return new Rectangle();
            }
        }

        public Rectangle BottomCollisionBox
        {
            get
            {
                if (bottomCollision)
                {
                    return new Rectangle((int)Position.X - (sprite.Width / 2), (int)Position.Y + (sprite.Height / 2), sprite.Width, 1);
                }
                else
                    return new Rectangle();
            }
        }

        #endregion

        #region Constructor
        public Area(Vector2 placement, int areaArray, string roomName )
        {
            this.position = placement;
            this.layer = 0.0f;
            this.scale = 1f;
            this.health = 1;
            //this.sprites = GameWorld.animationSprites["areaStart"];
            this.sprite = GameWorld.animationSprites["areaStart"][areaArray]; //sprites[areaArray];
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

        //public void ChangeRoom(string newRoom)
        //{
        //    if (RoomBoundaries.ContainsKey(newRoom))
        //    {
        //        CurrentRoomBoundary = RoomBoundaries[newRoom];
        //    }
        //}


        #endregion
    }
}
