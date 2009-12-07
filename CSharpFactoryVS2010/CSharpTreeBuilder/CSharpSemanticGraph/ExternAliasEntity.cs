using System;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an extern alias directive in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class ExternAliasEntity : SemanticEntity, IHasLexicalScope
  {
    #region State

    /// <summary>Gets the region of program text that is affected by this object.</summary>
    public SourceRegion LexicalScope { get; private set; }

    /// <summary>Gets the alias name.</summary>
    public string Alias { get; private set; }

    /// <summary>Gets the reference to a namespace entity.</summary>
    public ExternAliasNodeToRootNamespaceEntityResolver RootNamespaceReference { get; private set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternAliasEntity"/> class.
    /// </summary>
    /// <param name="lexicalScope">The region of program text where the extern alias has effect.</param>
    /// <param name="externAliasNode">The name of the alias.</param>
    // ----------------------------------------------------------------------------------------------
    public ExternAliasEntity(SourceRegion lexicalScope, ExternAliasNode externAliasNode)
    {
      if (lexicalScope == null)
      {
        throw new ArgumentNullException("lexicalScope");
      }
      if (externAliasNode == null)
      {
        throw new ArgumentNullException("externAliasNode");
      }

      LexicalScope = lexicalScope;
      RootNamespaceReference = new ExternAliasNodeToRootNamespaceEntityResolver(externAliasNode);
      Alias = externAliasNode.Identifier;
    }

    // This type of entity cannot be affected by type arguments, so no generic clone support here.

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the aliased namespace entity. Null if not yet resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceEntity AliasedRootNamespace
    {
      get
      {
        return RootNamespaceReference.Target;
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
