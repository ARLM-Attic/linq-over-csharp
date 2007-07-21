using System;
using CSharpParser;
using CSharpParser.ProjectModel;

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
      int errors = parser.Parse();
      Console.WriteLine("{0} errors detected", errors);
      foreach (SourceFile file in parser.Files)
      {
        foreach (NamespaceFragment ns in file.Namespaces)
        {
          foreach (TypeDeclaration td in ns.TypeDeclarations)
          {
            DisplayTypeInfo(td, 0);
          }
        }
      }
      return errors == 0;
    }

    public void DisplayTypeInfo(TypeDeclaration td, int level)
    {
      Console.WriteLine("{0}{1}: ({2}, {3})", String.Empty.PadLeft(level * 2, ' '),
        td.ParametrizedName, td.StartLine, td.StartColumn);
      foreach (TypeReference bte in td.BaseTypes)
      {
        Console.WriteLine("{0}{1}: ({2}, {3})", String.Empty.PadLeft(level * 2 + 2, ' '),
          bte.Name, bte.StartLine, bte.StartColumn);
      }
      foreach (TypeDeclaration subType in td.NestedTypes)
      {
        DisplayTypeInfo(subType, level + 1);
      }
    }
  }
}
