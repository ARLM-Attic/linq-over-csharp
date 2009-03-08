namespace CSharpParserTest.TestFiles
{
  internal class ConstDeclarations
  {
    private const int a = 32;
    public const string b = @"hahaha";
    internal new const char c = 'd';
    protected const System.Double d = 123.456D;
    protected const System.Single single1 = 123.456F;
    protected const System.Decimal decimal1 = 123.456M;
    protected const System.Single single2 = 123.456F;
    protected const System.Decimal decimal2 = 123.456M;
    protected const System.Single double2 = 123.456F;
    protected const System.Decimal decimal3 = 123.456M;
    protected const char char1 = 'X';
    protected const char char2 = '\x1111';
    protected const char char3 = '\u1233';
    protected const char char4 = '\U00002345';
  }
}
