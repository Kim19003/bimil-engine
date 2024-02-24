using Microsoft.Xna.Framework;

namespace BimilEngine.Source.Engine.Models
{
    public class ShadowSettings
    {
        public Vector2 Offset { get; set; } = new Vector2(2, 2);
        public Color Color { get; set; } = Color.Black;

        public ShadowSettings()
        {

        }

        public ShadowSettings(Vector2 offset, Color color)
        {
            Offset = offset;
            Color = color;
        }
    }
}