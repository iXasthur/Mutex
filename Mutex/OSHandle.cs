﻿using System;
using System.Runtime.InteropServices;

namespace Mutex
{
    internal class OSHandle : IDisposable
    {
        [DllImport("Kernel32.dll",
            EntryPoint = "CloseHandle",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern bool CloseHandle(IntPtr handle);
        
        
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