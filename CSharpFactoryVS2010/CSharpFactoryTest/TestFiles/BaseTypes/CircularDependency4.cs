class gB<T> : gD<T> { }
class gC<U> : gB<U> { }
class gD<V> : gC<V> { }

class Master
{
  class Inner
  {
    class gB<T> : gD<T> { }
    class gC<U> : gB<U> { }
    class gD<V> : gC<V> { }
  }
}