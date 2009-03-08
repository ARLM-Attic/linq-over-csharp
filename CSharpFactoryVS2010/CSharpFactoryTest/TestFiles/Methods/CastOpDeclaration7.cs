public struct MyStruct
{
  public static implicit operator MyStruct?(MyStruct a) { return new MyStruct(); }
  public static implicit operator MyStruct(MyStruct? a) { return new MyStruct(); }
}