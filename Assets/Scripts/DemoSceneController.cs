using System.Net.Http;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QuickEye.HowToAsync
{
    // TODO:
    // Add README.md
    // explain difference between async and multithreading
    public class DemoSceneController : MonoBehaviour
    {
        [SerializeField]
        private CategoryListView categoryListView;

        [SerializeField]
        private CodeExampleListView exampleList;

        private void Awake()
        {
            categoryListView.Init(
                new ExampleCategory("Async Method Invocation", new[]
                {
                    new CodeExample("Bad Example", AsyncCodeInvocation_BadExample),
                    new CodeExample("Good Examples", UniTask.UnityAction(AsyncCodeInvocation_GoodExamples)),
                }),
                new ExampleCategory("I/O bound work", new[]
                {
                    new CodeExample("Download File", UniTask.UnityAction(DownloadFileAsync)),
                }),
                new ExampleCategory("CPU bound work", new[]
                {
                    new CodeExample("Don't run Unity API on thread pool",
                        UniTask.UnityAction(CalculateComplexData_BadExample)),
                    new CodeExample("Don't switch to main thread to fix this issue",
                        UniTask.UnityAction(CalculateComplexData_BadExample2)),
                    new CodeExample("Run CPU bound work on thread pool without Unity API ",
                        UniTask.UnityAction(CalculateComplexData_GoodExample)),
                }),
                new ExampleCategory("Locking threads", new[]
                {
                    new CodeExample("Modifying One Resource From Multiple Threads",
                        UniTask.UnityAction(ModifyingOneResourceFromMultipleThreads)),
                })
            );
            categoryListView.SelectionChanged += category =>
            {
                exampleList.Setup(category.Examples);
            };
            // exampleList.AddNew().Init("Gotchas", new []
            // {
            //     new CodeExample("What out for thread switch",UniTask.UnityAction(SpawningSecondThread_WithoutTaskRun)),
            // });
        }

        private static void AsyncCodeInvocation_BadExample()
        {
            // without awaiting async methods you wont see any exceptions that are thrown inside those functions
            // and compiler will give you warnings
            ThrowSomeExceptionsAsync();
            Debug.Log(
                $"Exception was thrown but it didn't take an effect, task wasn't awaited or got `.Forget()` call");
        }

        private static async UniTaskVoid AsyncCodeInvocation_GoodExamples()
        {
            try
            {
                // await it like this
                await ThrowSomeExceptionsAsync();
            }
            catch (DemoException)
            {
                Debug.Log("Exception was caught successfully");
            }

            try
            {
                // or like this
                var task = ThrowSomeExceptionsAsync();
                // some other work
                await task;
            }
            catch (DemoException)
            {
                Debug.Log("Exception was caught successfully");
            }

            try
            {
                // or if you don't want to wait until it is finished call Forget() on the UniTask/UniTaskVoid
                // but in this case we won't be able to catch this exception
                ExampleMethod().Forget();
            }
            catch (DemoException)
            {
                // because we didn't awaited the call we won't get here
                Debug.Log("Exception was caught successfully");
            }

            // If the method will never be awaited use `UniTaskVoid` as the return type
            // it is more efficient
            async UniTaskVoid ExampleMethod()
            {
                await ThrowSomeExceptionsAsync();
            }
        }

        // I/O bound Task
        private static async UniTaskVoid DownloadFileAsync()
        {
            LogThread("Downloading File");
            var client = new HttpClient();
            var getStringTask =
                client.GetByteArrayAsync("http://speedtest.ftp.otenet.gr/files/test10Mb.db");

            // You shouldn't use `UniTask.RunOnThreadPool` for I/O bound work
            var contents = await getStringTask;

            LogThread($"File Downloaded {contents.Length}");
        }

        // TODO:  I/O bound task bad example (using thread pool)

        // CPU bound Task
        private static async UniTaskVoid CalculateComplexData_BadExample()
        {
            LogThread("Start data calculation");
            try
            {
                var result = await UniTask.RunOnThreadPool(() => DoSynchronousOperation(true));
            }
            catch (UnityException e)
            {
                // We will get an exception because we tried to use Unity API that is thread sensitive outside of the main thread.
                Debug.LogError($"Data Calculation Failed with exception:");
                throw e;
            }
        }

        // CPU bound Task
        private static async UniTaskVoid CalculateComplexData_BadExample2()
        {
            LogThread("Start data calculation");
            var result = await UniTask.RunOnThreadPool(async () =>
            {
                // We can switch to main thread inside the task
                // but this will eliminate the advantage of running task asynchronously (Game will freeze)
                await UniTask.SwitchToMainThread();
                return DoSynchronousOperation(true);
            });
            LogThread($"Calculate data result {result}");
        }

        // CPU bound Task
        private static async UniTaskVoid CalculateComplexData_GoodExample()
        {
            LogThread("Start data calculation");
            // Run the logic on another thread but this time don't use Unity API
            // Making the method that accepts `useUnityApi` flag is just for the example purpose and not a part of a good practice
            var result = await UniTask.RunOnThreadPool(() => DoSynchronousOperation(false));
            LogThread($"Calculate data result {result}");
        }

        private static int DoSynchronousOperation(bool useUnityApi)
        {
            var rnd = new System.Random();
            var sum = 0;
            for (int i = 0; i < 50_000_000; i++)
            {
                sum += useUnityApi ? UnityEngine.Random.Range(1, 3) : rnd.Next(1, 3);
            }

            return sum;
        }

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

        private static async UniTaskVoid TestIfUnityAPIThreadSafe()
        {
            await UniTask.RunOnThreadPool(() =>
            {
                Debug.Log($"Hejooo");
                //Debug.Log($"MES: {SystemInfo.deviceModel}");
                Debug.Log($"Hejooo2");
            });
        }

        private static async UniTask ThrowSomeExceptionsAsync()
        {
            Debug.Log($"Should throw an exception");
            throw new DemoException();
            await UniTask.CompletedTask;
        }

        // TODO: show how Task.Delay can change your current thread
        private async UniTaskVoid SpawningSecondThread_WithoutTaskRun()
        {
        }

        private static void LogThread(string message) =>
            Debug.Log($"Thread[{Thread.CurrentThread.ManagedThreadId}] {message}");
    }
}