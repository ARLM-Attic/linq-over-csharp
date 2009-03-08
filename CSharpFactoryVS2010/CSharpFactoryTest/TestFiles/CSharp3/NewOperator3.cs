using System.Collections.Generic;

public class Point
{
  public double X { get; set; }
  public double Y { get; set; }
}

public class MyClass
{
  public static List<Point> Points = new List<Point>
  {
    new Point {X = 0, Y = 0},
    new Point {X = 0, Y = 0},
    new Point {X = 0, Y = 0},
  };

  public static List<List<Point>> MorePoints = new List<List<Point>>
  {
    new List<Point> {new Point {X = 0, Y = 0}, new Point {X = 0, Y = 0}, new Point {X = 0, Y = 0} },
    new List<Point> {new Point {X = 0, Y = 0}, new Point {X = 0, Y = 0}, new Point {X = 0, Y = 0} },
    new List<Point> {new Point {X = 0, Y = 0}, new Point {X = 0, Y = 0}, new Point {X = 0, Y = 0} },
  };

  public static Dictionary<string, Point> StringPoints = new Dictionary<string, Point>
  {
    {"1", new Point()},
    {"2", new Point()},
    {"3", new Point()}
  };
}