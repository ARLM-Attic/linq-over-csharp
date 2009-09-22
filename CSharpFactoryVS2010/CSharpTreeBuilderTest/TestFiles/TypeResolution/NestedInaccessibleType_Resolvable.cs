using N;

class A : B
{
  Nested a;
}

class B
{
  private class Nested { }
}

namespace N
{
  class Nested
  { }
}