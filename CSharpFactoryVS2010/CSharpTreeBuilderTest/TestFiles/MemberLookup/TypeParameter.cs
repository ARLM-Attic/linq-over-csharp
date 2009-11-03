class A<T> where T : B, I1, I2
{
  // Selecting members of T (contextType) that are accessible in class A, named "GetType", with 0 type params.
  T t;
  
  void M(T t)
  {
    t.GetType();
  }
}

class B : C
{
  // This member won't be selected because it's not accessible.
  new private void GetType() {}
}

class C
{
  // This member will be selected.
  new public void GetType() {}
}

interface I1
{
  // This member won't be selected (hidden by C.GetType()).
  void GetType();
  // This member will be selected.
  void GetType(int i);
}

interface I2
{
  // This member won't be selected (hidden by C.GetType()).
  int GetType { get; set; }
}