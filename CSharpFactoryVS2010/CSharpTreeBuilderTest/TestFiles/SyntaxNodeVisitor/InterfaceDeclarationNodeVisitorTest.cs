using System;

[Obsolete]
public interface MyInterface<T1, T2> : IDisposable
  where T1 : struct
  where T2 : new()
{
  // method-declaration
  [Obsolete]
  void Method();

  // property-declaration
  int A
  {
    get;
    set;
  }

  // event-declaration
  event Action<string> MyEvent;

  // indexer-declaration
  int this[int a]
  {
    get;
  }
}