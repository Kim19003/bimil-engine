using System;
using System.Collections.Generic;
using System.Linq;
using Bimil.Engine.Managers;
using Bimil.Engine.Models;
using Bimil.Engine.Objects;
using Bimil.Engine.Objects.Bases;

namespace Bimil.Engine.Handlers
{
    public sealed class SceneHandler
    {
        /// <summary>
        /// All the scenes.
        /// </summary>
        public IReadOnlyCollection<Scene2D> Scenes => _scenes.ToArray();
        private readonly HashSet<Scene2D> _scenes = new();
        
        /// <summary>
        /// The active scene.
        /// </summary>
        public Scene2D ActiveScene => _activeScene;
        private Scene2D _activeScene;

        /// <summary>
        /// All the gadgets.
        /// </summary>
        public (Scene2D Scene, IReadOnlyCollection<object> Gadgets)[] AllGadgets => Scenes
            .Select(scene => (scene, scene.Gadgets))
            .ToArray();
        /// <summary>
        /// All the sprites.
        /// </summary>
        public (Scene2D Scene, IReadOnlyCollection<object> Sprites)[] AllSprites => Scenes
            .Select(scene => (scene, scene.Sprites))
            .ToArray();
        /// <summary>
        /// Everything.
        /// </summary>
        public (Scene2D Scene, IReadOnlyCollection<object> Things)[] Everything => Scenes
            .Select(scene => (scene, scene.Everything))
            .ToArray();

        /// <summary>
        /// Initialize scenes action. Invoked, when a scene is loaded.
        /// </summary>
        public Action InitializeScenesAction { get; set; }

        public SceneHandler()
        {

        }

        public SceneHandler(Action initializeScenesAction)
        {
            InitializeScenesAction = initializeScenesAction;
        }

        /// <summary>
        /// Add a scene.
        /// </summary>
        public void AddScene(Scene2D scene)
        {
            if (_scenes.Any(s => s.Name == scene.Name))
                throw new Exception($"Scene with name {scene.Name} already exists");

            // If the scene doesn't have any active cameras, add first found camera as active camera
            if (!scene.ActiveCameras.Any())
            {
                if (scene.Gadgets.FirstOrDefault(gadget => gadget is Camera2D) is not Camera2D foundCamera)
                {
                    LogManager.DoConsoleLog($"PUA: Scene '{scene.Name}' does not have any cameras!", LogLevel.Warning);
                }
                else
                {
                    scene.AddActiveCamera(foundCamera);
                    LogManager.DoConsoleLog($"Scene '{scene.Name}' didn't have any active cameras, so we added the first camera found as the active camera.");
                }
            }
            
            _scenes.Add(scene);
        }

        /// <summary>
        /// Destroy a scene by name.
        /// </summary>
        public void DestroyScene(string sceneName)
        {
            if (!_scenes.Any(scene => scene.Name == sceneName))
                throw new Exception($"Scene with name {sceneName} does not exist");

            _scenes.RemoveWhere(scene => scene.Name == sceneName);
        }

        /// <summary>
        /// Destroy a scene by reference.
        /// </summary>
        public void DestroyScene(Scene2D scene)
        {
            if (!_scenes.Any(s => s == scene))
                throw new Exception($"Scene with name {scene.Name} does not exist");

            _scenes.RemoveWhere(s => s == scene);
        }

        /// <summary>
        /// Load a scene by name.
        /// </summary>
        public void LoadScene(string sceneName)
        {
            if (_scenes.Any() && !_scenes.Any(scene => scene.Name == sceneName))
                throw new Exception($"Scene with name {sceneName} does not exist");
            else if (InitializeScenesAction == null)
                throw new NullReferenceException("The scene creation action is null!");

            InitializeScenesAction.Invoke();

            SetActiveSceneAndBuildIt(sceneName);
        }

        /// <summary>
        /// Load a scene by reference.
        /// </summary>
        public void LoadScene(Scene2D scene)
        {
            if (_scenes.Any() && !_scenes.Any(s => s == scene))
                throw new Exception($"Scene with name {scene.Name} does not exist");
            else if (InitializeScenesAction == null)
                throw new NullReferenceException("The scene creation action is null!");

            InitializeScenesAction.Invoke();

            SetActiveSceneAndBuildIt(scene);
        }

        /// <summary>
        /// Set the active scene by name and build it.
        /// </summary>
        private void SetActiveSceneAndBuildIt(string sceneName)
        {
            if (!_scenes.Any(scene => scene.Name == sceneName))
                throw new Exception($"Scene with name {sceneName} does not exist");

            Scene2D foundScene = _scenes.FirstOrDefault(scene => scene.Name == sceneName);
            foundScene.Build();

            _activeScene = foundScene;
        }

        /// <summary>
        /// Set the active scene by reference and build it.
        /// </summary>
        private void SetActiveSceneAndBuildIt(Scene2D scene)
        {
            if (!_scenes.Any(s => s == scene))
                throw new Exception($"Scene with name {scene.Name} does not exist");

            Scene2D foundScene = _scenes.FirstOrDefault(s => s == scene);
            foundScene.Build();

            _activeScene = _scenes.FirstOrDefault(s => s == foundScene);
        }

        /// <summary>
        /// Reset the scene handler.
        /// </summary>
        public void Reset(bool resetSceneCreationAction = false)
        {
            _scenes.Clear();
            _activeScene = null;

            if (resetSceneCreationAction)
                InitializeScenesAction = null;
        }
    }
}