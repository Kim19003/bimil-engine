using Microsoft.Xna.Framework;
using Bimil.Engine.Objects.Bases;
using Bimil.Engine.Handlers;
using Bimil.Engine;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Shared;
using Genbox.VelcroPhysics.Definitions;
using Genbox.VelcroPhysics.Collision.Shapes;
using Genbox.VelcroPhysics.Collision.ContactSystem;
using Genbox.VelcroPhysics.Utilities;
using Bimil.Engine.Managers;
using Bimil.Engine.Models;
using Bimil.Game.Constants;
using Bimil.Engine.Other.Extensions;
using System.Linq;

namespace Bimil.Game.Sprites
{
    public class SomeObject : PhysicsSprite2D
    {
        public SomeObject(string textureName, Vector2 position, Scene2D associatedScene = null)
            : base(textureName, associatedScene)
        {
            Position = position;

            Rigidbody2D = new(this, Root.Core.PhysicsWorld.CreateBody(new()
                {
                    Type = BodyType.Kinematic,
                    Position = AbsolutePosition,
                })
            );
            Vertices rectangleVertices = PolygonUtils.CreateRectangle(8, 8);
            Rigidbody2D.Body.CreateFixture(new FixtureDef()
            {
                Shape = new PolygonShape(rectangleVertices, 1f),
            });
        }

        public override void Update(GameTime gameTime)
        {
            AssociatedScene.AddOrUpdateDraw(new(Rigidbody2D.Body.FixtureList.First(), Color.Red, 0));

            // ---------
            base.Update(gameTime);
        }

        public override void FixedUpdate(GameTime gameTime, GameTime fixedGameTime)
        {

            // ---------
            base.FixedUpdate(gameTime, fixedGameTime);
        }

        public override void Draw(GameTime gameTime, AnimationHandler animationHandler = null)
        {

            // ---------
            base.Draw(gameTime, animationHandler);
        }

        public override void OnCollisionEnter2D(Fixture current, Fixture other, Contact contact)
        {
            PhysicsSprite2D otherSprite = other.GetParentOfBody();
            if (otherSprite is Player player && other.GetName() == Identifiers.PLAYER_BODY)
            {
                // player.Destroy();
                LogManager.DoScreenLog("Player destroyed", shadowSettings: new(new Vector2(1, 1), new(0, 0, 0, 0.75f)));
            }
        }
    }
}