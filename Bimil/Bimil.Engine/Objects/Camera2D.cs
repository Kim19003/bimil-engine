using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bimil.Engine.Objects.Bases;
using Bimil.Engine.Other;

namespace Bimil.Engine.Objects
{
    public class Camera2D : Gadget2D
    {
        public float Depth { get; set; } = 1.0f;

        public Vector2 MatrixPosition { get; set; } = Vector2.Zero;
        public Matrix Matrix
        {
            get
            {
                Viewport viewport = Viewport;

                return Matrix.CreateTranslation(new Vector3(-MatrixPosition.X, -MatrixPosition.Y, 0)) *
                    Matrix.CreateScale(new Vector3(Depth, Depth, 1f)) *
                    Matrix.CreateTranslation(new Vector3(viewport.Width * 0.5f, viewport.Height * 0.5f, 0));
            }
        }

        public Rectangle WorldPointBounds => Helpers.GetWorldPointBounds(Viewport, Matrix);

        public Viewport Viewport
        {
            get
            {
                return new((int)Position.X, (int)Position.Y, (int)Scale.X, (int)Scale.Y);
            }
        }

        public SpriteSortMode SortMode { get; set; } = SpriteSortMode.Deferred;
        public BlendState BlendState { get; set; } = null;
        public SamplerState SamplerState { get; set; } = null;
        public DepthStencilState DepthStencilState { get; set; } = null;
        public RasterizerState RasterizerState { get; set; } = null;
        public Effect Effect { get; set; } = null;

        public Camera2D(Vector2 position, Scene2D associatedScene = null)
            : base(associatedScene)
        {
            MatrixPosition = position;
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
        }
    }
}
