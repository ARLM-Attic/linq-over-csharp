﻿using N;
using C = E;

namespace A
{
  using N;
  using C = E;

  class B : D
  {
    public C c = new C();

    //public class C { }
  }

  class C { }
}

class C { }

class D
{
  public class C { }        // D+C
}

namespace N
{
  public class C { }
}

class E { }