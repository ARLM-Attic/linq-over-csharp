public struct MyClass
{
  public static int operator ++(MyClass a) { return 0; }
  public static int operator ++(MyClass? a) { return 0; }
  public static int operator --(MyClass a) { return 0; }
  public static int operator --(MyClass? a) { return 0; }
}

public struct MyClass1
{
  public static MyClass1 operator ++(MyClass1 a) { return new MyClass1(); }
  public static MyClass1 operator ++(MyClass1? a) { return new MyClass1(); }
  public static MyClass1? operator --(MyClass1 a) { return new MyClass1?(); }
  public static MyClass1? operator --(MyClass1? a) { return new MyClass1?(); }
}

public class MyClass2
{
  public static MyClass3 operator ++(MyClass2 a) { return new MyClass3(); }
}

public class MyClass3: MyClass2 {}