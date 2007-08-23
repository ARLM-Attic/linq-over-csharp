namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This interface defines the behaviour of a type declaration scope.
  /// </summary>
  /// <remarks>
  /// A type declaration scope is a source file and a namespace.
  /// </remarks>
  // ==================================================================================
  public interface ITypeDeclarationScope
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the external aliases of the declaration unit
    /// </summary>
    // --------------------------------------------------------------------------------
    ExternalAliasCollection ExternAliases { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of using clauses in this declaration unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    UsingClauseCollection Usings { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of namespace declarations in this project file
    /// </summary>
    // --------------------------------------------------------------------------------
    NamespaceFragmentCollection NestedNamespaces { get; }
  }
}
