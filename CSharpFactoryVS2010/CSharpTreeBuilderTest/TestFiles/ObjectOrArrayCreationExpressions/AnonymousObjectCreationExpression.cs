class AnonymousObjectCreationExpression
{
  void DummyMethod()
  {
    var a1 = new { x, AnonymousObjectCreationExpression.xs1, global::AnonymousObjectCreationExpression.xs2, A = 1 };
    var a2 = new { };
  }

  private int x;

  private static int xs1;
  private static int xs2;
}
