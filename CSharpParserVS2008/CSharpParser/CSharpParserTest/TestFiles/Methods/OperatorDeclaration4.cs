public class MyClass
{
  public static int operator -(ref MyClass a) { return 0; }
  public static int operator !(out MyClass a) { return 0; }
  public static int operator +(ref MyClass a) { return 0; }
}