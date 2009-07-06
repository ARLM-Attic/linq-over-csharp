class DelegateCreationExpression
{
  void DummyMethod()
  {
    var myDelegate = new MyDelegate(MyTarget);
  }

  void MyTarget(int i, int j)
  {}

  private delegate void MyDelegate(int i, int j);
}
