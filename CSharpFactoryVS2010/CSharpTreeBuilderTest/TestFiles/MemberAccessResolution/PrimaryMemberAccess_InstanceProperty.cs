class A
{
  int P
  {
    get { return 0; }
  }

  void M(A a)
  {
    int x = a.P;
  }
}