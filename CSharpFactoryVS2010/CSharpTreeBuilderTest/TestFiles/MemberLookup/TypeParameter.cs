class A<T> where T : B, I1, I2
{
  int a;

  void M(T t)
  {
    t.GetType();
    t.GetType(1);
    t.GetType = 1;
    (t as I2).GetType = 1;
  }
}

class B : C
{
  new private int GetType;
}

class C
{
  new public int GetType;
}

interface I1
{
  void GetType();
  void GetType(int i);
}

interface I2
{
  int GetType { get; set; }
}