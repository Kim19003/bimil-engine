using Microsoft.Xna.Framework;

namespace Bimil.Engine.Models.DrawShapes
{
    public abstract class DrawShapeBase
    {
        public Color Color { get; set; }
        public float LineThickness { get; set; }
        public float LayerDepth { get; set; }
    }
}