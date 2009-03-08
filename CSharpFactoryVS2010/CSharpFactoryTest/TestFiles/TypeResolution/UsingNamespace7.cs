using System.IO;
using A = System.IO.FileAccess;

namespace A
{
  namespace X
  {
    using alias = A;
  }
}

namespace ns1
{
  using A = System.IO.FileAccess;

  namespace A
  {
    namespace X
    {
      using alias = A;
    }
  }
}

