using System;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a using alias directive in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class UsingAliasEntity : UsingEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="UsingAliasEntity"/> class.
    /// </summary>
    /// <param name="lexicalScope">The region of program text where the using entity has effect.</param>
    /// <param name="alias">The name of the alias.</param>
    /// <param name="namespaceorTypeName">A namespace-or-type-name AST node.</param>
    // ----------------------------------------------------------------------------------------------
    public UsingAliasEntity(SourceRegion lexicalScope, string alias, NamespaceOrTypeNameNode namespaceorTypeName)
      : base(lexicalScope)
    {
      if (alias == null)
      {
        throw new ArgumentNullException("alias");
      }
      if (namespaceorTypeName == null)
      {
        throw new ArgumentNullException("namespaceorTypeName");
      }

      Alias = alias;
      NamespaceOrTypeReference = new NamespaceOrTypeNameNodeBasedNamespaceOrTypeEntityReference(namespaceorTypeName);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the alias name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Alias { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference to the aliased namespace or type entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeNameNodeBasedNamespaceOrTypeEntityReference NamespaceOrTypeReference { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the aliased namespace entity. Null if not yet resolved, or the entity is not a namespace.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceEntity AliasedNamespace
    {
      get
      {
        return NamespaceOrTypeReference.TargetEntity as NamespaceEntity;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the aliased type entity. Null if not yet resolved, or the entity is not a type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity AliasedType
    {
      get
      {
        return NamespaceOrTypeReference.TargetEntity as TypeEntity;
      }
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
      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}
