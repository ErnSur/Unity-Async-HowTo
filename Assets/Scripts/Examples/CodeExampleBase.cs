using System.Threading;
using UnityEngine;

namespace QuickEye.HowToAsync
{
    public abstract class CodeExampleBase
    {
        protected static void Log(string message) =>
            Debug.Log($"Thread[{Thread.CurrentThread.ManagedThreadId}] {message}");
    }
}