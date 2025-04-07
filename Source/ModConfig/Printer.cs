using KL.Utils;
using UnityEngine;

namespace ModConfig.Misc
{
    internal static class Printer
    {
        public enum Verbose { Normal, StackTrace, FullDebug }
        private static readonly string LogMessage = "[MC]> ";
        public static void Log(object toLog, Verbose modifier = Verbose.Normal)
        {
            string messageToLog = string.Empty;

            messageToLog += $"{HandleVerbose(modifier, toLog)}";
            D.Log(messageToLog);
        }
        public static void Warn(object toLog, Verbose modifier = Verbose.Normal)
        {
            string messageToLog = $"{HandleVerbose(modifier, toLog)}";
            Debug.LogWarning(messageToLog);
        }
        public static void Error(object toLog, Verbose modifier = Verbose.Normal)
        {
            string messageToLog = $"{HandleVerbose(modifier, toLog)}";
            D.Err(messageToLog);
        }
        private static string HandleVerbose(Verbose verbose, object toLog)
        {
            switch (verbose)
            {
                case Verbose.Normal:
                    return LogMessage + (toLog?.ToString() ?? "null");
                case Verbose.StackTrace:
                    return LogMessage + $"\nStacktrace:\n{new System.Diagnostics.StackTrace().ToString()}\n";
                default:
                    return LogMessage + (toLog?.ToString() ?? "");
            }
        }
    }
}
