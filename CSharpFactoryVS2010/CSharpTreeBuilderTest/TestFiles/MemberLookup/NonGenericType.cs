class A
{
  B b;

  void M(B b)
  {
    b.GetType();
  }
}

class B: C
{
  new private void GetType() { }
}

class C : D
{
  public override void GetType() { }
}

class D
{
  new public virtual void GetType() { }
}