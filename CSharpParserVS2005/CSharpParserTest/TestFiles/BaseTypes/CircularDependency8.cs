using System;

interface gB<T> : IDisposable, gD<T> { }
interface gC<U> : gB<U> { }
interface gD<V> : gC<V> { }