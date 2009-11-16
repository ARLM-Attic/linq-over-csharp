public class A<T1>
{
  public T1 a1;
  public T1[] a2;
  public A<T1> a3;
  public A<T1[]> a4;
  public A<A<T1>> a5;
}

public class B
{
  public A<int> b;
}