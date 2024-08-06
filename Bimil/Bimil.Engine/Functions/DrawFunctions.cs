using System.Collections.Generic;
using System.Linq;
using Genbox.VelcroPhysics.Collision.Shapes;
using Genbox.VelcroPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bimil.Engine.Handlers;
using Bimil.Engine.Managers;
using Bimil.Engine.Models;
using Bimil.Engine.Objects;
using Bimil.Engine.Objects.Bases;
using Bimil.Engine.Other;
using IDrawable = Bimil.Engine.Interfaces.IDrawable;
using System;
using Bimil.Engine.Models.DrawShapes;
using Bimil.Engine.Interfaces;
using Bimil.Engine.Other.Extensions;

namespace Bimil.Engine.Functions
{
    public static class DrawFunctions
    {
        public static void DrawSprites(ScreenHandler screenHandler, SceneHandler sceneHandler, GameTime gameTime)
        {
            // Get the sprites from the active scene
            IReadOnlyCollection<object> sprites = sceneHandler.ActiveScene.Sprites;

            if (sprites == null || !sprites.Any()) return;

            foreach (Camera2D activeCamera in sceneHandler.ActiveScene.ActiveCameras.OrderByDescending(c => c.CameraLevel))
            {
                HandleSprites(sprites, activeCamera, gameTime);
            }

            if (sceneHandler.ActiveScene.ActiveCameras.Any())
                Root.Graphics.GraphicsDevice.Viewport = screenHandler.Viewport;

            // Handle the screen level sprites
            HandleSprites(sprites, null, gameTime);
        }

        private static void HandleSprites(IReadOnlyCollection<object> sprites, Camera2D drawCamera, GameTime gameTime)
        {
            HashSet<object> cameraLevelSprites = sprites
                .Where(s => ((Transform2D)s).CameraLevel == (drawCamera == null ? -1 : drawCamera.CameraLevel))
                .ToHashSet();

            if (!cameraLevelSprites.Any()) return;

            if (drawCamera != null)
            {
                /*
                    Change the viewport to the camera's viewport (note: we don't reset the viewpoint for the
                    screen level here (when drawCamera is null), as it's already done before this function is called)
                */
                Root.Graphics.GraphicsDevice.Viewport = drawCamera.Viewport;
            }

            // Get the sort mode, blend state, sampler state, depth stencil state, rasterizer state, effect, and matrix from the camera
            SpriteSortMode sortMode = drawCamera?.SortMode ?? SpriteSortMode.Deferred;
            BlendState blendState = drawCamera?.BlendState;
            SamplerState samplerState = drawCamera?.SamplerState;
            DepthStencilState depthStencilState = drawCamera?.DepthStencilState;
            RasterizerState rasterizerState = drawCamera?.RasterizerState;
            Effect effect = drawCamera?.Effect;
            Matrix? transformMatrix = drawCamera?.Matrix;

            // Begin the sprite batch
            Root.SpriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);

            foreach (object cameraLevelSprite in cameraLevelSprites)
            {
                if (cameraLevelSprite == null || cameraLevelSprite is not IDrawable) continue;

                AnimationHandler animationHandler = cameraLevelSprite is IAnimatable animatable
                    ? animatable.AnimationHandler
                    : null;
                ((IDrawable)cameraLevelSprite).Draw(gameTime, animationHandler);
            }

            // End the sprite batch
            Root.SpriteBatch.End();
        }

        public static void DrawDraws(ScreenHandler screenHandler, SceneHandler sceneHandler, GameTime gameTime)
        {
            // Get the draws from the active scene
            IReadOnlyCollection<Draw> draws = sceneHandler.ActiveScene.Draws;

            if (draws == null || !draws.Any()) return;
            
            // Draw the debugs
            foreach (Camera2D activeCamera in sceneHandler.ActiveScene.ActiveCameras.OrderByDescending(c => c.CameraLevel))
            {
                HandleDraws(sceneHandler.ActiveScene, draws, activeCamera, gameTime);
            }
            
            if (sceneHandler.ActiveScene.ActiveCameras.Any())
                Root.Graphics.GraphicsDevice.Viewport = screenHandler.Viewport;

            // Handle the screen level draws
            HandleDraws(sceneHandler.ActiveScene, draws, null, gameTime);
        }

        public static void DrawGrid(GridSettings gridSettings)
        {
            Root.Graphics.GraphicsDevice.Viewport = gridSettings.Camera.Viewport;

            // Begin the sprite batch
            Root.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                transformMatrix: gridSettings.Camera.Matrix);

            Vector2 cameraMatrixPosition = gridSettings.Camera.MatrixPosition;
            Vector2 cellSize = gridSettings.CellSize;

            Vector2 roundedCameraPosition = new((int)(cameraMatrixPosition.X / cellSize.X) * cellSize.X, (int)(cameraMatrixPosition.Y / cellSize.Y) * cellSize.Y);

            // Draw the grid (also add the rounded camera position, so that the grid is "infinite" in length and height)
            int gridWidth = gridSettings.Camera.Viewport.Width + Math.Abs((int)roundedCameraPosition.X);
            int gridHeight = gridSettings.Camera.Viewport.Height + Math.Abs((int)roundedCameraPosition.Y);
            int adjustedScreenWidth = -gridWidth;
            int adjustedScreenHeight = -gridHeight;

            // Draw the vertical lines
            for (int x = adjustedScreenWidth; x < gridWidth; x += (int)gridSettings.CellSize.X)
            {
                float adjustedX = x - (int)gridSettings.CellSize.X / 2; // Adjust the x position to center the lines
                Root.SpriteBatch.DrawLine(new(new Vector2(adjustedX, adjustedScreenHeight), new Vector2(adjustedX, gridHeight), gridSettings.LineColor,
                    gridSettings.LineThickness, gridSettings.SortingLayer));
            }

            // Draw the horizontal lines
            for (int y = adjustedScreenHeight; y < gridHeight; y += (int)gridSettings.CellSize.Y)
            {
                Root.SpriteBatch.DrawLine(new(new Vector2(adjustedScreenWidth, y), new Vector2(gridWidth, y), gridSettings.LineColor,
                    gridSettings.LineThickness, gridSettings.SortingLayer));
            }

            // End the sprite batch
            Root.SpriteBatch.End();
        }

        private static void HandleDraws(Scene2D activeScene, IReadOnlyCollection<Draw> draws, Camera2D drawCamera, GameTime gameTime)
        {
            HashSet<Draw> cameraLevelDraws = draws
                    .Where(d => d.CameraLevel == (drawCamera == null ? -1 : drawCamera.CameraLevel))
                    .ToHashSet();

            if (!cameraLevelDraws.Any()) return;

            if (drawCamera != null)
            {
                /*
                    Change the viewport to the camera's viewport (note: we don't reset the viewpoint for the
                    screen level here (when drawCamera is null), as it's already done before this function is called)
                */
                Root.Graphics.GraphicsDevice.Viewport = drawCamera.Viewport;
            }

            // Begin the sprite batch
            Root.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                transformMatrix: drawCamera?.Matrix);

            foreach (Draw cameraLevelDraw in cameraLevelDraws)
            {
                if (cameraLevelDraw.Object == null) continue;

                switch (cameraLevelDraw.Object)
                {
                    case Log log:
                        HandleLogDraw(activeScene, log, cameraLevelDraw.Color, cameraLevelDraw.LifeTime, gameTime);
                        break;
                    case Fixture fixture:
                        HandleFixtureDraw(cameraLevelDraw.LineThickness, fixture, cameraLevelDraw.Color);
                        break;
                    case Camera2D camera:
                        HandleCameraDraw(cameraLevelDraw.CameraLevel, cameraLevelDraw.LineThickness, camera, cameraLevelDraw.Color);
                        break;
                    case DrawShapeBase drawShape:
                        HandleDrawShapeDraw(drawShape);
                        break;
                    // TODO: Add more debug drawing logic here if needed (if so, also create a constructor for it in the Draw class)
                }
            }

            // End the sprite batch
            Root.SpriteBatch.End();
        }

        private static Dictionary<Log, (Log LogMirror, float LastDrawTime, float LifeTime, float TotalLifeTime)> _logDrawTimes = new();
        private static void HandleLogDraw(Scene2D activeScene, Log log, Color color, float lifeTime, GameTime gameTime)
        {
            float totalElapsedMilliseconds = (float)gameTime.TotalGameTime.TotalMilliseconds;

            if (_logDrawTimes.ContainsKey(log))
            {
                bool isMessageDifferent = _logDrawTimes[log].LogMirror.Message != log.Message;
                bool isLevelDifferent = _logDrawTimes[log].LogMirror.Level != log.Level;
                bool isTimeDifferent = _logDrawTimes[log].LogMirror.Time != log.Time;
                bool isTypeDifferent = _logDrawTimes[log].LogMirror.Type != log.Type;
                bool isPositionDifferent = _logDrawTimes[log].LogMirror.Position != log.Position;

                bool logUpdated = isMessageDifferent || isLevelDifferent || isTimeDifferent || isTypeDifferent || isPositionDifferent;

                bool lifeTimeChanged = lifeTime != _logDrawTimes[log].LifeTime;
                if (logUpdated || lifeTimeChanged) // Update the log if it or it's lifetime has changed
                {
                    Log logMirror = new(log.Message, log.Level, log.Time, log.Type, log.Position);

                    float newLifeTime = lifeTimeChanged
                        ? lifeTime
                        : _logDrawTimes[log].LifeTime;
                    float newTotalLifeTime = lifeTimeChanged
                        ? totalElapsedMilliseconds + lifeTime
                        : _logDrawTimes[log].TotalLifeTime;

                    _logDrawTimes[log] = (logMirror, totalElapsedMilliseconds, newLifeTime, newTotalLifeTime);
                }
                else
                {
                    _logDrawTimes[log] = (_logDrawTimes[log].LogMirror, totalElapsedMilliseconds, _logDrawTimes[log].LifeTime, _logDrawTimes[log].TotalLifeTime);
                }
            }
            else
            {
                Log logMirror = new(log.Message, log.Level, log.Time, log.Type, log.Position);
                _logDrawTimes.Add(log, (logMirror, totalElapsedMilliseconds, lifeTime, totalElapsedMilliseconds + lifeTime));
            }

            if (_logDrawTimes[log].LifeTime > 0 && _logDrawTimes[log].LastDrawTime >= _logDrawTimes[log].TotalLifeTime) // Remove the log after it's lifetime has passed
            {
                activeScene.RemoveDraw(new(log, Color.White));
                int logKey = LogManager.ShownScreenLogs.FirstOrDefault(l => l.Value == log).Key;
                LogManager.ShownScreenLogs.Remove(logKey);
                LogManager.ShownScreenLogs.RearrangeSequence(LogManager.LogScreenStartPosition, LogManager.ShownLogVerticalSpacing);
                _logDrawTimes.Remove(log);
                return;
            }

            if (log.ShadowSettings != null)
            {
                Root.SpriteBatch.DrawString(Root.LogFont, log.Message, log.Position + log.ShadowSettings.Offset, log.ShadowSettings.Color);
            }
            Root.SpriteBatch.DrawString(Root.LogFont, log.Message, log.Position, color);
        }

        private static void HandleFixtureDraw(float lineThickness, Fixture fixture, Color color)
        {
            Body body = fixture.Body ?? throw new Exception("The fixture is not attached to a body");
            
            switch (fixture.Shape)
            {
                case PolygonShape polygonShape:
                    List<Vector2> vertices = new();
                    foreach (Vector2 vertex in polygonShape.Vertices)
                    {
                        vertices.Add(body.Position + vertex);
                    }
                    Root.SpriteBatch.DrawPolygon(new(vertices.ToArray(), color, lineThickness));
                    break;
                case CircleShape circleShape:
                    Vector2 position = body.Position + circleShape.Position;
                    Circle circle = new(Helpers.ConvertToPoint(position), (int)circleShape.Radius);
                    Root.SpriteBatch.DrawCircle(new(circle, color, lineThickness));
                    break;
                // TODO: Add more shapes here if needed
            }
        }

        private static void HandleCameraDraw(int cameraLevel, float lineThickness, Camera2D camera, Color color)
        {
            if (cameraLevel == -1)
            {
                Viewport cameraViewport = camera.Viewport;
                Rectangle cameraScreenPointBounds = new(cameraViewport.X, cameraViewport.Y, cameraViewport.Width, cameraViewport.Height);
                Root.SpriteBatch.DrawRectangle(new(cameraScreenPointBounds, color, lineThickness));
            }
            else
            {
                Rectangle cameraWorldPointBounds = camera.WorldPointBounds;
                Root.SpriteBatch.DrawRectangle(new(cameraWorldPointBounds, color, lineThickness));
            }
        }

        private static void HandleDrawShapeDraw(object drawShape)
        {
            switch (drawShape)
            {
                case RectangleDrawShape rectangleDrawShape:
                    Root.SpriteBatch.DrawRectangle(rectangleDrawShape);
                    break;
                case CircleDrawShape circleDrawShape:
                    Root.SpriteBatch.DrawCircle(circleDrawShape);
                    break;
                case LineDrawShape lineDrawShape:
                    Root.SpriteBatch.DrawLine(lineDrawShape);
                    break;
                case PolygonDrawShape polygonDrawShape:
                    Root.SpriteBatch.DrawPolygon(polygonDrawShape);
                    break;
            }
        }
    }
}