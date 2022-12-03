using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;
namespace BlockingCollectionExample
{
    class Car
    {
        public int Year
        {
            get;
            set;
        }        
        public string Name
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"{this.Name} {this.Year}";
        }
    }
    class Program
    {
        private static AutoResetEvent eventX = new AutoResetEvent(false);
        static void Main(string[] args)
        {
            // task 1

            BlockingCollection<Car> park = new BlockingCollection<Car>()
            {
                new Car{Name="Ford", Year=1998},
                new Car{Name="Porche", Year=2000},
                new Car{Name="Subaru", Year=1999},
                new Car{Name="Skoda", Year=2001},
                new Car{Name="Micubishi", Year=2003},
            };

            Console.WriteLine("Task 1");
            var parallelQuery = park.AsParallel().Where(n => n.Year >= 2000).Select(n => n);
            foreach (var i in parallelQuery)
                Console.WriteLine(i.ToString());
            // task 2

            ConcurrentStack<Car> cars = new ConcurrentStack<Car>();

            Thread t = new Thread(new ThreadStart(() =>
            {
                foreach (var i in park)
                {
                    Thread.Sleep(2000);
                    cars.Push(i);
                    eventX.Set();
                }
            }));
            t.Start();

            Console.WriteLine("Task 2");
            Thread client = new Thread(new ThreadStart(() =>
            {
                var count = park.Count();
                while (count>1)
                {
                    eventX.WaitOne();
                    Car c;
                    cars.TryPop(out c);
                    Console.WriteLine(c.ToString());
                    count -= 1;
                }
            }));
            client.Start();
            client.Join();
        }
    }
}