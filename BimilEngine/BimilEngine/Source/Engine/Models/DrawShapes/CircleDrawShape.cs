using BimilEngine.Source.Engine.Interfaces;
using Microsoft.Xna.Framework;

namespace BimilEngine.Source.Engine.Models.DrawShapes
{
    public class CircleDrawShape : DrawShapeBase
    {
        public Circle Body { get; set; }

        public CircleDrawShape(Circle body, Color color, float lineThickness = 1f, float layerDepth = 0f)
        {
            Body = body;
            Color = color;
            LineThickness = lineThickness;
            LayerDepth = layerDepth;
        }
    }
}