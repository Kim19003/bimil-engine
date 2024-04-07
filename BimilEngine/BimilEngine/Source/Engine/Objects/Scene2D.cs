using System.Collections.Generic;
using System.Linq;
using BimilEngine.Source.Engine.Interfaces;
using BimilEngine.Source.Engine.Managers;
using BimilEngine.Source.Engine.Models;
using BimilEngine.Source.Engine.Objects.Bases;

namespace BimilEngine.Source.Engine.Objects
{
    public class Scene2D
    {
        /// <summary>
        /// Name of the scene.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gadgets collection. Use the AddGadget(s) and RemoveGadget(s) methods to add and remove gadgets.
        /// </summary>
        public IReadOnlyCollection<object> Gadgets => _gadgets;
        private readonly HashSet<object> _gadgets = new();
        /// <summary>
        /// Sprites collection. Use the AddSprite(s) and RemoveSprite(s) methods to add and remove sprites.
        /// </summary>
        public IReadOnlyCollection<object> Sprites => _sprites;
        private readonly HashSet<object> _sprites = new();
        /// <summary>
        /// Debug draws collection. Use the AddDraw(s) and RemoveDraw(s) methods to add and remove draws.
        /// </summary>
        public IReadOnlyCollection<Draw> Draws => _draws;
        private readonly HashSet<Draw> _draws = new();
        // Components
        // TODO: Add component collections if needed

        // Everything
        public IReadOnlyCollection<object> Everything
        {
            get
            {
                List<IReadOnlyCollection<object>> everything = new()
                {
                    Gadgets,
                    Sprites,
                    Draws,
                    // TODO: Add all new collections here
                };
                
                return everything.SelectMany(e => e).ToList();
            }
        }

        /// <summary>
        /// Active cameras collection. Use the AddActiveCamera(s) and RemoveActiveCamera(s) methods to add and remove active cameras.
        /// </summary>
        public IReadOnlyCollection<Camera2D> ActiveCameras => _activeCameras;
        private readonly HashSet<Camera2D> _activeCameras = new();

        public Scene2D(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Add a gadget.
        /// </summary>
        public void AddGadget(object gadget)
        {
            if (gadget is not Gadget2D) throw new System.Exception("Gadget must be of type Gadget2D");

            if (((Gadget2D)gadget).AssociatedScene == null)
                ((Gadget2D)gadget).AssociatedScene = this;
            else if (((Gadget2D)gadget).AssociatedScene != this)
                throw new System.Exception("Gadget must be associated with this scene");

            _gadgets.Add(gadget);
        }

        /// <summary>
        /// Add gadgets.
        /// </summary>
        public void AddGadgets(object[] gadgets)
        {
            foreach (object gadget in gadgets)
            {
                AddGadget(gadget);
            }
        }

        /// <summary>
        /// Remove a gadget.
        /// </summary>
        public void RemoveGadget(object gadget, bool destroyGadget = true)
        {
            if (gadget is not Gadget2D) throw new System.Exception("Gadget must be of type Gadget2D");

            if (destroyGadget)
            {
                Gadget2D gadgetReference = (Gadget2D)Gadgets.FirstOrDefault(t => t == gadget);
                gadgetReference.Destroy(removeObjectFromScene: false);
            }

            _gadgets.Remove(gadget);
        }

        /// <summary>
        /// Remove gadgets.
        /// </summary>
        public void RemoveGadgets(object[] gadgets, bool destroyGadgets = true)
        {
            foreach (object gadget in gadgets)
            {
                RemoveGadget(gadget, destroyGadgets);
            }
        }

        /// <summary>
        /// Add a sprite.
        /// </summary>
        public void AddSprite(object sprite)
        {
            if (sprite is not Sprite2D && sprite is not PhysicsSprite2D) throw new System.Exception("Sprite must be of type Sprite2D or PhysicsSprite2D");

            if (((Transform2D)sprite).AssociatedScene == null)
                ((Transform2D)sprite).AssociatedScene = this;
            else if (((Transform2D)sprite).AssociatedScene != this)
                throw new System.Exception("Sprite must be associated with this scene");

            if (sprite is Sprite2D sprite2D && _sprites.Any(s => s is PhysicsSprite2D))
                LogManager.DoConsoleLog($"PUA: Sprite '{sprite2D.Name}' is a Sprite2D, but there are PhysicsSprite2Ds in the scene.", LogLevel.Warning);
            else if (sprite is PhysicsSprite2D physicsSprite2D && _sprites.Any(s => s is Sprite2D))
                LogManager.DoConsoleLog($"PUA: Sprite '{physicsSprite2D.Name}' is a PhysicsSprite2D, but there are Sprite2Ds in the scene.", LogLevel.Warning);

            _sprites.Add(sprite);
        }

        /// <summary>
        /// Add sprites.
        /// </summary>
        public void AddSprites(object[] sprites)
        {
            foreach (object sprite in sprites)
            {
                AddSprite(sprite);
            }
        }

        /// <summary>
        /// Remove a sprite.
        /// </summary>
        public void RemoveSprite(object sprite, bool destroySprite = true)
        {
            if (sprite is not Sprite2D && sprite is not PhysicsSprite2D) throw new System.Exception("Sprite must be of type Sprite2D or PhysicsSprite2D");

            if (destroySprite)
            {
                object spriteReference = Sprites.FirstOrDefault(s => s == sprite);
                if (spriteReference is IDestroyable sprite2D)
                    sprite2D.Destroy(removeObjectFromScene: false);
            }

            _sprites.Remove(sprite);
        }

        /// <summary>
        /// Remove sprites.
        /// </summary>
        public void RemoveSprites(object[] sprites, bool destroySprites = true)
        {
            foreach (object sprite in sprites)
            {
                RemoveSprite(sprite, destroySprites);
            }
        }

        /// <summary>
        /// Add or update an object to be drawn. Uses draw.Object as the reference.
        /// </summary>
        public void AddOrUpdateDraw(Draw draw)
        {
            Draw existingDraw = Draws.FirstOrDefault(d => d.Object == draw.Object);
            if (existingDraw != null)
            {
                existingDraw.Color = draw.Color;
                existingDraw.LifeTime = draw.LifeTime;
                existingDraw.CameraLevel = draw.CameraLevel;
            }
            else
            {
                _draws.Add(draw);
            }
        }

        /// <summary>
        /// Add or update objects to be drawn. Uses draw.Object as the reference.
        /// </summary>
        public void AddOrUpdateDraws(Draw[] draws)
        {
            foreach (Draw draw in draws)
            {
                AddOrUpdateDraw(draw);
            }
        }

        /// <summary>
        /// Remove an object from being drawn. Uses draw.Object as the reference.
        /// </summary>
        public void RemoveDraw(Draw draw)
        {
            Draw existingDraw = Draws.FirstOrDefault(d => d.Object == draw.Object);
            if (existingDraw != null)
            {
                _draws.Remove(existingDraw);
            }
        }

        /// <summary>
        /// Remove objects from being drawn. Uses draw.Object as the reference.
        /// </summary>
        public void RemoveDraws(Draw[] draws)
        {
            foreach (Draw draw in draws)
            {
                RemoveDraw(draw);
            }
        }

        /// <summary>
        /// Add a camera to be used for drawing.
        /// </summary>
        public void AddActiveCamera(Camera2D camera)
        {
            _activeCameras.Add(camera);
        }

        /// <summary>
        /// Add cameras to be used for drawing.
        /// </summary>
        public void AddActiveCameras(Camera2D[] cameras)
        {
            foreach (Camera2D camera in cameras)
            {
                AddActiveCamera(camera);
            }
        }

        /// <summary>
        /// Remove a camera from being used for drawing.
        /// </summary>
        public void RemoveActiveCamera(Camera2D camera)
        {
            _activeCameras.Remove(camera);
        }

        /// <summary>
        /// Remove cameras from being used for drawing.
        /// </summary>
        public void RemoveActiveCameras(Camera2D[] cameras)
        {
            foreach (Camera2D camera in cameras)
            {
                RemoveActiveCamera(camera);
            }
        }
    }
}