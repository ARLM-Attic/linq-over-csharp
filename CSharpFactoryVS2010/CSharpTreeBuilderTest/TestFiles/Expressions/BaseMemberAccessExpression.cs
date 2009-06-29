public class BaseMemberAccessExpression : BaseClass
{
  public unsafe void DummyMethod()
  {
    int i3 = base.Field;
  }
}
public class BaseClass
{
  public int Field;
}
