using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QuickEye.HowToAsync
{
    public class UsingLockExample : CodeExampleBase
    {
        private const int CorrectResult = 10_000_000;
        private static int sum;
        private static readonly object lockObj = new object();

        [ExampleMethod("Modifying One Resource From Multiple Threads without lock", ExampleType.Bad)]
        private static async UniTaskVoid ModifyingOneResourceFromMultipleThreadsNoLock()
        {
            sum = 0;
            var task1 = UniTask.RunOnThreadPool(IncreaseSumNoLock);
            var task2 = UniTask.RunOnThreadPool(IncreaseSumNoLock);

            await (task1, task2); // Same as: await UniTask.WhenAll(task1, task2);
            Debug.Log($"Sum should be {CorrectResult} but is {sum}");
        }

        [ExampleMethod("Using lock")]
        private static async UniTaskVoid ModifyingOneResourceFromMultipleThreads()
        {
            sum = 0;
            var task1 = UniTask.RunOnThreadPool(IncreaseSumWithLock);
            var task2 = UniTask.RunOnThreadPool(IncreaseSumWithLock);

            await (task1, task2); // Same as: await UniTask.WhenAll(task1, task2);
            Debug.Log($"Sum should be {CorrectResult} and is {sum}");
        }

        private static void IncreaseSumNoLock()
        {
            for (int i = 0; i < CorrectResult/2; i++)
            {
                // Because meany threads are doing the same operation
                sum = sum + 1;
            }
        }

        private static void IncreaseSumWithLock()
        {
            // Lock is important here, without it you won't get an exception but the result will be unpredictable
            // this is because function is invoked 2 times simultaneously on a different threads
            // and both of the calls will try to modify the same variable `sum`
            tl.Log("Waits to Modify sum");
            lock (lockObj)
            {
                tl.Log("Starts Modifying sum");
                for (int i = 0; i < CorrectResult/2; i++)
                {
                    sum += 1;
                }
                tl.Log("Stops Modifying sum");
            }
        }
    }
}