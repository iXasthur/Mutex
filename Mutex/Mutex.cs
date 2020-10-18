using System.Threading;

namespace Mutex
{
    public class Mutex
    {
        private int _lockCounter;

        public void Lock()
        {
            while (Interlocked.CompareExchange(ref _lockCounter, 1, 0) == _lockCounter) Thread.Sleep(10);
        }

        public void Unlock()
        {
            Interlocked.Exchange(ref _lockCounter, 0);
        }
    }
}