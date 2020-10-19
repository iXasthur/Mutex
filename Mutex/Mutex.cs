using System;
using System.Threading;

namespace Mutex
{
    public class Mutex
    {
        private const int UnlockedId = -1;

        private int _lockThreadId = UnlockedId;

        private static int CurrentThreadId => Thread.CurrentThread.ManagedThreadId;

        public void Lock()
        {
            var sw = new SpinWait();
            while (Interlocked.CompareExchange(ref _lockThreadId, CurrentThreadId, UnlockedId) != UnlockedId)
                sw.SpinOnce();
        }

        public void Unlock()
        {
            if (Interlocked.CompareExchange(ref _lockThreadId, UnlockedId, CurrentThreadId) != CurrentThreadId)
                throw new Exception("Unable to unlock Mutex by " + CurrentThreadId + " Thread");
        }
    }
}