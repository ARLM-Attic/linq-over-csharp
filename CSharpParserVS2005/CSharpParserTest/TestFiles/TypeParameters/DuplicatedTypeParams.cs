using System;
using System.Collections;
using System.Collections.Generic;

namespace CSharpParserTest.TestFiles
{
  class Dummy1<A, A> : object
    where A : class
  {
  }

  struct Dummy2<A, B, C, B>
  {
  }

  interface Dummy3<A, B, C, C>
  {
  }
}
