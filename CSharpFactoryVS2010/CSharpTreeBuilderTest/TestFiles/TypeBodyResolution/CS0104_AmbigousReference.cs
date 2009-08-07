using B1;
using B2;

class A
{
  private C c;
}

namespace B1
{
  class C
  { }
}

namespace B2
{
  class C
  { }
}

// error CS0104: 'C' is an ambiguous reference between 'B1.C' and 'B2.C'