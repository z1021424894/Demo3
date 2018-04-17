using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System.Diagnostics;
using System.Web;
using System.Net;

namespace Async
{
    class Program
    {
        private static readonly Stopwatch sw = new Stopwatch();
        static void Main(string[] args)
        {
            //CancellationTokenSource cts1 = new CancellationTokenSource();

            //Task t1 = new Task((obj) =>
            //{
            //    Console.WriteLine("这是一个单任务");
            //}, cts1);

            //CancellationTokenSource cts2 = new CancellationTokenSource();

            //t1.ContinueWith((task) =>
            //{
            //    Console.WriteLine("这是第二个任务");
            //}, cts2.Token);

            //t1.Start();

            //if (cts1.IsCancellationRequested)
            //{
            //    cts1.Cancel();
            //}
            //byte[] b = Encoding.UTF8.GetBytes("你好啊");
            //Console.WriteLine(Encoding.UTF8.GetString(b));
            sw.Start();

            const string url1 = "http://www.cnblogs.com/";
            const string url2 = "http://www.cnblogs.com/liqingwen/";

            var t1 = CountCharactersAsync(1, url1);
            var t2 = CountCharactersAsync(2, url2);

            for (var i = 0; i < 3; i++)
            {
                ExtraOperation(i + 1);
            }

            Console.WriteLine($"{url1}的字符个数为 {t1.Result}");
            Console.WriteLine($"{url2}的字符个数为 {t2.Result}");
            Console.ReadKey();
        }

        private static async Task<int> CountCharactersAsync(int id, string address)
        {
            var wc = new WebClient();
            Console.WriteLine($"开始调用 id = {id}：{sw.ElapsedMilliseconds} ms");

            var result = await wc.DownloadStringTaskAsync(address);

            Console.WriteLine($"调用完成 id = {id}：{sw.ElapsedMilliseconds} ms");
            return result.Length;
        }
        private static void ExtraOperation(int id)
         {
             //这里是通过拼接字符串进行一些相对耗时的操作
            var s = "";

             for (var i = 0; i< 6000; i++)
             {
                 s += i;
             }
 
             Console.WriteLine($"id = {id} 的 ExtraOperation 方法完成：{sw.ElapsedMilliseconds} ms");
         }
    }
}
