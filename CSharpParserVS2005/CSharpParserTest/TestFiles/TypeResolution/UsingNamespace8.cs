namespace ns1
{
  using A = System.IO;

  class A {}

  namespace B
  {
    using alias = A;
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

