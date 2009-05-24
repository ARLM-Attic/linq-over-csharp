// ================================================================================================
// CSharpProject.cs
//
// Created: 2009.05.21, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.Cst;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.ProjectContent
{
  // ================================================================================================
  /// <summary>
  /// This class represents a C# project.
  /// </summary>
  // ================================================================================================
  public class CSharpProject
  {
    #region Lifecycle methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CSharpProject"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private CSharpProject()
    {
      Files = new SourceFileNodeCollection();
      SyntaxTree = new CSharpSyntaxTree();
      Errors = new CompilationMessageCollection();
      Warnings = new CompilationMessageCollection();
      SemanticsTree = new CSharpSemanticsTree();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a project parser using the specified working folder.
    /// </summary>
    /// <param name="workingFolder">Folder used as the working folder</param>
    // --------------------------------------------------------------------------------
    public CSharpProject(string workingFolder)
      : this(workingFolder, false)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a project parser using the specified working folder.
    /// </summary>
    /// <param name="workingFolder">Folder used as the working folder</param>
    /// <param name="addCSharpFiles">
    /// If true, all C# files are added to the project.
    /// </param>
    // --------------------------------------------------------------------------------
    public CSharpProject(string workingFolder, bool addCSharpFiles)
      : this()
    {
      ProjectProvider =
        addCSharpFiles
          ? (ProjectProviderBase) new FolderContentProvider(workingFolder)
          : new EmptyContentProvider(workingFolder);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a project parser using the specified content provider.
    /// </summary>
    /// <param name="content">Project content provider.</param>
    // --------------------------------------------------------------------------------
    public CSharpProject(ProjectProviderBase content)
      : this()
    {
      ProjectProvider = content;
    }

    #endregion

    #region Public Properties

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the provider responsible for project content information.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    public ProjectProviderBase ProjectProvider { get; private set; }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the source files in the current project.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    public SourceFileNodeCollection Files { get; private set; }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the syntax tree representing this project.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    public CSharpSyntaxTree SyntaxTree { get; private set; }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the errors raised during the build process.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    public CompilationMessageCollection Errors { get; private set; }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the warnings raised during the build process.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    public CompilationMessageCollection Warnings { get; private set; }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the semantics tree representing this project.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    public CSharpSemanticsTree SemanticsTree { get; private set; }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates the syntax tree from the project content
    /// </summary>
    // --------------------------------------------------------------------------------------------
    public void BuildSyntaxTree()
    {
      // --- Init trees, clear messages
      SyntaxTree.Reset();
      SemanticsTree.Reset();
      Errors.Clear();
      Warnings.Clear();

      // --- Create a syntax tree for each source file
      foreach (var sourceFile in ProjectProvider.SourceFiles)
      {
        var sourceFileNode = CSharpParser.BuildAstForSourceFile(sourceFile);
        if (sourceFileNode != null) SyntaxTree.SourceFileNodes.Add(sourceFileNode);
      }
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates the semantic tree from the syntax tree
    /// </summary>
    // --------------------------------------------------------------------------------------------
    public void BuildSemanticTree()
    {
      // --- Init the semantics tree, but leave existing messages
      SemanticsTree.Reset();

      // TODO: Implement semantics tree creation
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Build the sytax tree from the project content then creates the semantic tree from the 
    /// syntax tree.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    public void Build()
    {
      BuildSyntaxTree();
      BuildSemanticTree();
    }

    #endregion
  }
}