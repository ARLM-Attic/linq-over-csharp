public class MyClass
{
  static explicit operator int(MyClass a) { return 0; }
  private explicit operator byte(MyClass a) { return 0; }
  protected explicit operator short(MyClass a) { return 0; }
  internal explicit operator long(MyClass a) { return 0; }
  protected internal explicit operator uint(MyClass a) { return 0; }
}