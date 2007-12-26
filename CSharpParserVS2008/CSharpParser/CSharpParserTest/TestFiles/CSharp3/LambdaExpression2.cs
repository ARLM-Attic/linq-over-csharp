using System;

public class MyClass
{
  public delegate string MyDelegate(int x, string y);

  public static void WriteOut(int x, string y, MyDelegate op)
  {
    Console.WriteLine("{0}{1} --> {2}", x, y, op(x, y));
  }

  public static void Main()
  {
    WriteOut(23, "Hello", (int x, string y) => y + x);
    WriteOut(23, "Hello", delegate(int x, string y) { return y + x; });
    WriteOut(23, "Hello", (x, y) => y + x);
    WriteOut(23, "Hello", (int x, string y) => { return y + x; });
  }
}