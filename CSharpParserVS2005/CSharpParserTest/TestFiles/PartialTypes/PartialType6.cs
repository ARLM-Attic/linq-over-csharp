class A { }
struct B { }
class C { }
interface A { }
class B { }

namespace MyNamespace
{
  class A { }
  class B { }
  class C { }

  namespace SubNamespace
  {
    class A { }
    struct B { }
    enum C { }
  }
}

namespace MyNamespace.SubNamespace
{
  internal delegate void A();
  interface B { }
  struct C { }
}