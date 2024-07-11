using Microsoft.Xna.Framework;

namespace Bimil.Engine.Objects.Bases
{
    public abstract class Transform2D
    {
        /// <summary>
        /// Name of the transform.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Tag of the transform.
        /// </summary>
        public string Tag { get; set; } = string.Empty;
        /// <summary>
        /// Position of the transform.
        /// </summary>
        public Vector2 Position { get; set; } = Vector2.Zero;
        /// <summary>
        /// Scale of the transform.
        /// </summary>
        public Vector2 Scale { get; set; } = Vector2.One;
        /// <summary>
        /// Rotation of the transform.
        /// </summary>
        public float Rotation { get; set; } = 0f;
        /// <summary>
        /// Camera level of the transform.
        /// </summary>
        public int CameraLevel
        {
            get
            {
                return _cameraLevel;
            }
            set
            {
                if (value > 0)
                    _cameraLevel = value;
                else
                    _cameraLevel = 0;
            }
        }
        private int _cameraLevel = 0;

        /// <summary>
        /// Scene associated with the transform.
        /// </summary>
        public Scene2D AssociatedScene { get; set; } = null;

        public Transform2D()
        {
        }

        public Transform2D(Scene2D associatedScene = null)
        {
            AssociatedScene = associatedScene;
        }
    }
}