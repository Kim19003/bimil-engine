using System;
using System.Linq;
using Bimil.Engine;
using Bimil.Engine.Models;
using Bimil.Engine.Objects;
using Bimil.Engine.Objects.Bases;
using Bimil.Game.Gadgets;
using Bimil.Game.Models;
using Bimil.Game.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bimil.Game.Scenes
{
    public class Secondary : Scene2D
    {
        public Secondary(string name = nameof(Secondary)) : base(name)
        {
        }

        public override void Build()
        {
            Camera2D mainCamera = new(Vector2.Zero)
            {
                Scale = new(Root.Core.ScreenHandler.Width, Root.Core.ScreenHandler.Height),
                CameraLevel = 0,
                Name = "Main Camera",
                Depth = 2,
                SortMode = SpriteSortMode.Deferred,
                BlendState = BlendState.AlphaBlend,
                SamplerState = SamplerState.PointClamp,
            };

            AddGadgets(new object[]
            {
                mainCamera
            });

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
                    
                new Wall("Black Pixel", new(mainCameraBounds.X, mainCameraBounds.Y + mainCameraBounds.Height - 50),
                    new(mainCameraBounds.Width * 10, 100), new(1, 1), mainCamera.CameraLevel, "Floor", SpriteTags.WALL),
            });

            AddActiveCameras(new Camera2D[]
            {
                mainCamera,
            });
        }
    }
}