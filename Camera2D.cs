﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MortensKomeback2
{
    public class Camera2D
    {
        #region Fields

        private Vector2 position = new Vector2();
        private readonly GraphicsDevice _graphicsDevice;

        #endregion

        #region Properties

        /// <summary>
        /// Property to access the position of the object
        /// </summary>
        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        /// <summary>
        /// Used to set the zoomlevel of the viewport (scaled float)
        /// </summary>
        public float Zoom { get; set; }

        /// <summary>
        /// Used to rotate the camera
        /// </summary>
        public float Rotation { get; set; }

        public Vector2 ScreenSize;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for the camera viewport
        /// </summary>
        /// <param name="graphicsDevice">Defines which graphicsdevice to get viewport parameters from</param>
        /// <param name="position">Defines starting position of the viewport</param>
        public Camera2D(GraphicsDevice graphicsDevice, Vector2 position)
        {
            _graphicsDevice = graphicsDevice;
            Position = position;
            Zoom = 1f;
            Rotation = 0.0f;
            ScreenSize = new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Transforms the cameraviewport to return value
        /// </summary>
        /// <returns>Center of screen location</returns>
        public Matrix GetTransformation()
        {
            var screenCenter = new Vector3(_graphicsDevice.Viewport.Width / 2f, _graphicsDevice.Viewport.Height / 2f, 0);
            float scale = MathHelper.Min((_graphicsDevice.Viewport.Width / ScreenSize.X),(_graphicsDevice.Viewport.Height / ScreenSize.Y));
            return Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(Zoom * scale, Zoom * scale, 1) *
                   Matrix.CreateTranslation(screenCenter);
        }

        public Matrix InverseTransformation()
        {
            return Matrix.Invert(GetTransformation());
        }

        #endregion
    }
}
