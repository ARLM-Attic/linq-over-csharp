struct A { }
struct B { }
struct C { }
struct A { }
struct B { }

namespace MyNamespace
{
  struct A { }
  struct B { }
  struct C { }

  namespace SubNamespace
  {
    struct A { }
    struct B { }
    struct C { }
  }
}

namespace MyNamespace.SubNamespace
{
  struct A { }
  struct B { }
  struct C { }
}