﻿using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements a semantic graph visitor that resolves type references in type bodies 
  /// (members, member bodies).
  /// </summary>
  /// <remarks>
  /// Type references in expression are not resolved by this visitor. 
  /// Those are resolved by <see cref="ExpressionEvaluatorSemanticGraphVisitor"/> at a later stage.
  /// </remarks>
  // ================================================================================================
  public sealed class TypeResolverPass2SemanticGraphVisitor : TypeResolverSemanticGraphVisitorBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeResolverPass2SemanticGraphVisitor"/> class.
    /// </summary>
    /// <param name="errorHandler">Error handler object for error and warning reporting.</param>
    /// <param name="semanticGraph">The semantic graph that this visitor is working on.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeResolverPass2SemanticGraphVisitor(ICompilationErrorHandler errorHandler, SemanticGraph semanticGraph)
      : base(errorHandler, semanticGraph)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves type references in a TypeParameterEntity node.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(TypeParameterEntity entity)
    {
      foreach (var typeReference in entity.TypeReferenceConstraints)
      {
        typeReference.Resolve(entity, _ErrorHandler);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves type references in a FieldEntity node.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(FieldEntity entity)
    {
      // Resolve the type of the field
      if (entity.TypeReference != null)
      {
        entity.TypeReference.Resolve(entity, _ErrorHandler);

        if (!(entity.Parent is PropertyEntity) &&
          entity.Type == _SemanticGraph.GetTypeEntityByBuiltInType(BuiltInType.Void))
        {
          var errorPoint = entity.TypeReference is TypeNodeToTypeEntityResolver
                             ? ((TypeNodeToTypeEntityResolver) entity.TypeReference).SyntaxNode.StartToken
                             : null;

          _ErrorHandler.Error("CS0670", errorPoint, "Field cannot have void type");
        }
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves type references in a ConstantMemberEntity node.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(ConstantMemberEntity entity)
    {
      // Resolve the type of the field
      if (entity.TypeReference != null)
      {
        entity.TypeReference.Resolve(entity, _ErrorHandler);
      }

      if (entity.Type == _SemanticGraph.GetTypeEntityByBuiltInType(BuiltInType.Void))
      {
        var errorPoint = entity.TypeReference is TypeNodeToTypeEntityResolver
                           ? ((TypeNodeToTypeEntityResolver)entity.TypeReference).SyntaxNode.StartToken
                           : null;

        _ErrorHandler.Error("CS1547", errorPoint, "Keyword 'void' cannot be used in this context");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves type references in a PropertyEntity node.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(PropertyEntity entity)
    {
      // Resolve the type of the property
      if (entity.TypeReference != null)
      {
        entity.TypeReference.Resolve(entity, _ErrorHandler);

        if (entity.Type == _SemanticGraph.GetTypeEntityByBuiltInType(BuiltInType.Void))
        {
          var errorPoint = entity.TypeReference is TypeNodeToTypeEntityResolver
                             ? ((TypeNodeToTypeEntityResolver)entity.TypeReference).SyntaxNode.StartToken
                             : null;

          _ErrorHandler.Error("CS0547", errorPoint, "'{0}': property or indexer cannot have void type", entity.Name);

        }
      }

      // Resolve the interface reference for explicitly implemented members
      if (entity.InterfaceReference != null)
      {
        entity.InterfaceReference.Resolve(entity.Parent, _ErrorHandler);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves type references in a MethodEntity node.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(MethodEntity entity)
    {
      // Resolve the return type
      if (entity.ReturnTypeReference != null)
      {
        // Note that the resolution context is the method, because it can be the method's type parameter too
        entity.ReturnTypeReference.Resolve(entity, _ErrorHandler);
      }

      // Resolve the interface reference for explicitly implemented members
      if (entity.InterfaceReference != null)
      {
        // Note that the resolution context is not the method, but its parent
        entity.InterfaceReference.Resolve(entity.Parent, _ErrorHandler);
      }

      // Resolve parameter types
      foreach (var parameter in entity.Parameters)
      {
        if (parameter.TypeReference != null)
        {
          // Note that the resolution context is the method, because it can be the method's type parameter too
          parameter.TypeReference.Resolve(entity, _ErrorHandler);
        }
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves type references in a local variable node.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(LocalVariableEntity entity)
    {
      if (entity.TypeReference != null)
      {
        entity.TypeReference.Resolve(entity, _ErrorHandler);
      }
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves type references in a local constant node.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(LocalConstantEntity entity)
    {
      if (entity.TypeReference != null)
      {
        entity.TypeReference.Resolve(entity, _ErrorHandler);
      }
    }
  }
}
