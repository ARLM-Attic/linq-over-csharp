class A
{
  A P1
  {
    get { return new A(); }
  }

  int P2
  {
    get { return 0; }
  }

  void M(A a)
  {
    int x = a.P1.P2;
  }
}