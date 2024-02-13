
using Microsoft.Xna.Framework;
using BimilEngine.Source.Engine.Interfaces;

namespace BimilEngine.Source.Engine.Objects.Bases
{
    public abstract class Component2D : IDestroyable
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public Scene2D AssociatedScene { get; set; }
        public object AssociatedGameObject { get; protected set; }
        
        public Component2D(object associatedGameObject, Vector2 position, Vector2 scale,
            string name = "", string tag = "", Scene2D associatedScene = null)
        {
            Name = name;
            Tag = tag;
            Position = position;
            Scale = scale;

            AssociatedScene = associatedScene;
            AssociatedGameObject = associatedGameObject;
        }

        public virtual void Destroy(bool removeObjectFromScene = true)
        {
            AssociatedGameObject = null;
            AssociatedScene = null;
        }
    }
}