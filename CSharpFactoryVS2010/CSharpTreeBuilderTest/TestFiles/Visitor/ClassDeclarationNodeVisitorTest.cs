using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

[Serializable, CLSCompliant(false)]
[DebuggerDisplay("a", Name = "a", Type = "a")]
public class MyClass<T1, T2> : Exception, IDisposable
  where T1 : Exception
  where T2 : class, new()
{
  // constant-declaration
  [DebuggerDisplay("a")] 
  private const int c1 = 0, c2 = 0;

  // field-declaration
  [DebuggerDisplay("a")]
  private int f1, f2 = 0;

  // method-declaration
  [DebuggerStepThrough()]
  public void GenericMethod<T3, T4>
  (
    ref T3 t3,
    out T4 t4,
    [InAttribute]params int[] ints
  )
    where T3 : class
    where T4 : struct
  {
    t4 = default(T4);
  }

  // method-declaration (explicit interface implementation)
  void IDisposable.Dispose()
  {
  }

  // property-declaration
  [DebuggerHidden()]
  public int A { get; set; }

  // event-declaration
  [Obsolete]
  public event Action<string> e1 = null;

  // event-declaration (with accessors)
  [Obsolete]
  public event Action<string> MyEvent
  {
    add { }
    remove { }
  }

  // indexer-declaration
  [DebuggerHidden()]
  public int this[int a, int b]
  {
    get { return 0; }
    set { }
  }

  // operator-declaration
  [DebuggerStepThrough()]
  public static MyClass<T1, T2> operator ++(MyClass<T1, T2> m)
  {
    return null;
  }

  // operator-declaration (conversion operator)
  [DebuggerStepThrough()]
  public static implicit operator string(MyClass<T1, T2> m)
  {
    return null;
  }

  // constructor-declaration
  [DebuggerStepThrough()]
  public MyClass(int a, int b)
  {
  }

  // constructor-declaration (with constructor initializer)
  public MyClass()
    : this(0, 0)
  {
  }

  // destructor-declaration
  [DebuggerNonUserCode()]
  ~MyClass()
  {
  }

  [DebuggerDisplay("a")]
  public class SubType
  { 
  }
}