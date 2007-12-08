class cB<T> : cD<int> { }
class cC<U> : cB<int> { }
class cD<V> : cC<int> { }

class Master
{
  class cB<T> : cD<int> { }
  class cC<U> : cB<int> { }
  class cD<V> : cC<int> { }
}
