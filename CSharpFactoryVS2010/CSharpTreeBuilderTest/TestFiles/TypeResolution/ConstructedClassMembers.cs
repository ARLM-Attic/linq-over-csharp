unsafe class A<T1, T2> : B<T1>
  where T1:struct
{
  A<int, long> a0;

  T1 a1;

  B<T2> a2 { get; set; }

  T1? a3;

  A<int, T2> M1(T1[] p1, A<int, B<T2>> p2) 
  { 
    return null; 
  }

}

class A2<T1, T2>
{
  A2<T2, T1> a1;

  A2<byte, char> a2;
}

class B<T>
{
}