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
        
        private static Mutex _mutex = new Mutex();

        private static void Main(string[] args)
        {
            var thread = new Thread(TerminalWriteLoop);
            thread.Start();
            thread = new Thread(TerminalWriteLoop);
            thread.Start();

            CloseHandleAsync(GetStdHandle(StdOutputHandle), 800);
        }

        private static void TerminalWriteLoop()
        {
            while (true)
                try
                {
                    _mutex.Lock();
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ": I'm alive ^_^");
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ": I'm alive ^_^");
                    _mutex.Unlock();
                    Thread.Sleep(500);
                }
                catch
                {
                    _mutex.Unlock();
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