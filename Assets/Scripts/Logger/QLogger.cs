using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace QuickEye.HowToAsync
{
    public class ThreadLogger : ILogHandler
    {
        private static Dictionary<int, Color> threadColors = new Dictionary<int, Color>();

        private static Color[] ColorPool = new[]
        {
            Color.green,
            Color.cyan,
            Color.magenta,
            Color.red,
        };

        public static void ClearColorCache() => threadColors.Clear();

        private static Color GetThreadColor(int threadId)
        {
            if (!threadColors.TryGetValue(threadId, out var color))
            {
                threadColors[threadId] = color = ColorPool[threadColors.Count];
            }

            return color;
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var mes = $"Thread[{threadId}]".In(GetThreadColor(threadId)) + $" {format}";
            Debug.unityLogger.logHandler.LogFormat(logType, context, mes, args);
        }

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            Debug.unityLogger.LogException(exception, context);
        }
    }
}