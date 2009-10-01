class A
{
  B<int> b;

  void M(B<int> b)
  {
    b.GetType();
  }
}

class B<T> : C<T>
{
  new private T GetType() { return default(T); }
}

class C<T> : D<T>
{
  public override T GetType() { return default(T); }
}

class D<T>
{
  new public virtual T GetType() { return default(T); }
}