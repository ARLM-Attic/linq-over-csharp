using System;
using CSharpFactory.ProjectContent;
using CSharpFactory.ProjectModel;

namespace CSharpParserTest
{
  public abstract class ParserTestBed
  {
    protected const string WorkingFolder = @"C:\Work\LINQOverCSharp\CSharpFactoryVS2010\CSharpFactoryTest\TestFiles";
    protected const string AsmFolder = WorkingFolder + @"\Assemblies";
    protected const string TempOutputFolder =
      @"C:\Work\LINQOverCSharp\CSharpFactoryVS2010\CSharpFactoryTest\TestFiles\Temp";

    public bool InvokeParser(string fileName)
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder, false);
      parser.AddFile(fileName);
      AddCommonSource(parser);
      return InvokeParser(parser);
    }

    public bool InvokeParser(CompilationUnit parser)
    {
      int errors = parser.Parse();
      Console.WriteLine("{0} errors detected", errors);
      foreach (Error error in parser.Errors)
      {
        Console.WriteLine("({0}, {1}) in {4}: {2}: {3}", error.Line, error.Column,
          error.Code, error.Description, error.File);
      }
      Console.WriteLine();
      Console.WriteLine("{0} warnings detected", parser.Warnings.Count);
      foreach (Error warning in parser.Warnings)
      {
        Console.WriteLine("({0}, {1}) in {4}: {2}: {3}", warning.Line, warning.Column,
          warning.Code, warning.Description, warning.File);
      }
      Console.WriteLine();
      return errors == 0;
    }

    protected virtual void AddCommonSource(CompilationUnit parser)
    {
    }
  }
}
