using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QuickEye.HowToAsync
{
    public class AsyncInvocationBadExample : IExampleScript
    {
        public void ExecuteExample()
        {
            AsyncCodeInvocation_BadExample();
        }
        
        [ExampleMethod("Async Code Invocation")]
        private static void AsyncCodeInvocation_BadExample()
        {
            // without awaiting async methods you wont see any exceptions that are thrown inside those functions
            // and compiler will give you warnings
            ThrowSomeExceptionsAsync();
            Debug.Log(
                $"Exception was thrown but it didn't take an effect, task wasn't awaited or got `.Forget()` call");
        }
        
        private static async UniTask ThrowSomeExceptionsAsync()
        {
            Debug.Log($"Should throw an exception");
            throw new DemoException();
            await UniTask.CompletedTask;
        }
    }
}