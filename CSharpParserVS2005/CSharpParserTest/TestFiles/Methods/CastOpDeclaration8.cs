using System;

public struct MyStruct
{
  public static implicit operator IDisposable(MyStruct a) { return null; }
  public static implicit operator MyStruct(IDisposable a) { return null; }
}