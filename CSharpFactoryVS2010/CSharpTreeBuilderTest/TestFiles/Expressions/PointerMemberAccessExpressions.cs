public class PointerMemberAccessExpressions
{
  public unsafe void DummyMethod()
  {
    fixed (Point* p = &_point)
    {
      p->x = 1;              
    }
  }

  private Point _point;
}

struct Point
{
  public int x;
  public int y;
}
