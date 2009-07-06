class AnonymousObjectCreationExpression
{
  void DummyMethod()
  {
    var a = new { x, AnonymousObjectCreationExpression.xs1, global::AnonymousObjectCreationExpression.xs2, A = 1 };
  }

  private int x;

  private static int xs1;
  private static int xs2;
}
