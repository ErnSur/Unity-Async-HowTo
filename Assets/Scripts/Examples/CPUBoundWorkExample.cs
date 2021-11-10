using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QuickEye.HowToAsync
{
    public class CPUBoundWorkExample : CodeExampleBase
    {
        [ExampleMethod("Invoking Unsafe Unity Api Outside Main Thread", ExampleType.Bad)]
        private static async UniTaskVoid Example1()
        {
            Log("Start data calculation");
            try
            {
                var result = await UniTask.RunOnThreadPool(() => DoHeavyComputation(true));
            }
            catch (UnityException)
            {
                // We will get an exception because we tried to use Unity API that is thread sensitive outside of the main thread.
                Debug.LogError($"Data Calculation Failed with exception:");
                throw;
            }
        }

        [ExampleMethod("Switching To Main Thread To Do Heavy Computation", ExampleType.Bad)]
        private static async UniTaskVoid SwitchingToMainThreadToDoHeavyComputation()
        {
            Log("Start data calculation");
            var result = await UniTask.RunOnThreadPool(async () =>
            {
                // We can switch to main thread inside the task
                // but this will eliminate the advantage of running task asynchronously (Game will freeze)
                await UniTask.SwitchToMainThread();
                return DoHeavyComputation(true);
            });
            Log($"Calculate data result {result}");
        }

        [ExampleMethod("Offload Heavy Computation To Thread Pool")]
        private static async UniTaskVoid OffloadHeavyComputationToThreadPool()
        {
            Log("Start data calculation");
            // Run the logic on another thread but this time don't use Unity API
            // Making the method that accepts `useUnityApi` flag is just for the example purpose and not a part of a good practice
            var result = await UniTask.RunOnThreadPool(() => DoHeavyComputation(false));
            Log($"Calculate data result {result}");
        }

        private static int DoHeavyComputation(bool useUnityApi)
        {
            var rnd = new System.Random();
            var sum = 0;
            for (int i = 0; i < 50_000_000; i++)
            {
                sum += useUnityApi ? UnityEngine.Random.Range(1, 3) : rnd.Next(1, 3);
            }

            return sum;
        }
    }
}