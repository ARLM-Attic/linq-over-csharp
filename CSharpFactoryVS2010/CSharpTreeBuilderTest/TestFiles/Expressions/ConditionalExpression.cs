public class ConditionalExpression 
{
  public void DummyMethod()
  {
    var i1 = true ? 0 : 1;

    // right-associativity test
    var i2 = true ? 2 : false ? 3 : 4;
  }
}
