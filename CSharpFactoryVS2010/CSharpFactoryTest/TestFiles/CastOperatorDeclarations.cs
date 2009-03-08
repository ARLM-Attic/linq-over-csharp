namespace CSharpParserTest.TestFiles
{
  class CastOperatorDeclaration
  {
    public static implicit operator int(CastOperatorDeclaration dec)
    {
      return 1234;
    }

    public static explicit operator CastOperatorDeclaration(int value)
    {
      return null;
    }
  }
}
