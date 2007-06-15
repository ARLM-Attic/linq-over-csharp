using System;
using System.Collections;

namespace CSharpParserTest.TestFiles
{
  abstract class MethodDeclarations: IEnumerable
  {
    public static void SimpleMethod()
    {
      
    }

    protected string ReverseString(string source)
    {
      return source;
    }

    private abstract T AbstractMethod<T, U, V>(U par1, V par2);

    #region IEnumerable Members

    IEnumerator IEnumerable.GetEnumerator()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    #endregion
  }
}
