// References resolvable from root namespace
class A1 : C0
{ }
class A2 : C0.C1
{ }
class A3 : N1.N1C0
{ }
class A4 : N1.N2.N2C0
{ }
class A5 : N1.N2.N2C0.N2C1
{ }

// References resolvable from same namespace
namespace N3
{
  class N3C1 : N3C2
  { }
  class N3C2
  { }
}

// References resolvable from higher level namespace/type
namespace N4
{
  namespace N5
  {
    class N5C1 : N4C1
    { }

    class N5C2 : N6.N6C1
    {
      class N5C2C1 : N5C1 { }
      class N5C2C2 : N4C1 { }
      class N5C2C3 : N5C2C1 { }
    }
  }

  class N4C1
  { }

  namespace N6
  {
    class N6C1
    { }
  }
}

// Base classes
class C0
{
  public class C1
  { }
}

namespace N1
{
  class N1C0
  { }

  namespace N2
  {
    class N2C0
    {
      public class N2C1
      { }
    }
  }
}
