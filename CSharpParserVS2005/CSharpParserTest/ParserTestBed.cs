using System;
using CSharpParser.ProjectModel;
using CSharpParser.Semantics;

namespace CSharpParserTest
{
  public abstract class ParserTestBed
  {
    protected const string WorkingFolder = @"C:\Work\LINQOverCSharp\CSharpParserVS2005\CSharpParserTest\TestFiles";

    public bool InvokeParser(string fileName)
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(fileName);
      return InvokeParser(parser);
    }

    public bool InvokeParser(CompilationUnit parser)
    {
      parser.AddAssemblyReference("mscorlib");
      parser.AddAssemblyReference("System");
      int errors = parser.Parse();
      Console.WriteLine("{0} errors detected", errors);
      foreach (Error error in parser.Errors)
      {
        Console.WriteLine("({0}, {1}) in {4}: {2}: {3}", error.Line, error.Column,
          error.Code, error.Description, error.File);
      }
      Console.WriteLine();
      //parser.GlobalHierarchy.Trace(Console.Out, 0);
      foreach (NamespaceHierarchy nsh in parser.NamespaceHierarchies.Values)
      {
        //nsh.Trace(Console.Out, 0);
      }
      return errors == 0;
    }
  }
}
