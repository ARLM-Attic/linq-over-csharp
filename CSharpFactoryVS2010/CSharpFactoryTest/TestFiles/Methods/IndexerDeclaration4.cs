public class MyClass
{
  public int this[int a, ref int b] { get { return 0; } }
  public int this[int a, ParamClass b] { get { return 0; } }
  public int this[int a, out string b] 
  { 
    get { b = new ParamClass(); return 0; } 
  }

  public int this[] { get { return 0; } }
}

public class ParamClass { }