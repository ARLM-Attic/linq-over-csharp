using System;

public class MyClass
{
  public delegate int MyDelegate();

  public static void WriteOut(MyDelegate op)
  {
    Console.WriteLine("{0}", op());
  }

  public static void Main()
  {
    WriteOut(() => 123);
    WriteOut(delegate() { return 123; });
    WriteOut(() => { return 123; });
  }
}