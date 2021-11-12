using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QuickEye.HowToAsync
{
    public class CatchingExceptionsInAsyncCode : CodeExampleBase
    {
        [ExampleMethod("Missing exceptions in async code", ExampleType.Bad)]
        private static void Example()
        {
            // without awaiting async methods you wont see any exceptions that are thrown inside those functions
            // This is dangerous because if anything will break inside not awaited async method you wont know about it
            // Compiler should give you warnings about it
            ThrowSomeExceptionsAsync();
            Debug.Log(
                $"Exception was thrown but it didn't take an effect, task wasn't awaited or got `.Forget()` call");
        }
        
        [ExampleMethod("Missing exceptions in async code 2")]
        private static void Example2()
        {
            try
            {
                // if you don't want to wait until task is finished call Forget() on the UniTask/UniTaskVoid
                // but in this case we won't be able to catch this exception
                // To work around it you should handle all exceptions inside `ThrowSomeExceptionsAsync`
                Debug.Log($"Will throw exception even tho it was invoked in try catch");
                ThrowSomeExceptionsAsync().Forget();
            }
            catch (DemoException)
            {
                // because we didn't awaited the call we won't get here
                Debug.Log("Exception was caught successfully");
            }
        }
        
        [ExampleMethod("Properly catching exception")]
        private static async UniTaskVoid Example3()
        {
            try
            {
                await ThrowSomeExceptionsAsync();
            }
            catch (DemoException)
            {
                Debug.Log("Exception was caught successfully");
            }
        }
        
        private static async UniTask ThrowSomeExceptionsAsync()
        {
            Debug.Log($"Should throw an exception");
            throw new DemoException();
            await UniTask.CompletedTask;
        }
    }
}