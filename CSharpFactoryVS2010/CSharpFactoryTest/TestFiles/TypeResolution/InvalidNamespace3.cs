namespace MyNamespace
{
  class MyNamespace { }

  namespace MyNamespace.SubNamespace
  {
    // --- These are valid
    class MyNamespace { }
    interface SubNamespace {}
  }
}

struct MyNamespace {}

namespace MyNamespace.MyNamespace.SubNamespace
{
  namespace MyNamespace
  {
  }
}