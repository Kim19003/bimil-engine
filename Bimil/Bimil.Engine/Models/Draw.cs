using Bimil.Engine.Models.DrawShapes;
using Microsoft.Xna.Framework;

namespace Bimil.Engine.Models
{
    public class Draw
    {
        /// <summary>
        /// The object to draw.
        /// </summary>
        public object Object { get; set; }
        /// <summary>
        /// The color of the draw. If the object is a draw shape, it's color will be used instead.
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// How long the draw should be visible for, in milliseconds. 0 = forever.
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
        /// Camera level of the draw.
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
        /// The line thickness of the draw. If the object is a draw shape, it's line thickness will be used instead.
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

        public Draw(object @object, Color color, int lifeTime = 0, int cameraLevel = -1, float lineThickness = 1f)
        {
            Initializer(@object, color, lifeTime, cameraLevel, lineThickness);
        }

        public Draw(RectangleDrawShape rectangleDrawShape, int lifeTime = 0, int cameraLevel = -1)
        {
            Initializer(rectangleDrawShape, lifeTime: lifeTime, cameraLevel: cameraLevel);
        }

        public Draw(CircleDrawShape circleDrawShape, int lifeTime = 0, int cameraLevel = -1)
        {
            Initializer(circleDrawShape, lifeTime: lifeTime, cameraLevel: cameraLevel);
        }

        public Draw(PolygonDrawShape polygonDrawShape, int lifeTime = 0, int cameraLevel = -1)
        {
            Initializer(polygonDrawShape, lifeTime: lifeTime, cameraLevel: cameraLevel);
        }

        public Draw(LineDrawShape lineDrawShape, int lifeTime = 0, int cameraLevel = -1)
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