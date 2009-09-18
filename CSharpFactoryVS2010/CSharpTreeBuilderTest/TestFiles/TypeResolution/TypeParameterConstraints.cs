class A<T1, T2, T3, T4>
  where T1 : B, T2, I1, I2<int>, T4, new()
{
}

class B
{ }

interface I1
{ }

interface I2<T>
{ }