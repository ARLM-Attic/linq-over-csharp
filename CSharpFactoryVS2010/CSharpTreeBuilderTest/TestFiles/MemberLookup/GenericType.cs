class A
{
  // Selecting members of B<int> (contextType) that are accessible in class A, named "GetType", with 0 type params.
  B<int> b;

  void M(B<int> b)
  {
    b.GetType();
  }
}

class B<T> : C<T>
{
  // Not accessible
  new private T GetType() { return default(T); }
}

class C<T> : D<T>
{
  // Overrides is omitted from member lookup
  public override T GetType() { return default(T); }
}

class D<T>
{
  // Selected by member lookup
  new public virtual T GetType() { return default(T); }
}
