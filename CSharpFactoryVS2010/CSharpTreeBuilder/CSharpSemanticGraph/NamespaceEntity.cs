using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a namespace node in the semantic graph.
  /// </summary>
  // ================================================================================================
  public class NamespaceEntity : NamespaceOrTypeEntity, IHasChildTypes
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceEntity()
    {
      ChildNamespaces = new List<NamespaceEntity>();
      ChildTypes = new List<TypeEntity>();
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
    public List<NamespaceEntity> ChildNamespaces { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of child namespaces.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public List<TypeEntity> ChildTypes { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child namespace. 
    /// Also sets the child's parent property, and defines child's name in the declaration space.
    /// </summary>
    /// <param name="namespaceEntity">The child namespace entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddChildNamespace(NamespaceEntity namespaceEntity)
    {
      ChildNamespaces.Add(namespaceEntity);
      namespaceEntity.Parent = this;
      DeclarationSpace.Define(namespaceEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child type. 
    /// Also sets the child's parent property, and defines child's name in the declaration space.
    /// </summary>
    /// <param name="typeEntity">The child type entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddChildType(TypeEntity typeEntity)
    {
      ChildTypes.Add(typeEntity);
      typeEntity.Parent = this;
      DeclarationSpace.Define(typeEntity);
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
      visitor.Visit(this);

      foreach (var childNamespace in ChildNamespaces)
      {
        childNamespace.AcceptVisitor(visitor);
      }

      foreach (var childTypes in ChildTypes)
      {
        childTypes.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
