class A1
{
  A2<A3>.A4<A5> a;
  A2<A3>.A6 b;
}

class A2<T1>
{
  public class A4<T2>
  { }

  public class A6
  { }
}

class A3
{}


class A5
{ }
