public class MyClass
{
  public abstract static int this[int a] { get { return 0; } }
  public abstract virtual int this[int a, string b] { get { return 0; } }
  public abstract sealed int this[int a, int b, int c] { get { return 0; } }
  public abstract override int this[int a, int b, int c, string d] { get { return 0; } }
  public abstract extern int this[int a, int b, int c, int d, int e] { get { return 0; } }
}