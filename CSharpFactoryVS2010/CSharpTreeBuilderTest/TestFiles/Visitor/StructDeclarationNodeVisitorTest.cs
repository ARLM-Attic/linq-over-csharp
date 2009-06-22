using System;

[Serializable]
public struct MyStruct<T1, T2> : IDisposable
  where T1 : struct
  where T2 : new()
{
  // constant-declaration
  [Obsolete]
  private const int c1 = 0;

  // field-declaration
  private int f1;

  // method-declaration
  public void Dispose()
  {
  }

  // property-declaration
  public int A
  {
    get { return 0; }
    set { }
  }

  // event-declaration (with accessors)
  public event Action<string> MyEvent
  {
    add { }
    remove {}
  }

  // indexer-declaration
  public int this[int a]
  {
    get { return 0; }
  }

  // operator-declaration
  public static MyStruct<T1, T2> operator ++(MyStruct<T1, T2> m)
  {
    return new MyStruct<T1, T2>();
  }

  // operator-declaration (conversion operator)
  public static implicit operator string(MyStruct<T1, T2> m)
  {
    return null;
  }

  // constructor-declaration (with constructor initializer)
  public MyStruct(int a)
  {
    f1 = 0;
  }

  public class SubType
  {
  }
}