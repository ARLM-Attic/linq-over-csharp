class A {}
class B {}
class C {}
class A {}
class B {}

namespace MyNamespace
{
  class A {}
  class B {}
  class C {}

  namespace SubNamespace
  {
    class A { }
    class B { }
    class C { }
  }
}

namespace MyNamespace.SubNamespace
{
  class A { }
  class B { }
  class C { }
}