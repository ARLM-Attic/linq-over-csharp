class A<T1, T2> : I<T1, T2>
{
  T3 B<T2, T3>(A<int, long> a, T1 t1, T2 t2, T3 t3) { return default(T3); }

  void C() {}

  T3 I<T1, T2>.B<T2, T3>(A<int, long> a, T1 t1, T2 t2, T3 t3) { return default(T3); }

  void global::I<T1, T2>.C() { }
}

interface I<T1, T2>
{
  T3 B<T2, T3>(A<int, long> a, T1 t1, T2 t2, T3 t3);

  void C();
}