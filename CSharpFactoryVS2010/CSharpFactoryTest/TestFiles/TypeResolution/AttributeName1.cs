using System;

namespace MyNamespace
{
  [AttributeUsage(AttributeTargets.All)]
  public class X : Attribute { }

  public class MyClass<U, V>
  {
    [@X]
    [@A]
    [System.@IO]
    public void Method() { }
  }
}