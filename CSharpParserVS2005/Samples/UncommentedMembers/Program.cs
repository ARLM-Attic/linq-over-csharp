using System;
using System.Collections.Generic;
using System.Diagnostics;
using CSharpParser.ProjectContent;
using CSharpParser.ProjectModel;

namespace CSharpParser.Samples.UncommentedMembers
{
  class Program
  {
    private const string CodeExplorerProjectFile = 
      @"C:\Work\LINQOverCSharp\CSharpParserVS2005\CSharpParser\CSharpParser.csproj";
    static void Main(/* string[] args */)
    {
      // --- Create a compilation unit according to the specified .csproj file
      CSharpProjectContent content = new CSharpProjectContent(CodeExplorerProjectFile);
      CompilationUnit project = new CompilationUnit(content);

      // --- Parse the project and measure parse time
      Stopwatch watch = new Stopwatch();
      watch.Start();
      project.Parse();
      watch.Stop();

      // --- Display errors and warnings
      Console.WriteLine("{0} errors found.", project.Errors.Count);
      Console.WriteLine("{0} warnings found.", project.Warnings.Count);

      // --- Display parse time and other parsing statistics
      Console.WriteLine();
      Console.WriteLine("Parsing time: {0} ms", watch.ElapsedMilliseconds);
      Console.WriteLine();

      // --- Scan for uncommented methods
      List<TypeCommentInfo> commentInfo = CSharpUtility.GetUncommentedMembers(project);
      if (commentInfo.Count == 0)
      {
        Console.WriteLine("All types and members are commented.");
      }
      else
      {
        // --- Display missing comment information
        foreach (TypeCommentInfo info in commentInfo)
        {
          Console.WriteLine(info.Type.FullName);
          foreach (MemberDeclaration member in info.Members)
          {
            Console.WriteLine("  {0}", member.Signature);
          }
        }
      }
    }
  }
}
