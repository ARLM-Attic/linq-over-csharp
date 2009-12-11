class A<T1>
{
  public static T1 t;
}

class B<T2>
{
  public static T2 b = global::A<T2>.t;
}

class C : B<int>
{
  public static int c = b;
}
