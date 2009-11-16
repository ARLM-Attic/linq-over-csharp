public class NonGeneric
{
  public class Generic1<T1, T2>
  {
    public class Generic2
    {
      public class Generic3<T3>
      {
      }
    }
  }
}

unsafe public class ConstructedTest
{
  NonGeneric.Generic1<int, long>.Generic2.Generic3<string> a1;
  NonGeneric.Generic1<int, long>.Generic2 a2;
  NonGeneric.Generic1<int, long> a3;

  NonGeneric.Generic1<int, long>.Generic2.Generic3<string>[] a4;
  NonGeneric.Generic1<int, long>.Generic2[] a5;
  NonGeneric.Generic1<int, long>[] a6;
  NonGeneric[] a7;

  int* a8;
  int*[] a9;
}