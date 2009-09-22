namespace A
{
  class B
  { }
}

namespace C
{
  using E = A;

  class D : E.B
  { }
}