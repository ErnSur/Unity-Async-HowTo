using System.Net.Http;
using Cysharp.Threading.Tasks;

namespace QuickEye.HowToAsync
{
    // TODO:  I/O bound task bad example (using thread pool)
    public class DownloadFileExample : CodeExampleBase
    {
        // I/O bound Task
        [ExampleMethod("Download File Async")]
        private static async UniTaskVoid DownloadFileAsync()
        {
            Log("Downloading File");
            var client = new HttpClient();
            var getStringTask =
                client.GetByteArrayAsync("http://speedtest.ftp.otenet.gr/files/test10Mb.db");

            // You shouldn't use `UniTask.RunOnThreadPool` for I/O bound work
            var contents = await getStringTask;

            Log($"File Downloaded {contents.Length}");
        }
    }
}