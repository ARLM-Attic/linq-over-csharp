class A1<T1> : A3<T1, A2<T1>>, I1<A1<T1>>
{ }

class A2<T2> : A3<int, long>, I1<int>
{ }

class A3<T3, T4>
{ }

interface I1<T>
{ }