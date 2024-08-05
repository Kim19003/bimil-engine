using Bimil.Engine.Models;
using Bimil.Engine.Objects.Bases;
using Genbox.VelcroPhysics.Dynamics;

namespace Bimil.Engine.Other.Extensions
{
    public static class FixtureExtensions
    {
        /// <summary>
        /// Tries to get the parent of the fixture's body.
        /// </summary>
        /// <returns>If found, the parent of the fixture's body. Otherwise, null.</returns>
        public static PhysicsSprite2D GetParentOfBody(this Fixture fixture)
        {
            return fixture?.Body?.GetParent();
        }

        /// <summary>
        /// Sets the name of the fixture.
        /// </summary>
        /// <param name="name">The name to set.</param>
        public static void SetName(this Fixture fixture, string name)
        {
            ((FixtureUserData)fixture?.UserData).Name = name;
        }

        /// <summary>
        /// Tries to get the name of the fixture.
        /// </summary>
        /// <returns>If found, the name of the fixture. Otherwise, null.</returns>
        public static string GetName(this Fixture fixture)
        {
            return ((FixtureUserData)fixture?.UserData)?.Name;
        }

        /// <summary>
        /// Sets the tag of the fixture.
        /// </summary>
        /// <param name="tag">The tag to set.</param>
        public static void SetTag(this Fixture fixture, string tag)
        {
            ((FixtureUserData)fixture?.UserData).Tag = tag;
        }

        /// <summary>
        /// Tries to get the tag of the fixture.
        /// </summary>
        /// <returns>If found, the tag of the fixture. Otherwise, null.</returns>
        public static string GetTag(this Fixture fixture)
        {
            return ((FixtureUserData)fixture?.UserData)?.Tag;
        }
    }
}