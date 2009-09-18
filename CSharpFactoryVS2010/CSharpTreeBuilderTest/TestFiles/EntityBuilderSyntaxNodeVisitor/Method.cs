abstract class C : B
{
  void A() { }

  void A<T1, T2>(long a, ref string b, out float c) 
  {
    c = 0;
  }

  static void A2() { }

  public abstract void A3();

  public virtual void A4() {}

  public override void A5() {}

  public new void A6() {}

}

class C2<T>
{
  void B<T1, T2>() {}
}

class B
{
  public virtual void A5() { }

  public void A6() { }
}