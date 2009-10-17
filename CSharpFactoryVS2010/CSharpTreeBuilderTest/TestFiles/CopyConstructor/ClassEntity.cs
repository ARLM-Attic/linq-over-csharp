public abstract class A<TA>
{
  public abstract void MA<TMA>() where TMA: class;
}

internal sealed class B<TB> : A<TB>
{
  public override void MA<TMA>()
  { 
  }
}