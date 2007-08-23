using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CSharpParser.ParserFiles;
using CSharpParser.ParserFiles.PPExpressions;
using CSharpParser.ProjectModel;
using CSharpParser.Semantics;
using Scanner=CSharpParser.ParserFiles.Scanner;
using Token=CSharpParser.ParserFiles.Token;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class is responsible for parsing all files in a C# project.
  /// </summary>
  // ==================================================================================
  public sealed class CompilationUnit: ICompilationErrorHandler
  {
    #region Constant values

    public const string ThisUnitName = "<this unit>";
    public const string GlobalHierarchyName = "<global namespace>";

    #endregion

    #region Private Fields

    private readonly SourceFileCollection _Files = new SourceFileCollection();
    private readonly NamespaceCollection _DeclaredNamespaces = new NamespaceCollection();
    private readonly TypeDeclarationCollection _DeclaredTypes = new TypeDeclarationCollection();
    private readonly string _WorkingFolder = string.Empty;
    private readonly ReferencedUnitCollection _ReferencedUnits = new ReferencedUnitCollection();
    private readonly List<string> _ConditionalSymbols = new List<string>();

    private CSharpSyntaxParser _Parser;
    private SourceFile _CurrentFile;
    private readonly ReferencedCompilation _ThisUnit;

    // --- Members related to error handling
    private readonly ErrorCollection _Errors = new ErrorCollection();
    private readonly ErrorCollection _Warnings = new ErrorCollection();
    private ICompilationErrorHandler _ErrorHandler;
    private TextWriter _ErrorStream;
    private string _ErrorMessageFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text
    private int _ErrorLineOffset;
    private string _ErrorFile;

    // --- Members related to semantics
    private readonly NamespaceHierarchy _GlobalHierarchy = new NamespaceHierarchy();
    private SourceResolutionTree _SourceResolutionTree;
    private readonly Dictionary<string, NamespaceHierarchy> _NamespaceHierarchies =
      new Dictionary<string, NamespaceHierarchy>();

#if DIAGNOSTICS

    private static SourceFile _CurrentLocation;

    public static SourceFile CurrentLocation
    {
      get { return _CurrentLocation; }
      set { _CurrentLocation = value; }
    }

#endif
    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a project parser using the specified working folder.
    /// </summary>
    /// <param name="workingFolder">Folder used as the working folder</param>
    // --------------------------------------------------------------------------------
    public CompilationUnit(string workingFolder): this(workingFolder, false)
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
    public CompilationUnit(string workingFolder, bool addCSharpFiles)
    {
      _ErrorHandler = this;
      _WorkingFolder = workingFolder;
      _CurrentFile = null;
      _ErrorLineOffset = -1;
      _ErrorFile = null;
      if (addCSharpFiles)
      {
        AddAllFilesFrom(_WorkingFolder);
      }
      _ThisUnit = new ReferencedCompilation(this, ThisUnitName);
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference for this compilation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ReferencedCompilation ThisUnit
    {
      get { return _ThisUnit; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parser.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CSharpSyntaxParser Parser
    {
      get { return _Parser; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of errors.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ErrorCollection Errors
    {
      get { return _Errors; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of warnings.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ErrorCollection Warnings
    {
      get { return _Warnings; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the object handling compilation errors.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ICompilationErrorHandler ErrorHandler
    {
      get { return _ErrorHandler; }
      set { _ErrorHandler = value; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of files included in the project.
    /// </summary>
    // --------------------------------------------------------------------------------
    public SourceFileCollection Files
    {
      get { return _Files; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the current file that is under compilation.
    /// </summary>
    // --------------------------------------------------------------------------------
    public SourceFile CurrentFile
    {
      get { return _CurrentFile; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the dictionary of all namespaces declared in this project.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceCollection DeclaredNamespaces
    {
      get { return _DeclaredNamespaces; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the dictionary of all types declared in this project.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeDeclarationCollection DeclaredTypes
    {
      get { return _DeclaredTypes; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of compilation references
    /// </summary>
    // --------------------------------------------------------------------------------
    public ReferencedUnitCollection ReferencedUnits
    {
      get { return _ReferencedUnits; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of conditional compilation symbols
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<string> ConditionalSymbols
    {
      get { return _ConditionalSymbols; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the global namespace hierarchy of this compilation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceHierarchy GlobalHierarchy
    {
      get { return _GlobalHierarchy; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the tree to resolve namespaces and types declared in the source code.
    /// </summary>
    // --------------------------------------------------------------------------------
    public SourceResolutionTree SourceResolutionTree
    {
      get { return _SourceResolutionTree; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the named namespace hierarchies of this compilation unit.
    /// </summary>
    /// <remarks>
    /// The global namespace hierarchy in not included among these hierarchies.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public Dictionary<string, NamespaceHierarchy> NamespaceHierarchies
    {
      get { return _NamespaceHierarchies; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a file to the project.
    /// </summary>
    /// <param name="fileName">File to add to the project.</param>
    // --------------------------------------------------------------------------------
    public void AddFile(string fileName)
    {
      string fullName = Path.Combine(_WorkingFolder, fileName);
        _Files.Add(new SourceFile(fullName, this));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds all .cs file in the specified folder and its subfolders to this project.
    /// </summary>
    /// <param name="path"></param>
    // --------------------------------------------------------------------------------
    public void AddAllFilesFrom(string path)
    {
      DirectoryInfo dir = new DirectoryInfo(path);
      // --- Add files in this folder
      foreach (FileInfo file in dir.GetFiles("*.cs"))
      {
        _Files.Add(new SourceFile(file.FullName, this));
      }
      // --- Add files in subfolders
      foreach (DirectoryInfo subDir in dir.GetDirectories())
      {
        AddAllFilesFrom(subDir.FullName);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new assembly reference to the C# project.
    /// </summary>
    /// <param name="name">Name of the assembly to add.</param>
    // --------------------------------------------------------------------------------
    public void AddAssemblyReference(string name)
    {
      _ReferencedUnits.Add(new ReferencedAssembly(name));  
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
      _ReferencedUnits.Add(new ReferencedAssembly(name, path));
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
      _ReferencedUnits.Add(new ReferencedAssembly(
        name, ReferencedAssembly.DotNetSystemFolder, alias));
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
      _ReferencedUnits.Add(new ReferencedAssembly(name, path, alias));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a list of conditional compilation symbol to the list of existing symbols.
    /// </summary>
    /// <param name="symbols">Array of symbols to add.</param>
    // --------------------------------------------------------------------------------
    public void AddConditionalCompilationSymbols(String[] symbols)
    {
      if (symbols != null)
      {
        for (int i = 0; i < symbols.Length; ++i)
        {
          symbols[i] = symbols[i].Trim();
          if (symbols[i].Length > 0 && !_ConditionalSymbols.Contains(symbols[i]))
          {
            _ConditionalSymbols.Add(symbols[i]);
          }
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a conditional directive to the list of existing conditionals
    /// </summary>
    /// <param name="symbol">Conditional directive</param>
    // --------------------------------------------------------------------------------
    public void AddConditionalDirective(String symbol)
    {
      if (!_ConditionalSymbols.Contains(symbol))
      {
        _ConditionalSymbols.Add(symbol);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Removes a conditional directive from the list of existing conditionals
    /// </summary>
    /// <param name="symbol">Conditional directive</param>
    // --------------------------------------------------------------------------------
    public void RemoveConditionalDirective(String symbol)
    {
      _ConditionalSymbols.Remove(symbol);
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
      return _ConditionalSymbols.Contains(symbol);
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
      MemoryStream memStream =
        new MemoryStream(new UTF8Encoding().GetBytes(preprocessorExpression));
      PPScanner scanner = new PPScanner(memStream);
      CSharpPPExprSyntaxParser parser = new CSharpPPExprSyntaxParser(scanner);
      parser.Parse();
      if (parser.ErrorFound)
      {
        return PPEvaluationStatus.Failed;
      }
      else return parser.Expression.Evaluate(_ConditionalSymbols)
        ? PPEvaluationStatus.True
        : PPEvaluationStatus.False;
    }

    #endregion

    #region ICompilationErrorHandler implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the output stream where errors should be written.
    /// </summary>
    // --------------------------------------------------------------------------------
    TextWriter ICompilationErrorHandler.ErrorStream
    {
      get { return _ErrorStream; }
      set { _ErrorStream = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the format of message to write to the output stream.
    /// </summary>
    // --------------------------------------------------------------------------------
    string ICompilationErrorHandler.ErrorMessageFormat
    {
      get { return _ErrorMessageFormat; }
      set { _ErrorMessageFormat = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Add a new error to the list of errors.
    /// </summary>
    /// <param name="code">Error code.</param>
    /// <param name="errorPoint">Token describing the error position.</param>
    /// <param name="description">Detailed error description.</param>
    // --------------------------------------------------------------------------------
    void ICompilationErrorHandler.Error(string code, Token errorPoint, string description)
    {
      SignError(_Errors, code, errorPoint, description, null);
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
    void ICompilationErrorHandler.Error(string code, Token errorPoint, string description,
      params object[] parameters)
    {
      SignError(_Errors, code, errorPoint, description, parameters);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Add a new warning to the list of warnings.
    /// </summary>
    /// <param name="code">Warning code.</param>
    /// <param name="warningPoint">Token describing the warning position.</param>
    /// <param name="description">Detailed warning description.</param>
    // --------------------------------------------------------------------------------
    void ICompilationErrorHandler.Warning(string code, Token warningPoint, string description)
    {
      SignError(_Warnings, code, warningPoint, description, null);
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
    void ICompilationErrorHandler.Warning(string code, Token warningPoint, string description,
                                          params object[] parameters)
    {
      SignError(_Warnings, code, warningPoint, description, parameters);
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

    #region Parser method

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
      // --- Init parsing
      (_Errors as IList<Error>).Clear();
      _GlobalHierarchy.Clear();
      _NamespaceHierarchies.Clear();

      // --- Syntax parsing
      foreach (SourceFile file in _Files)
      {
        Scanner scanner = new Scanner(File.OpenText(file.FullName).BaseStream);
        _CurrentFile = file;
#if DIAGNOSTICS
        _CurrentLocation = file;
#endif
        _Parser = new CSharpSyntaxParser(scanner, this, file);
        _Parser.Parse();
      }

      // --- Semantical parsing
      SemanticsParser semParser = new SemanticsParser(this);

      // --- Phase 1: Collect namespace hierarchy information from referenced assemblies
      // --- and namespaces declared by the source code. Set the resolvers for all 
      // --- namespaces and types.
      PrepareNamespaceHierarchies();
      SetNamespaceResolvers();
      SetTypeResolvers();
      
      // --- Phase 2: Create namspace hierarchies and resolve using clauses in the 
      // --- compilation unit
      ResolveExternAliasClauses();
      ResolveUsingDirectives();

      //// --- Phase 1: Import referenced assemblies into the namespace hierarchy
      //semParser.ImportReferences();
      //semParser.ImportExternalReferences();
      // --- Phase 3: Resolve all type references
      semParser.ResolveTypeReferences();

      // --- Return the number of errors found
      return _Errors.Count;
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
      _GlobalHierarchy.Clear();

      // --- Create a tree representing the types in the source code
      _SourceResolutionTree = new SourceResolutionTree(ThisUnitName);
      _GlobalHierarchy.AddTree(ThisUnitName, _SourceResolutionTree);

      // --- No named heirarchies found yet
      _NamespaceHierarchies.Clear();

      Dictionary<string, AssemblyResolutionTree> importedAssemblies = 
        new Dictionary<string, AssemblyResolutionTree>();

      // --- Go through all referenced assemblies, skip ohter kind of referencies.
      foreach (ReferencedUnit reference in ReferencedUnits)
      {
        ReferencedAssembly asmRef = reference as ReferencedAssembly;
        if (asmRef == null) continue;

        // --- Check, if this assembly has already been loaded into a resolution tree
        string treeName = asmRef.Assembly.FullName;
        AssemblyResolutionTree tree;
        if (!importedAssemblies.TryGetValue(treeName, out tree))
        {
          // --- Create and load the assembly
          tree = new AssemblyResolutionTree(asmRef.Assembly);
          importedAssemblies.Add(treeName, tree);
        }

        // --- At this point we have the assembly loaded.
        // --- Decide in which namespace hierarchy should this assembly be put.
        NamespaceHierarchy hierarchy;
        if (String.IsNullOrEmpty(asmRef.Alias))
        {
          // --- Put information into the global namespace hierarchy
          hierarchy = _GlobalHierarchy;
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

        // --- Now we have the hierarchy where the resolution tree should be added.
        // --- Check for duplication before add.
        if (!hierarchy.ContainsTree(treeName))
        {
          hierarchy.AddTree(treeName, tree);
        }
      }

      // --- Collect namespace information from the source code:
      // --- Register all declared namespaces.
      foreach (Namespace ns in DeclaredNamespaces)
      {
        _SourceResolutionTree.CreateNamespace(ns.Name);
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
        _CurrentFile = source;
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
        _CurrentFile = source;
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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Registers a namespace within a namespace hierarchy.
    /// </summary>
    /// <param name="hierarchy">Hierarchy</param>
    /// <param name="ns">Namespace information</param>
    /// <param name="resolverName">Name of resolver registering the namespace.</param>
    /// <returns>Node representing the namespace.</returns>
    // --------------------------------------------------------------------------------
    //private NamespaceResolutionNode RegisterNamespace(ResolutionNodeBase hierarchy, 
    //  string ns, string resolverName)
    //{
    //  NamespaceResolutionNode nsResolver;
    //  ResolutionNodeBase conflictingNode;
    //  if (!hierarchy.RegisterNamespace(ns, out nsResolver, out conflictingNode))
    //  {
    //    throw new InvalidOperationException(
    //      String.Format("Conflict when resolving namespace '{0}' in '{1}'",
    //                    ns, resolverName));
    //  }
    //  return nsResolver;
    //}

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
        _CurrentFile = file;
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
    private void ResolveExternAliasClauses(SourceFile file, NamespaceFragment nsFragment)
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
        _CurrentFile = file;
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
      if (nsFragment == null)
      {
        NamespaceOrTypeResolver.
          Resolve(usingClause.ReferencedName,ResolutionContext.SourceFile, file);
      }
      else
      {
        NamespaceOrTypeResolver.
          Resolve(usingClause.ReferencedName,ResolutionContext.Namespace, nsFragment);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the specified using directive within the provided namespace.
    /// </summary>
    /// <param name="usingClause">Using directive</param>
    /// <param name="nsFragment">Namespce to use for resolution</param>
    // --------------------------------------------------------------------------------
    //public void ResolveUsingDirective(UsingClause usingClause, NamespaceFragment nsFragment)
    //{
    //  ResolutionNodeList results;
    //  TypeReference nextNamePart;
    //  NamespaceHierarchy hierarchy;
    //  // --- Resolve using directive
    //  // --- Step 1: Select the appropriate namespace hierarchy
    //  if (!ObtainHierarchy(usingClause.ReferencedName, out hierarchy, out nextNamePart))
    //  {
    //    // --- Error, when obtaining the hierarchy, name resolution stops.
    //    return;
    //  }

    //  // --- Step 2: Now we have the namespace hierarchy, lets check that this 
    //  // --- namespace is valid
    //  TypeReference typeResolutionPart;
    //  results = ObtainName(nextNamePart, nsFragment, hierarchy, out typeResolutionPart);

    //  // --- Step 3: Check if resolution is type or namespace
    //  List<NamespaceResolutionNode> nsFound = results.GetNamespaceResolutions();
    //  List<TypeResolutionNode> typeFound = results.GetTypeResolutions();

    //  if (usingClause.HasAlias)
    //  {
    //    // --- We have a using alias directive. If name is fully resolved, it can 
    //    // --- be either a type or a namespace.

    //    // --- Name is fully resolved?
    //    if (typeResolutionPart == null)
    //    {
    //      if (nsFound.Count > 0 && typeFound.Count == 0)
    //      {
    //        // --- At this point we a have a using declaration fully resolved to a namespace. 
    //        ResolveToNamespace(hierarchy, usingClause, nextNamePart, nsFound);
    //        return;
    //      }
    //      else if (nsFound.Count == 0 && typeFound.Count == 1)
    //      {
    //        // --- At this point we have a using declaration fully resolved to a type.
    //        ResolveToType(usingClause, nextNamePart, typeFound[0].Resolver);
    //      }
    //    }
    //    else
    //    {
    //      // --- Name is partially resolved
    //    }
    //  }
    //  else
    //  {
    //    // --- We have a using directive. Only full name resolution is acceptable.
    //    // --- Check if partially resolved
    //    if (typeResolutionPart != null)
    //    {
    //      // --- Name is not fully resolved, we sign this fact.
    //      Parser.Error0246(typeResolutionPart.Token, typeResolutionPart.Name);
    //      return;
    //    }

    //    // --- Name must not be resolved to a type
    //    if (typeFound.Count > 0)
    //    {
    //      Parser.Error0138(nextNamePart.Token, nextNamePart.Name);
    //      return;
    //    }

    //    // --- At this point we a have a using declaration fully resolved to a namespace. 
    //    ResolveToNamespace(hierarchy, usingClause, nextNamePart, nsFound);
    //  }
    //}

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Selects the hierarchy for the specified type/namespace reference, and returns
    /// the reference where the name resolution should be carried on.
    /// </summary>
    /// <param name="type">Type/namespace to resolve</param>
    /// <param name="hierarchy">Resolving hierarchy</param>
    /// <param name="nextPart">Type reference for the next point of resolution.</param>
    /// <returns>
    /// True, if resolution is OK; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    //private bool ObtainHierarchy(TypeReference type,
    //  out NamespaceHierarchy hierarchy, out TypeReference nextPart)
    //{
    //  // --- If no namespace hierarchy specifier, use the global namespace hierarchy.
    //  hierarchy = _GlobalHierarchy;
    //  nextPart = type;
    //  if (!type.IsGlobal) return true;

    //  // --- At this point we have a global namespace specifier
    //  if (type.Name != "global")
    //  {
    //    // --- Check, if we have an 'extern alias' for the namespace hierarchy root
    //    if (!NamespaceHierarchies.TryGetValue(type.Name, out hierarchy))
    //    {
    //      // --- No such namespace hierarchy have been found.
    //      Parser.Error0432(type.Token, type.Name);
    //      return false;
    //    }
    //  }

    //  // --- Go on with the next part of the name
    //  nextPart = type.SubType;
    //  type.ResolveToNamespaceHierarchy();
    //  return true;
    //}

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Obtains the specified namespace within the given hierarchy.
    /// </summary>
    /// <param name="type">Type/namespace to resolve</param>
    /// <param name="fragment">Current namespace fragment.</param>
    /// <param name="hierarchy">Hierarchy to check for the namespace</param>
    /// <param name="nextPart">Type reference for the next point of resolution.</param>
    /// <returns>
    /// The list of resolver nodes that can resolve the specified namespace
    /// </returns>
    /// <remarks>
    /// If the 'nextPart' returned is the same as 'type' the no namespace part is
    /// resolved. In other cases 'nextPart' contains the point of the failed namespace
    /// resolution, while 'nsResolver' the resolver that can be used to continue the
    /// type resolution.
    /// </remarks>
    // --------------------------------------------------------------------------------
    //private ResolutionNodeList ObtainName(TypeReference type, 
    //  NamespaceFragment fragment, NamespaceHierarchy hierarchy, out TypeReference nextPart)
    //{
    //  // --- If we are not in the global namespace, we should search only that hierarchy
    //  // --- from its root.
    //  if (hierarchy != _GlobalHierarchy)
    //  {
    //    return hierarchy.FindName(type, out nextPart);
    //  }

    //  // --- We are in the global namespce hierarchy. If we are out of any namespace,
    //  // --- we can find the namespace among the source-declared namespaces or among
    //  // --- the namespaces of referenced assemblies. If we are within a namespace,
    //  // --- we go from the current namespace to the top namespace and try to find
    //  // --- the name.

    //  ResolutionNodeList result = new ResolutionNodeList();

    //  // --- Step 1: Check, if the source code has a definition for the namespace.
    //  if (fragment != null)
    //  {
    //    ResolutionNodeBase nsResolver = null;
    //    // --- We are within a namespace in the global declaration space
    //    // --- Check direct children of the current namespace fragment
    //    if (fragment.ResolverNode.Children.ContainsKey(type.Name))
    //    {
    //      nsResolver = fragment.ResolverNode;
    //    }
    //    else
    //    {
    //      // --- No child namespace with matching name, go up in the namespace 
    //      // --- hierarchy to check parent namespaces
    //      fragment = fragment.ParentNamespace;
    //      while (fragment != null)
    //      {
    //        if (fragment.ResolverNode.Children.ContainsKey(type.Name))
    //        {
    //          nsResolver = fragment.ResolverNode;
    //          break;
    //        }
    //        fragment = fragment.ParentNamespace;
    //      }
    //    }
    //    if (nsResolver != null)
    //    {
    //      TypeReference carryOnPart;
    //      ResolutionNodeBase sourceNode;
    //      if (nsResolver.FindName(type, out sourceNode, out carryOnPart) > 0)
    //      {
    //        if (carryOnPart == null)
    //        {
    //          // --- Name found in the source code, add it to the results
    //          result.Add(sourceNode);
    //        }
    //      }
    //    }
    //  }

    //  // --- Step 2: Check, if related assemblies have definition for the namespace
    //  TypeReference asmCarryOnPart;
    //  ResolutionNodeList asmResolutionNodes = hierarchy.FindName(type, out asmCarryOnPart);
    //  if (asmCarryOnPart == null)
    //  {
    //    // --- Found the full name in related assemblies. Merge the results with the source
    //    // --- code results.
    //    asmResolutionNodes.Merge(result);
    //    nextPart = null;
    //    return asmResolutionNodes;
    //  }

    //  if (!result.IsEmpty)
    //  {
    //    // --- We found the name only in the source code
    //    nextPart = null;
    //    return result;
    //  }

    //  // --- At this point no result is found.
    //  nextPart = asmCarryOnPart;
    //  return result;
    //}

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs that a using cluse has been resolved to a namespace.
    /// </summary>
    /// <param name="hierarchy">Namespace hierarchy</param>
    /// <param name="usingClause">Using clause resolved</param>
    /// <param name="resolvedPart">Part of the type resolved</param>
    /// <param name="nsFound">Resolvers</param>
    /// <remarks>
    /// Imports all types from the namespace into the specified namespace hierarchy.
    /// </remarks>
    // --------------------------------------------------------------------------------
    //private void ResolveToNamespace(NamespaceHierarchy hierarchy, UsingClause usingClause, 
    //  TypeReference resolvedPart, IEnumerable<NamespaceResolutionNode> nsFound)
    //{
    //  usingClause.ResolveToNamespace(nsFound);
    //  hierarchy.ImportNamespace(usingClause.ReferencedName.FullName);
    //  // --- Sign that the type references are resolved.
    //  while (resolvedPart != null)
    //  {
    //    resolvedPart.ResolveToNamespace();
    //    resolvedPart = resolvedPart.SubType;
    //  }
    //}

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs that a using alias clause has been resolved to a type.
    /// </summary>
    /// <param name="usingClause">Using clause resolved</param>
    /// <param name="resolvedPart">Part of the type resolved</param>
    /// <param name="type">Type the alias is resolved to.</param>
    // --------------------------------------------------------------------------------
    //private void ResolveToType(UsingClause usingClause, TypeReference resolvedPart,
    //  ITypeCharacteristics type)
    //{
    //  // --- Sign that the type references are resolved.
    //  usingClause.ResolveToType(type);
    //  while (resolvedPart != null)
    //  {
    //    resolvedPart.ResolveToNamespace();
    //    resolvedPart = resolvedPart.SubType;
    //  }
    //}

    #endregion

    #region Private methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Add a new error to the list of errors.
    /// </summary>
    /// <param name="errors">Collection where the error should be added.</param>
    /// <param name="code">Error code.</param>
    /// <param name="errorPoint">Token describing the error position.</param>
    /// <param name="description">Detailed error description.</param>
    /// <param name="parameters">Error parameters.</param>
    // --------------------------------------------------------------------------------
    private void SignError(ICollection<Error> errors, string code, Token errorPoint, string description,
                                          params object[] parameters)
    {
      Error error = new Error(
        code, 
        _ErrorLineOffset < 0 ? errorPoint.line : errorPoint.line + _ErrorLineOffset, 
        errorPoint.col, 
        errorPoint.pos,
        string.IsNullOrEmpty(_ErrorFile) ? _CurrentFile.Name : _ErrorFile, 
        description, 
        parameters);
      errors.Add(error);
      if (_ErrorStream != null)
      {
        _ErrorStream.WriteLine(_ErrorMessageFormat, errorPoint.line, errorPoint.col, description);
      }
    }

    #endregion

    #region Type resolution methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a name according to the provided type reference within the context of
    /// the specified namespace. Retrieves the tree node that resolves the provided 
    /// name and the part of the type reference that cannot be resolved.
    /// </summary>
    /// <param name="type">Namespace containing the type reference.</param>
    /// <param name="file">Sourcefile containing the type.</param>
    /// <param name="nsFragment">Namespace containing the type reference.</param>
    /// <param name="lastResolved">
    /// Last part of the type reference successfully resolved.
    /// </param>
    /// <returns>
    /// The resolution node representing the type or name resolved. If the resolution 
    /// cannot be done, returns null.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ResolutionNodeList ResolveName(TypeReference type, SourceFile file,
      NamespaceFragment nsFragment, out TypeReference lastResolved)
    {
      // --- Check, if the type reference is qualified 
      if (type.IsGlobal)
      {
        // --- Type is qualified. It can be 'global', a using alias reference or a
        // --- reference to an external alias.
        if (type.Name == "global")
        {
          // --- This is a "global::" qualified type. Resolve the other part of the name 
          // --- in the global namespace hierarchy
          return ResolveNameInHierarchy(_GlobalHierarchy, type.SubType, out lastResolved);
        }
        else
        {
          // --- Go from the inner namespace declaration toward the source file level
          // --- declarations and search for a matching using or extern alias.
          return ResolveNameWithAlias(type, file, nsFragment, out lastResolved);
        }
      }
      // --- We search within the global hierarchy using the specified context.
      return ResolveNameWithinContext(type, file, nsFragment, out lastResolved);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a name with an unqualifed name using the context provided by the
    /// given source file and namespace.
    /// </summary>
    /// <param name="type">Type to resolve.</param>
    /// <param name="file">Source file used.</param>
    /// <param name="fragment">Namespace fragment used.</param>
    /// <param name="lastResolved">Last part of the name resolved.</param>
    /// <returns>
    /// The node containing resolution information about the name.
    /// </returns>
    // --------------------------------------------------------------------------------
    private ResolutionNodeList ResolveNameWithinContext(TypeReference type, 
      SourceFile file, NamespaceFragment fragment, out TypeReference lastResolved)
    {
      if (file == null) throw new ArgumentNullException("file");
      lastResolved = null;

      if (type.SubType == null)
      {
        // --- Name is in form: I<A1, ... Ak>
        
      }
      else
      {
        // --- Name is in form: N.I<A1, ... Ak>
      }
      return null;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a name with a qualified alias member within the scope of a source file
    /// and a namespace.
    /// </summary>
    /// <param name="type">Type to resolve.</param>
    /// <param name="file">Source file used.</param>
    /// <param name="fragment">Namespace fragment used.</param>
    /// <param name="lastResolved">Last part of the name resolved.</param>
    /// <returns>
    /// The node containing resolution information about the name.
    /// </returns>
    // --------------------------------------------------------------------------------
    private ResolutionNodeList ResolveNameWithAlias(TypeReference type, SourceFile file, 
      NamespaceFragment fragment, out TypeReference lastResolved)
    {
      if (file == null) throw new ArgumentNullException("file");
      lastResolved = null;
      ResolutionNodeList aliasNodes = null;

      // --- Go through from the current namespace toward the enclosing namespace or
      // --- source file declarations.
      do
      {
        // --- Get the list of using clauses
        UsingClauseCollection usingAliases;
        if (fragment == null) usingAliases = file.Usings;
        else usingAliases = fragment.Usings;
        UsingClause usingAlias;
        if ((usingAlias = usingAliases[type.Name]) != null && usingAlias.IsResolved)
        {
          // --- Found a matching alias name.
          if (usingAlias.ReferencedName.IsResolvedToType)
          {
            // --- This aliases a type, however, a namespace is expected.
            Parser.Error0431(type.Token, type.Name);
            return null;
          }
          if (usingAlias.ReferencedName.IsResolvedToNamespace)
          {
            // --- We found a namespace and return it
            aliasNodes = new ResolutionNodeList(usingAlias.Resolvers);
            break;
          }
        }

        // --- We did not found any using alias, let's try the external aliases.
        // --- Get the list of extern aliases
        ExternalAliasCollection externAliases;
        if (fragment == null) externAliases = file.ExternAliases;
        else externAliases = fragment.ExternAliases;
        ExternalAlias externAlias;
        if ((externAlias = externAliases[type.Name]) != null)
        {
          // --- Check, if we have a named hierarchy for the extern alias
          if (externAlias.HasHierarchy)
          {
            aliasNodes = new ResolutionNodeList(externAlias.Hierarchy);
            break;
          }
        }

        // --- Go to the enclosing namespace or file declarations
        if (fragment != null) fragment = fragment.ParentNamespace;
      } while (fragment != null);

      // --- Is there any matching alias found?
      if (aliasNodes == null || aliasNodes.IsEmpty)
      {
        Parser.Error0431(type.Token, type.Name);
        return null;
      }

      // --- At this point we have a list of resolution tree nodes to lookup for the 
      // --- remaining parts of the name.
      return ResolveNameInForest(aliasNodes, type.SubType, out lastResolved);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a name represented by a type reference within the specified hierarchy.
    /// </summary>
    /// <param name="hierarchy">Hierarchy to resolve the name within.</param>
    /// <param name="type">Type to resolve.</param>
    /// <param name="lastResolved">Last part of the type resolved.</param>
    /// <returns>
    /// Node representing the successful name resolution, or null, if the name cannot  
    /// be resolved.
    /// </returns>
    /// <remarks>
    /// Because a hierarchy contains more than one trees, the name can be resolved in 
    /// more than one trees. If more successful resolutions found, name conflict is
    /// resolved before returning from this method.
    /// </remarks>
    // --------------------------------------------------------------------------------
    private ResolutionNodeList ResolveNameInHierarchy(NamespaceHierarchy hierarchy, 
      TypeReference type, out TypeReference lastResolved)
    {
      ResolutionNodeList forestNodes = new ResolutionNodeList(hierarchy);
      return ResolveNameInForest(forestNodes, type, out lastResolved);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a name represented by a type reference within the specified forest.
    /// </summary>
    /// <param name="forest">Hierarchy to resolve the name within.</param>
    /// <param name="type">Type to resolve.</param>
    /// <param name="lastResolved">Last part of the type resolved.</param>
    /// <returns>
    /// Node representing the successful name resolution, or null, if the name cannot  
    /// be resolved.
    /// </returns>
    /// <remarks>
    /// Because a forest contains more than one trees, the name can be resolved in 
    /// more than one trees. If more successful resolutions found, name conflict is
    /// resolved before returning from this method.
    /// </remarks>
    // --------------------------------------------------------------------------------
    private ResolutionNodeList ResolveNameInForest(ResolutionNodeList forest, 
      TypeReference type, out TypeReference lastResolved)
    {
      // --- Init name resolution
      ResolutionNodeList result = new ResolutionNodeList();
      lastResolved = type;
      int maxResolutionLength = 0;
      foreach (ResolutionNodeBase treeNode in forest)
      {
        TypeReference carryOnPart;

        // --- Resolve the name in the current tree
        ResolutionNodeBase resolvingNode = ResolveNameFromNode(treeNode, type, out carryOnPart);
        if (resolvingNode == null) continue;

        // --- How far we resolved the node?
        int depth = resolvingNode.Depth - treeNode.Depth;
        if (depth == 0) continue;

        // --- This time we could not resolve the name as deep as before, forget
        // --- about this result.
        if (depth < maxResolutionLength) continue;

        // --- This time we resolved the name deeper then before. Forget about
        // --- partial results found before and register only this new result.
        if (depth > maxResolutionLength)
        {
          result.Clear();
          maxResolutionLength = depth;
        }

        // --- Add the result found to the list resolution nodes
        result.Add(resolvingNode);
        lastResolved = carryOnPart;
      }
      return result;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a name represented by a type reference using the resolution tree node
    /// specified.
    /// </summary>
    /// <param name="node">Resolution tree node to use for resolution.</param>
    /// <param name="type">Type to reolve.</param>
    /// <param name="lastResolved">Last part of the type resolved.</param>
    /// <returns>
    /// Resolution node representing the part of the type resolved.
    /// </returns>
    // --------------------------------------------------------------------------------
    private ResolutionNodeBase ResolveNameFromNode(ResolutionNodeBase node, 
      TypeReference type, out TypeReference lastResolved)
    {
      ResolutionNodeBase currentNode = node;
      lastResolved = null;
      ResolutionNodeBase nextPart;
      do
      {
        // --- If the current node is a namespace and is not imported, we 
        // --- import its types.
        NamespaceResolutionNode nsNode = currentNode as NamespaceResolutionNode;
        if (nsNode != null) nsNode.ImportTypes();

        // --- Check, if the next part of the name can be resolved
        nextPart = currentNode.FindChild(type.Name);
        if (nextPart == null)
        {
          // --- No more parts can be resolved.
          return null;
        }

        // --- Name part successfully resolved. If the current node is a TypeNameResolutionNode,
        // --- we must look for the next TypeResolutionNode.
        TypeNameResolutionNode nameNode = nextPart as TypeNameResolutionNode;
        if (nameNode != null)
        {
          // --- We are dealing with a type.
          nextPart = nameNode.FindChild(type.TypeParameterCount.ToString());
          if (nextPart == null)
          {
            // --- We found the part name but not the one with correct number of
            // --- type parameters.
            return null;
          }
        }

        // --- A this point we succesfully resolved the current part of the name.
        lastResolved = type;
        type = type.SubType;
      } while (type != null);

      return nextPart;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the conflict when more type/namespace names have been found.
    /// </summary>
    /// <param name="type">Type to resolve.</param>
    /// <param name="resolvers">Resolvers of the name.</param>
    /// <returns>
    /// The only resolver after conflict handling, or null if the conflict cannot be
    /// resolved.
    /// </returns>
    // --------------------------------------------------------------------------------
    private ResolutionNodeBase ResolveNameConflicts(LanguageElement type, 
      ResolutionNodeList resolvers)
    {
      // --- No resolution found?
      if (resolvers.Count == 0) return null;

      // --- No conflict?
      if (resolvers.Count == 1) return resolvers[0];

      // --- In other cases we have a conflict
      // --- Separate the resolved names into four categories:
      // --- Category 1: Namespace declarations in referenced assemblies
      // --- Category 2: Namespace declarations in our source code
      // --- Category 3: Type declarations in referenced assemblies
      // --- Category 4: Type declaration in our source code
      List<NamespaceResolutionNode> nsInAsm = new List<NamespaceResolutionNode>();
      List<NamespaceResolutionNode> nsInCode = new List<NamespaceResolutionNode>();
      List<TypeResolutionNode> tyInAsm = new List<TypeResolutionNode>();
      List<TypeResolutionNode> tyInCode = new List<TypeResolutionNode>();
      foreach (ResolutionNodeBase item in resolvers)
      {
        NamespaceResolutionNode nsNode = item as NamespaceResolutionNode;
        if (nsNode != null)
        {
          if (nsNode.DefinedInSource) nsInCode.Add(nsNode);
          else nsInAsm.Add(nsNode);
        }
        TypeResolutionNode tyNode = item as TypeResolutionNode;
        if (tyNode != null)
        {
          if (tyNode.Resolver.DeclaringUnit is ReferencedCompilation) tyInCode.Add(tyNode);
          else tyInAsm.Add(tyNode);
        }
      }

      // --- Only namespace found?
      if (nsInCode.Count + nsInAsm.Count > 0 && tyInCode.Count + tyInAsm.Count == 0)
      {
        if (nsInCode.Count > 0) return nsInCode[0];
        return nsInAsm[0];
      }

      // --- Assembly type/source namespace conflict?
      if (tyInAsm.Count > 0 && nsInCode.Count > 0)
      {
        Parser.Warning0435(type.Token, nsInCode[0].Name,
          tyInAsm[0].Resolver.DeclaringUnit.Name);
        return nsInCode[0];
      }

      // --- Assembly type/source type conflict?
      if (tyInAsm.Count > 0 && tyInCode.Count > 0)
      {
        Parser.Warning0436(type.Token, tyInCode[0].Resolver.FullName,
          tyInAsm[0].Resolver.DeclaringUnit.Name);
        return tyInCode[0];
      }

      // --- Assembly namespace/source type conflict?
      if (nsInAsm.Count > 0 && tyInCode.Count > 0)
      {
        Parser.Warning0437(type.Token, tyInCode[0].Name,
          nsInAsm[0].Name);
        return tyInCode[0];
      }

      // --- Assembly namespace/source namsepace conflict?
      if (nsInAsm.Count > 0 && nsInCode.Count > 0)
      {
        return nsInCode[0];
      }

      // --- Same type in more than one assembly?
      if (tyInAsm.Count > 1)
      {
        Parser.Error0433(type.Token, type.Name,
          tyInAsm[0].Resolver.DeclaringUnit.Name,
          tyInAsm[1].Resolver.DeclaringUnit.Name);
        return null;
      }

      // --- If we are here, there is a conflict case we did not handle...
      throw new InvalidOperationException(
        String.Format("Name conflict not handled. tyInAsm={0}, tyInCode={1}, "+
        "nsInAsm={2}, nsInCode={3}",
        tyInAsm.Count, tyInCode.Count, nsInAsm.Count, nsInCode.Count));
    }

    #endregion
  }
}
