using System;
using Microsoft.Xna.Framework.Graphics;

namespace Bimil.Engine.Handlers
{
    public sealed class ScreenHandler
    {
        /// <summary>
        /// The max frames per second. 0 = unlimited.
        /// </summary>
        public int MaxFramesPerSecond
        {
            get
            {
                return _maxFramesPerSecond;
            }
            set
            {
                if (value > 0)
                    _maxFramesPerSecond = value;
                else
                    _maxFramesPerSecond = 0;
            }
        }
        private int _maxFramesPerSecond = 60;

        /// <summary>
        /// The frames per second. Use UpdateFramesPerSecond() in Draw() to update this value.
        /// </summary>
        public double FramesPerSecond
        {
            get
            {
                return _framesPerSecond;
            }
        }
        private double _framesPerSecond = 0;

        /// <summary>
        /// The frames per second rounded as integer.
        /// </summary>
        public int FramesPerSecondInt
        {
            get
            {
                return Convert.ToInt32(_framesPerSecond);
            }
        }

        /// <summary>
        /// Screen viewport.
        /// </summary>
        public Viewport Viewport => new(0, 0, Width, Height);

        /// <summary>
        /// Screen width.
        /// </summary>
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (value > 0)
                {
                    Root.Graphics.PreferredBackBufferWidth = value;
                    _width = value;
                }
            }
        }
        private int _width = 1280;

        /// <summary>
        /// Screen height.
        /// </summary>
        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (value > 0)
                {
                    Root.Graphics.PreferredBackBufferHeight = value;
                    _height = value;
                }
            }
        }
        private int _height = 720;

        /// <summary>
        /// Is the screen fullscreen?
        /// </summary>
        public bool IsFullScreen
        {
            get
            {
                return _isFullScreen;
            }
            set
            {
                Root.Graphics.IsFullScreen = value;
                _isFullScreen = value;
            }
        }
        private bool _isFullScreen = false;

        /// <summary>
        /// Use Vsync? Note: Vsync may cause lag spikes.
        /// </summary>
        public bool UseVsync
        {
            get
            {
                return _useVsync;
            }
            set
            {
                Root.Graphics.SynchronizeWithVerticalRetrace = value;
                _useVsync = value;
            }
        }
        private bool _useVsync = true;

        /// <summary>
        /// Apply the changes to the screen.
        /// </summary>
        public void ApplyChanges()
        {
            Root.Graphics.ApplyChanges();
        }

        /// <summary>
        /// Update the frames per second. This method should be called in the Draw() method.
        /// </summary>        
        public void UpdateFramesPerSecond(double fps)
        {
            _framesPerSecond = fps;
        }
    }
}