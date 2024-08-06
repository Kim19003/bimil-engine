using Genbox.VelcroPhysics.Collision.Shapes;
using Genbox.VelcroPhysics.Definitions;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Shared;
using Microsoft.Xna.Framework;
using Bimil.Engine.Objects.Bases;
using Bimil.Engine.Handlers;
using Bimil.Engine;
using System.Linq;

namespace Bimil.Game.Sprites
{
    public class Wall : PhysicsSprite2D
    {
        public Wall(string textureName, Vector2 position, Vector2 scale, Vector2 physicsScale, int cameraLevel = 0, string name = "", string tag = "",
            Scene2D associatedScene = null) : base(textureName, associatedScene)
        {
            Position = position;
            Scale = scale;
            PhysicsScale = physicsScale;
            CameraLevel = cameraLevel;
            Name = name;
            Tag = tag;

            Rigidbody2D = new(this, Root.Core.PhysicsWorld.CreateBody(new()
                {
                    Type = BodyType.Kinematic,
                    Position = AbsolutePosition,
                })
            );
            Vertices rectangleVertices = new()
            {
                new Vector2(-16, -4), // Top left
                new Vector2(16, -4), // Top right
                new Vector2(16, 16), // Bottom right
                new Vector2(-16, 16), // Bottom left
            };
            Rigidbody2D.Body.CreateFixture(new FixtureDef()
            {
                Shape = new PolygonShape(rectangleVertices, 1f),
            });

            // Rigidbody2D.Body.IgnoreCCD = true;
        }

        public override void Update(GameTime gameTime)
        {
            // AssociatedScene.AddOrUpdateDraw(new(Rigidbody2D.Body.FixtureList.First(), Color.Orange, cameraLevel: 0));

            // ---------
            base.Update(gameTime);
        }

        private float elapsedTime = 0f;
        private const float delay = 0.01f;
        public override void FixedUpdate(GameTime gameTime, GameTime fixedGameTime)
        {
            float deltaTime = (float)fixedGameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += deltaTime;

            if (elapsedTime >= delay)
            {
                Rigidbody2D.Body.LinearVelocity = new Vector2(20, Rigidbody2D.Body.LinearVelocity.Y);
                
                elapsedTime = 0f;
            }

            // ---------
            base.FixedUpdate(gameTime, fixedGameTime);
        }

        public override void Draw(GameTime gameTime, AnimationHandler animationHandler = null)
        {

            // ---------
            base.Draw(gameTime, animationHandler);
        }
    }
}