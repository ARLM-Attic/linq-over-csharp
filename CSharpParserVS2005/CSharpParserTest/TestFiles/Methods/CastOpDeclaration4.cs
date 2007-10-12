public class MyClass
{
  public static explicit operator int(ref MyClass a) { return 0; }
  public static explicit operator byte(out MyClass a) { return 0; }
  public static explicit operator short(ref MyClass a) { return 0; }
}