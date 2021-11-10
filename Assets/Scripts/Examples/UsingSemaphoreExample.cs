using System;
using System.Net.Http;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QuickEye.HowToAsync
{
    public class UsingSemaphoreExample : CodeExampleBase
    {
        [ExampleMethod("Using Semaphore I/O Bound")]
        private static async UniTaskVoid UsingSemaphoreIOBound()
        {
            var semaphoreSlim = new SemaphoreSlim(1, 1);
            var contents = 0;
            var cancellationTokenSource = new CancellationTokenSource();
            Debug.Log($"MES1");
            var task1 = DownloadFile(cancellationTokenSource);
            Debug.Log($"MES2");
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            Debug.Log($"MES3");
            //cancellationTokenSource.Cancel();
            Debug.Log($"MES5");
            var task2 = DownloadFile(cancellationTokenSource);
            var task3 = DownloadFile(cancellationTokenSource);

            Debug.Log($"MES6");

            // await UniTask.WhenAny(task1, task2, task3); // Same as: await UniTask.WhenAll(task1, task2);
            Debug.Log($"End: {contents}");

            async UniTask DownloadFile(CancellationTokenSource cts)
            {
                try
                {
                    await semaphoreSlim.WaitAsync(cts.Token);
                    var client = new HttpClient();

                    var getStringTask =
                        client.GetByteArrayAsync("http://speedtest.ftp.otenet.gr/files/test10Mb.db");
                    Debug.Log($"Hejjo");
                    contents = (await getStringTask).Length;
                }
                catch (Exception e)
                {
                    Debug.Log($"MES: {e}");
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }
        }
        [ExampleMethod("Using Semaphore CPU Bound")]
        private static async UniTaskVoid UsingSemaphoreCPUBound()
        {
            var semaphoreSlim = new SemaphoreSlim(1, 1);
            var sum = 0;
            var task1 = UniTask.RunOnThreadPool(IncreaseSum);
            var task2 = UniTask.RunOnThreadPool(IncreaseSum);

            await (task1, task2); // Same as: await UniTask.WhenAll(task1, task2);
            Debug.Log($"End: {sum}");

            void IncreaseSum()
            {
                semaphoreSlim.Wait();
                for (int i = 0; i < 50_000_000; i++)
                {
                    sum += 1;
                }

                semaphoreSlim.Release();
            }
        }
    }
}