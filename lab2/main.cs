using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

public class HightEventArgs : EventArgs
{
    private Double oldHeight;
    private Double newHeight;

    public Double OldHeight
    {
        get { return this.oldHeight; }
    }

    public Double NewHeight
    {
        get { return this.newHeight; }
    }

    public HightEventArgs(Double oldHeight, Double newHeight)
    {
        this.oldHeight = oldHeight;
        this.newHeight = newHeight;
    }
}

public delegate void HeightEventHandler(object sender, HightEventArgs args);

class Altmeter
{
    private Double height = 0;
    public event HeightEventHandler autopilot;

    public Double Height
    {
        get
        {
            return this.height;
        }
        set
        {
            var p = this.height;
            this.height = value;
            if (value >= 5000)
            {
                this.autopilot(this, new HightEventArgs(p, value));
            }
        }
    }

    public Altmeter(HeightEventHandler h)
    {
        this.addListener(h);
    }

    public void addListener(HeightEventHandler h)
    {
        this.autopilot += h;
    }

    public void delListener(HeightEventHandler h)
    {
        this.autopilot -= h;
    }

}

class Plane
{
    private String name;
    private Double size;
    private Double height;
    public Double Height
    {
        get
        {
            return this.height;
        }
        set
        {
            this.alt.Height = value;
            this.height = value;
        }
    }

    private Altmeter alt;

    public Plane(String name, Double size)
    {
        this.alt = new Altmeter(this.HeightHandler);
        this.name = name;
        this.size = size;
    }

    ~Plane()
    {
        this.alt.delListener(this.HeightHandler);
    }

    private void HeightHandler(object sender, HightEventArgs args)
    {
        Console.WriteLine($"Autopilot on: {args.OldHeight}->{args.NewHeight}");
        this.height = 4500;
        this.alt.Height = 4500;
    }
}

class Program
{
    public static void Main(string[] args)
    {
        var p = new Plane("Мрія", 67);
        var rand = new Random();
        for (var i = 0; i < 5; i++)
        {
            p.Height = rand.Next(3000, 8000);
            Console.WriteLine("<----->");
        }
    }
}
