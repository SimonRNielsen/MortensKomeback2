using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortensKomeback2
{
    internal class Button : Menu
    {
        #region Fields

        private Color[] buttonColor = new Color[3] { Color.DarkRed, Color.Yellow, Color.White };
        private Color[] backgroundColor = new Color[2] { Color.White, Color.Gray };
        private int buttonColorIndex;
        private int backgroundColorIndex;
        private int buttonID;
        private bool collision = false;
        private string buttonText;
        private float textXDisplacement;

        #endregion

        #region Properties

        public override Rectangle CollisionBox
        {
            get { return new Rectangle((int)Position.X - (sprite.Width / 2), (int)Position.Y - (sprite.Height / 2), sprite.Width, sprite.Height); }
        }

        #endregion

        #region Constructor

        public Button(Vector2 spawnPosition, int buttonType)
        {
            sprite = GameWorld.commonSprites["button"];
            position = spawnPosition;
            layer = 0.998f;
            buttonID = buttonType;
            switch (buttonID)
            {
                case 0:
                    buttonText = "Close";
                    textXDisplacement = -7;
                    break;
            }
        }

        #endregion

        #region Methods


        public override void LoadContent(ContentManager content)
        {
            //throw new NotImplementedException();
        }

        public override void OnCollision(GameObject gameObject)
        {
            collision = true;
            switch (buttonID)
            {
                case 0:
                    if (GameWorld.LeftMouseButtonClick)
                        GameWorld.CloseMenu = true;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }


        public override void Update(GameTime gameTime)
        {
            switch (buttonID)
            {
                case 0:
                    if (collision)
                        buttonColorIndex = 1;
                    else
                        buttonColorIndex = 0;
                    break;
            }
            collision = false;
        }


        public void CheckCollision(MousePointer mousePointer)
        {
            if (CollisionBox.Intersects(mousePointer.CollisionBox))
            {
                OnCollision(this);
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Position, null, backgroundColor[backgroundColorIndex], rotation, new Vector2(sprite.Width / 2, sprite.Height / 2), scale, SpriteEffects.None, layer);
            spriteBatch.DrawString(GameWorld.mortensKomebackFont, buttonText, new Vector2(Position.X + textXDisplacement, Position.Y), buttonColor[buttonColorIndex], 0f, new Vector2(18, 8), 2f, SpriteEffects.None, 0.999f);
        }

        #endregion
    }
}
