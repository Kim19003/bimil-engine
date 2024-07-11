using Bimil.Engine.Handlers;

namespace Bimil.Engine.Interfaces
{
    public interface IAnimatable
    {
        AnimationHandler AnimationHandler { get; set; }
    }
}