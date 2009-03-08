public public class A {}
internal internal class B
{
  protected protected class C {}
  protected internal internal protected class D {}
  private class E {}
}

public extern extern class F {}
public class G
{
  private volatile volatile int a;
  public virtual virtual void b();
  public virtual virtual void c();
}

public sealed sealed class H: G
{
  public new new virtual void b();
  public override override void c();
  public static static int d;
  public abstract abstract int e { get; }
}