public class BaseElementAccessExpression : BaseElementAccessExpressionBase
{
  public unsafe void DummyMethod()
  {
    int i4 = base[1];
  }
}

public class BaseElementAccessExpressionBase
{
  public int this[int i]
  {
    get { return 0; }
  }
}
