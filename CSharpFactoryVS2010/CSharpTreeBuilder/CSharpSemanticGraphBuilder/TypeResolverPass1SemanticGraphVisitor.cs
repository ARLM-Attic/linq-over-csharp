using System.Linq;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraph;
using System;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements a semantic graph visitor that resolves the following references.
  /// - Resolves namespace and type names in using directives.
  /// - Resolves base types (because base entities are needed for subsequent resolution steps).
  /// - Also resolves built-in type aliases.
  /// - After resolving base types, implicit base types are set.
  /// </summary>
  // ================================================================================================
  public sealed class TypeResolverPass1SemanticGraphVisitor : TypeResolverSemanticGraphVisitorBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeResolverPass1SemanticGraphVisitor"/> class.
    /// </summary>
    /// <param name="errorHandler">Error handler object for error and warning reporting.</param>
    /// <param name="semanticGraph">The semantic graph that this visitor is working on.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeResolverPass1SemanticGraphVisitor(ICompilationErrorHandler errorHandler, SemanticGraph semanticGraph)
      :base(errorHandler,semanticGraph)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves extern alias reference.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(ExternAliasEntity entity)
    {
      if (entity.Parent == null)
      {
        throw new ApplicationException("UsingNamespaceEntity.Parent should not be null.");
      }

      entity.RootNamespaceReference.Resolve(entity, _ErrorHandler);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves namespace reference in a using namespace entity.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(UsingNamespaceEntity entity)
    {
      if (entity.Parent == null)
      {
        throw new ApplicationException("UsingNamespaceEntity.Parent should not be null.");
      }

      entity.NamespaceReference.Resolve(entity, _ErrorHandler);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves namespace-or-type reference in a using alias entity.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(UsingAliasEntity entity)
    {
      if (entity.Parent == null)
      {
        throw new ApplicationException("UsingAliasEntity.Parent should not be null.");
      }

      entity.NamespaceOrTypeReference.Resolve(entity, _ErrorHandler);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves type references in a TypeEntity node.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(TypeEntity entity)
    {
      ResolveBaseTypeReferences(entity);
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves type references in a DelegateEntity node.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(DelegateEntity entity)
    {
      // Resolve return type
      if (entity.ReturnTypeReference != null)
      {
        entity.ReturnTypeReference.Resolve(entity, _ErrorHandler);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves type references in a EnumEntity node.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(EnumEntity entity)
    {
      // Resolve underlying type
      if (entity.UnderlyingTypeReference != null)
      {
        entity.UnderlyingTypeReference.Resolve(entity, _ErrorHandler);

        // Check the underlying type if it is legal
        if (entity.UnderlyingType != null)
        {
          if (!CanBeEnumBase(entity.UnderlyingType))
          {
            var errorPoint = entity.UnderlyingTypeReference is TypeNodeToTypeEntityResolver
                               ? ((TypeNodeToTypeEntityResolver) entity.UnderlyingTypeReference).SyntaxNode.StartToken
                               : null;

            _ErrorHandler.Error("CS1008", errorPoint, "Type byte, sbyte, short, ushort, int, uint, long, or ulong expect");
          }
        }
      }
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves base type references in a type.
    /// </summary>
    /// <param name="entity">A type entity</param>
    // ----------------------------------------------------------------------------------------------
    private void ResolveBaseTypeReferences(TypeEntity entity)
    {
      // Resolve all base type references
      foreach (var typeEntityReference in entity.BaseTypeReferences)
      {
        typeEntityReference.Resolve(entity, _ErrorHandler);
      }

      // If it's a partial type then there may be duplicates in the base type list that has to be eliminated
      if (entity is ICanBePartial && ((ICanBePartial)entity).IsPartial)
      {
        entity.EliminateDuplicateBaseTypeReferences();

        // If more than one (different) base classes were specified, that's an error
        if (entity.BaseTypeCount > 1)
        {
          _ErrorHandler.Error("CS0263", null, "Partial declarations of '{0}' must not specify different base classes",
                              entity.FullyQualifiedName);
        }
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a value indicating whether a type can be enum base.
    /// </summary>
    /// <param name="typeEntity">A type entity.</param>
    /// <returns>True if the type can be enum base, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    private bool CanBeEnumBase(TypeEntity typeEntity)
    {
      if (typeEntity == _SemanticGraph.GetTypeEntityByBuiltInType(BuiltInType.Byte)
        || typeEntity == _SemanticGraph.GetTypeEntityByBuiltInType(BuiltInType.Sbyte)
        || typeEntity == _SemanticGraph.GetTypeEntityByBuiltInType(BuiltInType.Short)
        || typeEntity == _SemanticGraph.GetTypeEntityByBuiltInType(BuiltInType.Ushort)
        || typeEntity == _SemanticGraph.GetTypeEntityByBuiltInType(BuiltInType.Int)
        || typeEntity == _SemanticGraph.GetTypeEntityByBuiltInType(BuiltInType.Uint)
        || typeEntity == _SemanticGraph.GetTypeEntityByBuiltInType(BuiltInType.Long)
        || typeEntity == _SemanticGraph.GetTypeEntityByBuiltInType(BuiltInType.Ulong)
        )
      {
        return true;
      }

      return false;
    }

    #endregion
  }
}
