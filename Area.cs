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
        private string room;

        #endregion

        #region Properties

        public Rectangle LeftCollisionBox
        {
            get
            {
                return new Rectangle((int)Position.X - (sprite.Width / 2), (int)Position.Y - (sprite.Height / 2), 1, sprite.Height);
            }
        }

        public Rectangle RightCollisionBox
        {
            get
            {
                return new Rectangle((int)Position.X + (sprite.Width / 2), (int)Position.Y - (sprite.Height / 2), 1, sprite.Height);
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
        public Area(Vector2 placement, int areaArray, string roomName)
        {
            this.position = placement;
            this.layer = 0.0f;
            this.scale = 1f;
            this.health = 1;
            room = roomName;
            //this.sprites = GameWorld.animationSprites["areaStart"];
            this.sprite = GameWorld.animationSprites["areaStart"][areaArray]; //sprites[areaArray];
            switch (room)
            {
                case "a":
                    topCollision = true;
                    break;
                case "b":
                    break;
                case "c":
                    bottomCollision = true;
                    break;
                case "d":
                    topCollision = true;
                    bottomCollision = true;
                    break;
                default:
                    topCollision = true;
                    bottomCollision = true;
                    break;
            }
        }

        #endregion

        #region Method
        public override void LoadContent(ContentManager content)
        {

        }

        public override void OnCollision(GameObject gameObject)
        {
            if (BottomCollisionBox.Intersects(gameObject.CollisionBox))
            {
                (gameObject as Player).Velocity = new Vector2((gameObject as Player).Velocity.X, (gameObject as Player).Velocity.Y - 1);
            }
            else if (LeftCollisionBox.Intersects(gameObject.CollisionBox))
            {
                (gameObject as Player).Velocity = new Vector2((gameObject as Player).Velocity.X + 1, (gameObject as Player).Velocity.Y);
            }
            else if (RightCollisionBox.Intersects(gameObject.CollisionBox))
            {
                (gameObject as Player).Velocity = new Vector2((gameObject as Player).Velocity.X - 1, (gameObject as Player).Velocity.Y);
            }
            else if (TopCollisionBox.Intersects(gameObject.CollisionBox))
            {
                (gameObject as Player).Velocity = new Vector2((gameObject as Player).Velocity.X, (gameObject as Player).Velocity.Y + 1);
            }
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

        public override void CheckCollision(GameObject gameObject)
        {

            //if ((gameObject as Player).InRoom == room)
            if (topCollision && bottomCollision)
            {
                if (BottomCollisionBox.Intersects(gameObject.CollisionBox) || LeftCollisionBox.Intersects(gameObject.CollisionBox) || RightCollisionBox.Intersects(gameObject.CollisionBox) || TopCollisionBox.Intersects(gameObject.CollisionBox))
                {
                    OnCollision(gameObject);
                }
            }
            else if (topCollision)
            {
                if (TopCollisionBox.Intersects(gameObject.CollisionBox) || LeftCollisionBox.Intersects(gameObject.CollisionBox) || RightCollisionBox.Intersects(gameObject.CollisionBox))
                {
                    OnCollision(gameObject);
                }
            }
            else if (bottomCollision)
            {
                if (BottomCollisionBox.Intersects(gameObject.CollisionBox) || LeftCollisionBox.Intersects(gameObject.CollisionBox) || RightCollisionBox.Intersects(gameObject.CollisionBox))
                {
                    OnCollision(gameObject);
                }
            }
            else
            {
                if (LeftCollisionBox.Intersects(gameObject.CollisionBox) || RightCollisionBox.Intersects(gameObject.CollisionBox))
                {
                    OnCollision(gameObject);
                }
            }
        }

        #endregion
    }
}
