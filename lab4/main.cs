using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

class SerialException : Exception
{
    public SerialException() { }

    public SerialException(string txt) : base($"SerialException: {txt}") { }
}

[Serializable]
class SerialNumber : IComparable
{
    private String clas;
    private long timestamp;

    public int CompareTo(object o)
    {
        try
        {
            var sn = (SerialNumber)o;
            return this.timestamp.CompareTo(sn.Timestamp);
        }
        catch
        {
            throw new SerialException("Object not type of SerialNumber!");
        }
    }

    public String Class
    {
        private set
        {
            if (value.Length != 2)
            {
                throw new SerialException("Too long or too short class identificator!");
            }
            this.clas = value;
        }
        get
        {
            return this.clas;
        }
    }

    public long Timestamp
    {
        get
        {
            return this.timestamp;
        }
    }

    public SerialNumber(String clas = "xm")
    {
        try
        {
            this.Class = clas;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        this.timestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
    }

    public override String ToString()
    {
        return $"{this.clas}{this.timestamp}";
    }

}

[Serializable]

class Component : IComparable
{
    private static List<SerialNumber> history;
    private String name;
    private SerialNumber serial;
    private String country;
    private float price;

    public SerialNumber Serial
    {
        get
        {
            return this.serial;
        }
    }

    public String Name
    {
        get
        {
            return this.name;
        }
    }

    public String Country
    {
        get
        {
            return this.country;
        }
    }

    public Component()
    {
        this.name = "None";
        this.serial = new SerialNumber("xx");
        this.country = "ua";
        this.price = 0;

        Component.history.Add(this.serial);
    }

    public Component(String name, String country, float price)
    {
        this.name = name;
        this.serial = new SerialNumber("xx");
        this.country = country;
        this.price = price;

        Component.history.Add(this.serial);
    }

    public int CompareTo(object o)
    {
        try
        {
            var cp = (Component)o;
            return this.serial.CompareTo(cp.serial);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return 0;
        }
    }

    public override String ToString()
    {
        return $"name: {this.name}; serial: {this.serial}; country: {this.country}; price: {this.price}";
    }
}

class Box<T> where T : Component
{
    private List<T> list;

    public void Display()
    {
        foreach (var i in this.list)
            Console.WriteLine(i.ToString());
    }

    public void DisplayBy(String name = "", String country = "")
    {
        if (name != "" && country != "")
            foreach (var i in this.list)
                if (i.Name == name && i.Country == country)
                {
                    Console.WriteLine(i.ToString());
                    return;
                }
        if (name != "")
            foreach (var i in this.list)
                if (i.Name == name)
                {
                    Console.WriteLine(i.ToString());
                    return;
                }
        if (country != "")
            foreach (var i in this.list)
                if (i.Country == country)
                {
                    Console.WriteLine(i.ToString());
                    return;
                }
    }

    public void Sort()
    {
        this.list.Sort();
    }

    public void Add(T elem)
    {
        this.list.Add(elem);
    }

    public void Serialize()
    {  
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fsout = new FileStream("data.bin", FileMode.Create, FileAccess.Write, FileShare.None);
        try
        {
          using (fsout)
          {
            bf.Serialize(fsout, this.list);
          }
        }
        catch
        {
          Console.WriteLine("An error has occured");
        }
    }

}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("");
    }
}