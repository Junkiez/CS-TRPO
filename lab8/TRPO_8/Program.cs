namespace TRPO_8
{
    internal static class Program
    {
        private static object capacity = 0;
        static void Main()
        {
            Console.Write("Input capacity ->");
            capacity = int.Parse(Console.ReadLine());
            var r = new Random();
            var group = 1;
            var time = 0;
            int count = 0;
            while ((int)capacity > 0)
            {
                var t = new Thread(new ThreadStart(() =>
                {
                    lock (capacity)
                    {
                        var g = group;
                        var count = r.Next(1, 5);
                        if ((int)capacity < count)
                            count = (int)capacity;
                        capacity = (int)capacity - count;
                        if (count == 0) return;
                        var groupTime = r.Next(5, 10);
                        time += groupTime;
                        Thread.Sleep(1000 * groupTime);
                        Console.WriteLine($"Outgoing {count}; group {g}");
                    }
                }));
                t.Start();
                t.Join();
                group += 1;
            }
            Console.WriteLine($"Time: {time}");
        }
    }
}