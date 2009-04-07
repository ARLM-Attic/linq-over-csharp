using System.Collections.Generic;
namespace MyNamespace
{
  public class A: List<string>, IEnumerable<string>
  { }

  public class B<X, Y>: Dictionary<X, Y>
  { }
}

public class C<Z>: System.Collections.Generic.List<Z>
{ }