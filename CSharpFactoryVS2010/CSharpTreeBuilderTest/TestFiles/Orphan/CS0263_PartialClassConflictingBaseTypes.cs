partial class A : B
{
}
partial class A : C
{
}

class B { }
class C { }

// error CS0263: Partial declarations of 'A' must not specify different base classes