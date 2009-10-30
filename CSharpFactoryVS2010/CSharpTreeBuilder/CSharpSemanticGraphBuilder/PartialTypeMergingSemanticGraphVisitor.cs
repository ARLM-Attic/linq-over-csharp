using System.Linq;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements a semantic graph visitor that merges partial type entities into 1 entity.
  /// </summary>
  // ================================================================================================
  public sealed class PartialTypeMergingSemanticGraphVisitor : SemanticGraphVisitor
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Applies the visitor's logic to an entity.
    /// </summary>
    /// <param name="entity">An entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(ClassEntity entity)
    {
      FindAndMergePartialTypes(entity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Applies the visitor's logic to an entity.
    /// </summary>
    /// <param name="entity">An entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(StructEntity entity)
    {
      FindAndMergePartialTypes(entity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Applies the visitor's logic to an entity.
    /// </summary>
    /// <param name="entity">An entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(InterfaceEntity entity)
    {
      FindAndMergePartialTypes(entity);
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds the siblings of a partial type entity, and merges them.
    /// </summary>
    /// <typeparam name="TEntityType">The type of the entity.</typeparam>
    /// <param name="entity">A possibly partial type entity.</param>
    // ----------------------------------------------------------------------------------------------
    private static void FindAndMergePartialTypes<TEntityType>(TEntityType entity)
      where TEntityType : GenericCapableTypeEntity, ICanBePartial
    {
      if (entity.IsPartial && entity.Parent is IHasChildTypes)
      {
        var childTypeHavingParent = entity.Parent as IHasChildTypes;
        var potentialPartialSiblings =
          childTypeHavingParent.GetChildTypes<TEntityType>(entity.Name, entity.OwnTypeParameterCount).ToArray();

        // Iterate through the sibling class entities and merge partials.
        foreach (var sibling in potentialPartialSiblings)
        {
          if (entity != sibling && sibling.IsPartial)
          {
            MergeTypes(entity, sibling);
            childTypeHavingParent.RemoveChildType(sibling);
          }
        }
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Merges a type entity into another type entity.
    /// </summary>
    /// <param name="targetTypeEntity">The type entity that is the target of the merger.</param>
    /// <param name="sourceTypeEntity">The type entity that is the source of the merger.</param>
    // ----------------------------------------------------------------------------------------------
    private static void MergeTypes(TypeEntity targetTypeEntity, TypeEntity sourceTypeEntity)
    {
      // Merge base type references
      foreach (var baseTypeReference in sourceTypeEntity.BaseTypeReferences)
      {
        targetTypeEntity.AddBaseTypeReference(baseTypeReference);
      }

      // Merge members
      foreach (var member in sourceTypeEntity.Members)
      {
        targetTypeEntity.AddMember(member);
      }

      // Relink syntax nodes from sourceTypeEntity to targetTypeEntity
      foreach (var syntaxNode in sourceTypeEntity.SyntaxNodes)
      {
        targetTypeEntity.AddSyntaxNode(syntaxNode);
        syntaxNode.SemanticEntities.Remove(sourceTypeEntity);
        syntaxNode.SemanticEntities.Add(targetTypeEntity);
      }

      // TODO:
      // Base class: if present then must be the same --> it will be checked after base type resolution.
      // Base interfaces: union --> duplicates will be eliminated after base type resolution.
      // Type parameters: must be the same (number, name and order)
      // Accesibility modifiers: must be the same
      // If one or more partial declarations of a class include an abstract modifier, the class is considered abstract 
      // If one or more partial declarations of a class include a sealed modifier, the class is considered sealed 
      // Type constraints: must be the same or completely missing
      // When the unsafe modifier is used on a partial type declaration, only that particular part is considered an unsafe context 
      // Attributes: combined
      // Members: union
    }

    #endregion
  }
}
