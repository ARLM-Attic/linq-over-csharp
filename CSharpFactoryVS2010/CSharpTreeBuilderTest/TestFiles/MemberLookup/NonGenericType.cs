class A
{
  // Looking up members of B (context entity) at this field (accessing entity) by the name "GetType".
  B b;
}

class B : C
{
  // This member won't be selected because it's not accessible in classA.
  new private void GetType() { }
}

class C : D
{
  // This member won't be selected because it's an override.
  public override void GetType() { }
}

class D
{
  // This member will be selected when type parameter count = 0.
  new public virtual void GetType() { }

  // This member will be selected when type parameter count = 0 or 1.
  public void GetType<T>() { }

  // This member will be selected when type parameter count = 0 or 2.
  public void GetType<T1,T2>() { }
}