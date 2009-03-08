public class MyClass
{
  static explicit operator int(MyClass a) { return 0; }
  private static explicit operator byte(MyClass a) { return 0; }
  protected static explicit operator short(MyClass a) { return 0; }
  internal static explicit operator long(MyClass a) { return 0; }
  protected internal static explicit operator uint(MyClass a) { return 0; }
}