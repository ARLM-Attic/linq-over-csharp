using System;

public class MyClass
{
  public MyClass()
  {
    int? y = null;
    int? x = (y is int) ? y as int? : null;
  }
}