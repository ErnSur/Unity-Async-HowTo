using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace QuickEye.HowToAsync
{
    public class SwitchingThreadsAccidentallyExample : CodeExampleBase
    {
        // TODO: Doesn't work?
        [ExampleMethod("Thread Can Change After Await", ExampleType.ImportantToRemember)]
        private static async UniTaskVoid ThreadCanChangeAfterAwait()
        {
            Log("Work started on main thread");
            await Task.Delay(TimeSpan.FromSeconds(1));
            await Task.Delay(TimeSpan.FromSeconds(4));
            Log("Work continues on another thread");
        }
        
        [ExampleMethod("Using UniTask API Equivalents", ExampleType.ImportantToRemember)]
        private static async UniTaskVoid UsingUniTaskAPIEquivalents()
        {
            Log("Work started on main thread");
            // UniTask equivalent methods have more options related to Unity environment
            await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: true, PlayerLoopTiming.Update);
            Log("Work continues on main thread");
        }
    }
}