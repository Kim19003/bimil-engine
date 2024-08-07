using Bimil.Engine.Interfaces;
using Bimil.Engine.Objects.Bases;
using Genbox.VelcroPhysics.Dynamics;

namespace Bimil.Engine.Models
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
        /// Is the rigidbody destroyed?
        /// </summary>
        public bool IsDestroyed => _isDestroyed;
        private bool _isDestroyed = false;

        public Rigidbody2D(PhysicsSprite2D parent, Body body)
        {
            _parent = parent;
            _body = body;

            _body.UserData = new BodyUserData()
            {
                Parent = _parent
            };

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