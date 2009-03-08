using System;

public class MyClass
{
  public MyClass()
  {
    int? y = null;
    int? x = y is int ? y as int? : null;
    int? z = y is int? == true ? y as int? : null;
  }
}