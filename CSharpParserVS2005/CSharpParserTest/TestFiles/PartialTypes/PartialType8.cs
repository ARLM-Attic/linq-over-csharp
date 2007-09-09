class A { }
class B { }
class C { }
class A<T> { }
class B<T> { }

namespace MyNamespace
{
  class A { }
  class B { }
  class C { }

  namespace SubNamespace
  {
    class A<T> { }
    class B<T> { }
    class C<T> { }
  }
}

namespace MyNamespace.SubNamespace
{
  class A<T,U> { }
  class B<T,U> { }
  class C<T,U> { }
}