namespace BimilEngine.Source.Engine.Models
{
    public class Collision2D
    {
        public object CurrentCollider { get; set; }
        public object OtherCollider { get; set; }
        public QuickDirection2D CollisionDirection { get; set; }
        
        public Collision2D()
        {
        }

        public Collision2D(object currentCollider, object otherCollider, QuickDirection2D collisionDirection)
        {
            CurrentCollider = currentCollider;
            OtherCollider = otherCollider;
            CollisionDirection = collisionDirection;
        }

        // public bool IsEitherTrigger()
        // {
        //     return CurrentCollider.IsTrigger || OtherCollider.IsTrigger;
        // }
    }
}