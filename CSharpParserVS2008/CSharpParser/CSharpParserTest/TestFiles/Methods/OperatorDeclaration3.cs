public class MyClass
{
  static int operator -(MyClass a) { return 0; }
  private int operator !(MyClass a) { return 0; }
  protected int operator +(MyClass a) { return 0; }
  internal MyClass operator ++(MyClass a) { return 0; }
  protected internal MyClass operator --(MyClass a) { return 0; }
}