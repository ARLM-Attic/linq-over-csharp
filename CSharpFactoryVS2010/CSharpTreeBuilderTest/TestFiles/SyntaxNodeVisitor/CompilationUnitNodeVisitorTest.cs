extern alias Extern;

using System;
using alias = System;

[assembly: AssemblyTitle("a")]

namespace MyNamespace
{
  namespace EmbeddedNamespace
  {
    class EmbeddedClass
    {
    }
  }
}

class MyClass
{
}

struct MyStruct
{
}

interface MyInterface
{
}

enum MyEnum
{
}

delegate void MyDelegate();