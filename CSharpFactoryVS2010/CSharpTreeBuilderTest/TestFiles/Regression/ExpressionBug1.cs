using System.Collections;

public class MyClass
{
  public MyClass()
  {
    object element = null;
    var x = element is IEnumerable && !(element is string);
  }
}