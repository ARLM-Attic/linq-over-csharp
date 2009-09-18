class A<T1, T2, T3, T4>
  where T1 : B, T2, I1, I2, T4, new()
  where T2 : class
  where T3 : struct
{
}

class B
{}

interface I1
{}

interface I2
{ }