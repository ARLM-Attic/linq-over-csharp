public class Point
{
  public double X { get; set;}
  public double Y { get; set;}
}

public class MyClass
{
  public static Point P1 = new Point() {X = 0};
  public static Point P2 = new Point { X = 0 };
  public static Point P3 = new Point { X = 0, Y = 0 };
}