using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;
using CSharpFactory.ParserFiles.PPExpressions;
using CSharpFactory.ProjectContent;
using CSharpFactory.Semantics;
using CSharpFactory.Syntax;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class is responsible for parsing all files in a C# project.
  /// </summary>
  // ==================================================================================
  public sealed class CompilationUnit: ICompilationErrorHandler
  {
    #region Constant values

    /// <summary>Display name of this compilation unit.</summary>
    public const string ThisUnitName = "<this unit>";
    /// <summary>Display name of the global namespace.</summary>
    public const string GlobalHierarchyName = "global namespace";

    #endregion

    #region Private Fields

    // --- Members related to error handling
    private int _ErrorLineOffset;
    private string _ErrorFile;

    // --- Members related to semantics
    private NamespaceOrTypeResolver _NamespaceOrTypeResolver;

    // --- Types to check and fix (arrays, pointers, generic types)
    private readonly List<TypeReference> _TypesToFix = new List<TypeReference>();

    // --- Documentation comments to check
    private readonly List<CommentInfo> _CommentsToCheck = new List<CommentInfo>();

    // --- Diagnostic counters
    private int _ResolutionCounter;
    private int _ResolvedToSystemType;
    private int _ResolvedToSourceType;
    private int _ResolvedToNamespace;
    private int _ResolvedToName;
    private int _ResolvedToHierarchy;
    private readonly List<TypeReferenceLocation> _Locations =
      new List<TypeReferenceLocation>();

    #endregion

    #region Lifecycle methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CompilationUnit"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private CompilationUnit()
    {
      ErrorMessageFormat = "-- line {0} col {1}: {2}";
      NamespaceHierarchies = new Dictionary<string, NamespaceHierarchy>();
      GlobalHierarchy = new NamespaceHierarchy();
      ConditionalSymbols = new List<string>();
      DeclaredTypes = new TypeDeclarationCollection();
      DeclaredNamespaces = new NamespaceCollection();
      Files = new SourceFileCollection();
      ErrorHandler = this;
      CurrentFile = null;
      _ErrorLineOffset = -1;
      _ErrorFile = null;
      ThisUnit = new ReferencedCompilation(this, ThisUnitName);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a project parser using the specified working folder.
    /// </summary>
    /// <param name="workingFolder">Folder used as the working folder</param>
    // --------------------------------------------------------------------------------
    public CompilationUnit(string workingFolder):
      this(workingFolder, false)
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
    public CompilationUnit(string workingFolder, bool addCSharpFiles): this()
    {
      if (addCSharpFiles)
      {
        ProjectProvider = new FolderContentProvider(workingFolder);
      }
      else
      {
        ProjectProvider = new EmptyContentProvider(workingFolder);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a project parser using the specified content provider.
    /// </summary>
    /// <param name="content">Project content provider.</param>
    // --------------------------------------------------------------------------------
    public CompilationUnit(ProjectProviderBase content): this()
    {
      ProjectProvider = content;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the compilation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Name
    {
      get { return ProjectProvider.Name; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the working folder of this compilation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string WorkingFolder
    {
      get { return ProjectProvider.WorkingFolder; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference for this compilation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ReferencedCompilation ThisUnit { get; private set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parser.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CSharpSyntaxParser Parser { get; private set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of errors.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CompilationMessageCollection Errors 
    {
      get { return SemanticsTree.Errors;  }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of warnings.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CompilationMessageCollection Warnings
    {
      get { return SemanticsTree.Warnings; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the object handling compilation errors.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ICompilationErrorHandler ErrorHandler { get; set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of files included in the project.
    /// </summary>
    // --------------------------------------------------------------------------------
    public SourceFileCollection Files { get; private set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the current file that is under compilation.
    /// </summary>
    // --------------------------------------------------------------------------------
    public SourceFile CurrentFile { get; private set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the dictionary of all namespaces declared in this project.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceCollection DeclaredNamespaces { get; private set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the dictionary of all types declared in this project.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeDeclarationCollection DeclaredTypes { get; private set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of compilation references
    /// </summary>
    // --------------------------------------------------------------------------------
    public ImmutableCollection<ReferencedUnit> ReferencedUnits
    {
      get { return ProjectProvider.References; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of conditional compilation symbols
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<string> ConditionalSymbols { get; private set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the global namespace hierarchy of this compilation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceHierarchy GlobalHierarchy { get; private set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the tree to resolve namespaces and types declared in the source code.
    /// </summary>
    // --------------------------------------------------------------------------------
    public SourceResolutionTree SourceResolutionTree { get; private set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the named namespace hierarchies of this compilation unit.
    /// </summary>
    /// <remarks>
    /// The global namespace hierarchy in not included among these hierarchies.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public Dictionary<string, NamespaceHierarchy> NamespaceHierarchies { get; private set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the file where XML documentation should be put.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string XmlDocumentFile { get; set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if XML document comments should be processed or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasXmlDocumentFile
    {
      get { return !String.IsNullOrEmpty(XmlDocumentFile); }  
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the source file node currently under syntax parsing.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SourceFileNode SourceFileNode { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the syntax tree after the compilation unit has been parsed.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ISyntaxTree SyntaxTree { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the syntax tree after the compilation unit has been parsed.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ISemanticsTree SemanticsTree { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the project provider.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ProjectProviderBase ProjectProvider { get; private set; }

    #endregion

    #region Properties related to diagnostics

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the count of references resolved.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int ResolutionCounter
    {
      get { return _ResolutionCounter; }
      set { _ResolutionCounter = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the count of references resolved to system types.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int ResolvedToSystemType
    {
      get { return _ResolvedToSystemType; }
      set { _ResolvedToSystemType = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the location of type references.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<TypeReferenceLocation> Locations
    {
      get { return _Locations; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the count of references resolved to source-declared types.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int ResolvedToSourceType
    {
      get { return _ResolvedToSourceType; }
      set { _ResolvedToSourceType = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the count of references resolved to a namespace.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int ResolvedToNamespace
    {
      get { return _ResolvedToNamespace; }
      set { _ResolvedToNamespace = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the count of references resolved to a namespace hierarchy.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int ResolvedToHierarchy
    {
      get { return _ResolvedToHierarchy; }
      set { _ResolvedToHierarchy = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the count of references resolved to simple names.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int ResolvedToName
    {
      get { return _ResolvedToName; }
      set { _ResolvedToName = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resets the diagnostic counters.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void ResetDiagnosticCounters()
    {
      _ResolutionCounter = 0;
      _ResolvedToSystemType = 0;
      _ResolvedToSourceType = 0;
      _ResolvedToNamespace = 0;
      _ResolvedToHierarchy = 0;
      _ResolvedToName = 0;
      _Locations.Clear();
    }

    #endregion

    #region Public events

    // --------------------------------------------------------------------------------
    /// <summary>
    /// This event is called before any parser initialization occurres.
    /// </summary>
    /// <remarks>
    /// Subcribers of this event can add their own initialization code.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public event EventHandler<ParseCancelEventArgs> BeforeInitParse;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// This event is called after any parser initialization finished.
    /// </summary>
    /// <remarks>
    /// Subcribers of this event can add their own initialization code.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public event EventHandler<ParseCancelEventArgs> AfterInitParse;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// This event is called before a referenced unit is parsed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public event EventHandler<ParseReferencedUnitEventArgs> BeforeParseReferencedUnit;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// This event is called after a referenced unit is parsed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public event EventHandler<ParseReferencedUnitEventArgs> AfterParseReferencedUnit;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// This event is called before a source file is parsed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public event EventHandler<ParseFileEventArgs> BeforeParseFile;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// This event is called after a source file is parsed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public event EventHandler<ParseFileEventArgs> AfterParseFile;

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a file to the project.
    /// </summary>
    /// <param name="fileName">File to add to the project.</param>
    /// <remarks>
    /// File name is relative to the working folder.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public void AddFile(string fileName)
    {
      ProjectProvider.AddFile(fileName);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a file to the project using full file name.
    /// </summary>
    /// <param name="fileName">File to add to the project.</param>
    // --------------------------------------------------------------------------------
    public void AddFileWithFullName(string fileName)
    {
      ProjectProvider.AddFileWithFullName(fileName);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new assembly reference to the C# project.
    /// </summary>
    /// <param name="name">Name of the assembly to add.</param>
    // --------------------------------------------------------------------------------
    public void AddAssemblyReference(string name)
    {
      ProjectProvider.AddAssemblyReference(name);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new assembly reference to the C# project.
    /// </summary>
    /// <param name="name">Name of the assembly to add.</param>
    /// <param name="path">Path of the assembly file.</param>
    // --------------------------------------------------------------------------------
    public void AddAssemblyReference(string name, string path)
    {
      ProjectProvider.AddAssemblyReference(name, path);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new assembly reference to the C# project.
    /// </summary>
    /// <param name="name">Name of the assembly to add.</param>
    /// <param name="alias">Assembly alias name</param>
    // --------------------------------------------------------------------------------
    public void AddAliasedReference(string alias, string name)
    {
      ProjectProvider.AddAliasedReference(alias, name);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new assembly reference to the C# project.
    /// </summary>
    /// <param name="name">Name of the assembly to add.</param>
    /// <param name="path">Path of the assembly file.</param>
    /// <param name="alias">Assembly alias name</param>
    // --------------------------------------------------------------------------------
    public void AddAliasedReference(string alias, string name, string path)
    {
      ProjectProvider.AddAliasedReference(alias, name, path);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a type declaration to the container of types declared in this compilation
    /// unit.
    /// </summary>
    /// <param name="type">Type to add</param>
    /// <remarks>
    /// If a type declaration is in the container it is not added again.
    /// </remarks>
    // --------------------------------------------------------------------------------
    internal void AddTypeDeclaration(TypeDeclaration type)
    {
      DeclaredTypes.Add(type);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a conditional compilation symbol to the list of existing symbols.
    /// </summary>
    /// <param name="symbol">Symbols to add.</param>
    // --------------------------------------------------------------------------------
    public void AddConditionalCompilationSymbol(string symbol)
    {
      ProjectProvider.AddConditionalSymbol(symbol);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a list of conditional compilation symbol to the list of existing symbols.
    /// </summary>
    /// <param name="symbols">Array of symbols to add.</param>
    // --------------------------------------------------------------------------------
    public void AddConditionalCompilationSymbols(String[] symbols)
    {
      foreach (var symbol in symbols) 
        AddConditionalCompilationSymbol(symbol);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a conditional directive to the list of existing conditionals
    /// </summary>
    /// <param name="symbol">Conditional directive</param>
    // --------------------------------------------------------------------------------
    public void AddConditionalDirective(String symbol)
    {
      symbol = symbol.Trim();
      if (!ConditionalSymbols.Contains(symbol)) ConditionalSymbols.Add(symbol);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Removes a conditional directive from the list of existing conditionals
    /// </summary>
    /// <param name="symbol">Conditional directive</param>
    // --------------------------------------------------------------------------------
    public void RemoveConditionalDirective(String symbol)
    {
      ConditionalSymbols.Remove(symbol.Trim());
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the specified symbol is a conditional symbol or not.
    /// </summary>
    /// <param name="symbol">Symbol to check.</param>
    /// <returns>
    /// True, if the symbol is a conditional symbol; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public bool IsConditionalSymbolDefined(String symbol)
    {
      return ConditionalSymbols.Contains(symbol);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates the specified preprocessor expression.
    /// </summary>
    /// <param name="preprocessorExpression">Expression to evaluate</param>
    /// <returns>
    /// Result of the preprocessor evaluation
    /// </returns>
    // --------------------------------------------------------------------------------
    public PPEvaluationStatus EvaluatePreprocessorExpression(string preprocessorExpression)
    {
      var memStream =
        new MemoryStream(new UTF8Encoding().GetBytes(preprocessorExpression));
      var scanner = new PPScanner(memStream);
      var parser = new CSharpPPExprSyntaxParser(scanner);
      parser.Parse();
      if (parser.ErrorFound)
      {
        return PPEvaluationStatus.Failed;
      }
      return parser.Expression.Evaluate(ConditionalSymbols)
               ? PPEvaluationStatus.True
               : PPEvaluationStatus.False;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a type to the list of type to fix.
    /// </summary>
    /// <param name="type">Type reference to fix up</param>
    // --------------------------------------------------------------------------------
    internal void AddTypeToFix(TypeReference type)
    {
      if (type.TypeModifiers.Count > 0 || type.Arguments.Count > 0 ||type.IsNullable)
      {
        _TypesToFix.Add(type);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a comment to the list of comments to be checked later.
    /// </summary>
    /// <param name="comment">Comment to add</param>
    // --------------------------------------------------------------------------------
    internal void AddCommentToFix(CommentInfo comment)
    {
      _CommentsToCheck.Add(comment);  
    }

    #endregion

    #region ICompilationErrorHandler implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the output stream where errors should be written.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TextWriter ErrorStream { get; set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the format of message to write to the output stream.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string ErrorMessageFormat { get; set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Add a new error to the list of errors.
    /// </summary>
    /// <param name="code">Error code.</param>
    /// <param name="errorPoint">Token describing the error position.</param>
    /// <param name="description">Detailed error description.</param>
    // --------------------------------------------------------------------------------
    void ICompilationErrorHandler.Error(string code, ParserFiles.Token errorPoint, 
      string description)
    {
      SignError(Errors, code, errorPoint, description, null);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Add a new error to the list of errors.
    /// </summary>
    /// <param name="code">Error code.</param>
    /// <param name="errorPoint">Token describing the error position.</param>
    /// <param name="description">Detailed error description.</param>
    /// <param name="parameters">Error parameters.</param>
    // --------------------------------------------------------------------------------
    void ICompilationErrorHandler.Error(string code, ParserFiles.Token errorPoint, 
      string description,
      params object[] parameters)
    {
      SignError(Errors, code, errorPoint, description, parameters);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Add a new warning to the list of warnings.
    /// </summary>
    /// <param name="code">Warning code.</param>
    /// <param name="warningPoint">Token describing the warning position.</param>
    /// <param name="description">Detailed warning description.</param>
    // --------------------------------------------------------------------------------
    void ICompilationErrorHandler.Warning(string code, ParserFiles.Token warningPoint, string description)
    {
      SignError(Warnings, code, warningPoint, description, null);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Add a new warning to the list of warnings.
    /// </summary>
    /// <param name="code">Warning code.</param>
    /// <param name="warningPoint">Token describing the warning position.</param>
    /// <param name="description">Detailed warning description.</param>
    /// <param name="parameters">Warning parameters.</param>
    // --------------------------------------------------------------------------------
    void ICompilationErrorHandler.Warning(string code, ParserFiles.Token warningPoint, string description,
                                          params object[] parameters)
    {
      SignError(Warnings, code, warningPoint, description, parameters);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Redirects line numbering and file name handling.
    /// </summary>
    /// <param name="currentLine">Current source line.</param>
    /// <param name="lineNumber">New line number.</param>
    /// <param name="fileName">Redirected filename.</param>
    // --------------------------------------------------------------------------------
    void ICompilationErrorHandler.Redirect(int currentLine, int lineNumber, string fileName)
    {
      _ErrorLineOffset = lineNumber - currentLine;
      _ErrorFile = fileName;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resets the line number and file name redirection.
    /// </summary>
    // --------------------------------------------------------------------------------
    void ICompilationErrorHandler.ResetRedirection()
    {
      _ErrorLineOffset = -1;
      _ErrorFile = null;
    }

    #endregion

    #region Parse method and its helpers

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Parses all the files within the project.
    /// </summary>
    /// <returns>
    /// Number of errors.
    /// </returns>
    // --------------------------------------------------------------------------------
    public int Parse()
    {
      if (!InitParse()) return -1;
      if (!ParseReferencedProjects()) return -1;
      if (!ParseSyntax()) return -1;

      // --- Semantical parsing
      _NamespaceOrTypeResolver = new NamespaceOrTypeResolver(Parser);

      // --- Phase 1: Collect namespace hierarchy information from referenced assemblies
      // --- and namespaces declared by the source code. Set the resolvers for all 
      // --- namespaces and types. 
      // --- Result: After this phase we can resolve all fully qualified namespace and 
      // --- type names. We are not ready to use aliases or namespaces referenced by 
      // --- using directives.
      PrepareNamespaceHierarchies();
      SetNamespaceResolvers();
      SetTypeResolvers();
      
      // --- Phase 2: Resolve the extern alias, using alias and using directives in the
      // --- source code. 
      // --- Result: After this phase we are able to resolve any namespace and type
      // --- names that has been properly referenced.
      ResolveExternAliasClauses();
      ResolveUsingDirectives();

      // --- Phase 3: We check if there are any types having the same fully qualified names.
      // --- Only partial types having the same declaration type (class, struct or interface)
      // --- are accepted, any other types are ignored. In case of multiple type declarations
      // --- the first (the parser detected it first) type declaration fragment is processed,
      // --- others are ignored. After multiple types have been checked, the base type names
      // --- are resolved and checked. Types are checked for circular dependency.
      // --- Result: Types declared in source code are unambigous, they have their proper
      // --- place in the object hierarchy and they are free from circular dependencies.
      CheckMultipleAndPartialDeclaration();
      ResolveAndCheckBaseTypes();
      ConsolidatePartialTypeDeclarations();
      CheckCircularDependency();

      // --- Phase 4: Resolve all remaining type references and check type declarations
      ResolveTypeReferences();
      BuildConstructedTypes();
      CheckTypeDeclarations();
      CheckTypeConstraintDeclarations();
      CheckMemberDeclaration();

      // --- Phase 5: Resolve all remaining type references

      // --- Phase 6: Check partial types
      CheckPartialTypes();

      // --- Documentation comment checking phase
      if (HasXmlDocumentFile) CheckDocumentComments();

      // --- Return the number of errors found
      return Errors.Count;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes the parsing process
    /// </summary>
    /// <returns>
    /// True, if parsing has been successfully initialized; false, if cancelled.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    private bool InitParse()
    {
      // --- Notify subscribers (BeforeInitParse event)
      bool cancelled = false;
      OnBeforeInitParse(ref cancelled);
      if (cancelled) return false;

      // --- Fill up the source files from the project content provider
      Files.Clear();
      foreach (var file in ProjectProvider.SourceFiles)
      {
        Files.Add(new SourceFile(file.FullName, this));
      }

      // --- Fill up the conditional symbols defined
      ConditionalSymbols.Clear();
      foreach (var symbol in ProjectProvider.ConditionalSymbols)
      {
        ConditionalSymbols.Add(symbol);
      }

      // --- Set up the syntax and semantics trees
      SyntaxTree = new SyntaxTree();
      SemanticsTree = new SemanticsTree();

      GlobalHierarchy.Clear();
      NamespaceHierarchies.Clear();
      ResetDiagnosticCounters();

      // --- Notify subscribers (AfterInitParse event)
      OnAfterInitParse(ref cancelled);
      return (!cancelled);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Parses all referenced project before parsing this compilation unit
    /// </summary>
    /// <returns>
    /// True, if parsing has been successfully done; false, if cancelled.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    private bool ParseReferencedProjects()
    {
      // --- First we compile all referenced projects.
      bool cancelled = false;
      foreach (ReferencedUnit unit in ProjectProvider.References)
      {
        var compilation = unit as ReferencedCompilation;
        if (compilation == null) continue;
        var refUnit = compilation.CompilationUnit;

        // --- Notify subscribers (BeforeParseReferencedUnit event)
        OnBeforeParseReferencedUnit(refUnit, ref cancelled);
        if (cancelled) return false;

        refUnit.Parse();

        // --- Notify subscribers (AfterParseReferencedUnit event)
        OnAfterParseReferencedUnit(refUnit, ref cancelled);
        if (cancelled) return false;
      }
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Executes syntax parsing on all files belonging to this project.
    /// </summary>
    /// <returns>
    /// True, if parsing has been successfully done; false, if cancelled.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    private bool ParseSyntax()
    {
      bool cancelled = false;
      foreach (SourceFile file in Files)
      {
        var scanner = new Scanner(File.OpenText(file.FullName).BaseStream);
        CurrentFile = file;

        // --- Notify subscribers (BeforeParseFile event)
        OnBeforeParseFile(file, ref cancelled);
        if (cancelled) return false;

        SourceFileNode = new SourceFileNode(file.FullName);
        SyntaxTree.SourceFileNodes.Add(SourceFileNode);
        Parser = new CSharpSyntaxParser(scanner, this, file, SourceFileNode);
        Parser.Parse();

        // --- Notify subscribers (AfterParseFile event)
        OnAfterParseFile(file, ref cancelled);
        if (cancelled) return false;
      }
      return true;
    }

    #endregion

    #region Private event fire methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Fires the BeforeInitParse event
    /// </summary>
    /// <param name="cancelled">
    /// True value indicates that parsing is about to cancel.
    /// </param>
    // --------------------------------------------------------------------------------
    private void OnBeforeInitParse(ref bool cancelled)
    {
      if (BeforeInitParse != null)
      {
        var args = new ParseCancelEventArgs(this);
        BeforeInitParse.Invoke(this, args);
        cancelled = args.Cancel;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Fires the AfterInitParse event
    /// </summary>
    /// <param name="cancelled">
    /// True value indicates that parsing is about to cancel.
    /// </param>
    // --------------------------------------------------------------------------------
    private void OnAfterInitParse(ref bool cancelled)
    {
      if (AfterInitParse != null)
      {
        var args = new ParseCancelEventArgs(this);
        AfterInitParse.Invoke(this, args);
        cancelled = args.Cancel;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Fires the BeforeParseReferenceUnit event
    /// </summary>
    /// <param name="refUnit">Reference unit parsed</param>
    /// <param name="cancelled">
    /// True value indicates that parsing is about to cancel.
    /// </param>
    // --------------------------------------------------------------------------------
    private void OnBeforeParseReferencedUnit(CompilationUnit refUnit,
      ref bool cancelled)
    {
      if (BeforeParseReferencedUnit != null)
      {
        var args = new ParseReferencedUnitEventArgs(refUnit, this);
        BeforeParseReferencedUnit.Invoke(this, args);
        cancelled = args.Cancel;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Fires the AfterParseReferenceUnit event
    /// </summary>
    /// <param name="refUnit">Reference unit parsed</param>
    /// <param name="cancelled">
    /// True value indicates that parsing is about to cancel.
    /// </param>
    // --------------------------------------------------------------------------------
    private void OnAfterParseReferencedUnit(CompilationUnit refUnit, 
      ref bool cancelled)
    {
      if (AfterParseReferencedUnit != null)
      {
        var args = new ParseReferencedUnitEventArgs(this, refUnit);
        AfterParseReferencedUnit.Invoke(this, args);
        cancelled = args.Cancel;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Fires the BeforeParseFile event
    /// </summary>
    /// <param name="file">Source file about to parse</param>
    /// <param name="cancelled">
    /// True value indicates that parsing is about to cancel.
    /// </param>
    // --------------------------------------------------------------------------------
    private void OnBeforeParseFile(SourceFile file,
      ref bool cancelled)
    {
      if (BeforeParseFile != null)
      {
        var args = new ParseFileEventArgs(this, file);
        BeforeParseFile.Invoke(this, args);
        cancelled = args.Cancel;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Fires the AfterParseFile event
    /// </summary>
    /// <param name="file">Source file parsed</param>
    /// <param name="cancelled">
    /// True value indicates that parsing is about to cancel.
    /// </param>
    // --------------------------------------------------------------------------------
    private void OnAfterParseFile(SourceFile file,
      ref bool cancelled)
    {
      if (AfterParseFile != null)
      {
        var args = new ParseFileEventArgs(this, file);
        AfterParseFile.Invoke(this, args);
        cancelled = args.Cancel;
      }
    }

    #endregion

    #region Resolve type references

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in the related compliation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void ResolveTypeReferences()
    {
      foreach (SourceFile source in Files)
      {
        CurrentFile = source;
        source.ResolveTypeReferences(ResolutionContext.SourceFile, source, null);
      }
    }

    #endregion

    #region Methods for preparing the namespace hierarchies

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Prepares the global namespace hierarchy and hierarchies defined by the aliased
    /// assembly referencies.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Collects all namespaces for the global namespace hierarchy from referenced 
    /// assemblies with no aliases. Aliases assemblies define separate namespace
    /// hierarchies.
    /// </para>
    /// <para>
    /// Global namespace information is also collected from the namespaces defined
    /// by the source code of this compilation unit.
    /// </para>
    /// </remarks>
    // --------------------------------------------------------------------------------
    private void PrepareNamespaceHierarchies()
    {
      // --- Init data structures
      GlobalHierarchy.Clear();

      // --- Create a tree representing the types in the source code
      SourceResolutionTree = new SourceResolutionTree(ThisUnitName);
      GlobalHierarchy.AddTree(ThisUnitName, SourceResolutionTree);

      // --- No named heirarchies found yet
      NamespaceHierarchies.Clear();

      var importedAssemblies = new Dictionary<string, AssemblyResolutionTree>();

      // --- Go through all referenced assemblies, skip ohter kind of referencies.
      foreach (var reference in ReferencedUnits)
      {
        TypeResolutionTree resolutionTree;
        NamespaceHierarchy hierarchy;
        string treeName;

        var asmRef = reference as ReferencedAssembly;
        if (asmRef != null)
        {
          // --- This reference is a referenced assembly.
          // --- Check, if this assembly has already been loaded into a resolution tree
          treeName = asmRef.Assembly.FullName;
          AssemblyResolutionTree tree;
          if (!importedAssemblies.TryGetValue(treeName, out tree))
          {
            // --- Create and load the assembly
            tree = new AssemblyResolutionTree(asmRef.Assembly);
            importedAssemblies.Add(treeName, tree);
          }

          // --- At this point we have the assembly loaded.
          // --- Decide in which namespace hierarchy should this assembly be put.
          if (String.IsNullOrEmpty(asmRef.Alias))
          {
            // --- Put information into the global namespace hierarchy
            hierarchy = GlobalHierarchy;
          }
          else
          {
            // --- Put information into a named hierarchy
            if (!NamespaceHierarchies.TryGetValue(asmRef.Alias, out hierarchy))
            {
              hierarchy = new NamespaceHierarchy();
              NamespaceHierarchies.Add(asmRef.Alias, hierarchy);
            }
          }
          resolutionTree = tree;
        }
        else
        {
          var compilation = reference as ReferencedCompilation;
          if (compilation == null) continue;

          // --- This reference is a compilation unit. This unit has to be compiled before
          // --- so we can use its type resolution tree.
          hierarchy = GlobalHierarchy;
          resolutionTree = compilation.CompilationUnit.SourceResolutionTree;
          treeName = compilation.Name;
        }

        // --- Now we have the hierarchy where the resolution tree should be added.
        // --- Check for duplication before add.
        if (!hierarchy.ContainsTree(treeName))
        {
          hierarchy.AddTree(treeName, resolutionTree);
        }
      }

      // --- Collect namespace information from the source code:
      // --- Register all declared namespaces.
      foreach (Namespace ns in DeclaredNamespaces)
      {
        SourceResolutionTree.CreateNamespace(ns.Name);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets resolver information for all namespaces declared in the source code.
    /// </summary>
    /// <remarks>
    /// Namespaces use thier resolvers when looking for types and names declared in 
    /// their scope.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public void SetNamespaceResolvers()
    {
      foreach (SourceFile source in Files)
      {
        CurrentFile = source;
        foreach (NamespaceFragment fragment in source.NestedNamespaces)
        {
          fragment.SetNamespaceResolvers();
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets resolver information for all types declared in the source code.
    /// </summary>
    /// <remarks>
    /// Namespaces use thier resolvers when looking for types and names declared in 
    /// their scope.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public void SetTypeResolvers()
    {
      foreach (SourceFile source in Files)
      {
        CurrentFile = source;
        foreach (TypeDeclaration type in source.TypeDeclarations)
        {
          type.SetTypeResolvers();
        }
        foreach (NamespaceFragment fragment in source.NestedNamespaces)
        {
          fragment.SetTypeResolvers();
        }
      }
    }

    #endregion

    #region 'extern alias' resolution

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the extern alias clauses declared in the compilation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    private void ResolveExternAliasClauses()
    {
      foreach (SourceFile file in Files)
      {
        CurrentFile = file;
        ResolveExternAliasClauses(file, null);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all using directives in the specified source file within the provided
    /// namespace.
    /// </summary>
    /// <param name="file">Source file.</param>
    /// <param name="nsFragment">Namespace within the file.</param>
    /// <remarks>
    /// If no nsFragment is null, the 'using' clauses of the source file should be
    /// resolved.
    /// </remarks>
    // --------------------------------------------------------------------------------
    private void ResolveExternAliasClauses(SourceFile file, ITypeDeclarationScope nsFragment)
    {
      if (file == null) throw new ArgumentNullException("file");

      ITypeDeclarationScope scope = file.NamespaceScope(nsFragment);
      // --- Resolve 'exter alias' in this declaration space
      foreach (ExternalAlias alias in scope.ExternAliases)
      {
        // --- Check, if we have an 'extern alias' for the namespace hierarchy root
        NamespaceHierarchy hierarchy;
        if (!NamespaceHierarchies.TryGetValue(alias.Name, out hierarchy))
        {
          // --- No such namespace hierarchy have been found.
          Parser.Error0430(alias.Token, alias.Name);
        }
        else
        {
          alias.Hierarchy = hierarchy;
        }
      }

      // --- Resolve 'extern alias' clauses in child namespaces recursively.
      foreach (NamespaceFragment fragment in scope.NestedNamespaces)
      {
        ResolveExternAliasClauses(file, fragment);
      }
    }

    #endregion

    #region 'using' resolution

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all using directives in the compilation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    private void ResolveUsingDirectives()
    {
      foreach (SourceFile file in Files)
      {
        CurrentFile = file;
        ResolveUsingDirectives(file, null);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all using directives in the specified source file within the provided
    /// namespace.
    /// </summary>
    /// <param name="file">Source file.</param>
    /// <param name="nsFragment">Namespace within the file.</param>
    /// <remarks>
    /// If no nsFragment is null, the 'using' clauses of the source file should be
    /// resolved.
    /// </remarks>
    // --------------------------------------------------------------------------------
    private void ResolveUsingDirectives(SourceFile file, NamespaceFragment nsFragment)
    {
      if (file == null) throw new ArgumentNullException("file");

      ITypeDeclarationScope scope = file.NamespaceScope(nsFragment);
      // --- Resolve the using clauses in two steps. In first step we resolve usings,
      // --- in second step we commit the resolution. It is because using alias clauses
      // --- defined here should not influence the other using clauses. When resolving 
      // --- types we accept only committed using clauses.

      // --- Step 1: Resolve usings in this file or namespace
      foreach (UsingClause usingClause in scope.Usings)
      {
        ResolveUsingDirective(usingClause, file, nsFragment);
      }

      // --- Step 2: Commit resolutions
      foreach (UsingClause usingClause in scope.Usings)
      {
        usingClause.SignResolved();
      }

      // --- Resolve using clauses in child namespaces recursively.
      foreach (NamespaceFragment fragment in scope.NestedNamespaces)
      {
        ResolveUsingDirectives(file, fragment);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the specified using directive within the provided namespace.
    /// </summary>
    /// <param name="usingClause">Using directive</param>
    /// <param name="file">Source file.</param>
    /// <param name="nsFragment">Namespce to use for resolution</param>
    // --------------------------------------------------------------------------------
    public void ResolveUsingDirective(UsingClause usingClause, SourceFile file, 
      NamespaceFragment nsFragment)
    {
      if (usingClause == null) throw new ArgumentNullException("usingClause");
      if (file == null) throw new ArgumentNullException("file");
      var info =
        nsFragment == null
          ? _NamespaceOrTypeResolver.
              Resolve(usingClause.ReferencedName, ResolutionContext.SourceFile, file, null)
          : _NamespaceOrTypeResolver.
              Resolve(usingClause.ReferencedName, ResolutionContext.Namespace, nsFragment, null);

      // --- Exit, if resolution failed.
      if (!info.IsResolved) return;

      // --- Using directive cannot be resolved to a type
      if (!usingClause.HasAlias && info.Target == ResolutionTarget.Type)
      {
        Parser.Error0138(info.CurrentPart.Token, info.Type.FullName);
        return;
      }

      // --- At this point using directive is successfully resolved.
      usingClause.SetResolvers(info.Results);
    }

    #endregion

    #region Base type checking methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the base types of all types declared within the compilation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void ResolveAndCheckBaseTypes()
    {
      foreach (SourceFile file in Files)
      {
        CurrentFile = file;

        // --- Resolve types declared in the global namespace
        foreach (TypeDeclaration type in file.TypeDeclarations)
        {
          ResolveAndCheckBaseType(type, ResolutionContext.SourceFile, file);
        }

        // --- Resolve types within nested namespaces
        foreach(NamespaceFragment ns in file.NestedNamespaces)
          ResolveBaseTypeInNamespace(ns);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the base types of the specified type declaration within the 
    /// specified namespace and its nested namespaces.
    /// </summary>
    /// <param name="ns">
    /// Namespace in which the base types of type declarations should be resolved.
    /// </param>
    // --------------------------------------------------------------------------------
    private void ResolveBaseTypeInNamespace(ITypeDeclarationScope ns)
    {
      // --- Resolve base types of types declared in this namespace
      foreach (TypeDeclaration type in ns.TypeDeclarations)
      {
        ResolveAndCheckBaseType(type, ResolutionContext.Namespace, ns);
      }

      // --- Resolve types of nested namespaces
      foreach (NamespaceFragment nested in ns.NestedNamespaces)
        ResolveBaseTypeInNamespace(nested);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the base types of the specified type declaration partition.
    /// </summary>
    /// <param name="type">Type declaration to resolve its base types.</param>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="contextObject">Object representing the current context.</param>
    // --------------------------------------------------------------------------------
    private void ResolveAndCheckBaseType(TypeDeclaration type,
      ResolutionContext contextType,
      ITypeDeclarationScope contextObject)
    {
      // --- Resolve each base type reference
      foreach (TypeReference baseType in type.InterfaceList)
      {
        // --- Resolve the type only, if not resolved yet.
        if (!baseType.IsResolved)
        {
          NamespaceOrTypeResolverInfo info = _NamespaceOrTypeResolver.
            Resolve(baseType, contextType, contextObject, type);
          if (!info.IsResolved) continue;
        }
      }
      type.CheckBaseTypeSemantics();

      // --- Resolve base types in nested types
      foreach (TypeDeclaration nestedType in type.NestedTypes)
        ResolveAndCheckBaseType(nestedType, contextType, contextObject);
    }

    #endregion

    #region Circular dependency check methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if there are any circular dependencies within the compilation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void CheckCircularDependency()
    {
      foreach (TypeDeclaration type in DeclaredTypes)
      {
        CheckCircularDependency(type);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the base types of the specified type declaration and of all nested
    /// type declarations.
    /// </summary>
    /// <param name="type">Type declaration to resolve its base types.</param>
    // --------------------------------------------------------------------------------
    private void CheckCircularDependency(TypeDeclaration type)
    {
      // --- Only valid classes and interfaces should be checked for circular dependency
      if (!type.IsValid || (!type.IsClass && !type.IsInterface)) return;

      // --- We use a BFS algorithm to find circular dependency, start from 
      // --- the current type.
      var typeQueue = new Queue<TypeDeclaration>();
      typeQueue.Enqueue(type);
      
      // --- We dequeue a type declaration from the queue and enqueue its directly 
      // --- dependent types into the queue, if not put before. If anytime we put an 
      // --- instance into the queue that equals with 'type', it means we found a circular 
      // --- dependency for type. We do this check while the queue becomes empty.
      var typesAlreadyQueued = new Dictionary<string, int>();
      var circuitFound = false;
      while (typeQueue.Count > 0 && !circuitFound)
      {
        TypeDeclaration toCheck = typeQueue.Dequeue();

        // --- Queue the base types of this type
        foreach (ITypeAbstraction dependsOn in toCheck.DirectlyDependsOn)
        {
          // --- We check only typed declared within the source, because binary
          // --- types cannot cause circular dependency.
          var genType = dependsOn as GenericType;
          var typeDecl = dependsOn as TypeDeclaration;
          if (genType != null) typeDecl = genType.ConstructingType as TypeDeclaration;
          if (typeDecl == null || !typeDecl.IsValid) continue;

          if (typeDecl == type)
          {
            // --- We found a circular reference
            if (type.IsClass)
            {
              Parser.Error0146(type.Token, type.Name, toCheck.Name);
            }
            else
            {
              Parser.Error0529(type.Token, type.Name, toCheck.Name);
            }
            type.Invalidate();
            circuitFound = true;
            break;
          }

          // --- Put this type into the queue if we never examined it
          string typeName = dependsOn.FullName;
          if (!typesAlreadyQueued.ContainsKey(typeName))
          {
            // --- We should check this type declared in the source
            typeQueue.Enqueue(typeDecl);
            typesAlreadyQueued.Add(typeName, 0);
          }
        }
      }
    }

    #endregion

    #region Multiple type declaration checks

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if there are any types declared more that once
    /// </summary>
    // --------------------------------------------------------------------------------
    private void CheckMultipleAndPartialDeclaration()
    {
      foreach (TypeDeclaration type in DeclaredTypes)
      {
        // --- Check only, if there are no partitions
        if (type.Parts.Count == 0) continue;

        // --- Step 1: Check the number of partial and non-partial partitions
        int partialClassCount = 0;
        int partialStructCount = 0;
        int partialIntfCount = 0;
        foreach (TypeDeclaration partition in type.Parts)
        {
          if (partition is ClassDeclaration)
          {
            // --- Calculate class declaration counters
            if (partition.IsPartial) partialClassCount++;
          }
          else if (partition is StructDeclaration)
          {
            // --- Calculate struct declaration counters
            if (partition.IsPartial) partialStructCount++;
          }
          else if (partition is InterfaceDeclaration)
          {
            // --- Calculate interface declaration counters
            if (partition.IsPartial) partialIntfCount++;
          }
        }

        // --- Step 2: Go through all partitions and check for errors
        bool partialClassFound = false;
        bool partialStructFound = false;
        bool partialIntfFound = false;
        bool anyTypeFound = false;
        Visibility classAccess = Visibility.Default;
        Visibility structAccess = Visibility.Default;
        Visibility intfAccess = Visibility.Default;
        bool classAccessConflictFound = false;
        bool structAccessConflictFound = false;
        bool intfAccessConflictFound = false;
        bool classMixed = false;
        bool structMixed = false;
        bool intfMixed = false;

        // --- We go through each type declaration part and check for:
        // --- Check 1: Missing 'partial' modifier
        // --- Check 2: Defining the same partial type as different declarations 
        // ---   (e.g. mixed structs, classes or interfaces)
        // --- Check 3: Incompatible access modifiers
        foreach (TypeDeclaration partition in type.Parts)
        {
          // --- Checks for classes
          if (partition is ClassDeclaration)
          {
            // --- Check partial declaration
            if (partialClassCount > 0)
            {
              if (partition.IsPartial) partialClassFound = true;
              else RaiseMissingPartialModifierError(type, partition);
            }
            if (partialClassCount == 0 && anyTypeFound)
              RaiseMultipleTypeError(type, partition);
            if (partition.IsPartial && (partialStructFound || partialIntfFound))
              if (!classMixed)
              {
                RaiseMultiplePartialTypeError(type, partition);
                classMixed = true;
              }

            // --- Check for access compatibility
            if (!partition.HasDefaultVisibility)
            {
              if (classAccess != Visibility.Default && classAccess != partition.Visibility)
              {
                if (!classAccessConflictFound) RaiseIncompatibleAccessorError(type, partition);
                classAccessConflictFound = true;
              }
              else classAccess = partition.Visibility;
            }
          }
          // --- Checks for structs
          else if (partition is StructDeclaration)
          {
            // --- Check partial declaration
            if (partialStructCount > 0)
            {
              if (partition.IsPartial) partialStructFound = true;
              else RaiseMissingPartialModifierError(type, partition);
            }
            if (partialStructCount == 0 && anyTypeFound)
              RaiseMultipleTypeError(type, partition);
            if (partition.IsPartial && (partialClassFound || partialIntfFound))
              if (!structMixed)
              {
                RaiseMultiplePartialTypeError(type, partition);
                structMixed = true;
              }

            // --- Check for access compatibility
            if (!partition.HasDefaultVisibility)
            {
              if (structAccess != Visibility.Default && structAccess != partition.Visibility)
              {
                if (!structAccessConflictFound) RaiseIncompatibleAccessorError(type, partition);
                structAccessConflictFound = true;
              }
              else structAccess = partition.Visibility;
            }
          }
          // --- Checks for interfaces
          else if (partition is InterfaceDeclaration)
          {
            // --- Check partial declaration
            if (partialIntfCount > 0)
            {
              if (partition.IsPartial) partialIntfFound = true;
              else RaiseMissingPartialModifierError(type, partition);
            }
            if (partialIntfCount == 0 && anyTypeFound)
              RaiseMultipleTypeError(type, partition);
            if (partition.IsPartial && (partialClassFound || partialStructFound))
              if (!intfMixed)
              {
                RaiseMultiplePartialTypeError(type, partition);
                intfMixed = true;
              }

            // --- Check for access compatibility
            if (!partition.HasDefaultVisibility)
            {
              if (intfAccess != Visibility.Default && intfAccess != partition.Visibility)
              {
                if (!intfAccessConflictFound) RaiseIncompatibleAccessorError(type, partition);
                intfAccessConflictFound = true;
              }
              else intfAccess = partition.Visibility;
            }
          }
          // --- Checks for other types
          else
          {
            if (anyTypeFound)
              RaiseMultipleTypeError(type, partition);
          }
          anyTypeFound = true;
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Raises a compilation error CS0101 for the specified type declaration.
    /// </summary>
    /// <param name="type">Erronous type</param>
    /// <param name="partition">Erronous partition</param>
    // --------------------------------------------------------------------------------
    private void RaiseMultipleTypeError(LanguageElement type, TypeDeclaration partition)
    {
      Parser.Error0101(partition.Token,
        partition.EnclosingNamespace == null
        ? GlobalHierarchyName
        : partition.EnclosingNamespace.Name,
        partition.Name);
      type.Invalidate();
      partition.Invalidate();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Raises a compilation error CS0260 for the specified type declaration.
    /// </summary>
    /// <param name="type">Erronous type</param>
    /// <param name="partition">Erronous partition</param>
    // --------------------------------------------------------------------------------
    private void RaiseMissingPartialModifierError(LanguageElement type, 
      LanguageElement partition)
    {
      Parser.Error0260(partition.Token, partition.Name);
      type.Invalidate();
      partition.Invalidate();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Raises a compilation error CS0261 for the specified type declaration.
    /// </summary>
    /// <param name="type">Erronous type</param>
    /// <param name="partition">Erronous partition</param>
    // --------------------------------------------------------------------------------
    private void RaiseMultiplePartialTypeError(LanguageElement type,
      LanguageElement partition)
    {
      Parser.Error0261(partition.Token, partition.Name);
      type.Invalidate();
      partition.Invalidate();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Raises a compilation error CS0261 for the specified type declaration.
    /// </summary>
    /// <param name="type">Erronous type</param>
    /// <param name="partition">Erronous partition</param>
    // --------------------------------------------------------------------------------
    private void RaiseIncompatibleAccessorError(LanguageElement type,
      LanguageElement partition)
    {
      Parser.Error0262(partition.Token, partition.Name);
      type.Invalidate();
      partition.Invalidate();
    }

    #endregion

    #region Partial type declaration checks

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Consolidates partial type fragments.
    /// </summary>
    /// <remarks>
    /// This consolidates affects only the type declaration part (visibility,
    /// modifiers, base types and interfaces) but no members.
    /// </remarks>
    // --------------------------------------------------------------------------------
    private void ConsolidatePartialTypeDeclarations()
    {
      foreach (TypeDeclaration type in DeclaredTypes)
      {
        type.ConsolidateDeclarationParts();
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks partial type declarations
    /// </summary>
    // --------------------------------------------------------------------------------
    private void CheckPartialTypes()
    {
      //foreach (TypeDeclaration type in _DeclaredTypes)
      //{

        // --- Check 7: The attributes of a type declared in multiple partitions are determined 
        // --- by combining, in an unspecified order, the attributes of each of its parts. 
        // --- If the same attribute is placed on multiple parts, it is equivalent to 
        // --- specifying that attribute multiple times on the type.

        // --- Check 8: When a partial generic type declaration includes constraints, 
        // --- the constraints shall agree with all other parts that include constraints. 
        // --- Specifically, each part that includes constraints shall have constraints for 
        // --- the same set of type parameters, and for each type parameter, the sets of 
        // --- primary, secondary, and constructor constraints shall be equivalent. Two 
        // --- sets of constraints are equivalent if they contain the same members. If no 
        // --- part of a partial generic type specifies type parameter constraints, the 
        // --- type parameters are considered unconstrained.

        // --- Check 9: It is a compile-time error to declare the same member in more 
        // --- than one part of the type, unless that member is a type having the partial 
        // --- modifier.

        // --- Check 10: If one or more parts of a partial declaration of a nested type 
        // --- include the new modifier, no warning is issued if the nested type hides
        // --- an available inherited member.

      //}
    }

    #endregion

    #region Type declaration checks

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if type declaration matches with the declaration rules.
    /// </summary>
    // --------------------------------------------------------------------------------
    private void CheckTypeDeclarations()
    {
      foreach (TypeDeclaration type in DeclaredTypes)
      {
        type.CheckTypeDeclaration();
      }
    }

    #endregion

    #region Member declaration checks

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks the members of type declarations
    /// </summary>
    // --------------------------------------------------------------------------------
    private void CheckMemberDeclaration()
    {
      foreach (TypeDeclaration type in TypesInSource)
      {
        type.ClassifyMembers();
        type.CheckMembers();
        type.CheckTypeParameters();
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Builds up constracted (array and pointer) types.
    /// </summary>
    // --------------------------------------------------------------------------------
    private void BuildConstructedTypes()
    {
      foreach (TypeReference typeRef in _TypesToFix)
      {
        typeRef.BuildConstructedType();
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks the constraint declarations related to type declarations.
    /// </summary>
    // --------------------------------------------------------------------------------
    private void CheckTypeConstraintDeclarations()
    {
      foreach (TypeDeclaration type in TypesInSource)
      {
        type.CheckConstraintDeclarations();
      }
    }

    #endregion

    #region Documentation comment checks

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks the documentation comments of all typed declared.
    /// </summary>
    // --------------------------------------------------------------------------------
    private void CheckDocumentComments()
    {
      // --- Check, if document commenta are placed in a valid location
      foreach (CommentInfo comment in _CommentsToCheck)
      {
        // --- Related element must be an attribute declaration of an element that is
        // --- valid place for a documentation comment
        var attr = comment.RelatedElement as AttributeDeclaration;
        if (attr != null)
        {
          var docElement = attr.RelatedElement as ISupportsDocumentationComment;
          if (docElement != null) continue;
        }

        // --- This documentation comment is misplaced
        Parser.Warning1587(comment.Token);
      }

      // --- Check document comments related to types
      foreach (ISupportsDocumentationComment type in DeclaredTypes)
      {
        type.ProcessDocumentationComment();

        // --- Check document comments related to type members
        foreach (ISupportsDocumentationComment member in type.DocumentableMembers)
        {
          member.ProcessDocumentationComment();
        }
      }
    }

    #endregion

    #region Private methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Add a new error to the list of errors.
    /// </summary>
    /// <param name="messageCollection">Collection where the error should be added.</param>
    /// <param name="code">Error code.</param>
    /// <param name="errorPoint">Token describing the error position.</param>
    /// <param name="description">Detailed error description.</param>
    /// <param name="parameters">Error parameters.</param>
    // --------------------------------------------------------------------------------
    private CompilationMessageNode SignError(CompilationMessageCollection messageCollection, string code, ParserFiles.Token errorPoint, string description,
                                              params object[] parameters)
    {
      var message = new CompilationMessageNode(SourceFileNode, code, errorPoint, description) 
        { Parameters = parameters };
      message.SetErrorLineOffset(_ErrorLineOffset);
      message.RedirectSourceFile(_ErrorFile);
      messageCollection.Add(message);
      return message;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Iterator to go through each type declared in the source code.
    /// </summary>
    /// <remarks>
    /// The iterator goes through all the source files and type declaration parts
    /// within the source files.
    /// </remarks>
    // --------------------------------------------------------------------------------
    private IEnumerable<TypeDeclaration> TypesInSource
    {
      get
      {
        foreach (SourceFile file in Files)
        {
          CurrentFile = file;
          foreach (TypeDeclaration type in GetTypesInDeclarationScope(file)) 
            yield return type;
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Iterator to go through each type declared within the specified declaration 
    /// scope.
    /// </summary>
    /// <param name="scope">Declaration scope.</param>
    /// <returns>
    /// Iterator of types.
    /// </returns>
    // --------------------------------------------------------------------------------
    private static IEnumerable<TypeDeclaration> GetTypesInDeclarationScope(
      ITypeDeclarationScope scope)
    {
      // --- Retrieve types declared within the scope 
      foreach (TypeDeclaration type in scope.TypeDeclarations)
        foreach (TypeDeclaration toReturn in GetTypesInType(type))
          yield return toReturn;

      // --- Retrieve types declared within a namespace fragment of this scope
      foreach (NamespaceFragment nestedNs in scope.NestedNamespaces)
        foreach (TypeDeclaration toReturn in GetTypesInDeclarationScope(nestedNs))
          yield return toReturn;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Iterator to go through each nested type within a type declaration.
    /// </summary>
    /// <param name="type">Type declaration.</param>
    /// <returns>
    /// Iterator of types.
    /// </returns>
    // --------------------------------------------------------------------------------
    private static IEnumerable<TypeDeclaration> GetTypesInType(TypeDeclaration type)
    {
      // --- Return the type itself
      yield return type;

      // --- Iterate through this type's nested types recursively
      foreach (TypeDeclaration nestedType in type.NestedTypes)
        foreach (TypeDeclaration toReturn in GetTypesInType(nestedType))
          yield return toReturn;
    }

    #endregion
  }
}
