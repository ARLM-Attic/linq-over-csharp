using System.IO;

namespace MyNamespace
{
  namespace System.IO
  {
    class A {}
  }

  namespace Other
  {
    using System.IO;

    class B
    {
      public A _A;
      public FileMode _FileMode;
    }
  }
}