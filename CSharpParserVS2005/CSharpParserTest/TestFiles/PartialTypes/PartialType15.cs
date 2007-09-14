class A { }
class B { }

partial class C { }
partial class C : A { }
partial class C : B { }
partial class C : A, B { }
partial class C : B, B { }
partial class C : B, A { }

