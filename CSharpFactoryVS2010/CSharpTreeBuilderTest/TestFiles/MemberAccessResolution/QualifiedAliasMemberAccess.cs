using N1_Alias = N1;
using N2_Alias = N1.N2;

namespace N1
{
  namespace N2
  {
    public class C1
    {
      public static int c1;
    }

    namespace N3
    {
      public class D1
      {
        public static int d1;
      }
    }
  }
}

class A
{
  private static int a1 = global::N1.N2.C1.c1;
  private static int a2 = N1_Alias::N2.N3.D1.d1;
  private static int a3 = N1_Alias::N2.C1.c1;
  private static int a4 = N2_Alias::C1.c1;
}