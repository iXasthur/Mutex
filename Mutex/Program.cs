using System;

namespace Mutex
{
    class Program
    {
        static void Main(string[] args)
        {
            OSHandle handle = new OSHandle(IntPtr.Zero);
            handle.Dispose();
        }
    }
}