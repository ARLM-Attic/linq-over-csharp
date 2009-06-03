using System;
using System.IO;
using CSharpFactory.ProjectModel;

namespace CSharpParserTest
{
  public abstract class ParserTestBed
  {
    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Default working drive, can be overridden with the "LOCDrive" environment variable.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    protected const string WorkingDrive = @"C:\";

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Default solution path, can be overridden with the "LOCPath" environment variable.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    protected const string WorkingPath = @"Work\LINQOverCSharp\CSharpFactoryVS2010";

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Folder for test files, cannot be overridden.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    protected const string TestFilePath = @"CSharpFactoryTest\TestFiles";

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the current working folder.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    protected static string WorkingFolder
    {
      get
      {
        var locDrive = Environment.GetEnvironmentVariable("LOCDrive") ?? WorkingDrive;
        var locPath = Environment.GetEnvironmentVariable("LOCPath") ?? WorkingPath;
        return Path.Combine(Path.Combine(locDrive, locPath), TestFilePath);
      }
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the current "Assemblies" folder.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    protected static string AsmFolder
    {
      get { return WorkingFolder + @"\Assemblies"; }
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the current temporary output folder.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    protected static string TempOutputFolder
    {
      get { return WorkingFolder + @"\Temp"; }
    }

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
      foreach (var error in parser.Errors)
      {
        Console.WriteLine("({0}, {1}) in {4}: {2}: {3}", error.Line, error.Column,
          error.Code, error.Description, error.SourceFileNode.Name);
      }
      Console.WriteLine();
      Console.WriteLine("{0} warnings detected", parser.Warnings.Count);
      foreach (var warning in parser.Warnings)
      {
        Console.WriteLine("({0}, {1}) in {4}: {2}: {3}", warning.Line, warning.Column,
          warning.Code, warning.Description, warning.SourceFileNode.Name);
      }
      Console.WriteLine();
      return errors == 0;
    }

    protected virtual void AddCommonSource(CompilationUnit parser)
    {
    }
  }
}
