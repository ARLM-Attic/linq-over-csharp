public class MyClass
{
  public void A(int a, int b)
  {
  }

  public void A(int a, ref int b)
  {
  }

  public void A(int a, out int b)
  {
  }

  public void B(System.Int32 a, ref int b)
  {
  }

  public void B(int a, out System.Int32 b)
  {
  }

  public void C(int a, ref ParamClass b)
  {
  }

  public void C(int a, out ParamClass b)
  {
  }
}

public class ParamClass { }