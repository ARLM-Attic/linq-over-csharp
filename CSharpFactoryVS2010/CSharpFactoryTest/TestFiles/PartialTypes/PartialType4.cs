enum A { Value1 }
enum B { Value1 }
enum C { Value1 }
enum A { Value1 }
enum B { Value1 }

namespace MyNamespace
{
  enum A { Value1 }
  enum B { Value1 }
  enum C { Value1 }

  namespace SubNamespace
  {
    enum A { Value1 }
    enum B { Value1 }
    enum C { Value1 }
  }
}

namespace MyNamespace.SubNamespace
{
  enum A { Value1 }
  enum B { Value1 }
  enum C { Value1 }
}