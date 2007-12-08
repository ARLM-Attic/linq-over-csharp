public interface MyInterface
{
  // --- Valid modifier
  new void Method0();

  // --- Invalid modifier
  public void Method1();
  private void Method2();
  protected void Method3();
  internal void Method4();
  protected internal void Method5();
  internal protected void Method6();
}