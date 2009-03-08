public class MyClass
{
  // --- Valid accessors
  public void Method1() {}
  private void Method2() {}
  protected void Method3() {}
  internal void Method4() {}
  protected internal void Method5() {}
  internal protected void Method6() {}

  // --- Invalid accessors
  public private void Method7() {}
  protected private void Method8() {}
  internal public void Method9() {}
  protected public void Method10() {}
}