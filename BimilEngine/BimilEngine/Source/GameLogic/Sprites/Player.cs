using System.Linq;
using Genbox.VelcroPhysics.Collision.Shapes;
using Genbox.VelcroPhysics.Definitions;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Shared;
using Genbox.VelcroPhysics.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BimilEngine.Source.Engine.Managers;
using BimilEngine.Source.Engine.Models;
using BimilEngine.Source.Engine.Objects;
using BimilEngine.Source.Engine.Objects.Bases;
using BimilEngine.Source.Engine.Other;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Genbox.VelcroPhysics.Collision.ContactSystem;
using BimilEngine.Source.Engine.Interfaces;
using BimilEngine.Source.Engine.Handlers;

namespace BimilEngine.Source.GameLogic.Sprites
{
    public class Player : PhysicsSprite2D, IAnimatable
    {
        public float MovementSpeed { get; set; } = 160f;
        public float JumpPower { get; set; } = 200f;

        public bool IsGrounded { get; private set; }

        public AnimationHandler AnimationHandler { get; set; } = new();

        public Player(string texturePath, Vector2 position, Vector2 scale, Vector2 physicsScale, int cameraLevel = 0, string name = "", string tag = "",
            Scene2D associatedScene = null) : base(texturePath, position, scale, physicsScale, cameraLevel, name, tag, associatedScene)
        {
            Rigidbody2D = new(this, Environment2D.PhysicsWorld.CreateBody(new()
                {
                    Type = BodyType.Dynamic,
                    Position = position,
                    // FixedRotation = true,
                    GravityScale = 4f
                })
            );
            Vertices rectangleVertices = PolygonUtils.CreateRectangle(4, 16);
            Rigidbody2D.Body.CreateFixture(new FixtureDef()
            {
                Shape = new PolygonShape(rectangleVertices, 1f),
            });

            // Rigidbody2D.Body.Restitution = 1f; // Bounciness
            // Body.Friction = 0.5f; // Friction
            // Rigidbody2D.Body.Mass = 60f; // Use this to set the mass
            Rigidbody2D.Body.IsBullet = true; // Use this to make the body a bullet (enable CCD)
            Rigidbody2D.Body.FixedRotation = true; // Use this to make the body not rotate
            // Rigidbody2D.Body.AngularVelocity = 0.1f; // Use this to set the angular (rotation) velocity

            // Body.IsSensor = true; // Use this to make the body a trigger

            // List<Fixture> fixtures = Environment2D.PhysicsWorld.RayCast(Vector2.Zero, Vector2.Zero); // Use this to raycast
        }

        // RectangleDrawShape bodyDrawShape = new(new(0, 0, 20, 20), Color.Red);

        public override void Start()
        {
            // AssociatedScene.AddOrUpdateDebugDraw(new(bodyDrawShape, cameraLevel: Environment2D.ActiveScene.ActiveCameras.First().CameraLevel));

            // ---------
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            // if (Keyboard.GetState().IsKeyPressed(Keys.Y))
            // {
            //     bodyDrawShape.Body = new(bodyDrawShape.Body.X, bodyDrawShape.Body.Y - 1, bodyDrawShape.Body.Width, bodyDrawShape.Body.Height);
            // }

            Camera2D activeCamera = Environment2D.ActiveScene.ActiveCameras.FirstOrDefault();
            activeCamera.MatrixPosition = Position;

            // ---------
            base.Update(gameTime);
        }

        public override void FixedUpdate(GameTime gameTime, GameTime fixedGameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState keyboardState = Keyboard.GetState();

            bool isKeyDownLeft = keyboardState.IsKeyDown(Keys.Left);
            bool isKeyDownRight = keyboardState.IsKeyDown(Keys.Right);
            bool isKeyPressedUp = keyboardState.IsKeyPressed(Keys.Up);
            bool isKeyDownDown = keyboardState.IsKeyDown(Keys.Down);
            bool isKeyShiftDown = keyboardState.IsKeyDown(Keys.LeftShift);

            // if (isKeyShiftDown)
            //     MovementSpeed = 100f;
            // else
            //     MovementSpeed = 10f;

            if (isKeyDownLeft) // Moving left
                Rigidbody2D.Body.LinearVelocity = new Vector2(-MovementSpeed * 100 * deltaTime, Rigidbody2D.Body.LinearVelocity.Y);
            else if (isKeyDownRight) // Moving right
                Rigidbody2D.Body.LinearVelocity = new Vector2(MovementSpeed * 100 * deltaTime, Rigidbody2D.Body.LinearVelocity.Y);
            else // Not moving left or right
                Rigidbody2D.Body.LinearVelocity = new Vector2(0, Rigidbody2D.Body.LinearVelocity.Y);

            if (isKeyPressedUp) // Jumping
                Rigidbody2D.Body.LinearVelocity = new Vector2(Rigidbody2D.Body.LinearVelocity.X, -JumpPower);

            // ---------
            base.FixedUpdate(gameTime, fixedGameTime);
        }

        public override void Draw(GameTime gameTime, float interpolationAlpha = 0f)
        {

            // ---------
            base.Draw(gameTime, interpolationAlpha);
        }

        public override void OnCollisionEnter2D(Fixture current, Fixture other, Contact contact)
        {
            LogManager.DoConsoleLog($"Collision Enter", LogLevel.Debug);

            // PhysicsSprite2D otherSprite = other.GetParent();
            // if (((Sprite2D)collision.OtherCollider.AssociatedGameObject).Tag == SpriteTags.Wall)
            // {
                

            //     // HandleCollisionBounce(collision.CollisionDirection, MovementSpeed);
            // }
        }

        public override void OnCollisionStay2D(Fixture current, Fixture other, Contact contact)
        {
            LogManager.DoConsoleLog($"Collision Stay", LogLevel.Debug);

            // if (((Sprite2D)collision.OtherCollider.AssociatedGameObject).Tag == SpriteTags.Wall)
            // {
                
            // }
        }

        public override void OnCollisionExit2D(Fixture current, Fixture other, Contact contact)
        {
            LogManager.DoConsoleLog($"Collision Exit", LogLevel.Debug);

            // if (((Sprite2D)collision.OtherCollider.AssociatedGameObject).Tag == SpriteTags.Wall)
            // {
                
            // }
        }

        // private void HandleCollisionBounce(Direction2D? collisionDirection, float bouncePower)
        // {
        //     switch (collisionDirection)
        //     {
        //         case Direction2D.Left:
        //             Body.Position = new Vector2(Position.X + bouncePower, Position.Y);
        //             break;
        //         case Direction2D.Right:
        //             Body.Position = new Vector2(Position.X - bouncePower, Position.Y);
        //             break;
        //         case Direction2D.Up:
        //             Body.Position = new Vector2(Position.X, Position.Y + bouncePower);
        //             break;
        //         case Direction2D.Down:
        //             Body.Position = new Vector2(Position.X, Position.Y - bouncePower);
        //             break;
        //     }
        // }
    }
}