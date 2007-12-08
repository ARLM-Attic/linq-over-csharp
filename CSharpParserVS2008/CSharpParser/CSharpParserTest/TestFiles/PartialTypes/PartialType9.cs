struct A { }
struct B { }
struct C { }
struct A<T> { }
struct B<T> { }

namespace MyNamespace
{
  struct A { }
  struct B { }
  struct C { }

  namespace SubNamespace
  {
    struct A<T> { }
    struct B<T> { }
    struct C<T> { }
  }
}

namespace MyNamespace.SubNamespace
{
  struct A<T, U> { }
  struct B<T, U> { }
  struct C<T, U> { }
}