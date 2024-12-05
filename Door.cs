using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortensKomeback2
{
    internal class Door : GameObject
    {
        #region field
        private DoorTypes type;
        private bool doOnce = false;
        private List<Area> myAreas;

        private readonly Vector2 teleportPosition;
        #endregion

        #region properties
        internal DoorTypes Type { get => type; set => type = value; }

        #endregion

        #region constructor
        public Door(float xPosition, float yPosition, DoorTypes dt, Vector2 teleportPosition, List<Area> areaList)
        {
            this.position.X = xPosition;
            this.position.Y = yPosition;
            this.Type = dt;
            this.teleportPosition = teleportPosition;
            this.layer = 0.2f;
            myAreas = areaList;
        }


        #endregion

        #region method
        public override void LoadContent(ContentManager content)
        {
            this.sprites = GameWorld.animationSprites["doorStart"];
            switch (Type)
            {
                case DoorTypes.Open:
                    this.Sprite = sprites[1];
                    break;
                case DoorTypes.Closed:
                    this.Sprite = sprites[0];
                    break;
                case DoorTypes.Locked:
                    this.Sprite = sprites[2];
                    break;
            }
        }

        /// <summary>
        /// Collision between the door and player
        /// </summary>
        /// <param name="gameObject">A gameObject</param>
        public override void OnCollision(GameObject gameObject)
        {
            Unlock();
            Open();
            // does not behave correctly when placed on the side
            if ((this.CollisionBox.Center.X >= gameObject.CollisionBox.Left && this.CollisionBox.Center.X <= gameObject.CollisionBox.Right) &&
                   (this.CollisionBox.Center.Y >= gameObject.CollisionBox.Top && this.CollisionBox.Center.Y <= gameObject.CollisionBox.Bottom))
                Teleport(GameWorld.PlayerInstance);
        }

        public override void Update(GameTime gameTime)
        {
            switch (Type)
            {
                case DoorTypes.Open:
                    this.Sprite = sprites[1];
                    break;
                case DoorTypes.Closed:
                    this.Sprite = sprites[0];
                    break;
                case DoorTypes.Locked:
                    this.Sprite = sprites[2];
                    break;
            }
            if (myAreas != null && !doOnce)
            {
                doOnce = true;
                float maxDistance = 2000;
                Area myRoom = null;
                foreach (Area area in myAreas)
                {
                    float distance = Vector2.Distance(position, area.Position);
                    if (distance < maxDistance)
                    {
                        myRoom = area;
                        maxDistance = distance;
                    }
                }
                if (position.Y < (myRoom.Position.Y - 300))
                {
                    //Normal
                }
                else if (position.Y > (myRoom.Position.Y + 300))
                {
                    spriteEffectIndex = 2;
                }
                else if (position.X < (myRoom.Position.X - 500))
                {
                    rotation = (((22f / 7f) / 2f) * 3f);
                    spriteEffectIndex = 1;
                }
                else if (position.X > (myRoom.Position.X + 500))
                {
                    rotation = (((22f / 7f) / 2f));
                }
            }
        }

        /// <summary>
        /// Opening a door that is either closed or unlocked
        /// </summary>
        public void Open()
        {
            if (type == DoorTypes.Closed)
            {
                Type = DoorTypes.Open;
            }
        }

        /// <summary>
        /// Unlocking a door if the player has a key
        /// </summary>
        public void Unlock()
        {
            if( type == DoorTypes.Locked)
            {
                Item key = Player.FindKey();
                if (key == null)
                {
                    return;
                }
                Type = DoorTypes.Open;
                Player.RemoveItem(key); //Remove key when used
            }
        }

        /// <summary>
        /// Teleporting the player and camera when the player is teleporting to another room
        /// </summary>
        /// <param name="player">Player</param>
        public void Teleport(Player player)
        {
            player.Position = this.teleportPosition;
        }
        #endregion
    }
}
