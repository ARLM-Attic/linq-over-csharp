class ArrayCreationExpression
{
  unsafe void DummyMethod()
  {
    var a1 = new int[1, 2][][,] { { null, null } };
    var a2 = new int**[5, 6];
    var a3 = new[,] { { a = 7, 8 }, { 9, 10 } };
    var a4 = new int[][,] { null, null };
  }

  private int a;
}