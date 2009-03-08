using System;
using System.Collections;
using System.Collections.Generic;

namespace CSharpParserTest.TestFiles
{
  class DuplicatedConstraint<A, B, C, D, E, F> : object
    where A : class
    where A : struct
    where C : new()
    where D : ArrayList, IEnumerable<A>
    where E : D, new()
    where C : ArrayList, IEnumerable<B>, IEquatable<D>, new()
  {
  }
}
