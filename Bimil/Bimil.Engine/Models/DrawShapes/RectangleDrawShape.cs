using Microsoft.Xna.Framework;

namespace Bimil.Engine.Models.DrawShapes
{
    public class RectangleDrawShape : DrawShapeBase
    {
        public Rectangle Body { get; set; }
        public float Radius
        {
            get => _radius;
            set => _radius = value < 0 ? 0 : value;
        }
        private float _radius = 0f;

        public RectangleDrawShape(Rectangle body, Color color, float radius = 0f, float lineThickness = 1f, float layerDepth = 0f)
        {
            Body = body;
            Color = color;
            Radius = radius;
            LineThickness = lineThickness;
            LayerDepth = layerDepth;
        }
    }
}