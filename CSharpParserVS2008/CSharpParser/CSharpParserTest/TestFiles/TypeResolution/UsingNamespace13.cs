namespace MyNamespace.SubNamespace
{
  class B { }
}

namespace OtherNamespace
{
  class A { }
  class B : A { }
}

namespace OtherNamespace
{
  using A = MyNamespace.SubNamespace;
  using B = MyNamespace.SubNamespace.B;
  class W : B { } // Error: B is ambiguous
  class X : A.B { } // Error: A is ambiguous
  class Y : A::B { } // Ok: uses N1.N2.B
  class Z : OtherNamespace.B { } // Ok: uses N3.B
}
