using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QuickEye.HowToAsync
{
    public class UsingLockExample : CodeExampleBase
    {
        [ExampleMethod("Modifying One Resource From Multiple Threads")]
        private static async UniTaskVoid ModifyingOneResourceFromMultipleThreads()
        {
            var lockObj = new object();
            var sum = 0;
            var task1 = UniTask.RunOnThreadPool(IncreaseSum);
            var task2 = UniTask.RunOnThreadPool(IncreaseSum);

            await (task1, task2); // Same as: await UniTask.WhenAll(task1, task2);
            Debug.Log($"End: {sum}");

            void IncreaseSum()
            {
                // Lock is important here, without it you won't get an exception but the result will be unpredictable
                // this is because function is invoked 2 times simultaneously on a different threads
                // and both of the calls will try to modify the same variable `sum`
                lock (lockObj)
                {
                    for (int i = 0; i < 50_000_000; i++)
                    {
                        sum += 1;
                    }
                }
            }
        }
    }
}