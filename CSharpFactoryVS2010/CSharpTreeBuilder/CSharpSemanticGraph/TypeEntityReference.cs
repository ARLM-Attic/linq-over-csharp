using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a reference to a type entity.
  /// </summary>
  // ================================================================================================
  public class TypeEntityReference : NamespaceOrTypeEntityReference
  {
    private TypeEntity _TypeEntity;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeEntityReference"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntityReference(TypeOrNamespaceNode typeOrNamespaceNode)
      : base(typeOrNamespaceNode)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the reference to resolved state, and sets the resolved entity.
    /// Also creates a constructed type entity if needed.
    /// </summary>
    /// <param name="resolvedEntity">The result of the resolution.</param>
    // ----------------------------------------------------------------------------------------------
    public override void SetResolved(NamespaceOrTypeEntity resolvedEntity)
    {
      base.SetResolved(resolvedEntity);

      if (resolvedEntity is TypeEntity)
      {
        _TypeEntity = resolvedEntity as TypeEntity;

        if (SyntaxNode.NullableToken != null)
        {
          _TypeEntity = new NullableTypeEntity(_TypeEntity);
        }

        bool isFirstStar = true;
        foreach (var pointerToken in SyntaxNode.PointerTokens)
        {
          // If it's pointer to unknown type (void*) then the first '*' should be swallowed, because that's part of 'void*'
          if (_TypeEntity is PointerToUnknownTypeEntity && isFirstStar)
          {
            isFirstStar = false;
          }
          else
          {
            _TypeEntity = new PointerToTypeEntity(_TypeEntity);
          }
        }

        foreach (var rankSpecifier in SyntaxNode.RankSpecifiers)
        {
          _TypeEntity = new ArrayTypeEntity(_TypeEntity, rankSpecifier.Rank);
        }

        // Note: Possible memory usage optimization: store the constructed types in SemanticGraph's
        // _NamespaceOrTypeEntities collection, avoid creating multiple instances of the same constructed type.
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the (possibly constructed) type entity resolved from the syntax node.
    /// </summary>
    /// <remarks>
    /// In case of a constructed type, ResolvedEntity property contains the underlying type.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity TypeEntity
    {
      get { return _TypeEntity; }
    }
  }
}
