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
    public const string GlobalHierarchyName = "global namespace";

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
    private NamespaceOrTypeResolver _NamespaceOrTypeResolver;

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
        _Parser = new CSharpSyntaxParser(scanner, this, file);
        _Parser.Parse();
      }

      // --- Semantical parsing
      _NamespaceOrTypeResolver = new NamespaceOrTypeResolver(_Parser);

      // --- Phase 1: Collect namespace hierarchy information from referenced assemblies
      // --- and namespaces declared by the source code. Set the resolvers for all 
      // --- namespaces and types.
      PrepareNamespaceHierarchies();
      SetNamespaceResolvers();
      SetTypeResolvers();
      
      // --- Phase 2: Create namespace hierarchies and resolve using clauses in the 
      // --- compilation unit
      ResolveExternAliasClauses();
      ResolveUsingDirectives();

      // --- Phase 3: Resolve base types and check base type validity
      ResolveBaseTypes();
      CheckCircularDependency();

      // --- Phase 4: Resolve type references belonging to members
      ResolveMemberTypeReferences();

      ResolveTypeReferences();

      // --- Return the number of errors found
      return _Errors.Count;
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
        _CurrentFile = source;
        source.ResolveTypeReferences(ResolutionContext.SourceFile, source);
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
      NamespaceOrTypeResolverInfo info;
      if (nsFragment == null)
      {
        info = _NamespaceOrTypeResolver.
          Resolve(usingClause.ReferencedName,ResolutionContext.SourceFile, file, null);
      }
      else
      {
        info = _NamespaceOrTypeResolver.
          Resolve(usingClause.ReferencedName,ResolutionContext.Namespace, nsFragment, null);
      }

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
    public void ResolveBaseTypes()
    {
      foreach (SourceFile file in _Files)
      {
        _CurrentFile = file;

        // --- Resolve types declared in the global namespace
        foreach (TypeDeclaration type in file.TypeDeclarations)
        {
          ResolveBaseTypes(type, ResolutionContext.SourceFile, file);
        }

        // --- Resolve types within nested namespaces
        foreach(NamespaceFragment ns in file.NestedNamespaces)
        {
          ResolveBaseTypes(ns);
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the base types of the specified type declaration and of all nested
    /// type declarations.
    /// </summary>
    /// <param name="type">Type declaration to resolve its base types.</param>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="contextObject">Object representing the current context.</param>
    // --------------------------------------------------------------------------------
    private void ResolveBaseTypes(TypeDeclaration type, ResolutionContext contextType, 
      ITypeDeclarationScope contextObject)
    {
      // --- Resolve each base type reference
      bool typeValid = true;
      bool firstOnList = true;
      foreach (TypeReference baseType in type.BaseTypes)
      {
        // --- Resolve the type only, if not resolved yet.
        if (!baseType.IsResolved)
        {
          NamespaceOrTypeResolverInfo info = _NamespaceOrTypeResolver.
            Resolve(baseType, contextType, contextObject, type);
          if (!info.IsResolved) continue;
        }
        
        // --- Check, if the base type is valid in its context.
        baseType.Validate(CheckBaseType(type, baseType, firstOnList));
        typeValid &= baseType.IsValid;
        firstOnList = false;
      }
      type.Validate(typeValid);

      // --- Resolve base types in nested types
      foreach (TypeDeclaration nestedType in type.NestedTypes)
      {
        ResolveBaseTypes(nestedType, contextType, contextObject);
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
    private void ResolveBaseTypes(NamespaceFragment ns)
    {
      // --- Resolve base types of types declared in this namespace
      foreach (TypeDeclaration type in ns.TypeDeclarations)
      {
        ResolveBaseTypes(type, ResolutionContext.Namespace, ns);
      }

      // --- Resolve types of nested namespaces
      foreach (NamespaceFragment nested in ns.NestedNamespaces)
      {
        ResolveBaseTypes(nested);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the base type is valid in the context of the specified type 
    /// declaration.
    /// </summary>
    /// <param name="type">Type declaration that inherits from baseType.</param>
    /// <param name="baseType">Base type to check</param>
    /// <param name="firstOnList">Indicates if the type is the first on the list.</param>
    /// <returns>
    /// True, if base type is valid; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    private bool CheckBaseType(TypeDeclaration type, TypeReference baseType, 
      bool firstOnList)
    {
      TypeReference rightMostPart = baseType.RightMostPart;
      bool isOk;

      // --- Check 1: The base type cannot be resolved to a type parameter
      if (rightMostPart.Target == ResolutionTarget.TypeParameter)
      {
        Parser.Error0689(baseType.Token, baseType.ResolvingTypeParameter.Name);
        return false;
      }

      // --- Check 2: The base type cannot be resolved to a namespace
      if (rightMostPart.Target == ResolutionTarget.Namespace)
      {
        Parser.Error0118(baseType.Token, baseType.FullName, "namespace", "type");
        return false;
      }

      // --- Check 3: Check if base types is at least as accessible as the type
      // --- Even if we found accessibility issue, we go on with checks.
      isOk = CheckBaseTypeAccessibility(type, rightMostPart);

      // --- Checks for base types of classes
      if (type.IsClass)
      {
        // --- Check 4a: Base types of classes cannot be special classes
        if (rightMostPart.ResolvingType.TypeObject.Equals(typeof(Array)) ||
         rightMostPart.ResolvingType.TypeObject.Equals(typeof (Delegate)) ||
         rightMostPart.ResolvingType.TypeObject.Equals(typeof (Enum)) ||
         rightMostPart.ResolvingType.TypeObject.Equals(typeof (ValueType))
        )
        {
          Parser.Error0644(baseType.Token, type.Name, rightMostPart.ResolvingType.FullName);
          return false;
        }

        // --- Check 4b: Classes cannot have multiple base types
        if (!firstOnList && !rightMostPart.ResolvingType.IsInterface)
        {
          Parser.Error1721(baseType.Token, type.Name, type.BaseTypes[0].FullName,
                           rightMostPart.ResolvingType.FullName);
          return false;
        }

        // --- Check 4c: Base types of classes cannot be sealed types
        if (rightMostPart.ResolvingType.IsSealed)
        {
          Parser.Error0509(baseType.Token, type.Name, rightMostPart.ResolvingType.FullName);
          return false;
        }
      }
      // --- Checks for base types of structs
      else if (type.IsStruct)
      {
        // --- Check 5a: Only interface types allowed on interface list.
        if (!rightMostPart.ResolvingType.IsInterface)
        {
          Parser.Error0527(baseType.Token, rightMostPart.ResolvingType.FullName);
          return false;
        }
      }
      // --- Checks for base types of enums
      else if (type.IsEnum)
      {
        // --- Check 6a: Only integral types are allowed in form of keywords.
        if (baseType.HasSubType ||
          (
            baseType.Name != "sbyte" &&
            baseType.Name != "byte" &&
            baseType.Name != "short" &&
            baseType.Name != "ushort" &&
            baseType.Name != "int" &&
            baseType.Name != "uint" &&
            baseType.Name != "long" &&
            baseType.Name != "ulong"
          ))
        {
          Parser.Error1008(baseType.Token);
          return false;
        }
      }
      // --- Check base types of interfaces
      else if (type.IsInterface)
      {
        // --- Check 7a: Only interface types allowed on interface list.
        if (!rightMostPart.ResolvingType.IsInterface)
        {
          Parser.Error0527(baseType.Token, rightMostPart.ResolvingType.FullName);
          return false;
        }
      }
      return isOk;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the base type is accessible in the context of the specified type 
    /// declaration.
    /// </summary>
    /// <param name="type">Type declaration that inherits from baseType.</param>
    /// <param name="baseType">Base type to check</param>
    /// <returns>
    /// True, if base type is accessible; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    private bool CheckBaseTypeAccessibility(TypeDeclaration type, TypeReference baseType)
    {
      // --- Base interfaces can have any accessibility
      if (baseType.IsInterface) return true;

      // --- Is the base type a referenced type?
      TypeDeclaration baseDecl = baseType.ResolvingType as TypeDeclaration;

      if (baseDecl != null)
      {
        // --- This type has been declared in the source code
        if (
             (type.IsPublic && baseDecl.IsNotPublic) ||
             (type.Visibility == Visibility.Protected &&
               (
                 baseDecl.Visibility == Visibility.Private ||
                 baseDecl.Visibility == Visibility.Protected
               )
             ) ||
             (type.Visibility == Visibility.Internal &&
               (
                 baseDecl.Visibility == Visibility.Private ||
                 baseDecl.Visibility == Visibility.Protected
               )
             ) ||
             (type.Visibility == Visibility.ProtectedInternal &&
               (
                 baseDecl.Visibility == Visibility.Private ||
                 baseDecl.Visibility == Visibility.Internal ||
                 baseDecl.Visibility == Visibility.Protected
               )
             ) 
           )
        {
          Parser.Error0060(type.Token, baseType.FullName, type.Name);
          return false;
        }
      }
      else
      {
        // --- This type is referenced from an assembly, can be used only if it is
        // --- visible from outside.
        return baseType.ResolvingType.IsVisible;
      }
      return true;
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
      foreach (SourceFile file in _Files)
      {
        _CurrentFile = file;

        // --- Resolve types declared in the global namespace
        foreach (TypeDeclaration type in file.TypeDeclarations)
        {
          CheckCircularDependency(type);
        }

        // --- Resolve types within nested namespaces
        foreach (NamespaceFragment ns in file.NestedNamespaces)
        {
          CheckCircularDependency(ns);
        }
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
      Queue<TypeDeclaration> typeQueue = new Queue<TypeDeclaration>();
      typeQueue.Enqueue(type);
      
      // --- We dequeue a type declaration from the queue and enqueue its directly 
      // --- dependent types into the queue, if not put before. If anytime we put an 
      // --- instance into the queue that equals with 'type', it means we found a circular 
      // --- dependency for type. We do this check while the queue becomes empty.
      Dictionary<string, int> typesAlreadyQueued = new Dictionary<string, int>();
      bool circuitFound = false;
      while (typeQueue.Count > 0 && !circuitFound)
      {
        TypeDeclaration toCheck = typeQueue.Dequeue();

        // --- Queue the base types of this type
        foreach (ITypeCharacteristics dependsOn in toCheck.DirectlyDependsOn)
        {
          // --- We check only typed declared within the source, because binary
          // --- types cannot cause circular dependency.
          TypeDeclaration typeDecl = dependsOn as TypeDeclaration;
          if (typeDecl == null || !typeDecl.IsValid) continue;

          if (typeDecl == type)
          {
            // --- We found a circular reference
            if (type.IsClass)
            {
              Parser.Error0146(type.Token, type.Name, toCheck.FullName);
            }
            else
            {
              Parser.Error0529(type.Token, type.Name, toCheck.FullName);
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

      // --- Check circular dependencies in nested types
      foreach (TypeDeclaration nestedType in type.NestedTypes)
      {
        CheckCircularDependency(nestedType);
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
    private void CheckCircularDependency(NamespaceFragment ns)
    {
      // --- Check circular dependencies in types declared within this namespace
      foreach (TypeDeclaration type in ns.TypeDeclarations)
      {
        CheckCircularDependency(type);
      }

      // --- Check circular dependencies within nested namespaces
      foreach (NamespaceFragment nested in ns.NestedNamespaces)
      {
        CheckCircularDependency(nested);
      }
    }

    #endregion

    #region Member type resolution methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the member type references of all types declared within the 
    /// compilation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void ResolveMemberTypeReferences()
    {
      foreach (SourceFile file in _Files)
      {
        _CurrentFile = file;

        // --- Resolve members of types declared in the global namespace
        foreach (TypeDeclaration type in file.TypeDeclarations)
        {
          ResolveMemberTypeReferences(type, ResolutionContext.SourceFile, file);
        }

        // --- Resolve members of types within nested namespaces
        foreach (NamespaceFragment ns in file.NestedNamespaces)
        {
          ResolveMemberTypeReferences(ns);
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the member type references of the specified type declaration and 
    /// of all nested type declarations.
    /// </summary>
    /// <param name="type">
    /// Type declaration to resolve its member type references.
    /// </param>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="contextObject">Object representing the current context.</param>
    // --------------------------------------------------------------------------------
    private void ResolveMemberTypeReferences(TypeDeclaration type, ResolutionContext contextType,
      ITypeDeclarationScope contextObject)
    {
      type.ResolveTypeReferencesInMembers(contextType, contextObject);
      // --- Resolve member type references in nested types
      foreach (TypeDeclaration nestedType in type.NestedTypes)
      {
        ResolveMemberTypeReferences(nestedType, contextType, contextObject);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the member type references of the specified type declaration within 
    /// the specified namespace and its nested namespaces.
    /// </summary>
    /// <param name="ns">
    /// Namespace in which the member type references should be resolved.
    /// </param>
    // --------------------------------------------------------------------------------
    private void ResolveMemberTypeReferences(NamespaceFragment ns)
    {
      // --- Resolve member type references of types declared in this namespace
      foreach (TypeDeclaration type in ns.TypeDeclarations)
      {
        ResolveMemberTypeReferences(type, ResolutionContext.Namespace, ns);
      }

      // --- Resolve membere type references of nested namespaces
      foreach (NamespaceFragment nested in ns.NestedNamespaces)
      {
        ResolveMemberTypeReferences(nested);
      }
    }

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
  }
}
