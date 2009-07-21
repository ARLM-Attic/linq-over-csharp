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
    /// Gets the (possibly constructed) type entity resolved from the syntax node.
    /// </summary>
    /// <remarks>
    /// In case of a constructed type, ResolvedEntity property contains the underlying type.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity TypeEntity
    {
      get
      {
        TypeEntity typeEntity = null;

        if (ResolutionState == ResolutionState.Resolved && ResolvedEntity is TypeEntity)
        {
          typeEntity = ResolvedEntity as TypeEntity;

          if (SyntaxNode.NullableToken!=null)
          {
            typeEntity = new NullableTypeEntity(typeEntity);
          }

          foreach (var pointerToken in SyntaxNode.PointerTokens)
          {
            typeEntity = new PointerTypeEntity(typeEntity);
          }

          foreach (var rankSpecifier in SyntaxNode.RankSpecifiers)
          {
            typeEntity = new ArrayTypeEntity(typeEntity, rankSpecifier.Rank);
          }
        }

        return typeEntity;
      }
    }
  }
}
