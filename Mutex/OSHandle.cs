using System;
using System.Runtime.InteropServices;

namespace Mutex
{
    internal class OSHandle : IDisposable
    {
        private readonly Mutex _mutex = new Mutex();

        private bool _disposed;

        public OSHandle(IntPtr handle)
        {
            Handle = handle;
        }

        public IntPtr Handle { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [DllImport("Kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);

        ~OSHandle()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            _mutex.Lock();
            if (!_disposed)
            {
                if (disposing && Handle != IntPtr.Zero)
                {
                    CloseHandle(Handle);
                    Handle = IntPtr.Zero;
                }

                _disposed = true;
            }

            _mutex.Unlock();
        }
    }
}