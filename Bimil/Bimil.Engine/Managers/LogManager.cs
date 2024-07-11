using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Bimil.Engine.Models;
using Bimil.Engine.Objects;
using Bimil.Engine.Other;

namespace Bimil.Engine.Managers
{
    public static class LogManager
    {
        public static string LogFilePath { get; set; } = string.Empty;

        public static Log[] ScreenLogs => _screenLogs.ToArray();
        private static readonly HashSet<Log> _screenLogs = new();
        public static Log[] ConsoleLogs => _consoleLogs.ToArray();
        private static readonly HashSet<Log> _consoleLogs = new();
        public static Log[] FileLogs => _fileLogs.ToArray();
        private static readonly HashSet<Log> _fileLogs = new();

        public static Log[] AllLogs
        {
            get
            {
                List<Log[]> logs = new()
                {
                    ScreenLogs,
                    ConsoleLogs,
                    FileLogs
                };

                return logs.SelectMany(l => l).ToArray();
            }
        }

        public static bool EnableScreenLogging { get; set; } = true;
        public static bool EnableConsoleLogging { get; set; } = true;
        public static bool EnableFileLogging { get; set; } = true;

        public static IReadOnlyDictionary<string, string> Abbreviations { get; } = new Dictionary<string, string>
        {
            { "INFO", "Information" },
            { "WARN", "Warning" },
            { "ERROR", "Error" },
            { "DEBUG", "Debug" },

            { "PUA", "Potentially Unwanted Action" },
        };

        /// <summary>
        /// The shown screen logs. Key = order of the log, value = the log.
        /// </summary>
        public static Dictionary<int, Log> ShownScreenLogs { get; } = new();
        public static Vector2 LogScreenStartPosition
        {
            get
            {
                Camera2D camera = Root.EngineCore.ActiveScene.ActiveCameras.FirstOrDefault();
                Rectangle cameraScreenPointBounds = camera.Viewport.Bounds;
                return new(cameraScreenPointBounds.X + 20, cameraScreenPointBounds.Y + cameraScreenPointBounds.Height - 40);
            }
        }
        public static int ShownLogVerticalSpacing { get; } = -30;

        private static readonly int _maxShownScreenLogs = 10;
        public static Log DoScreenLog(string message, LogLevel logLevel = LogLevel.Information, int lifeTime = 5000, ShadowSettings shadowSettings = null)
        {
            if (!EnableScreenLogging)
                return null;

            Vector2 logScreenStartPosition = LogScreenStartPosition;
            Vector2 logScreenPosition = logScreenStartPosition;

            if (ShownScreenLogs.Count >= _maxShownScreenLogs) // If trying to show more than X logs, remove the first log from the shown logs
            {
                int firstLogKey = ShownScreenLogs.Keys.Min();
                Log firstLog = ShownScreenLogs[firstLogKey];

                Root.EngineCore.ActiveScene.RemoveDraw(new(firstLog, Color.White));
                ShownScreenLogs.Remove(firstLogKey);

                ShownScreenLogs.RearrangeSequence(logScreenStartPosition, ShownLogVerticalSpacing);
            }

            if (ShownScreenLogs.Any())
            {
                Log lastLog = ShownScreenLogs[ShownScreenLogs.Keys.Max()];

                logScreenPosition = lastLog.Position + new Vector2(0, ShownLogVerticalSpacing);
            }

            Log thisLog = new(message, logLevel, DateTime.Now, LogType.Screen, logScreenPosition)
            {
                ShadowSettings = shadowSettings
            };

            ShownScreenLogs.AddToSequence(thisLog);

            Color logColor = logLevel switch
            {
                LogLevel.Warning => Color.Orange,
                LogLevel.Error => Color.Red,
                LogLevel.Debug => Color.Cyan,
                _ => Color.White
            };

            Root.EngineCore.ActiveScene.AddOrUpdateDraw(new(thisLog, logColor, lifeTime > 0 ? lifeTime : 0));
            
            _screenLogs.Add(thisLog);

            return thisLog;
        }

        public static Log DoConsoleLog(string message, LogLevel logLevel = LogLevel.Information, bool useForegroundColoring = false)
        {
            if (!EnableConsoleLogging)
                return null;

            DateTime dateTimeNow = DateTime.Now;

            switch (logLevel)
            {
                case LogLevel.Warning:
                    if (useForegroundColoring)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[WARN ({dateTimeNow})] {message}");
                    break;
                case LogLevel.Error:
                    if (useForegroundColoring)
                        Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[ERROR ({dateTimeNow})] {message}");
                    break;
                case LogLevel.Debug:
                    if (useForegroundColoring)
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"[DEBUG ({dateTimeNow})] {message}");
                    break;
                default:
                    if (useForegroundColoring)
                        Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"[INFO {dateTimeNow}] {message}");
                    break;
            }

            if (useForegroundColoring)
                Console.ResetColor();

            Log thisLog = new(message, logLevel, dateTimeNow, LogType.Console);

            _consoleLogs.Add(thisLog);

            return thisLog;
        }

        public static Log DoFileLog(string message, LogLevel logLevel = LogLevel.Information)
        {
            if (!EnableFileLogging)
                return null;

            if (string.IsNullOrEmpty(LogFilePath))
                throw new Exception("Log file path is not set!");

            DateTime dateTimeNow = DateTime.Now;

            switch (logLevel)
            {
                case LogLevel.Warning:
                    File.AppendAllText(LogFilePath, $"[WARN ({dateTimeNow})] {message}\n");
                    break;
                case LogLevel.Error:
                    File.AppendAllText(LogFilePath, $"[ERROR ({dateTimeNow})] {message}\n");
                    break;
                case LogLevel.Debug:
                    File.AppendAllText(LogFilePath, $"[DEBUG ({dateTimeNow})] {message}\n");
                    break;
                default:
                    File.AppendAllText(LogFilePath, $"[INFO ({dateTimeNow})] {message}\n");
                    break;
            }

            Log thisLog = new(message, logLevel, dateTimeNow, LogType.File);

            _fileLogs.Add(thisLog);

            return thisLog;
        }

        public static void ClearShownScreenLogs()
        {
            if (Root.EngineCore.ActiveScene != null)
            {
                Draw[] logDraws = Root.EngineCore.ActiveScene.Draws.Where(dw => dw.Object is Log).ToArray();
                Root.EngineCore.ActiveScene.RemoveDraws(logDraws);
            }

            ShownScreenLogs.Clear();
        }
    }
}