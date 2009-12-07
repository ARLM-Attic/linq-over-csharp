class A<T1,T2>
{
  public void M(T1 p1, T2 p2)
  {}

  public static B<int,long> b;

  public static void D()
  {
    b.a.M(1,2);
  }
}

class B<T3,T4>
{
  public A<T4,T3> a;
}