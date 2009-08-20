class A:B
{
  E x1;
  E<int> x2;
  F.G x3;
  F<int>.G x4;
  F<int>.G<int> x5;

  int E;
}

class B:C
{
}

class C:D
{
  public class E
  {
  }
  public class E<T1>
  {
  }
}

class D
{
  public class E
  {
  }

  public class F
  {
    public class G
    {}
  }

  public class F<T2>
  {
    public class G
    {
    }
    public class G<T3>
    {
    }
  }
}

class E
{
}

public class F
{
  public class G
  { }
}