class AnonymousMethodExpressions
{
  void DummyMethod()
  {
    // Anonymous method without signature
    Dvoid d1 = delegate {};

    // Anonymous method with empty signature
    Dvoid d2 = delegate() { };

    // Anonymous method with signature
    Dint d3 = delegate(int i, ref int j, out int k) { k = 1; return i; };
  }

  private delegate void Dvoid();
  private delegate int Dint(int i, ref int j, out int k);
}