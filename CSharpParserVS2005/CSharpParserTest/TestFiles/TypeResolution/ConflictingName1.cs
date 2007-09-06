using System;
using System.Data;

namespace MyNamespace
{
  public class MyClass
  {
    private bool _Flag;

    public MyClass()
    {
      _Flag = this is ValueType;
    }
  }
}