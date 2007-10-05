public class MyClass
{
  static int operator -(MyClass a) { return 0; }
  private static int operator !(MyClass a) { return 0; }
  protected static int operator +(MyClass a) { return 0; }
  internal static MyClass operator ++(MyClass a) { return 0; }
  protected internal static MyClass operator --(MyClass a) { return 0; }
}