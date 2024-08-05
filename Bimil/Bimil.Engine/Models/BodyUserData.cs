using Bimil.Engine.Objects.Bases;

namespace Bimil.Engine.Models
{
    public class BodyUserData
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public PhysicsSprite2D Parent { get; set; }
        public object AdditionalData { get; set; }
    }
}