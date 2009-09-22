class A
{
  global::A x1;
  global::B<int> x2;
  global::C.D x3;
  global::C.E<int> x4;
  global::C.E<int>.F<long> x5;
}

class B<T1>
{
}

namespace C
{
  class D {}
  class E<T1> 
  {
    public class F<T2> { }
  }
}

