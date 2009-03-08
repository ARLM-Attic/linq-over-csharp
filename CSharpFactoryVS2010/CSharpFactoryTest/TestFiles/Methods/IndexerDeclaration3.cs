public class MyClass
{
  private abstract int this[int a] { get { return 0; } }
  private virtual int this[int a, int b] { get { return 0; } }
  private override int this[int a, int b, int c] { get { return 0; } }
}