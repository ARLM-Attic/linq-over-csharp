public class Point
{
  public double X { get; set; }
  public double Y { get; set; }
}

public class Rectangle
{
  Point p1 = new Point();
  Point p2 = new Point();

  public Point P1
  {
    get { return p1; }
  }
  public Point P2
  {
    get { return p2; }
  }
}

public class MyClass
{
  public static Point P1 = new Point() { X = 0 };
  public static Point P2 = new Point { X = 0 };
  public static Point P3 = new Point { X = 0, Y = 0 };
  public static Rectangle r =
    new Rectangle { P1 = { X = 0, Y = 1 }, P2 = { X = 2, Y = 3 } };
}