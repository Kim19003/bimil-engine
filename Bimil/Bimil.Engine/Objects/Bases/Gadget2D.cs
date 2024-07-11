using System.Linq;
using Microsoft.Xna.Framework;
using Bimil.Engine.Interfaces;

namespace Bimil.Engine.Objects.Bases
{
    public abstract class Gadget2D : Transform2D, IStartable, IUpdatable, IDestroyable
    {
        public Gadget2D(Scene2D associatedScene = null)
            : base(associatedScene)
        {
            // TODO: Add your initialization logic to-be-inherited here
        }

        public virtual void Start()
        {
            // TODO: Add your start logic to-be-inherited here
        }

        public virtual void Update(GameTime gameTime)
        {
            // TODO: Add your update logic to-be-inherited here
        }

        public virtual void FixedUpdate(GameTime gameTime, GameTime fixedGameTime)
        {
            // TODO: Add your fixed update logic to-be-inherited here
        }

        public void Destroy(bool removeObjectFromScene = true)
        {
            if (AssociatedScene != null)
            {
                object gadgetReference = AssociatedScene.Gadgets.FirstOrDefault(s => s == this);
                DestroyAssociatedComponents(gadgetReference);
                if (removeObjectFromScene)
                    AssociatedScene.RemoveGadget(gadgetReference, destroyGadget: false);
                AssociatedScene = null;
            }
        }

        private static void DestroyAssociatedComponents(object gadgetReference)
        {
            // TODO: Add more logic here if needed
        }
    }
}