public sealed class MyClass
{
  private void MyMethod(string a, int b, MyClass c) { }
  private void MyMethod(string a, int b) { }
  private void MyMethod(string a) { }

  private bool MyMethod(string a, int b, MyClass c)
  {
    return false;
  }
  private bool MyMethod(string a, int b)
  {
    return false;
  }
}

public struct MyStruct
{
  private void MyMethod(string a, int b, MyClass c) { }
  private void MyMethod(string a, int b) { }
  private void MyMethod(string a) { }

  private bool MyMethod(string a, int b, MyClass c)
  {
    return false;
  }
  private bool MyMethod(string a, int b)
  {
    return false;
  }
}