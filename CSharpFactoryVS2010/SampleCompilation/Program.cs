using System;
using System.Linq;
using System.Diagnostics;
using CSharpFactory.ProjectContent;
using CSharpFactory.ProjectModel;

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
    private const string CSharpParserProjectFile = @"C:\Work\LINQOverCSharp\CSharpFactoryVS2010\CSharpFactory\CSharpFactory.csproj";

    static void Main(/* string[] args */)
    {
      // --- Create a compilation unit according to the specified .csproj file
      var content = new CSharp9ProjectContentProvider(CSharpParserProjectFile);
      var project = new CompilationUnit(content);

      // --- Parse the project and measure parse time
      var watch = new Stopwatch();
      watch.Start();
      project.Parse();
      watch.Stop();

      // --- Display errors
      Console.WriteLine("{0} errors found.", project.Errors.Count);
      foreach (var error in project.Errors)
      {
        Console.WriteLine("--- Error {0}: {1} line: {2}, column: {3}, file: {4}",
                          error.Code, error.Description, error.Line, error.Column, error.SourceFileNode.Name);
      }

      // --- Display warnings
      Console.WriteLine("{0} warnings found.", project.Warnings.Count);
      foreach (var warning in project.Warnings)
      {
        Console.WriteLine("--- Warning {0}: {1} line: {2}, column: {3}, file: {4}",
                          warning.Code, warning.Description, warning.Line, warning.Column, warning.SourceFileNode.Name);
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

      // --- List source files with more than one type declarations

      var multiTypedFiles =
        from file in project.Files
        where file.NestedNamespaces.Count > 1 ||
              (file.NestedNamespaces.Count == 1 &&
               file.NestedNamespaces[0].TypeDeclarations.Count > 1)
        select file;
      Console.WriteLine("Files with multiple types {0} from {1}:", 
        multiTypedFiles.Count(), project.Files.Count);
      foreach (var file in multiTypedFiles)
      {
        Console.WriteLine("  {0}", file.FullName);
      }
      Console.WriteLine();
    }
  }
}
