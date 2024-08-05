using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Bimil.Engine.Other.Extensions
{
    public static class KeyboardStateExtensions
    {
        private static readonly Dictionary<Keys, bool> previousKeyStates = new();

        public static bool IsKeyPressed(this KeyboardState currentKeyboardState, Keys key)
        {
            // Check if the key is in the dictionary
            if (!previousKeyStates.ContainsKey(key))
            {
                // If not, add it with an initial value of false
                previousKeyStates.Add(key, false);
            }

            // Check if the key is pressed in the current state and was not pressed in the previous state
            bool isKeyPressedOnce = currentKeyboardState.IsKeyDown(key) && !previousKeyStates[key];

            // Update the previous key state for the next frame
            previousKeyStates[key] = currentKeyboardState.IsKeyDown(key);

            return isKeyPressedOnce;
        }
    }
}