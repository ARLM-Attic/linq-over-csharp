using System;
using System.Collections;
using System.Collections.Generic;

namespace CSharpParserTest.TestFiles
{
  class Constrainted<A, B, C, D, E, F>: object 
    where A: class 
    where B: struct 
    where C: new()
    where D: ArrayList, IEnumerable<A>
    where E: D, new()
    where F: ArrayList, IEnumerable<B>, IEquatable<D>, new()
  {
  }
}
