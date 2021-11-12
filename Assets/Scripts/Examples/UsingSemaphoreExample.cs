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

            var task1 = DownloadAndSetFile();
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            var task2 = DownloadAndSetFile();
            var task3 = DownloadAndSetFile();

            await UniTask.WhenAny(task1, task2, task3); 
            //await UniTask.WhenAll(task1, task2);
            Debug.Log($"End: {contents}");

            async UniTask DownloadAndSetFile()
            {
                tl.Log("Waits to download");
                await semaphoreSlim.WaitAsync();
                tl.Log("Starts download");
                try
                {
                    var file = (await DownloadFileAsync()).Length;
                    contents = file;
                }
                finally
                {
                    semaphoreSlim.Release();
                }
                tl.Log("Finished download");
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

        private static async UniTask<byte[]> DownloadFileAsync()
        {
            var client = new HttpClient();
            return await client.GetByteArrayAsync("http://speedtest.ftp.otenet.gr/files/test10Mb.db");
        }
    }
}