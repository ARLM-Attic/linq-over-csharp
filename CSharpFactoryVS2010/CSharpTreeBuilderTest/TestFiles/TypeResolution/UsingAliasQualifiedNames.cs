using X = Y;

class A
{
  X::Y1 x0;
  X::V.W.Y2 x1;
  X::Y3<int,long> x2;
}

namespace B
{
  using X = Y;

  namespace C
  {
    class A
    {
      X::Y1 x0;
      X::V.W.Y2 x1;
      X::Y3<int, long> x2;
    }
  }
}

namespace Y
{
  class Y1
  {}

  namespace V.W
  {
    class Y2 
    {}
  }

  class Y3<T1, T2>
  { }
}