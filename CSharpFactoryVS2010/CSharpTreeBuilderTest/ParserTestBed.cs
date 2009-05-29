// ================================================================================================
// ParserTestBed.cs
//
// Created: 2009.05.29, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilderTest
{
  // ================================================================================================
  /// <summary>
  /// This class is intended to be a base class for parser tests.
  /// </summary>
  // ================================================================================================
  public abstract class ParserTestBed
  {
    protected const string WorkingFolder = 
      @"C:\Work\LINQOverCSharp\CSharpFactoryVS2010\CSharpTreeBuilderTest\TestFiles";
    protected const string AsmFolder = WorkingFolder + @"\Assemblies";
    protected const string TempOutputFolder = WorkingFolder + @"\Temp";

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Invokes the parser for the specified C# source file.
    /// </summary>
    /// <param name="fileName">Name of the C# source file.</param>
    /// <returns>
    /// True, if parsing is successful; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------------------
    public bool InvokeParser(string fileName)
    {
      var project = new CSharpProject(WorkingFolder, false);
      project.ProjectProvider.AddFile(new PhysicalSourceFile(fileName));
      AddCommonSource(project);
      return InvokeParser(project);
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Invokes the parser for the specified C# project.
    /// </summary>
    /// <param name="project">The project to invoke the parser on.</param>
    /// <returns>
    /// True, if parsing is successful; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------------------
    public bool InvokeParser(CSharpProject project)
    {
      project.Build();
      var errors = project.Errors.Count;
      Console.WriteLine("{0} errors detected", errors);
      foreach (var error in project.Errors)
      {
        Console.WriteLine("({0}, {1}) in {4}: {2}: {3}", error.Line, error.Column,
          error.Code, error.Description, error.SourceFileNode.Name);
      }
      Console.WriteLine();
      Console.WriteLine("{0} warnings detected", project.Warnings.Count);
      foreach (var warning in project.Warnings)
      {
        Console.WriteLine("({0}, {1}) in {4}: {2}: {3}", warning.Line, warning.Column,
          warning.Code, warning.Description, warning.SourceFileNode.Name);
      }
      Console.WriteLine();
      return errors == 0;
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Override this methos to add common source files to your test projects.
    /// </summary>
    /// <param name="project">Project to be parsed.</param>
    // --------------------------------------------------------------------------------------------
    protected virtual void AddCommonSource(CSharpProject project)
    {
    }
  }
}