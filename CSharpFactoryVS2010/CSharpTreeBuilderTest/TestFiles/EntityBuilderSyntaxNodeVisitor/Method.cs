abstract class C
{
  void A() { }

  void A<T1, T2>(long a, ref string b, out float c) 
  {
    c = 0;
  }

  static void A2() { }

  public abstract void A3();

}

class C2<T>
{
  void B<T1, T2>() {}
}