namespace ns1
{
  using A = System.IO;

  class A<T> {}

  namespace B
  {
    using alias = A<int>;
  }

  namespace ns1
  {
    using A = System.IO;

    class A { }

    namespace B
    {
      using alias = A;
    }

  }
}

