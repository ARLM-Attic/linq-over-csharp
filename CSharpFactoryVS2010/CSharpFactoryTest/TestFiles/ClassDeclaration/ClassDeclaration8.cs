using System.Collections;
using System.Collections.Generic;

namespace MyNamespace
{
  public class A: List<string>, IEnumerable<string>
  { }

  public class B<X, Y>: Dictionary<X, Y>
    where X: class, IEnumerable<Y>, new()
    where Y: struct
  { }
}

public class C<Z>: System.Collections.Generic.List<Z>
  where Z: Hashtable, IEnumerable<string>
{ }