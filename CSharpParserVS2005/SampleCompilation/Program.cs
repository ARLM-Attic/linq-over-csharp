using System;
using System.Diagnostics;
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
    const string NUnitCoreInterfacesFolder = @"C:\Work\LINQOverCSharp\CSharpParserVS2005\CSharpParserTest\LargeTestProjects\NUnit.Core.Interfaces";
    const string NUnitCoreFolder = @"C:\Work\LINQOverCSharp\CSharpParserVS2005\CSharpParserTest\LargeTestProjects\NUnit.Core";
    const string CSLAFolder = @"C:\Work\LINQOverCSharp\CSharpParserVS2005\CSharpParserTest\LargeTestProjects\CSLA";
    private const string CSharpParserFolder = @"C:\Work\LINQOverCSharp\CSharpParserVS2005\CSharpParser";

    static void Main(string[] args)
    {
      // --- Create a compilation unit with all .cs source files in the specified folder
      CompilationUnit project = new CompilationUnit(CSharpParserFolder, true);

      // --- Parse the project (sytax analysis only in this project phase)
      // --- and measure parse time
      Stopwatch watch = new Stopwatch();
      watch.Start();
      TypeReference.ResolutionCounter = 0;
      TypeReference.ResolvedToSystemType = 0;
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
      foreach(Namespace ns in project.DeclaredNamespaces)
      {
        Console.WriteLine("  {0}: with {1} fragments.", ns.Name, ns.Fragments.Count);
      }

      // --- Display typed declared
      Console.WriteLine();
      Console.WriteLine("{0} types have been declared.", project.DeclaredTypes.Count);
      foreach (TypeDeclaration td in project.DeclaredTypes)
      {
        Console.WriteLine("  {0}: with {1} members.", td.Name, td.Members.Count);
      }

      // --- Display parse time
      Console.WriteLine();
      Console.WriteLine("Parsing time: {0} ms", watch.ElapsedMilliseconds);
      Console.WriteLine("Type references: {0}", TypeReference.Locations.Count);
      Console.WriteLine("Type resolutions: {0}", TypeReference.ResolutionCounter);
      Console.WriteLine("Resolved to system type: {0}", TypeReference.ResolvedToSystemType);
    }
  }
}
