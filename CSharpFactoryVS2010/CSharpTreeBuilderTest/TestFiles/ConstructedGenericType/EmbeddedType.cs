public class A
{
  public class B<T1>
  {
    public class C
    {
      public class D<T2>
      {}
    }
  }
}

public class X<T3>
{
  public A.B<int>.C.D<long> x1;
  public A.B<int>.C.D<T3> x2;
  //public object x3 = typeof(A.B<>.C.D<>);
}