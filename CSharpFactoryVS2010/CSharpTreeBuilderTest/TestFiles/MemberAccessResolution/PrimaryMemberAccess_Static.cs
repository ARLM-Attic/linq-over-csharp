namespace N1
{
  namespace N2
  {
    public class C1
    {
      public static int c1;
    }
  }
}

class A
{
  private static int a1 = N1.N2.C1.c1;  
}