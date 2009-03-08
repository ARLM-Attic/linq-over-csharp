namespace CSharpParserTest.TestFiles
{
  class SimpleStatements
  {
    public static void EmptyMethod()
    {
      ;
      checked { ; }
      unchecked { ; }
      unsafe { ; }
    }
  }
}
