public class ConditionalExpression 
{
  public void DummyMethod()
  {
    var i1 = true ? 0 : 1;

    // right-associativity test
    var i2 = true ? 2 : false ? 3 : 4;

    // this was buggy (generated a syntax error)
    var i5 = 5 is int ? (int)6 : 7;
  }
}
