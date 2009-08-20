using B;

class A
{
  private C<int>.D<long> c;
}

namespace B
{
  class C<T1>
  {
    public class D<T2>
    { }
  }
}