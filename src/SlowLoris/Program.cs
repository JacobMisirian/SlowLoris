using System;
using System.Threading;

namespace SlowLoris
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            new Loris().Attack("127.0.0.1", 80, false, 200);
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
