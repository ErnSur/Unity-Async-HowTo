using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QuickEye.HowToAsync
{
    public class InvokingAsyncCodeWithUniTask : CodeExampleBase
    {
        [ExampleMethod("Await Async Method")]
        private static async UniTaskVoid Example1()
        {
            tl.Log("before invoke");
            // when you await async method you are waiting for it to finish its job
            await DoSomethingAsync();
            tl.Log("after invoke");
        }

        [ExampleMethod("Awaiting Task Object")]
        private static async UniTaskVoid AwaitingTaskObject_Example()
        {
            try
            {
                // or like this
                var task = DoSomethingAsync();
                // some other work
                await task;
            }
            catch (DemoException)
            {
                Debug.Log("Exception was caught successfully");
            }
        }

        [ExampleMethod("CallAsyncFromNonAsyncMethod_Example")]
        private static void CallAsyncFromNonAsyncMethod_Example()
        {
            try
            {
                // if you don't want to wait until task is finished call Forget() on the UniTask/UniTaskVoid
                // but in this case we won't be able to catch this exception
                tl.Log($"Will throw exception even tho it was invoked in try catch");
                ExampleMethod().Forget();
            }
            catch (DemoException)
            {
                // because we didn't awaited the call we won't get here
                Debug.Log("Exception was caught successfully");
            }

            // If the method will never be awaited use `UniTaskVoid` as the return type
            // it is more efficient and takes care of throwing exceptions on the main thread
            async UniTaskVoid ExampleMethod()
            {
                await DoSomethingAsync();
            }
        }

        private static async UniTask DoSomethingAsync()
        {
            tl.Log("Async method start");
            await UniTask.Delay(TimeSpan.FromSeconds(.5f));
            tl.Log("Async method end");
        }

        private static async UniTaskVoid TestIfUnityAPIThreadSafe()
        {
            await UniTask.RunOnThreadPool(() =>
            {
                Debug.Log($"Hejooo");
                //Debug.Log($"MES: {SystemInfo.deviceModel}");
                Debug.Log($"Hejooo2");
            });
        }
    }
}