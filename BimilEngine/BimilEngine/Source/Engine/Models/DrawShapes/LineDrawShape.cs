using Microsoft.Xna.Framework;

namespace BimilEngine.Source.Engine.Models.DrawShapes
{
    public class LineDrawShape : DrawShapeBase
    {
        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }

        public LineDrawShape(Vector2 start, Vector2 end, Color color, float lineThickness = 1f, float layerDepth = 0f)
        {
            Start = start;
            End = end;
            Color = color;
            LineThickness = lineThickness;
            LayerDepth = layerDepth;
        }
    }
}