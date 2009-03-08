public struct MyClass
{
  public static int operator +(int a) { return 0; }
  public static int operator +(MyClass a) { return 0; }
  public static int operator +(MyClass? a) { return 0; }
  public static int operator -(MyClass[] a) { return 0; }
}