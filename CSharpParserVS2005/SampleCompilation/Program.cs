using System;
using System.Diagnostics;
using CSharpParser.ProjectContent;
using CSharpParser.ProjectModel;

namespace SampleCompilation
{
  // ==================================================================================
  /// <summary>
  /// This sample console application demonstrates how to use CSharpParser to parse a
  /// C# application.
  /// </summary>
  /// <remarks>
  /// You can change the working folder to that application you want to parse. Compile
  /// and run this application and see the result!
  /// </remarks>
  // ==================================================================================
  class Program
  {
    private const string CodeExplorerProjectFile = @"C:\Work\LINQOverCSharp\CSharpParserVS2005\CodeExplorer\CodeExplorer.csproj";

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

      // --- Display errors
      Console.WriteLine("{0} errors found.", project.Errors.Count);
      foreach (Error error in project.Errors)
      {
        Console.WriteLine("--- Error {0}: {1} line: {2}, column: {3}, file: {4}",
                          error.Code, error.Description, error.Line, error.Column, error.File);
      }

      // --- Display warnings
      Console.WriteLine("{0} warnings found.", project.Warnings.Count);
      foreach (Error warning in project.Warnings)
      {
        Console.WriteLine("--- Warning {0}: {1} line: {2}, column: {3}, file: {4}",
                          warning.Code, warning.Description, warning.Line, warning.Column, warning.File);
      }

      // --- Display files parsed
      Console.WriteLine();
      Console.WriteLine("{0} files have been parsed.", project.Files.Count);

      // --- Display namespaces declared 
      Console.WriteLine();
      Console.WriteLine("{0} namespaces have been declared.", project.DeclaredNamespaces.Count);
      foreach (Namespace ns in project.DeclaredNamespaces)
      {
        Console.WriteLine("  {0}: with {1} fragments.", ns.Name, ns.Fragments.Count);
      }

      // --- Display typed declared
      Console.WriteLine();
      Console.WriteLine("{0} types have been declared.", project.DeclaredTypes.Count);
      foreach (TypeDeclaration td in project.DeclaredTypes)
      {
        Console.WriteLine("  {0}: with {1} members and {2} parts.",
                          td.Name, td.Members.Count, td.PartCount);
      }

      // --- Display parse time and other parsing statistics
      Console.WriteLine();
      Console.WriteLine("Parsing time: {0} ms", watch.ElapsedMilliseconds);
      Console.WriteLine("Type references: {0}", project.Locations.Count);
      Console.WriteLine("Type resolutions: {0}", project.ResolutionCounter);
      Console.WriteLine("Resolved to system type: {0}", project.ResolvedToSystemType);
      Console.WriteLine("Resolved to source type: {0}", project.ResolvedToSourceType);
      Console.WriteLine("Resolved to namespace: {0}", project.ResolvedToNamespace);
      Console.WriteLine("Resolved to hierarchy: {0}", project.ResolvedToHierarchy);
      Console.WriteLine("Resolved to simple name: {0}", project.ResolvedToName);
      Console.WriteLine();
    }
  }
}
