using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

abstract class Transport
{

    protected string action;
    protected long timestamp;

    protected abstract void SetAction();

    public String GetInfo()
    {
        return $"{this.action} {this.timestamp}";
    }

    protected Transport()
    {
        this.timestamp = long.Parse(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString().Split('.')[1]);
        this.SetAction();
        this.Move();
    }

    protected void Move()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"It's {this.action}");
        Console.ForegroundColor = ConsoleColor.White;
    }
}

class Plane : Transport
{
    public Plane() : base() { }

    protected override void SetAction()
    {
        this.action = "fly";
    }
}

class Car : Transport
{
    protected override void SetAction()
    {
        this.action = "drive";
    }
}

class Ship : Transport
{
    public Ship()
    {
        this.action = "SAIL";
    }

    protected override void SetAction()
    {
        this.action = "sail";
    }
}

interface IVehicleCollection<T>
{
    public T getElem(int index);

    public void pushElem(int index, T elem);

    public System.Collections.IEnumerable elems();

}

class VehicleCollection<T> : IVehicleCollection<T> where T : Transport
{
    private SortedList sortedList;

    public VehicleCollection()
    {
        this.sortedList = new SortedList();
    }

    public T getElem(int index)
    {
        return (T)this.sortedList[index];         // cast
    }

    public void pushElem(int index, T elem)
    {
        this.sortedList.Add(index, elem);
    }

    public System.Collections.IEnumerable elems()
    {
        foreach (var i in this.sortedList)
            yield return i;            // generator
    }

}

class VehicleCollectionT<T> : IVehicleCollection<T> where T : Transport
{
    private SortedList<int, T> sortedList;

    public VehicleCollectionT()
    {
        this.sortedList = new SortedList<int, T>();
    }

    public T getElem(int index)
    {
        return this.sortedList[index];
    }

    public void pushElem(int index, T elem)
    {
        this.sortedList.Add(index, elem);
    }

    public System.Collections.IEnumerable elems()
    {
        foreach (var i in this.sortedList)
            yield return i;
    }

}

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("# Створення і заповнення колекції");
        var collection = new VehicleCollection<Transport>();
        var collectionT = new VehicleCollectionT<Transport>();
        var rand = new Random();
        for (int i = 0; i < 7; i += 1)
        {
            var r = rand.Next(1, 4);
            Transport t;
            switch (r)
            {
                case 1:
                    t = new Car();
                    break;
                case 2:
                    t = new Ship();
                    break;
                default:
                    t = new Plane();
                    break;
            }
            collection.pushElem(i, t);
            Thread.Sleep(150);
            switch (r)
            {
                case 1:
                    t = new Car();
                    break;
                case 2:
                    t = new Ship();
                    break;
                default:
                    t = new Plane();
                    break;
            }
            collectionT.pushElem(i, t);
            Thread.Sleep(150);
        }

        Console.WriteLine("# Обхід узагальненої колекції");

        foreach (var i in collectionT.elems())
        {
            Console.WriteLine(i);
        }

        Console.WriteLine("# Обхід колекції");

        foreach (DictionaryEntry i in collection.elems())
        {
            Console.WriteLine("Key = {0}, Value = {1}", i.Key, i.Value);
        }

        Console.WriteLine("# Отримання елемента узагальненої колекції");

        for (int i = 0; i < 7; i += 1)
        {
            Console.WriteLine(collectionT.getElem(i).GetInfo());
        }

        Console.WriteLine("# Отримання елемента колекції");

        for (int i = 0; i < 7; i += 1)
        {
            Console.WriteLine(collection.getElem(i).GetInfo());
        }
    }
}
