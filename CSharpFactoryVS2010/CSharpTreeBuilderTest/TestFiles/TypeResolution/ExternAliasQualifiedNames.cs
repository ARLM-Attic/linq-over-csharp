extern alias MyExternAlias;

class A
{
  MyExternAlias::Class0 x0;
  MyExternAlias::A.B.Class1 x1;
  MyExternAlias::A.B.Generic1<int,long> x2;
}

namespace B
{
  extern alias MyExternAlias;

  namespace C
  {
    class A
    {
      MyExternAlias::Class0 x0;
      MyExternAlias::A.B.Class1 x1;
      MyExternAlias::A.B.Generic1<int, long> x2;
    }
  }
}