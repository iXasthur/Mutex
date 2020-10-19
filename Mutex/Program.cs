using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Mutex
{
    internal class Program
    {
        [DllImport("kernel32.dll",
            EntryPoint = "GetStdHandle",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
        
        
        private const int StdOutputHandle = -11;

        private static void Main(string[] args)
        {
            var thread = new Thread(TerminalWriteLoop);
            thread.Start();

            CloseHandleAsync(GetStdHandle(StdOutputHandle), 2000);
        }

        private static void TerminalWriteLoop()
        {
            while (true)
                try
                {
                    Console.WriteLine("I'm alive ^_^");
                    Thread.Sleep(250);
                }
                catch
                {
                    break;
                }
        }

        private static async void CloseHandleAsync(IntPtr ptr, int delay)
        {
            Console.WriteLine("Handle " + ptr + " will be closed in " + delay + " msecs");
            await Task.Delay(delay);
            Console.WriteLine("Closing handle " + ptr);
            new OSHandle(ptr).Dispose();
        }
    }
}