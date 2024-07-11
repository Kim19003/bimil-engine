using System;
using Microsoft.Xna.Framework;

namespace Bimil.Engine.Models
{
    public class Log
    {
        public string Message { get; set; }
        public LogLevel Level { get; set; }
        public DateTime Time { get; set; }
        public LogType Type { get; set; }
        public Vector2 Position { get; set; }
        public ShadowSettings ShadowSettings { get; set; }

        public Log()
        {

        }

        public Log(string message, LogLevel level, DateTime time, LogType type, Vector2 position = default)
        {
            Message = message;
            Level = level;
            Time = time;
            Type = type;
            Position = position;
        }
    }
}