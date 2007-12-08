public struct MyClass
{
  public static implicit operator int(int a) { return 0; }
  public static implicit operator int(MyClass a) { return 0; }
  public static implicit operator int(MyClass? a) { return 0; }
  public static explicit operator int(MyClass[] a) { return 0; }
}