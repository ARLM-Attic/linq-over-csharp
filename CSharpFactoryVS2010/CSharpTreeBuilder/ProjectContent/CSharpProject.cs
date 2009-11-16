// ================================================================================================
// CSharpProject.cs
//
// Created: 2009.05.21, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.Collections.Generic;
using System.IO;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;

// TODO: remove reference to CSharpTreeBuilder.Cst
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.Cst;
// TODO: remove reference to CSharpTreeBuilder.CSharpAstBuilder
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.ProjectContent
{
  // ================================================================================================
  /// <summary>
  /// This class represents a C# project.
  /// </summary>
  // ================================================================================================
  public class CSharpProject: ICompilationErrorHandler
  {
    #region Lifecycle methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CSharpProject"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private CSharpProject()
    {
      SyntaxTree = new CSharpSyntaxTree();
      Errors = new CompilationMessageCollection();
      Warnings = new CompilationMessageCollection();
      SemanticGraph = new SemanticGraph(this);
      ConditionalSymbols = new List<string>();

      // --- Set up the default error handling parameters
      ErrorMessageFormat = "-- line {0} col {1}: {2}";
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
    /// Gets the semantic graph representing this project.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    public SemanticGraph SemanticGraph { get; private set; }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the list of conditional symbols.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    public List<string> ConditionalSymbols { get; private set; }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a conditional compilation symbol to the list of existing symbols.
    /// </summary>
    /// <param name="symbol">Symbols to add.</param>
    // --------------------------------------------------------------------------------------------
    public void AddConditionalCompilationSymbol(string symbol)
    {
      ProjectProvider.AddConditionalSymbol(symbol);
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a list of conditional compilation symbol to the list of existing symbols.
    /// </summary>
    /// <param name="symbols">Array of symbols to add.</param>
    // --------------------------------------------------------------------------------------------
    public void AddConditionalCompilationSymbols(String[] symbols)
    {
      foreach (var symbol in symbols)
        AddConditionalCompilationSymbol(symbol);
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates the syntax tree from the project content
    /// </summary>
    // --------------------------------------------------------------------------------------------
    public void BuildSyntaxTree()
    {
      // --- Init trees, clear messages
      SyntaxTree.Reset();
      ConditionalSymbols.Clear();
      foreach (var item in ProjectProvider.ConditionalSymbols) ConditionalSymbols.Add(item);
      Errors.Clear();
      Warnings.Clear();

      // --- Create a syntax tree for each source file
      foreach (var sourceFile in ProjectProvider.SourceFiles)
      {
        SourceInProgress = new CompilationUnitNode(sourceFile.FullName);
        CSharpParser.BuildAstForCompilationUnit(SourceInProgress, this);
        SyntaxTree.CompilationUnitNodes.Add(SourceInProgress);
      }
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates the semantic tree from the syntax tree
    /// </summary>
    // --------------------------------------------------------------------------------------------
    public void BuildSemanticTree()
    {
      // Create a factory that imports metadata from assemblies to the semantic graph
      var factory = new MetadataImporterSemanticEntityFactory(this, SemanticGraph);

      // Load mscorlib entities into the semantic graph
      factory.ImportTypesIntoSemanticGraph(typeof(int).Assembly.Location, "global");
      SemanticGraph.BuildState = SemanticGraphBuildState.MscorlibImported;

      // Load other referenced assemblies into the semantic graph
      foreach (var referencedUnit in ProjectProvider.References)
      {
        if (referencedUnit is ReferencedAssembly)
        {
          var referencedAssembly = referencedUnit as ReferencedAssembly;
          var alias = string.IsNullOrEmpty(referencedAssembly.Alias) ? "global" : referencedAssembly.Alias;

          factory.ImportTypesIntoSemanticGraph(Path.Combine(referencedAssembly.FilePath, referencedAssembly.Name), alias);
        }
      }
      SemanticGraph.BuildState = SemanticGraphBuildState.ReferencedUnitsImported;

      // Create entities from ASTs
      SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(this));
      SemanticGraph.BuildState = SemanticGraphBuildState.SyntaxTreesImported;

      // Merge partial type entities.
      SemanticGraph.AcceptVisitor(new PartialTypeMergingSemanticGraphVisitor());
      SemanticGraph.BuildState = SemanticGraphBuildState.PartialTypesMerged;

      // Resolve type references in 2 passes (1. type declarations, 2. type bodies)
      SemanticGraph.AcceptVisitor(new TypeResolverPass1SemanticGraphVisitor(this, SemanticGraph));
      SemanticGraph.BuildState = SemanticGraphBuildState.TypeDeclarationsResolved;

      SemanticGraph.AcceptVisitor(new TypeResolverPass2SemanticGraphVisitor(this, SemanticGraph));
      SemanticGraph.BuildState = SemanticGraphBuildState.TypeBodiesResolved;

      // Evaluate expressions in the semantic graph.
      SemanticGraph.AcceptVisitor(new ExpressionEvaluatorSemanticGraphVisitor(this, SemanticGraph));
      SemanticGraph.BuildState = SemanticGraphBuildState.ExpressionsEvaluated;

      // TODO: continue
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

    #region Implementation of ICompilationErrorHandler

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Defines the format of message to write to the output stream.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    public string ErrorMessageFormat { get ; set; }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Add a new error to the list of errors.
    /// </summary>
    /// <param name="code">Error code.</param>
    /// <param name="errorPoint">Token describing the error position.</param>
    /// <param name="description">Detailed error description.</param>
    // --------------------------------------------------------------------------------------------
    void ICompilationErrorHandler.Error(string code, Token errorPoint, string description)
    {
      SignError(Errors, code, errorPoint, description, null);
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Add a new error to the list of errors.
    /// </summary>
    /// <param name="code">Error code.</param>
    /// <param name="errorPoint">Token describing the error position.</param>
    /// <param name="description">Detailed error description.</param>
    /// <param name="parameters">Error parameters.</param>
    // --------------------------------------------------------------------------------------------
    void ICompilationErrorHandler.Error(string code, Token errorPoint, string description, 
      params object[] parameters)
    {
      SignError(Errors, code, errorPoint, description, parameters);
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Add a new warning to the list of warnings.
    /// </summary>
    /// <param name="code">Warning code.</param>
    /// <param name="warningPoint">Token describing the warning position.</param>
    /// <param name="description">Detailed warning description.</param>
    // --------------------------------------------------------------------------------------------
    void ICompilationErrorHandler.Warning(string code, Token warningPoint, string description)
    {
      SignError(Warnings, code, warningPoint, description, null);
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Add a new warning to the list of warnings.
    /// </summary>
    /// <param name="code">Warning code.</param>
    /// <param name="warningPoint">Token describing the warning position.</param>
    /// <param name="description">Detailed warning description.</param>
    /// <param name="parameters">Warning parameters.</param>
    // --------------------------------------------------------------------------------------------
    void ICompilationErrorHandler.Warning(string code, Token warningPoint, string description, 
      params object[] parameters)
    {
      SignError(Warnings, code, warningPoint, description, parameters);
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Redirects line numbering and file name handling.
    /// </summary>
    /// <param name="currentLine">Current source line.</param>
    /// <param name="lineNumber">New line number.</param>
    /// <param name="fileName">Redirected filename.</param>
    // --------------------------------------------------------------------------------------------
    void ICompilationErrorHandler.Redirect(int currentLine, int lineNumber, string fileName)
    {
      ErrorLineOffset = lineNumber - currentLine;
      ErrorFile = fileName;
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Resets the line number and file name redirection.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    void ICompilationErrorHandler.ResetRedirection()
    {
      ErrorLineOffset = -1;
      ErrorFile = null;
    }

    #endregion

    #region Helper members

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the error line offset.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    private int ErrorLineOffset { get; set; }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the error file.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    private string ErrorFile { get; set; }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the source in progress.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    private CompilationUnitNode SourceInProgress { get; set; }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Add a new error to the list of errors.
    /// </summary>
    /// <param name="messageCollection">Collection where the error should be added.</param>
    /// <param name="code">Error code.</param>
    /// <param name="errorPoint">Token describing the error position.</param>
    /// <param name="description">Detailed error description.</param>
    /// <param name="parameters">Error parameters.</param>
    // --------------------------------------------------------------------------------------------
    private void SignError(CompilationMessageCollection messageCollection, string code, 
      Token errorPoint, string description, params object[] parameters)
    {
      var message = new CompilationMessageNode(SourceInProgress, code, errorPoint, description) 
        { Parameters = parameters };
      message.SetErrorLineOffset(ErrorLineOffset);
      message.RedirectSourceFile(ErrorFile);
      messageCollection.Add(message);
    }

    #endregion
  }
}