public struct MyStruct
{
  public static bool operator true(MyStruct a) { return true; }
  public static bool operator false(MyStruct? a) { return true; }
}

public struct MyStruct1
{
  public static bool operator true(MyStruct1 a) { return true; }
  public static bool operator false(MyStruct1 a) { return true; }
  public static bool operator true(MyStruct1? a) { return true; }
  public static bool operator false(MyStruct1? a) { return true; }
}