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
        private int edges = 110;

        #endregion

        #region Properties

        public string Room { get => room; }

        public Rectangle LeftCollisionBox
        {
            get
            {
                return new Rectangle((int)Position.X - (sprite.Width / 2) + edges + 15, (int)Position.Y - (sprite.Height / 2), 1, sprite.Height);
            }
        }

        public Rectangle RightCollisionBox
        {
            get
            {
                return new Rectangle((int)Position.X + (sprite.Width / 2) - edges - 15, (int)Position.Y - (sprite.Height / 2), 1, sprite.Height);
            }
        }

        public Rectangle TopCollisionBox
        {
            get
            {
                if (topCollision)
                {
                    return new Rectangle((int)Position.X - (sprite.Width / 2) + edges + 15, (int)Position.Y - (sprite.Height / 2) + edges, sprite.Width - (edges * 2) - 30, 1);
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
                    return new Rectangle((int)Position.X - (sprite.Width / 2) + edges + 15, (int)Position.Y + (sprite.Height / 2) - edges, sprite.Width - (edges * 2) - 30, 1);
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
                case "Room1":
                    topCollision = true;
                    break;
                case "Room1a":
                case "Room1b":
                    break;
                case "Room1c":
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


        public override void Update(GameTime gameTime)
        {
            if (room == GameWorld.PlayerInstance.InRoom)
                if (GameWorld.PlayerInstance.Position.Y < position.Y && topCollision || GameWorld.PlayerInstance.Position.Y > position.Y && bottomCollision)
                    GameWorld.Camera.Position = position;
                else
                    GameWorld.Camera.Position = new Vector2(position.X, GameWorld.PlayerInstance.Position.Y);
        }

        //public void ChangeRoom(string newRoom)
        //{
        //    if (RoomBoundaries.ContainsKey(newRoom))
        //    {
        //        CurrentRoomBoundary = RoomBoundaries[newRoom];
        //    }
        //}

        /// <summary>
        /// Runs a method to check for several collisions at the same time and exports the results to another function if any collisions return true
        /// </summary>
        /// <param name="gameObject">Object to check for collisions with</param>
        public override void CheckCollision(GameObject gameObject)
        {
            bool top;
            bool bottom;
            bool left;
            bool right;
            Collisions(gameObject, out top, out bottom, out left, out right);
            if (top || bottom || left || right)
            {
                Collided(gameObject, top, bottom, left, right);
            }
        }

        /// <summary>
        /// Checks for up to 4 different collisions simultanoiusly
        /// </summary>
        /// <param name="gameObject">Object to check for collisions with</param>
        /// <param name="top">Returns true if TopCollisionBox is collided with</param>
        /// <param name="bottom">Returns true if BottomCollisionBox is collided with</param>
        /// <param name="left">Returns true if LeftCollisionBox is collided with</param>
        /// <param name="right">Returns true if RightCollisionBox is collided with</param>
        private void Collisions(GameObject gameObject, out bool top, out bool bottom, out bool left, out bool right)
        {
            top = false;
            bottom = false;
            left = false;
            right = false;

            if (topCollision && TopCollisionBox.Intersects(gameObject.CollisionBox))
            {
                top = true;
            }
            if (bottomCollision && BottomCollisionBox.Intersects(gameObject.CollisionBox))
            {
                bottom = true;
            }
            if (LeftCollisionBox.Intersects(gameObject.CollisionBox))
            {
                left = true;
            }
            if (RightCollisionBox.Intersects(gameObject.CollisionBox))
            {
                right = true;
            }
        }

        /// <summary>
        /// Determines actions upon collision(s)
        /// </summary>
        /// <param name="gameObject">Object to manipulate</param>
        /// <param name="top">Enforces certain parameters if TopCollisionBox was collided with</param>
        /// <param name="bottom">Enforces certain parameters if BottomCollisionBox was collided with</param>
        /// <param name="left">Enforces certain parameters if LeftCollisionBox was collided with</param>
        /// <param name="right">Enforces certain parameters if RightCollisionBox was collided with</param>
        private void Collided(GameObject gameObject, bool top, bool bottom, bool left, bool right)
        {
            if (bottom)
            {
                (gameObject as Player).Velocity = new Vector2((gameObject as Player).Velocity.X, (gameObject as Player).Velocity.Y - 1);
                gameObject.Position = new Vector2(gameObject.Position.X, gameObject.Position.Y - 1);
            }
            if (left)
            {
                (gameObject as Player).Velocity = new Vector2(0, (gameObject as Player).Velocity.Y);
                gameObject.Position = new Vector2(gameObject.Position.X + 1, gameObject.Position.Y);
            }
            if (right)
            {
                (gameObject as Player).Velocity = new Vector2((gameObject as Player).Velocity.X - 1, (gameObject as Player).Velocity.Y);
                gameObject.Position = new Vector2(gameObject.Position.X - 1, gameObject.Position.Y);
            }
            if (top)
            {
                (gameObject as Player).Velocity = new Vector2((gameObject as Player).Velocity.X, (gameObject as Player).Velocity.Y + 1);
                gameObject.Position = new Vector2(gameObject.Position.X, gameObject.Position.Y + 1);
            }
        }


        public override void OnCollision(GameObject gameObject)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
