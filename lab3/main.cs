using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

interface IRand
{
    void random();
    string ToString();
}

class Elem : IComparable, IRand
{

    public int val
    {
        get;
        private set;
    }

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;
        Elem otherElem = obj as Elem;
        if (otherElem != null)
            return this.val.CompareTo(otherElem.val);
        else
            throw new ArgumentException("Object is not a Elem");
    }

    public override string ToString()
    {
        return this.val.ToString();
    }

    public void random()
    {
        this.val = new Random().Next(0, 23);
    }

    public Elem(int val)
    {
        this.val = val;
    }

    public Elem(object obj)
    {
        if (obj == null) throw new ArgumentException("Object is null");
        Elem otherElem = obj as Elem;
        if (otherElem != null)
            this.val = otherElem.val;
        else
            throw new ArgumentException("Object is not a Elem");
    }

    public Elem()
    {
        this.val = 0;
    }
}


class Matrix<T> where T : class, IComparable, IRand, new()
{

    public T[,] list;
    private int rows;
    private int cols;

    public Matrix(int rows, int cols)
    {
        this.cols = cols;
        this.rows = rows;
        this.list = new T[rows, cols];
        for (var i = 0; i < rows; i += 1)
        {
            for (var j = 0; j < cols; j += 1)
            {
                this.list[i, j] = new T();
                this.list[i, j].random();
            }
        }
        this.Print();
    }

    public void Print()
    {
        for (var i = 0; i < rows; i += 1)
        {
            for (var j = 0; j < cols; j += 1)
            {
                Console.Write($" {this.list[i, j].ToString()} ");
            }
            Console.WriteLine(" ");
        }
    }

    public void NonNullRowsCount()
    {
      var l = this.rows;
      for (var i = 0; i < rows; i += 1)
        {
            for (var j = 0; j < cols; j += 1)
            {
              if(this.list[i, j].CompareTo(new T()) == 0){
                l-=1;
                break;
              }
            }
        }
      Console.WriteLine($" NonNullRowsCount: {l}");
    }


    public void MaxOfMultiple()
    {
      var l = new ArrayList();
      for (var i = 0; i < rows; i += 1)
        {
            for (var j = 0; j < cols; j += 1)
            {
              l.Add(this.list[i, j]);
            }
        }
      l.Sort();
      var a = l.ToArray();
      Array.Reverse(a);
      var e = new T();
      foreach(var i in a){
        if(e.ToString() == i.ToString()){
          Console.WriteLine($" MaxOfMultiple: {i.ToString()}");
          return;
        }
        e = i as T;
      }
    }

}


class Program
{
    public static void Main(string[] args)
    {
      var m = new Matrix<Elem>(3, 5);
      m.NonNullRowsCount();
      m.MaxOfMultiple();
    }
}
