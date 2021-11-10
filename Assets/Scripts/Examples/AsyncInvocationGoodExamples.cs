using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QuickEye.HowToAsync
{
    public class AsyncInvocationGoodExamples : IExampleScript
    {
        public void ExecuteExample()
        {
            AwaitAsyncMethod_Example().Forget();
            AwaitingTaskObject_Example().Forget();
            CallAsyncFromNonAsyncMethod_Example();
        }

        [ExampleMethod("AwaitAsyncMethod_Example")]
        private static async UniTaskVoid AwaitAsyncMethod_Example()
        {
            try
            {
                // by awaiting async method call will are waiting for it to finish its job
                await MethodThatCanThrowExceptionAsync();
            }
            catch (DemoException)
            {
                Debug.Log("Exception was caught successfully");
            }
        }

        [ExampleMethod("AwaitingTaskObject_Example")]
        private static async UniTaskVoid AwaitingTaskObject_Example()
        {
            try
            {
                // or like this
                var task = MethodThatCanThrowExceptionAsync();
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
                Debug.Log($"Will throw exception even tho it was invoked in try catch");
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
                await MethodThatCanThrowExceptionAsync();
            }
        }

        private static async UniTask MethodThatCanThrowExceptionAsync()
        {
            Debug.Log($"Should throw an exception");
            throw new DemoException();
            await UniTask.CompletedTask;
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