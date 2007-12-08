public struct MyStruct
{
  public static MyStruct operator +(int a, int b) { return new MyStruct(); }
  public static MyStruct operator -(MyStruct a, int b) { return new MyStruct(); }
  public static MyStruct operator *(int a, MyStruct b) { return new MyStruct(); }
  public static MyStruct operator /(MyStruct? a, int b) { return new MyStruct(); }
  public static MyStruct operator %(int a, MyStruct? b) { return new MyStruct(); }
  public static MyStruct operator |(int a, int b) { return new MyStruct(); }
}