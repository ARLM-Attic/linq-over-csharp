namespace N1
{
  namespace N2
  {
    public class C1
    {
      public int c1;
    }
  }
}

class A
{
  private int a1;

  void M(N1.N2.C1 c)
  {
    a1 = c.c1;
  }
}