/// <summary>
/// This is MyClass. OK.
/// </summary>
public class MyClass
{
  /// <summary>
  /// This is MyClass ctor. OK.
  /// </summary>
  public MyClass(int a) {}

  /// <summary>
  /// This is MyClass ctor. OK.
  /// </summary>
  /// <param name="a">A</param>
  /// <param name="b">B</param>
  public MyClass(int a, string b) {}

  /// <summary>
  /// This is MyClass ctor. Warnings.
  /// </summary>
  /// <param name="a">A</param>
  /// <param name="b"></param>
  /// <param name="a">A duplicated</param>
  /// <param name="d">Invalid parameter name</param>
  public MyClass(int a, string b, double c) {}

  /// <summary>
  /// This is Method1. Warnings.
  /// </summary>
  /// <param></param>
  /// <param></param>
  protected void Method1(int a, int b) {}
}