using System;
using Microsoft.Xna.Framework.Graphics;
using BimilEngine.Source.GameLogic;

namespace BimilEngine.Source.Engine.Handlers
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
                    Globals.Graphics.PreferredBackBufferWidth = value;
                    _width = value;
                }
            }
        }
        private int _width = Environment2D.DEFAULT_SCREEN_WIDTH;

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
                    Globals.Graphics.PreferredBackBufferHeight = value;
                    _height = value;
                }
            }
        }
        private int _height = Environment2D.DEFAULT_SCREEN_HEIGHT;

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
                Globals.Graphics.IsFullScreen = value;
                _isFullScreen = value;
            }
        }
        private bool _isFullScreen = Globals.Graphics.IsFullScreen;

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
                Globals.Graphics.SynchronizeWithVerticalRetrace = value;
                _useVsync = value;
            }
        }
        private bool _useVsync = Globals.Graphics.SynchronizeWithVerticalRetrace;

        /// <summary>
        /// Apply the changes to the screen.
        /// </summary>
        public void ApplyChanges()
        {
            Globals.Graphics.ApplyChanges();
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