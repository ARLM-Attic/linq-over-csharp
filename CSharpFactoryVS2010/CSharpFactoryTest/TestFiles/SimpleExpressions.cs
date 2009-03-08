using System;

namespace CSharpParserTest.TestFiles
{
  public static class SimpleExpressions
  {
    public static void DummyMethod()
    {
      int x = 6;
      int y = x = 8;
      int a1 = 2*3*4;
      int a2 = 2 + 3 + 4;
      int a3 = 2 + 3 * 4 + 5;
    }
  }
}
