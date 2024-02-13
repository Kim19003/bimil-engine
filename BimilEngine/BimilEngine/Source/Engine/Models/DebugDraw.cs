using BimilEngine.Source.Engine.Models.DrawShapes;
using Microsoft.Xna.Framework;

namespace BimilEngine.Source.Engine.Models
{
    public class DebugDraw
    {
        /// <summary>
        /// The object to draw.
        /// </summary>
        public object Object { get; set; }
        /// <summary>
        /// The color of the debug draw. If the object is a draw shape, it's color will be used instead.
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// How long the debug draw should be visible for, in milliseconds. 0 = forever.
        /// </summary>
        public int LifeTime
        {
            get
            {
                return _lifeTime;
            }
            set
            {
                if (value > 0)
                    _lifeTime = value;
                else
                    _lifeTime = 0;
            }
        }
        private int _lifeTime = 0;
        /// <summary>
        /// Camera level of the debug draw.
        /// </summary>
        public int CameraLevel
        {
            get
            {
                return _cameraLevel;
            }
            set
            {
                if (value > -1)
                    _cameraLevel = value;
                else
                    _cameraLevel = -1;
            }
        }
        private int _cameraLevel = -1;
        /// <summary>
        /// The line thickness of the debug draw. If the object is a draw shape, it's line thickness will be used instead.
        /// </summary>
        public float LineThickness
        {
            get
            {
                return _lineThickness;
            }
            set
            {
                if (value > 0f)
                    _lineThickness = value;
                else
                    _lineThickness = 0f;
            }
        }
        private float _lineThickness = 0f;

        public DebugDraw(object @object, Color color, int lifeTime = 0, int cameraLevel = -1, float lineThickness = 1f)
        {
            Initializer(@object, color, lifeTime, cameraLevel, lineThickness);
        }

        public DebugDraw(RectangleDrawShape rectangleDrawShape, int lifeTime = 0, int cameraLevel = -1)
        {
            Initializer(rectangleDrawShape, lifeTime: lifeTime, cameraLevel: cameraLevel);
        }

        public DebugDraw(CircleDrawShape circleDrawShape, int lifeTime = 0, int cameraLevel = -1)
        {
            Initializer(circleDrawShape, lifeTime: lifeTime, cameraLevel: cameraLevel);
        }

        public DebugDraw(PolygonDrawShape polygonDrawShape, int lifeTime = 0, int cameraLevel = -1)
        {
            Initializer(polygonDrawShape, lifeTime: lifeTime, cameraLevel: cameraLevel);
        }

        public DebugDraw(LineDrawShape lineDrawShape, int lifeTime = 0, int cameraLevel = -1)
        {
            Initializer(lineDrawShape, lifeTime: lifeTime, cameraLevel: cameraLevel);
        }

        private void Initializer(object @object = null, Color color = default, int lifeTime = 0, int cameraLevel = -1, float lineThickness = 1f)
        {
            Object = @object;
            Color = color;
            LifeTime = lifeTime;
            CameraLevel = cameraLevel;
            LineThickness = lineThickness;
        }
    }
}