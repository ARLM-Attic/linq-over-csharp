// ================================================================================================
// ProjectProviderBase.cs
//
// Created: 2009.03.04, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.IO;
using CSharpFactory.Collections;
using CSharpFactory.ProjectModel;
using CSharpFactory.SolutionHierarchy;

namespace CSharpTreeBuilder.ProjectContent
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a project provider that is able to define source files, 
  /// referenced assemblies, conditionals for a project.
  /// </summary>
  // ================================================================================================
  public abstract class ProjectProviderBase
  {
    #region Lifecycle methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectProviderBase"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected ProjectProviderBase()
    {
      SourceFiles = new ImmutableCollection<SourceFileBase>();
      ConditionalSymbols = new ImmutableCollection<string>();
      References = new ImmutableCollection<ReferencedUnit>();
      AddAssemblyReference("mscorlib");
    }

    #endregion

    #region Public properties

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the project.
    /// </summary>
    /// <value>The name.</value>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; protected set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the working folder of the project.
    /// </summary>
    /// <value>The working folder.</value>
    // ----------------------------------------------------------------------------------------------
    public string WorkingFolder { get; protected set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of source files in the project.
    /// </summary>
    /// <value>The source files.</value>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<SourceFileBase> SourceFiles { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of conditional symbols used when compiling the project.
    /// </summary>
    /// <value>The conditional symbols.</value>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<string> ConditionalSymbols { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the references used during the compilation of the project.
    /// </summary>
    /// <value>The references.</value>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<ReferencedUnit> References { get; private set; }

    #endregion

    #region ReferencedUnit operations

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new assembly reference to the C# project.
    /// </summary>
    /// <param name="name">Name of the assembly to add.</param>
    /// <param name="alias">Assembly alias name</param>
    // ----------------------------------------------------------------------------------------------
    public void AddAliasedReference(string alias, string name)
    {
      References.Add(
        new ReferencedAssembly(name, string.Empty, alias));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new assembly reference to the C# project.
    /// </summary>
    /// <param name="name">Name of the assembly to add.</param>
    /// <param name="path">Path of the assembly file.</param>
    /// <param name="alias">Assembly alias name</param>
    // ----------------------------------------------------------------------------------------------
    public void AddAliasedReference(string alias, string name, string path)
    {
      References.Add(new ReferencedAssembly(name, path, alias));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new assembly reference to the C# project.
    /// </summary>
    /// <param name="name">Name of the assembly to add.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddAssemblyReference(string name)
    {
      References.Add(new ReferencedAssembly(name));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new assembly reference to the C# project.
    /// </summary>
    /// <param name="name">Name of the assembly to add.</param>
    /// <param name="path">Path of the assembly file.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddAssemblyReference(string name, string path)
    {
      References.Add(new ReferencedAssembly(name, path));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new project reference to the C# project.
    /// </summary>
    /// <param name="unit">Compilation unit representing the project.</param>
    /// <param name="name">Name of the project to add.</param>
    // --------------------------------------------------------------------------------
    public void AddProjectReference(ProjectProviderBase unit, string name)
    {
      References.Add(new ReferencedCompilation(unit, name));
    }

    #endregion

    #region ConditionalSymbols operations

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a conditional directive to the list of existing conditionals
    /// </summary>
    /// <param name="symbol">Conditional directive</param>
    // --------------------------------------------------------------------------------
    public void AddConditionalSymbol(string symbol)
    {
      symbol = symbol.Trim();
      if (!ConditionalSymbols.Contains(symbol))
      {
        ConditionalSymbols.Add(symbol);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Removes a conditional directive from the list of existing conditionals
    /// </summary>
    /// <param name="symbol">Conditional directive</param>
    // --------------------------------------------------------------------------------
    public void RemoveConditionalDirective(string symbol)
    {
      ConditionalSymbols.Remove(symbol);
    }

    #endregion

    #region SourceFileOperations

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a physical source file to the project.
    /// </summary>
    /// <param name="fileName">File to add to the project.</param>
    /// <remarks>
    /// File name is relative to the working folder.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public void AddFile(string fileName)
    {
      string fullName = Path.Combine(WorkingFolder, fileName);
      SourceFiles.Add(new PhysicalSourceFile(fullName));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a physical source file specified with full name to the project.
    /// </summary>
    /// <param name="fullName">Full name of the file to add to the project.</param>
    /// <remarks>
    /// File name is relative to the working folder.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public void AddFileWithFullName(string fullName)
    {
      SourceFiles.Add(new PhysicalSourceFile(fullName));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a source file to the project.
    /// </summary>
    /// <param name="file">File to add to the project.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddFile(SourceFileBase file)
    {
      SourceFiles.Add(file);
    }

    #endregion
  }
}