namespace N1
{
  public class C1
  {
    public static int c1;
  }
}

class A
{
  private static int a1 = N1.C1.c1;
}