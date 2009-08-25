partial class A<T> : B, I1, I2
{
 int a1;
}
partial class A<T> : I2, I3
{
 int a2;
}

class B { }

interface I1 { }
interface I2 { }
interface I3 { }