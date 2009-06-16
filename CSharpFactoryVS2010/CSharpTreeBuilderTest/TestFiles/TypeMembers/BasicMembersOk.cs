namespace MyNamespace
{
  public class MyClass
  {
    public struct MyStruct
    {
      public enum MyEnum
      {
        Value1 = 1,
        Value2 = 2
      }
    }

    public const int IntConst1 = 1;

    public string Field1;

    public short ShortProperty { get; set; }

    public string StringProperty
    {
      get { return Field1; }
      set { Field1 = value; }
    }
  }
}