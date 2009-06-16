using System.Collections.Generic;

namespace MyNamespace
{
  public struct A
  { }

  public struct B<X>: IDictionary<string, X>
    where X: struct, IEnumerable
  {
  }

  public struct C
  {
    internal struct D { }
    private struct E { }
  }
}

public struct F
{
}

