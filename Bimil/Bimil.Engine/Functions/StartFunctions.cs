using System.Collections.Generic;
using System.Linq;
using Bimil.Engine.Handlers;
using Bimil.Engine.Interfaces;

namespace Bimil.Engine.Functions
{
    public static class StartFunctions
    {
        public static void StartEverything(SceneHandler sceneHandler)
        {
            // Get everything from the active scene
            IReadOnlyCollection<object> everything = sceneHandler.ActiveScene.Everything;

            if (everything == null || !everything.Any()) return;

            // Start everything
            foreach (object thing in everything)
            {
                if (thing == null) continue;

                if (thing is IStartable startable)
                    startable.Start();
            }
        }
    }
}