using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

    public const string ThisUnitName = "<<ThisUnit>>";

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
    private readonly NamespaceHierarchy _GlobalHierarchy = new NamespaceHierarchy("global");
    private readonly Dictionary<string, NamespaceHierarchy> _NamespaceHierarchies =
      new Dictionary<string, NamespaceHierarchy>();

    private readonly Dictionary<string, List<string>> _ImportedAssemblies = 
      new Dictionary<string, List<string>>();

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
      // --- and namespaces declared by the source code
      PrepareNamespaceHierarchies();
      semParser.SetResolvers();
      
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
      _NamespaceHierarchies.Clear();
      _ImportedAssemblies.Clear();

      // --- Go through all referenced assemblies, skip ohter kind of referencies.
      foreach (ReferencedUnit reference in ReferencedUnits)
      {
        ReferencedAssembly asmRef = reference as ReferencedAssembly;
        if (asmRef == null) continue;
        if (String.IsNullOrEmpty(asmRef.Alias))
        {
          // --- Put namespace information into the global namespace hierarchy
          CollectNamespaces(GlobalHierarchy, asmRef.Assembly);
        }
        else
        {
          // --- Collect only the name of the hierarchy
          NamespaceHierarchy hierarchy;
          if (!NamespaceHierarchies.TryGetValue(asmRef.Alias, out hierarchy))
          {
            hierarchy = new NamespaceHierarchy(asmRef.Alias);
            NamespaceHierarchies.Add(asmRef.Alias, hierarchy);
            CollectNamespaces(hierarchy, asmRef.Assembly);
          }
        }
      }

      // --- Collect namespace information from the source code:
      // --- Register all declared namespaced
      foreach (Namespace ns in DeclaredNamespaces)
      {
        NamespaceResolutionNode nsResolver;
        ResolutionNodeBase conflictingNode;
        if (!GlobalHierarchy.RegisterNamespace(ns.Name, out nsResolver,
          out conflictingNode))
        {
          // --- This situation should not occure!
          throw new InvalidOperationException(
            "Type and namespace conflict within the compilation unit");
        }
        nsResolver.AddResolver(ThisUnit.Name, typeof(int).Assembly);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Collects namespace information from the specified assembliy.
    /// </summary>
    /// <param name="nsHierarchy">Destination hierarchy of namespaces</param>
    /// <param name="asm">Assembly to collect the namespaces from</param>
    // --------------------------------------------------------------------------------
    private void CollectNamespaces(NamespaceHierarchy nsHierarchy, Assembly asm)
    {
      // --- If the hierarchy already imported the assembly, there is no need to
      // --- import the namespaces again
      if (nsHierarchy.ImportedAssemblies.Contains(asm.FullName)) return;

      // --- If any namespace hierarchy imported the assembly, we take their
      // --- namespace list without going through the assembly's types
      List<string> namespaceList;
      if (_ImportedAssemblies.TryGetValue(asm.FullName, out namespaceList))
      {
        foreach (string nameSpace in namespaceList)
        {
          // --- This namespace is not collected yet from this assembly instance
          NamespaceResolutionNode nsResolver =
            RegisterNamespace(nsHierarchy, nameSpace, asm.FullName);
          nsResolver.AddResolver(asm.FullName, asm);
        }
      }
      else
      {
        // --- Store namespaces already imported to avoid repeated processing
        Dictionary<string, int> nsCache = new Dictionary<string, int>();
        List<string> nsList = new List<string>();

        // --- This is the first time we import namespaces from this assembly. We go
        // --- through its types to obtain namespace names.
        foreach (Type type in asm.GetTypes())
        {
          if (!String.IsNullOrEmpty(type.Namespace) && !nsCache.ContainsKey(type.Namespace))
          {
            // --- This namespace is not collected yet from this assembly instance
            NamespaceResolutionNode nsResolver =
              RegisterNamespace(nsHierarchy, type.Namespace, asm.FullName);
            nsResolver.AddResolver(asm.FullName, asm);
            nsCache.Add(type.Namespace, 0);
            nsList.Add(type.Namespace);
          }
        }

        // --- Sign that namespaces in the assembly have been imported
        _ImportedAssemblies.Add(asm.FullName, nsList);
      }
      // --- Sign that namespaces in the assembly have been imported into this hierarchy
      nsHierarchy.ImportedAssemblies.Add(asm.FullName);
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
    private NamespaceResolutionNode RegisterNamespace(ResolutionNodeBase hierarchy, 
      string ns, string resolverName)
    {
      NamespaceResolutionNode nsResolver;
      ResolutionNodeBase conflictingNode;
      if (!hierarchy.RegisterNamespace(ns, out nsResolver, out conflictingNode))
      {
        throw new InvalidOperationException(
          String.Format("Conflict when resolving namespace '{0}' in '{1}'",
                        ns, resolverName));
      }
      return nsResolver;
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
    private void ResolveExternAliasClauses(SourceFile file, NamespaceFragment nsFragment)
    {
      if (file == null) throw new ArgumentNullException("file");

      // --- Obtain the collection of 'extern alias' clauses
      ExternalAliasCollection aliases = nsFragment == null
        ? file.ExternAliases
        : nsFragment.ExternAliases;

      // --- Resolve 'exteralias' in this declaration space
      foreach (ExternalAlias alias in aliases)
      {
        // --- Check, if we have an 'extern alias' for the namespace hierarchy root
        if (!NamespaceHierarchies.ContainsKey(alias.Name))
        {
          // --- No such namespace hierarchy have been found.
          Parser.Error0430(alias.Token, alias.Name);
        }
      }

      // --- Obtain the collection of the child namespace fragments 
      NamespaceFragmentCollection childNamespaces = nsFragment == null
        ? file.Namespaces
        : nsFragment.NestedNamespaces;

      // --- Resolve 'extern alias' clauses in child namespaces recursively.
      foreach (NamespaceFragment fragment in childNamespaces)
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

      // --- Obtain the collection of using clauses
      UsingClauseCollection usings = nsFragment == null 
        ? file.Usings 
        : nsFragment.Usings;

      // --- Resolve usings in this file
      foreach (UsingClause usingClause in usings)
      {
        // --- Resolve the clause
        if (usingClause.HasAlias)
        {
          // --- Resolve using aliases
        }
        else
        {
          // --- Resolve using directive
          // --- Step 1: Select the appropriate namespace hierarchy
          NamespaceHierarchy hierarchy;
          TypeReference nextNamePart;
          if (!ObtainHierarchy(usingClause.ReferencedName, out hierarchy, out nextNamePart))
            continue;

          // --- Step 2: Now we have the namespace hierarchy, lets check that this 
          // --- namespace is valid
          NamespaceResolutionNode nsResolver;
          TypeReference typeResolutionPart;
          if (ObtainNamespace(nextNamePart, nsFragment, hierarchy, out nsResolver,
                          out typeResolutionPart))
          {
            // --- Check if partially resolved
            if (typeResolutionPart != null)
            {
              Parser.Error0246(typeResolutionPart.Token, typeResolutionPart.Name);
            }
          }
          else
          {
            // --- Namespace is resolved to a type
            Parser.Error0138(nextNamePart.Token, nextNamePart.Name);
          }
        }
      }

      // --- Obtain the collection of the cild namespace fragments 
      NamespaceFragmentCollection childNamespaces = nsFragment == null
        ? file.Namespaces
        : nsFragment.NestedNamespaces;

      // --- Resolve using clauses in child namespaces recursively.
      foreach (NamespaceFragment fragment in childNamespaces)
      {
        ResolveUsingDirectives(file, fragment);
      }
    }

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
    private bool ObtainHierarchy(TypeReference type,
      out NamespaceHierarchy hierarchy, out TypeReference nextPart)
    {
      // --- If no namespace hierarchy specifier, use the global namespace hierarchy.
      hierarchy = _GlobalHierarchy;
      nextPart = type;
      if (!type.IsGlobal) return true;

      // --- At this point we have a global namespace specifier
      if (type.Name != "global")
      {
        // --- Check, if we have an 'extern alias' for the namespace hierarchy root
        if (!NamespaceHierarchies.TryGetValue(type.Name, out hierarchy))
        {
          // --- No such namespace hierarchy have been found.
          Parser.Error0432(type.Token, type.Name);
          return false;
        }
      }

      // --- Go on with the next part of the name
      nextPart = type.SubType;
      return true;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Obtains the specified namespace within the given hierarchy.
    /// </summary>
    /// <param name="type">Type/namespace to resolve</param>
    /// <param name="fragment">Current namespace fragment.</param>
    /// <param name="hierarchy">Hierarchy to check for the namespace</param>
    /// <param name="nsResolver">Resolver node for the found namespace</param>
    /// <param name="nextPart">Type reference for the next point of resolution.</param>
    /// <returns>
    /// True, if the namespace is partially resolved. False, if the namespace is resolved
    /// to a type.
    /// </returns>
    /// <remarks>
    /// If the 'nextPart' returned is the same as 'type' the no namespace part is
    /// resolved. In other cases 'nextPart' contains the point of the failed namespace
    /// resolution, while 'nsResolver' the resolver that can be used to continue the
    /// type resolution.
    /// </remarks>
    // --------------------------------------------------------------------------------
    private bool ObtainNamespace(TypeReference type, NamespaceFragment fragment, 
      NamespaceHierarchy hierarchy, out NamespaceResolutionNode nsResolver, 
      out TypeReference nextPart)
    {
      nsResolver = null;
      nextPart = type;
      ResolutionNodeBase node;
      // --- Find the first resolver.
      if (hierarchy == _GlobalHierarchy && fragment != null)
      {
        // --- We are within a namespace in the global declaration space
        // --- Check direct children of the current namespace fragment
        if (!fragment.ResolverNode.Children.TryGetValue(type.Name, out node))
        {
          fragment = fragment.ParentNamespace;
          while (fragment != null)
          {
            if (fragment.ResolverNode.Children.TryGetValue(type.Name, out node)) break;
            fragment = fragment.ParentNamespace;
          }
        }
      }

      // --- If we have not found the namespacetag yet, look in from the root
      if (!hierarchy.Children.TryGetValue(type.Name, out node)) return true;

      // --- Resolved to a namespace?
      nsResolver = node as NamespaceResolutionNode;
      if (nsResolver == null) return false;

      // --- At this point we have found the first part of the namespace
      nextPart = type.SubType;

      // --- Go and search for next part while there is any part left, or
      // --- further resolution fails.
      while (nextPart != null)
      {
        if (!nsResolver.Children.TryGetValue(nextPart.Name, out node)) return true;
        NamespaceResolutionNode temp = node as NamespaceResolutionNode;
        if (temp == null) return true;
        nsResolver = temp;
        nextPart = nextPart.SubType;
      }
      return true;
    }

    #endregion

    #region Methods for importing types within a namespace

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Imports the specified namespace into the given namespace hierarchy.
    /// </summary>
    /// <param name="hierarchy">Destination hierarchy of types.</param>
    /// <param name="nameSpace">Namespace to import</param>
    /// <returns>
    /// True, if the namespace exists in the hierarchy and types have been imported;
    /// otherwise, false.
    /// </returns>
    /// <remarks>
    /// Before calling this method, the namespace hierarchies should be prepared and
    /// filled up with the namespace information.
    /// </remarks>
    // --------------------------------------------------------------------------------
    private bool ImportTypesInNamespace(NamespaceHierarchy hierarchy, string nameSpace)
    {
      return true;
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
