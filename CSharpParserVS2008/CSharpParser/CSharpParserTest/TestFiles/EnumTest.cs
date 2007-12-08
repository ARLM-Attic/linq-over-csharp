using System;
using System.ComponentModel;

namespace CSharpParserTest.TestFiles
{
  [Serializable]
  [Flags]
  enum EnumTest1: byte
  {
    Value1,
    Value2 = 1,
    Value3,
    Value4 = Value1 | Value2,
  }

  [Flags]
  enum EnumTest2
  {
    [Description("Value1")]
    Value1,
    Value2 = 1,
    Value3,
    Value4 = Value1 | Value2
  }
}
