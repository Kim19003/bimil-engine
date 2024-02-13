using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BimilEngine.Source.Engine.Managers;
using BimilEngine.Source.Engine.Models;

namespace BimilEngine.Source.Engine
{
    public class Globals
    {
        public static GraphicsDeviceManager Graphics
        {
            get => _graphics;
            set
            {
                if (_graphics == null)
                    _graphics = value;
                else
                    LogManager.DoConsoleLog("GraphicsDeviceManager already set!", LogLevel.Error);
            }
        }
        private static GraphicsDeviceManager _graphics = null;
        public static ContentManager Content
        {
            get => _content;
            set
            {
                if (_content == null)
                    _content = value;
                else
                    LogManager.DoConsoleLog("ContentManager already set!", LogLevel.Error);
            }
        }
        private static ContentManager _content = null;
        public static SpriteBatch SpriteBatch
        {
            get => _spriteBatch;
            set
            {
                if (_spriteBatch == null)
                    _spriteBatch = value;
                else
                    LogManager.DoConsoleLog("SpriteBatch already set!", LogLevel.Error);
            }
        }
        private static SpriteBatch _spriteBatch = null;
        public static Texture2D PixelTexture
        {
            get => _pixelTexture;
            set
            {
                if (_pixelTexture == null)
                    _pixelTexture = value;
                else
                    LogManager.DoConsoleLog("Pixel already set!", LogLevel.Error);
            }
        }
        private static Texture2D _pixelTexture = null;
        public static Texture2D TransparentTexture
        {
            get => _transparentTexture;
            set
            {
                if (_transparentTexture == null)
                    _transparentTexture = value;
                else
                    LogManager.DoConsoleLog("TransparentTexture already set!", LogLevel.Error);
            }
        }
        private static Texture2D _transparentTexture = null;

        public static SpriteFont LogFont { get; set; }
    }
}