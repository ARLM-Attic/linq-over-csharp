namespace N1
{
}

namespace  N2A
{
  
}

namespace N2.N2A
{
  class A {}  
}

namespace N2
{
  namespace N2A
  {
    
  }

  namespace N2B
  {
    using N2A;
    class B: A {}
  }
}