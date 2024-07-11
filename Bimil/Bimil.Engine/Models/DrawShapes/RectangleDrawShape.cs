using Microsoft.Xna.Framework;

namespace Bimil.Engine.Models.DrawShapes
{
    public class RectangleDrawShape : DrawShapeBase
    {
        public Rectangle Body { get; set; }

        public RectangleDrawShape(Rectangle body, Color color, float lineThickness = 1f, float layerDepth = 0f)
        {
            Body = body;
            Color = color;
            LineThickness = lineThickness;
            LayerDepth = layerDepth;
        }
    }
}