namespace N1
{
  namespace N2
  {
    public class C1
    {
      public class C2<T>
      {
        public static T c1;
      }
    }
  }
}

class A
{
  private static int a1 = N1.N2.C1.C2<int>.c1;  
}