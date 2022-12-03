using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;

namespace TRPO_5
{
    internal static class Program
    {
        public static Random r = new Random();
        public static int time = 0;
        public static int group = 1;
        public static int capacity = 0;
        static ManualResetEvent evtObj = new ManualResetEvent(false);
        static void Main()
        {
            Console.Write("Input capacity ->");
            capacity = int.Parse(Console.ReadLine());
            while (capacity > 0)
            {
                CThread t = new CThread("Потiк 1", evtObj);
                evtObj.WaitOne();
                evtObj.Reset();
                group += 1;
            }
            Console.WriteLine($"Time: {time}");
        }
    }

    public class CThread
    {
        public Thread Thrd;
        ManualResetEvent mre;
        public CThread(string name, ManualResetEvent evt)
        {
            Thrd = new Thread(this.Run);
            Thrd.Name = name;
            mre = evt;
            Thrd.Start();
        }
        void Run()
        {
            var g = Program.group;
            var count = Program.r.Next(1, 5);
            if (Program.capacity < count)
                count = Program.capacity;
            Program.capacity -= count;
            if (count == 0) return;
            var groupTime = Program.r.Next(5, 10);
            Program.time += groupTime;
            Thread.Sleep(1000 * groupTime);
            Console.WriteLine($"Outgoing {count}; group {g}");
            mre.Set();
        }
    }
}