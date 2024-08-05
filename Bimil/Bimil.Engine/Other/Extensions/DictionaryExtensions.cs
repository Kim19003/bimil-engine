using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Bimil.Engine.Models;

namespace Bimil.Engine.Other.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Adds the log to the shown screen logs sequence. Sets a order number for the log to follow the sequence logic.
        /// </summary>
        public static void AddToSequence(this Dictionary<int, Log> shownScreenLogs, Log log)
        {
            int mininumOrderNumber = shownScreenLogs.Any() ? shownScreenLogs.Keys.Max() : 0;

            shownScreenLogs.Add(++mininumOrderNumber, log);
        }

        /// <summary>
        /// Rearranges the shown screen logs sequence. Sets new order number and position for every log to follow the sequence logic.
        /// </summary>
        public static void RearrangeSequence(this Dictionary<int, Log> shownScreenLogs, Vector2 logScreenStartPosition, int verticalSpacing)
        {
            Dictionary<int, Log> newShownScreenLogs = new();

            int orderNumber = 1;
            Vector2 previousLogPosition = Vector2.Zero;
            foreach (var shownScreenLog in shownScreenLogs.OrderBy(x => x.Key))
            {
                shownScreenLog.Value.Position = orderNumber == 1
                    ? logScreenStartPosition
                    : previousLogPosition + new Vector2(0, verticalSpacing);
                previousLogPosition = shownScreenLog.Value.Position;

                newShownScreenLogs.Add(orderNumber, shownScreenLog.Value);

                orderNumber++;
            }

            shownScreenLogs.Clear();
            foreach (var newShownScreenLog in newShownScreenLogs)
            {
                shownScreenLogs.Add(newShownScreenLog.Key, newShownScreenLog.Value);
            }
        }
    }
}