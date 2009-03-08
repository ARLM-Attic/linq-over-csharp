using System;

public abstract class MyClass
{
  public static implicit operator string(MyClass obj) 
  {
    return null; 
  }

  public static implicit operator int(MyClass obj)
  {
    return 0;
  }

  public static explicit operator int(MyClass obj)
  {
    return 0;
  }

  public static explicit operator MyClass(int value)
  {
    return null;
  }

  public static explicit operator MyClass(int value)
  {
    return null;
  }
}