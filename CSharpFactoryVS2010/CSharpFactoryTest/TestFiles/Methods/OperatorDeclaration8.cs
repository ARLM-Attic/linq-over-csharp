public struct MyClass
{
  public static int operator true(MyClass a) { return 0; }
  public static int operator true(MyClass? a) { return 0; }
  public static int operator false(MyClass a) { return 0; }
  public static int operator false(MyClass? a) { return 0; }
}