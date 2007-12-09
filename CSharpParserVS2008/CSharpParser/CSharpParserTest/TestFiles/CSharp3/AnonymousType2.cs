public class MyClass
{
  int _InternalField = 123;

  public MyClass()
  {
    var anonym1 = new { FirstName = "James", string.Empty, uint.MinValue, };
    var anonym2 = new { _InternalField };
    var anonym3 = new { _InternalField.GetType().Name, LastName = "Agent", Age = 60, };
  }
}