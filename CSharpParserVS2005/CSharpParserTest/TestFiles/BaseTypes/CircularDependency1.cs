class A: A {}

class B: D {}
class C: B {}
class D: C {}

class E: F.G {}
class F: E
{
  public class G {}
}

class gB<T> : gD<T> { }
class gC<U> : gB<U> { }
class gD<V> : gC<V> { }

class cB<T> : cD<int> { }
class cC<U> : cB<int> { }
class cD<V> : cC<int> { }


