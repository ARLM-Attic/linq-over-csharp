class ObjectCreationExpression
{
  void DummyMethod()
  {
    var a1 = new ObjectCreationExpression();
    var a2 = new ObjectCreationExpression(1, 2);
    var a3 = new ObjectCreationExpression() {A = 3};
    var a4 = new ObjectCreationExpression(4, 5) {A = 6};
    var a5 = new ObjectCreationExpression {A = 7, B = 8};
  }

  public ObjectCreationExpression()
  {
  }

  public ObjectCreationExpression(int a, int b)
  {
  }

  public int A;
  public int B;
}
