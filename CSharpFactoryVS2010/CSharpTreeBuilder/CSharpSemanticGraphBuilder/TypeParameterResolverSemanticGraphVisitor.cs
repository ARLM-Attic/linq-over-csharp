using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements a semantic graph visitor that resolves type parameters 
  /// to actual type arguments in a constructed type's semantic subtree.
  /// </summary>
  // ================================================================================================
  public sealed class TypeParameterResolverSemanticGraphVisitor : SemanticGraphVisitor
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeParameterResolverSemanticGraphVisitor"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterResolverSemanticGraphVisitor()
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Replaces type parameters with type arguments in the specified entity.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(ClassEntity entity)
    {
      //if (entity.IsConstructed)
      //{
      //  //InheritTypeParameterMap(entity);

      //  entity.BaseClass = MapType(entity.BaseClass, entity.TypeParameterMap) as ClassEntity;
      //}

      //foreach (var constructedType in entity.ConstructedEntities)
      //{
      //  constructedType.AcceptVisitor(this);
      //}
      
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Replaces type parameters with type arguments in the specified entity.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(FieldEntity entity)
    {
      //if (entity.IsConstructed)
      //{
      //  //InheritTypeParameterMap(entity);

      //  entity.Type = MapType(entity.Type, entity.TypeParameterMap);
      //}

      return true;
    }

    #region Private methods

    //private void InheritTypeParameterMap(SemanticEntity entity)
    //{
    //  if (entity.TypeParameterMap.IsEmpty && entity.Parent != null)
    //  {
    //    entity.TypeParameterMap = entity.Parent.TypeParameterMap;
    //  }
    //}

    private TypeEntity MapType(TypeEntity type, TypeParameterMap typeParameterMap)
    {
      if (type != null)
      {
        if (type is TypeParameterEntity)
        {
          type = typeParameterMap[type as TypeParameterEntity];
        }
        else if (type.IsConstructed)
        {
          type = type.GetConstructedEntity(type.TypeParameterMap.Combine(typeParameterMap), false) as TypeEntity;
        }
      }

      return type;
    }

    #endregion
  }
}
