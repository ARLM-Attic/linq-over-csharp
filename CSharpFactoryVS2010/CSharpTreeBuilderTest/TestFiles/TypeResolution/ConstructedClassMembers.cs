unsafe class A<T1, T2> : B<T1>
{
  A<int, long> a0;

  T1 a1;

  B<T2> a2 { get; set; }
  
  A<T2, T1> M1(T1[] p1, A<int, B<T2>> p2) 
  { 
    return null; 
  }
}

class B<T>
{
}