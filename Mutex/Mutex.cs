using System.Threading;

namespace Mutex
{
    public class Mutex
    {
        private int _lockCounter;

        public void Lock()
        {
            var sw = new SpinWait();
            while (Interlocked.CompareExchange(ref _lockCounter, 1, 0) == _lockCounter) sw.SpinOnce();
        }

        public void Unlock()
        {
            Interlocked.Exchange(ref _lockCounter, 0);
        }
    }
}