using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements extension methods to SemanticEntity, used in semantic analysis.
  /// </summary>
  // ================================================================================================
  public static class SemanticEntityExtensions
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns the first TSemanticEntityType entity found by traversing the parents upwards. 
    /// If the argument is itself a TSemanticEntityType entity, then it is returned.
    /// </summary>
    /// <typeparam name="TSemanticEntityType">The type of entity to be found. A subclass of SemanticEntity.</typeparam>
    /// <param name="entity">A semantic entity.</param>
    /// <returns>A TSemanticEntityType, or null if none found.</returns>
    // ----------------------------------------------------------------------------------------------
    public static TSemanticEntityType GetEnclosing<TSemanticEntityType>(this SemanticEntity entity)
      where TSemanticEntityType : SemanticEntity
    {
      if (entity == null)
      {
        return null;
      }

      if (entity is TSemanticEntityType)
      {
        return entity as TSemanticEntityType;
      }

      return GetEnclosing<TSemanticEntityType>(entity.Parent);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a value indicating whether a given entity is in a generic method declaration.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    /// <returns>True if the entity is in a generic method declaration, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public static MethodEntity GetEnclosingGenericMethodDeclaration(this SemanticEntity entity)
    {
      // If reached the root or a namespace or type entity then it's not in a generic method declaration.
      if (entity == null || entity is NamespaceOrTypeEntity)
      {
        return null;
      }

      // If the current entity is a generic method then return it.
      if (entity is MethodEntity && (entity as MethodEntity).OwnTypeParameterCount > 0)
      {
        return entity as MethodEntity;
      }

      // If not yet reached to decision then continue at the parent node.
      return GetEnclosingGenericMethodDeclaration(entity.Parent);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a value indicating whether a given entity is in the body of a type declaration
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    /// <returns>True if the entity is in the body of a type declaration, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public static bool IsInTypeDeclarationBody(this SemanticEntity entity)
    {
      // If reached the root or a namespace or type entity then it's not in type declaration body.
      if (entity == null || entity is NamespaceOrTypeEntity)
      {
        return false;
      }

      // Members are in type declaration body.
      if (entity is NonTypeMemberEntity)
      {
        return true;
      }

      // If not yet reached to decision then continue at the parent node.
      return IsInTypeDeclarationBody(entity.Parent);
    }
  }
}
