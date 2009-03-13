// ================================================================================================
// ExternalAlias.cs
//
// Reviewed: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;
using CSharpFactory.Semantics;
using System.Linq;

namespace CSharpFactory.ProjectModel
{
  // ================================================================================================
  /// <summary>
  /// This class represents an external alias declaration belonging to a file or to a
  /// namespace.
  /// </summary>
  // ================================================================================================
  public sealed class ExternalAlias : LanguageElement
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new external alias declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by the comment</param>
    // ----------------------------------------------------------------------------------------------
    public ExternalAlias(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the hierarchy belonging to this extern alias.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceHierarchy Hierarchy { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating this extern alias has a hierarchy.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool HasHierarchy
    {
      get { return Hierarchy != null; }
    }
  }

  // ================================================================================================
  /// <summary>
  /// This class represents a collection of external aliases within a project file.
  /// </summary>
  // ================================================================================================
  public class ExternalAliasCollection : ImmutableCollection<ExternalAlias>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>r
    /// Gets the external alias having the specified alias name.
    /// </summary>
    /// <param name="key">Alias name.</param>
    /// <returns>
    /// External alias, if found by the alias name; otherwise, null.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public ExternalAlias this[string key]
    {
      get { return this.FirstOrDefault(item => item.Name == key); }
    }
  }
}
