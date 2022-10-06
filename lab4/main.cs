using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class SerialException : Exception
{
    public SerialException() { }

    public SerialException(string txt) : base($"SerialException: {txt}") { }
}

public class SerialNumber : IComparable
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
        set
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
        set
        {
            this.timestamp = value;
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

    public SerialNumber()
    {
        try
        {
            this.Class = "xm";
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

public class Component : IComparable
{
    static private List<SerialNumber> history = new List<SerialNumber>();
    public String name;
    public SerialNumber serial;
    public String country;
    public float price;

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
        Thread.Sleep(1000);
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

    public Box()
    {
        this.list = new List<T>();
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
        foreach (var i in this.list)
            Console.WriteLine(i.ToString());
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
        System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(List<T>));
        System.IO.FileStream file = System.IO.File.Create("./SerializationOverview.xml");
        writer.Serialize(file, this.list);
        file.Close();
    }

    public void Serialize(T o)
    {
        System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(T));
        System.IO.FileStream file = System.IO.File.Create("./obj.xml");
        writer.Serialize(file, o);
        file.Close();
    }

    public void DeSerialize()
    {
        System.Xml.Serialization.XmlSerializer reader =
          new System.Xml.Serialization.XmlSerializer(typeof(List<T>));
        System.IO.StreamReader file = new System.IO.StreamReader(
            "./SerializationOverview.xml");
        this.list = (List<T>)reader.Deserialize(file);
        file.Close();
    }

    public T DeSerializeO()
    {
        System.Xml.Serialization.XmlSerializer reader =
          new System.Xml.Serialization.XmlSerializer(typeof(T));
        System.IO.StreamReader file = new System.IO.StreamReader(
            "./obj.xml");
        var o = (T)reader.Deserialize(file);
        file.Close();
        return o;
    }

}

public class Program
{
    public static void Main(string[] args)
    {
        var b = new Box<Component>();
        var c = new Component("hard drive", "us", 250);
        var c1 = new Component("monitor", "pl", 400);
        b.Add(c);
        b.DisplayBy();
        Console.WriteLine("");
        b.Serialize();
        b.Add(c1);
        b.DisplayBy();
        Console.WriteLine("");
        b.DeSerialize();
        b.DisplayBy();
      Console.WriteLine("\n");
      b.Serialize(c1);
      Console.WriteLine(b.DeSerializeO().ToString());
      
    }
}