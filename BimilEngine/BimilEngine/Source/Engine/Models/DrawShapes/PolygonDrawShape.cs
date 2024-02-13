using Microsoft.Xna.Framework;

namespace BimilEngine.Source.Engine.Models.DrawShapes
{
    public class PolygonDrawShape : DrawShapeBase
    {
        public Vector2[] Vertices { get; set; }

        public PolygonDrawShape(Vector2[] vertices, Color color, float lineThickness = 1f, float layerDepth = 0f)
        {
            Vertices = vertices;
            Color = color;
            LineThickness = lineThickness;
            LayerDepth = layerDepth;
        }
    }
}