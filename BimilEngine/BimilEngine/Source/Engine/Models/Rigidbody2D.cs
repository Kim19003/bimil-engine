using BimilEngine.Source.Engine.Interfaces;
using BimilEngine.Source.Engine.Objects.Bases;
using Genbox.VelcroPhysics.Dynamics;

namespace BimilEngine.Source.Engine.Models
{
    public class Rigidbody2D : IDestroyable
    {
        /// <summary>
        /// The parent of the rigidbody.
        /// </summary>
        public PhysicsSprite2D Parent => _parent;
        private PhysicsSprite2D _parent;
        /// <summary>
        /// The body of the rigidbody.
        /// </summary>
        public Body Body => _body;
        private Body _body;
        /// <summary>
        /// The interpolation of the rigidbody.
        /// </summary>
        public Interpolation2D Interpolation { get; set; } = Interpolation2D.None;
        /// <summary>
        /// Is the rigidbody destroyed?
        /// </summary>
        public bool IsDestroyed => _isDestroyed;
        private bool _isDestroyed = false;

        public Rigidbody2D(PhysicsSprite2D parent, Body body, Interpolation2D interpolation = Interpolation2D.None)
        {
            _parent = parent;
            _body = body;
            Interpolation = interpolation;

            _body.UserData = _parent;

            _body.OnCollision += _parent.CollisionEnterHandler;
            _body.OnSeparation += _parent.CollisionExitHandler;
        }

        /// <summary>
        /// Destroy the rigidbody.
        /// </summary>
        public void Destroy(bool removeObjectFromScene = true)
        {
            if (_isDestroyed) return;
            
            _parent = null;
            _body.RemoveFromWorld();
            _body = null;

            _isDestroyed = true;
        }
    }
}