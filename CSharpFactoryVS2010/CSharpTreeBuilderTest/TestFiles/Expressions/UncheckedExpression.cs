public class UncheckedExpression 
{
  public void DummyMethod()
  {
    int i = unchecked(p++);
  }

  private int p;
}
