public class MyClass
{
  public static explicit operator int() { return 0; }
  public static implicit operator byte() { return 0; }
  public static explicit operator short(MyClass a, int b, int c) { return 0; }
}