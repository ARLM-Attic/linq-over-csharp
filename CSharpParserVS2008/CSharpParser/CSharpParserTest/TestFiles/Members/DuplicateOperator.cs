public sealed class MyClass
{
  public static MyClass operator +(MyClass a, MyClass b) 
  {
    return null;
  }

  public static MyClass operator +(MyClass a, int b)
  {
    return null;
  }

  public static MyClass operator +(int a, MyClass b)
  {
    return null;
  }

  public static int operator +(int a, MyClass b)
  {
    return 0;
  }
}