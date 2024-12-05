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
        private DoorRotation doorRotation;

        private readonly Vector2 teleportPosition;
        #endregion

        #region properties
        internal DoorTypes Type { get => type; set => type = value; }
        internal DoorRotation DoorRotation { get => doorRotation; set => doorRotation = value; }

        #endregion

        #region constructor
        public Door(float xPosition, float yPosition, DoorTypes dt, DoorRotation dr, Vector2 teleportPosition)
        {
            this.position.X = xPosition;
            this.position.Y = yPosition;
            this.Type = dt;
            this.DoorRotation = dr;
            this.teleportPosition = teleportPosition;
            this.layer = 0.2f;
            
        }


        #endregion

        #region method
        public override void LoadContent(ContentManager content)
        {
            this.sprites = GameWorld.animationSprites["doorStart"];
            
            //Switch for the different kind of sprites to DoorTypes
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
                case DoorTypes.Secret:
                    this.Sprite = sprites[3];
                        break;

            }

            //Switch for the different kind of DoorRotation to make sure the sprite will fit correct up against the wall 
            switch (DoorRotation)
            {
                case DoorRotation.Top:
                    this.rotation = 0;
                    break;
                case DoorRotation.Bottom:
                    this.rotation = 600f;
                    break;
                case DoorRotation.Left:
                    this.rotation = 300f;
                    break;
                case DoorRotation.Right:
                    this.rotation = 900f;
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
                   (this.CollisionBox.Center.Y >= gameObject.CollisionBox.Top && this.CollisionBox.Center.Y <= gameObject.CollisionBox.Bottom) && Type == DoorTypes.Open)
                Teleport(GameWorld.PlayerInstance);
            if ((this.CollisionBox.Center.X >= gameObject.CollisionBox.Left && this.CollisionBox.Center.X <= gameObject.CollisionBox.Right) &&
                   (this.CollisionBox.Center.Y >= gameObject.CollisionBox.Top && this.CollisionBox.Center.Y <= gameObject.CollisionBox.Bottom) && Type == DoorTypes.Secret)
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
                case DoorTypes.Secret:
                    this.Sprite = sprites[3];
                    break;
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
                (key as QuestItem).IsUsed = true; //Remove key when used
            }
        }

        /// <summary>
        /// Teleporting the player and camera when the player is teleporting to another room
        /// </summary>
        /// <param name="player">Player</param>
        public void Teleport(Player player)
        {
            player.Position = this.teleportPosition;
            //GameWorld.Camera.Position = this.teleportPosition;
        }
        #endregion
    }
}
