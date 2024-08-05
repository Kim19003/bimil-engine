using System;
using System.Linq;
using Bimil.Engine;
using Bimil.Engine.Managers;
using Bimil.Engine.Models;
using Bimil.Engine.Objects;
using Bimil.Engine.Objects.Bases;
using Bimil.Game.Constants;
using Bimil.Game.Gadgets;
using Bimil.Game.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Object = Bimil.Game.Sprites.Object;

namespace Bimil.Game.Scenes
{
    public class Main : Scene2D
    {
        public Main(string name = nameof(Main)) : base(name)
        {
        }

        public override void Build()
        {
            AddGadgets(new object[]
            {
                new GameController()
                {
                    Name = "Game Controller",
                },
                new Camera2D(Vector2.Zero)
                {
                    Scale = new(Root.Core.ScreenHandler.Width, Root.Core.ScreenHandler.Height),
                    CameraLevel = 0,
                    Name = "Main Camera",
                    Depth = 2,
                    SortMode = SpriteSortMode.BackToFront,
                    BlendState = BlendState.AlphaBlend,
                    SamplerState = SamplerState.PointClamp,
                },
                // new Camera2D(new(ScreenHandler.ScreenWidth / 2, 0), new(ScreenHandler.ScreenWidth / 2, ScreenHandler.ScreenHeight), "Second Camera")
                // {
                //     Depth = 2,
                //     SortMode = SpriteSortMode.Deferred,
                //     BlendState = BlendState.AlphaBlend,
                //     SamplerState = SamplerState.PointClamp,
                // },
            });

            Camera2D mainCamera = (Camera2D)Gadgets.FirstOrDefault(s => s is Camera2D camera && camera.Name == "Main Camera");
            Rectangle mainCameraBounds = mainCamera.WorldPointBounds;

            AddSprites(new object[]
            {
                new Player("Square Head Idle Gun Looking Right", new(0, -2))
                {
                    Scale = new(1),
                    Rotation = 0,
                    PhysicsScale = new(0.5f, 1.8f),
                    CameraLevel = mainCamera.CameraLevel,
                    Name = "Player",
                    Tag = SpriteTags.PLAYER,
                },

                // new Wall("Grass P Mini", textureSize.Multiply(new(0, 0)),
                //     new(1), new(1, 1), mainCamera.CameraLevel, "Wall 3", SpriteTags.WALL),

                // new Wall("Grass P Mini", textureSize.Multiply(new(-2, 0)),
                //     new(1), new(1, 1), mainCamera.CameraLevel, "Wall 4", SpriteTags.WALL),

                // new Wall("Grass P Mini", textureSize.Multiply(new(2, 0)),
                //     new(1), new(1, 1), mainCamera.CameraLevel, "Wall 5", SpriteTags.WALL),
                    
                // new Wall("Pixels/Black Pixel", new(mainCameraBounds.X, mainCameraBounds.Y + mainCameraBounds.Height - 50), new(mainCameraBounds.Width * 10, 100), mainCamera.CameraLevel, "Floor", SpriteTags.WALL),
            });

            Vector2 previousPosition = new(0, 0);
            Random random = new();
            for (int i = 0; i < 50; i++)
            {
                Vector2 newPosition;
                if (i == 0)
                    newPosition = new(0, 0);
                else
                    newPosition = new Vector2(previousPosition.X + 2, previousPosition.Y - random.Next(-1, 2));
                Wall wall = new("Grass P Mini", newPosition,
                    new(1), new(1, 1), mainCamera.CameraLevel, $"Wall {i + 1}", SpriteTags.WALL);
                AddSprite(wall);
                previousPosition = newPosition;
            }

            previousPosition = new(0, 0);
            for (int i = 0; i < 1; i++)
            {
                Vector2 newPosition;
                if (i == 0)
                    newPosition = new(0, 0);
                else
                    newPosition = new Vector2(previousPosition.X + 64, previousPosition.Y - random.Next(-64, 64));
                Object sprite = new("Black Pixel", newPosition)
                {
                    Scale = new(16),
                    CameraLevel = mainCamera.CameraLevel,
                    Interpolation = Interpolation2D.None,
                    SortingLayer = 1,
                };
                AddSprite(sprite);
                previousPosition = newPosition;
            }

            // LogManager.EnableScreenLogging = false;
            LogManager.EnableConsoleLogging = false;
            LogManager.EnableFileLogging = false;

            // Camera2D secondCamera = (Camera2D)Gadgets.FirstOrDefault(s => s is Camera2D camera && camera.Name == "Second Camera");

            AddActiveCameras(new Camera2D[]
            {
                mainCamera,
                // secondCamera,
            });

            AddOrUpdateDraws(new Draw[]
            {
                // new(mainCamera, Color.Red, lineThickness: 5),
                // new(secondCamera, Color.Orange) { CameraLevel = secondCamera.Level },

                // new(((Player)Sprites.FirstOrDefault(s => s is Player)).Rigidbody2D.Body, Color.Green, cameraLevel: mainCamera.CameraLevel, lineThickness: 1f),
                // new(((Wall)Sprites.FirstOrDefault(s => ((Transform2D)s).Name == "Wall 3")).Rigidbody2D.Body, Color.Green, cameraLevel: mainCamera.CameraLevel, lineThickness: 1f),
                // new(((Wall)Sprites.FirstOrDefault(s => ((Transform2D)s).Name == "Wall 4")).Rigidbody2D.Body, Color.Green, cameraLevel: mainCamera.CameraLevel, lineThickness: 1f),
                // new(((Wall)Sprites.FirstOrDefault(s => ((Transform2D)s).Name == "Wall 5")).Rigidbody2D.Body, Color.Green, cameraLevel: mainCamera.CameraLevel, lineThickness: 1f),
            });
        }
    }
}