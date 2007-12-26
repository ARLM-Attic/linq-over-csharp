using System;

public class MyClass
{
  public delegate int MyDelegate(int x);

  public static void WriteOut(int x, MyDelegate op)
  {
    Console.WriteLine("{0} --> {1}", x, op(x));
  }

  public static void Main()
  {
    WriteOut(23, (int x) => 2 * x);
    WriteOut(23, delegate(int x) { return 2 * x; });
    WriteOut(23, x => 2*x);
    WriteOut(23, (int x) => { return 2 * x; });
  }
}