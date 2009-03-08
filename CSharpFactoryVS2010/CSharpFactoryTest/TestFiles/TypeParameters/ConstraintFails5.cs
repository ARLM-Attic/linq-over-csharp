using System.Drawing;

public class MyClass<A, B, C, D>
  where A : Point 
  where B : class, Point
  where C : struct, Point
  where D : IDeviceContext, Point, new()
{ }