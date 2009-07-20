using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a namespace node in the semantic graph.
  /// </summary>
  // ================================================================================================
  public class NamespaceEntity : NamespaceOrTypeEntity
  {
    /// <summary>Backing field for ChildNamespaces property.</summary>
    protected List<NamespaceEntity> _ChildNamespaces;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceEntity()
    {
      _ChildNamespaces = new List<NamespaceEntity>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this namespace is explicitly defined in code 
    /// (not inferred from assembly metadata).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsExplicit
    {
      get
      {
        return SyntaxNodes.Count > 0;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of child namespaces.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<NamespaceEntity> ChildNamespaces
    {
      get { return _ChildNamespaces; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a list of child namespaces, for unit testing only.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public List<NamespaceEntity> ChildNamespacesForUnitTests
    {
      get { return _ChildNamespaces; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child namespace. 
    /// Also sets the child's parent property, and defines child's name in the declaration space.
    /// </summary>
    /// <param name="namespaceEntity">The child namespace entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddChildNamespace(NamespaceEntity namespaceEntity)
    {
      _ChildNamespaces.Add(namespaceEntity);
      namespaceEntity.Parent = this;
      DeclarationSpace.DefineName(namespaceEntity.Name,namespaceEntity);
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      foreach (var childNamespace in _ChildNamespaces)
      {
        childNamespace.AcceptVisitor(visitor);
      }

      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}
