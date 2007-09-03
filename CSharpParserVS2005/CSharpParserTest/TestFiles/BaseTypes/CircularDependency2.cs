interface A: A {}

interface B : D { }
interface C : B { }
interface D : C { }

interface gB<T> : gD<T> { }
interface gC<U> : gB<U> { }
interface gD<V> : gC<V> { }

interface cB<T> : cD<int> { }
interface cC<U> : cB<int> { }
interface cD<V> : cC<int> { }



