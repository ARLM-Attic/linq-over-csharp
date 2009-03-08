public struct MyStruct
{
  public static bool operator <(MyStruct a, MyStruct b) { return false; }
  public static bool operator ==(MyStruct a, MyStruct b) { return false; }
  public static bool operator <=(MyStruct a, MyStruct b) { return false; }
}

public struct MyStruct1
{
  public static bool operator <(MyStruct1 a, MyStruct1? b) { return false; }
  public static bool operator ==(MyStruct1 a, MyStruct1? b) { return false; }
  public static bool operator <=(MyStruct1 a, MyStruct1? b) { return false; }
  public static bool operator >(MyStruct1 a, MyStruct1? b) { return false; }
  public static bool operator !=(MyStruct1 a, MyStruct1? b) { return false; }
  public static bool operator >=(MyStruct1 a, MyStruct1? b) { return false; }
}