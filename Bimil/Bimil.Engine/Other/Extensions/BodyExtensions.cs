using Bimil.Engine.Models;
using Bimil.Engine.Objects.Bases;
using Genbox.VelcroPhysics.Dynamics;

namespace Bimil.Engine.Other.Extensions
{
    public static class BodyExtensions
    {
        /// <summary>
        /// Tries to get the parent of the body.
        /// </summary>
        /// <returns>If found, the parent of the body. Otherwise, null.</returns>
        public static PhysicsSprite2D GetParent(this Body body)
        {
            return ((BodyUserData)body?.UserData).Parent;
        }

        /// <summary>
        /// Tries to get the name of the body.
        /// </summary>
        /// <returns>If found, the name of the body. Otherwise, null.</returns>
        public static string GetName(this Body body)
        {
            return ((BodyUserData)body?.UserData)?.Name;
        }

        /// <summary>
        /// Tries to get the tag of the body.
        /// </summary>
        /// <returns>If found, the tag of the body. Otherwise, null.</returns>
        public static string GetTag(this Body body)
        {
            return ((BodyUserData)body?.UserData)?.Tag;
        }

        /// <summary>
        /// Finds a fixture by name.
        /// </summary>
        /// <param name="name">The name of the fixture to find.</param>
        /// <returns>If found, the fixture. Otherwise, null.</returns>
        public static Fixture FindFixtureByName(this Body body, string name)
        {
            foreach (Fixture fixture in body.FixtureList)
            {
                if (fixture.GetName() == name)
                    return fixture;
            }

            return null;
        }
    }
}