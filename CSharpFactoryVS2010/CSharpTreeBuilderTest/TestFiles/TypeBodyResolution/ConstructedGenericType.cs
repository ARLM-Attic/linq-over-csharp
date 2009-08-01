namespace N
{
  class A<T1, T2>
  {
    public class B1
    { }
    public class B2<T1>
    { }
    public class B3<T3>
    { }

    A<A1, A2>.B1 b1;
    A<A1, A2>.B2<A3> b2;
    A<A1, A2>.B3<A4> b3;
  }

  class A1 { }
  class A2 { }
  class A3 { }
  class A4 { }
}
