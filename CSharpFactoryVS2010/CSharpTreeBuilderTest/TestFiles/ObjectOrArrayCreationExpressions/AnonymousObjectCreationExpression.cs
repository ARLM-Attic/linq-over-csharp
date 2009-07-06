class AnonymousObjectCreationExpression
{
  void DummyMethod()
  {
    var a = new { x, AnonymousObjectCreationExpression.xs, A = 2 };
  }

  private int x;
  
  private static int xs;
}
