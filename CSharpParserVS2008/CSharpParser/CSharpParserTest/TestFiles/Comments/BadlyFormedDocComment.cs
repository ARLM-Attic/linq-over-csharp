/// <summary>
/// This is MyClass. OK.
/// <summary>
public class MyClass
{
  /// <summary>
  /// This is MyClass ctor. OK.
  /// </remarks>
  public MyClass(int a) { }

  /// <summary>
  /// This is <c>MyClass ctor.
  /// </summary>
  public MyClass(int a, string b) { }

  /// This is MyClass ctor. Warnings.
  /// </summary>
  public MyClass(int a, string b, double c) { }
}