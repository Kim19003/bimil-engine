using Genbox.VelcroPhysics.Collision.Shapes;
using Genbox.VelcroPhysics.Definitions;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Shared;
using Genbox.VelcroPhysics.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Bimil.Engine.Models;
using Bimil.Engine.Objects;
using Bimil.Engine.Objects.Bases;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Genbox.VelcroPhysics.Collision.ContactSystem;
using Bimil.Engine.Interfaces;
using Bimil.Engine.Handlers;
using Bimil.Engine;
using System;
using Bimil.Engine.Models.DrawShapes;
using System.Linq;
using Bimil.Game.Constants;
using Bimil.Engine.Other.Extensions;
using Microsoft.Xna.Framework.Media;

namespace Bimil.Game.Sprites
{
    public class Player : PhysicsSprite2D, IAnimatable
    {
        public float MovementSpeed { get; set; } = 160f;
        public float JumpPower { get; set; } = 300f;

        public bool IsGrounded { get; private set; }

        public AnimationHandler AnimationHandler { get; set; } = new();

        public Player(string textureName, Vector2 position, Scene2D associatedScene = null)
            : base(textureName, associatedScene)
        {
            Position = position;

            Rigidbody2D = new(this, Root.Core.PhysicsWorld.CreateBody(new()
                {
                    Type = BodyType.Dynamic,
                    Position = AbsolutePosition,
                    // FixedRotation = true,
                    GravityScale = 4f
                })
            );
            Vertices rectangleVertices = PolygonUtils.CreateRectangle(4, 16);
            Rigidbody2D.Body.CreateFixture(new FixtureDef()
            {
                Shape = new PolygonShape(rectangleVertices, 1f),
                UserData = new FixtureUserData(Identifiers.PLAYER_BODY)
            });
            Rigidbody2D.Body.CreateFixture(new FixtureDef()
            {
                Shape = new CircleShape(5, 0f, new Vector2(0, 13)),
                IsSensor = true,
                UserData = new FixtureUserData(Identifiers.GROUND_SENSOR)
            });

            // Rigidbody2D.Body.Restitution = 1f; // Bounciness
            // Body.Friction = 0.5f; // Friction
            // Rigidbody2D.Body.Mass = 60f; // Use this to set the mass
            Rigidbody2D.Body.IsBullet = true; // Use this to make the body a bullet (enable CCD)
            Rigidbody2D.Body.FixedRotation = true; // Use this to make the body not rotate
            // Rigidbody2D.Body.AngularVelocity = 0.5f; // Use this to set the angular (rotation) velocity
            // Interpolation = Interpolation2D.None; // Use this to set the interpolation mode

            // Body.IsSensor = true; // Use this to make the body a trigger

            // List<Fixture> fixtures = Core.PhysicsWorld.RayCast(Vector2.Zero, Vector2.Zero); // Use this to raycast
        }

        RectangleDrawShape bodyDrawShape = new(new(0, 0, 50, 50), Color.Red, 10);
        // CircleDrawShape circle;
        public override void Start()
        {
            // Vector2 position = InterpolatedDrawPosition + new Vector2(-300, 0);
            // circle = new(new((int)position.X, (int)position.Y, 5), Color.Red);
            // AssociatedScene.AddOrUpdateDraw(new(circle, cameraLevel: CameraLevel));
            AssociatedScene.AddOrUpdateDraw(new(bodyDrawShape, cameraLevel: 0));

            Animation left = new(new DuratedTexture(TimeSpan.FromSeconds(1), Root.TextureBatch["Square Head Idle Gun Looking Left"]));
            Animation leftUp = new(new DuratedTexture(TimeSpan.FromSeconds(1), Root.TextureBatch["Square Head Idle Gun Looking Left Hands Up"]));
            Animation right = new(new DuratedTexture(TimeSpan.FromSeconds(1), Root.TextureBatch["Square Head Idle Gun Looking Right"]));
            Animation rightUp = new(new DuratedTexture(TimeSpan.FromSeconds(1), Root.TextureBatch["Square Head Idle Gun Looking Right Hands Up"]));

            AnimationHandler.AddAnimations(new[]
            {
                ("Left", left ),
                ("Left Up", leftUp ),
                ("Right", right ),
                ("Right Up", rightUp )
            });

            if (!Root.Core.AudioHandler.SoundEffects.ContainsKey("Jump"))
                Root.Core.AudioHandler.SoundEffects.Add("Jump", Root.SoundEffectBatch["Jump"]);

            // ---------
            base.Start();
        }

        readonly LineDrawShape _rayCastShape = new(default, default, Color.Green, 1f, 0);
        readonly Vector2 _rayCastDirection = new(0, 20);
        public override void Update(GameTime gameTime)
        {
            // if (Keyboard.GetState().IsKeyPressed(Keys.Y))
            // {
            //     bodyDrawShape.Body = new(bodyDrawShape.Body.X, bodyDrawShape.Body.Y - 1, bodyDrawShape.Body.Width, bodyDrawShape.Body.Height);
            // }

            Camera2D activeCamera = Root.Core.ActiveScene.ActiveCameras.FirstOrDefault();
            activeCamera.MatrixPosition = InterpolatedDrawPosition;

            // _rayCastShape.Start = InterpolatedDrawPosition;
            // _rayCastShape.End = InterpolatedDrawPosition + _rayCastDirection;
            // AssociatedScene.AddOrUpdateDraw(new(_rayCastShape, cameraLevel: 0));

            // AssociatedScene.AddOrUpdateDraw(new(Rigidbody2D.Body.FixtureList.First(), Color.Green, cameraLevel: 0));

            // ---------
            base.Update(gameTime);
        }

        bool _canMoveHorizontally = true;
        Vector2 _moveDirection = Vector2.Zero;
        bool _isMoving = false;
        bool _isJumping = false;
        public override void FixedUpdate(GameTime gameTime, GameTime fixedGameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState keyboardState = Keyboard.GetState();

            bool isKeyDownLeft = keyboardState.IsKeyDown(Keys.Left);
            bool isKeyDownRight = keyboardState.IsKeyDown(Keys.Right);
            bool isKeyPressedUp = keyboardState.IsKeyPressed(Keys.Up);
            bool isKeyDownDown = keyboardState.IsKeyDown(Keys.Down);
            bool isKeyShiftDown = keyboardState.IsKeyDown(Keys.LeftShift);

            // List<Fixture> fixtures = Core.PhysicsWorld.RayCast(InterpolatedDrawPosition, InterpolatedDrawPosition + _rayCastDirection);
            // if (fixtures.Any(x => x.GetParent().Tag == SpriteTags.Wall))
            // {
            //     IsGrounded = true;
            //     _isJumping = false;
            //     LogManager.DoConsoleLog($"Grounded", LogLevel.Debug);
            // }
            // else
            // {
            //     IsGrounded = false;
            //     LogManager.DoConsoleLog($"Not Grounded", LogLevel.Debug);
            // }

            // if (isKeyShiftDown)
            //     MovementSpeed = 100f;
            // else
            //     MovementSpeed = 10f;

            if (_canMoveHorizontally && isKeyDownLeft) // Moving left
            {
                Rigidbody2D.Body.LinearVelocity = new Vector2(-MovementSpeed * 100 * deltaTime, Rigidbody2D.Body.LinearVelocity.Y);
                _moveDirection = Direction2D.Left;
                if (!IsGrounded)
                    AnimationHandler.PlayAnimation("Left Up");
                else
                    AnimationHandler.PlayAnimation("Left");

                _isMoving = true;
            }
            else if (_canMoveHorizontally && isKeyDownRight) // Moving right
            {
                Rigidbody2D.Body.LinearVelocity = new Vector2(MovementSpeed * 100 * deltaTime, Rigidbody2D.Body.LinearVelocity.Y);
                _moveDirection = Direction2D.Right;
                if (!IsGrounded)
                    AnimationHandler.PlayAnimation("Right Up");
                else
                    AnimationHandler.PlayAnimation("Right");

                _isMoving = true;
            }
            else // Not moving left or right
            {
                Rigidbody2D.Body.LinearVelocity = new Vector2(0, Rigidbody2D.Body.LinearVelocity.Y);
                if (!IsGrounded)
                {
                    if (_moveDirection == Direction2D.Left)
                        AnimationHandler.PlayAnimation("Left Up");
                    else if (_moveDirection == Direction2D.Right)
                        AnimationHandler.PlayAnimation("Right Up");
                }
                else
                {
                    if (_moveDirection == Direction2D.Left)
                        AnimationHandler.PlayAnimation("Left");
                    else if (_moveDirection == Direction2D.Right)
                        AnimationHandler.PlayAnimation("Right");
                }

                _isMoving = false;
            }

            if (IsGrounded && isKeyPressedUp) // Jumping
            {
                Rigidbody2D.Body.LinearVelocity = new Vector2(Rigidbody2D.Body.LinearVelocity.X, -JumpPower);
                Root.Core.AudioHandler.PlaySoundEffect("Jump", InterpolatedDrawPosition, InterpolatedDrawPosition);

                _isJumping = true;
            }

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
            // LogManager.DoConsoleLog($"Collision Enter", LogLevel.Debug);

            contact.Friction = 0f; // Use this to set the friction (e.g. 0f = the player should not stick to the wall)
        }

        public override void OnCollisionStay2D(Fixture current, Fixture other, Contact contact)
        {
            // LogManager.DoConsoleLog($"Collision Stay", LogLevel.Debug);
        }

        public override void OnCollisionExit2D(Fixture current, Fixture other, Contact contact)
        {
            // LogManager.DoConsoleLog($"Collision Exit", LogLevel.Debug);
        }

        public override void OnTriggerEnter2D(Fixture current, Fixture other, Contact contact)
        {
            // LogManager.DoConsoleLog($"Trigger Enter", LogLevel.Debug);

            PhysicsSprite2D otherSprite = other.GetParentOfBody();
            if (current.FixtureId == 1 && otherSprite.Tag == SpriteTags.WALL)
            {
                IsGrounded = true;
                _isJumping = false;
            }
        }

        public override void OnTriggerStay2D(Fixture current, Fixture other, Contact contact)
        {
            // LogManager.DoConsoleLog($"Trigger stay", LogLevel.Debug);

            PhysicsSprite2D otherSprite = other.GetParentOfBody();
            if (current.FixtureId == 1 && otherSprite.Tag == SpriteTags.WALL && !_isJumping && !_isMoving)
            {
                Rigidbody2D.Body.LinearVelocity = otherSprite.Rigidbody2D.Body.LinearVelocity;
            }
        }

        public override void OnTriggerExit2D(Fixture current, Fixture other, Contact contact)
        {
            // LogManager.DoConsoleLog($"Trigger Exit", LogLevel.Debug);

            PhysicsSprite2D otherSprite = other.GetParentOfBody();
            if (current.FixtureId == 1 && otherSprite.Tag == SpriteTags.WALL)
            {
                IsGrounded = false;
                _isJumping = true;
            }
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