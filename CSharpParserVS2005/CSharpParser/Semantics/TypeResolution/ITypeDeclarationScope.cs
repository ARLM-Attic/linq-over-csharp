using System.Collections.Generic;
using CSharpParser.ProjectModel;

namespace CSharpParser.Semantics
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
    /// Gets the name of the scope
    /// </summary>
    // --------------------------------------------------------------------------------
    string ScopeName { get; }

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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the source file enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    SourceFile EnclosingSourceFile { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    NamespaceFragment EnclosingNamespace { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Iterator to iterate from the current scope toward the containing source file.
    /// </summary>
    // --------------------------------------------------------------------------------
    IEnumerable<ITypeDeclarationScope> ScopesToOuter { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Looks for the specified simple namespace or type name directly in this
    /// type declaration scope.
    /// </summary>
    /// <param name="type">Type reference representing the name to find.</param>
    /// <returns>
    /// Resolution nodes representing the name found, or null if the name cannot 
    /// be found.
    /// </returns>
    // --------------------------------------------------------------------------------
    ResolutionNodeList FindSimpleName(TypeReference type);

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if this declaration scope contains an extern-alias or a using-alias
    /// declaration with the specified name.
    /// </summary>
    /// <param name="name">Alias name to check.</param>
    /// <param name="foundInScope">Scope where the alias has been found.</param>
    /// <returns>
    /// True, if the this scope contains the alias; otherwise, false. If alias found, 
    /// foundInScope contains that scope declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool ContainsAlias(string name, out ITypeDeclarationScope foundInScope);
  }
}
