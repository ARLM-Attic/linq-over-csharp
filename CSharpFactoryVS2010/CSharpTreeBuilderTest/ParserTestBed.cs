// ================================================================================================
// ParserTestBed.cs
//
// Created: 2009.05.29, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.IO;
using CSharpTreeBuilder.Cst;
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
    protected const string TestFilePath = @"CSharpTreeBuilderTest\TestFiles";

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
        if (!locDrive.EndsWith(@"\"))
        {
          locDrive += @"\";
        }
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

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the folder and filename for test assemlby.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    protected static string TestAssemblyPathAndFilename
    {
      get { return WorkingFolder + @"\..\..\MetadataImportTestSubject\bin\debug\MetadataImportTestSubject.dll"; }
    }

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
      return InvokeParser(project, true, true);
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Invokes the parser for the specified C# project.
    /// </summary>
    /// <param name="project">The project to invoke the parser on.</param>
    /// <param name="buildSyntaxTree">True to invoke BuildSyntaxTree on project.</param>
    /// <param name="buildSemanticTree">True to invoke BuildSemanticTree on project.</param>
    /// <returns>
    /// True, if parsing is successful; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------------------
    public bool InvokeParser(CSharpProject project, bool buildSyntaxTree, bool buildSemanticTree)
    {
      if (buildSyntaxTree)
      {
        project.BuildSyntaxTree();
      }

      if (buildSemanticTree)
      {
        project.BuildSemanticTree();
      }

      var errors = project.Errors.Count;
      Console.WriteLine("{0} errors detected", errors);
      DumpMessages(project.Errors,Console.Out);
      Console.WriteLine();
      Console.WriteLine("{0} warnings detected", project.Warnings.Count);
      DumpMessages(project.Warnings, Console.Out);
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

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Dumps a collection of compilation messages to the specified output.
    /// </summary>
    /// <param name="messages">A collection of compilation messages.</param>
    /// <param name="writer">A TextWriter object.</param>
    // --------------------------------------------------------------------------------------------
    private static void DumpMessages(CompilationMessageCollection messages, TextWriter writer)
    {
      foreach (var message in messages)
      {
        writer.WriteLine(
          message.MessageToken == null
            ? string.Format("{0}: {1}", message.Code, message.Message)
            : string.Format("({0}, {1}) in {4}: {2}: {3}", message.Line, message.Column,
                            message.Code, message.Message, message.CompilationUnitNode.Name)
          );
      }
    }
  }
}