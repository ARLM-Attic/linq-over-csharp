using System;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a resolver that resolves a TypeNode to a TypeEntity.
  /// </summary>
  // ================================================================================================
  public sealed class TypeNodeToTypeEntityResolver : SyntaxNodeResolver<TypeEntity, TypeNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNodeToTypeEntityResolver"/> class.
    /// </summary>
    /// <param name="syntaxNode">A syntax node that be resolved.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeNodeToTypeEntityResolver(TypeNode syntaxNode)
      : base(syntaxNode)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNodeToTypeEntityResolver"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private TypeNodeToTypeEntityResolver(TypeNodeToTypeEntityResolver template, TypeParameterMap typeParameterMap)
      :base(template, typeParameterMap)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new resolver.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <returns>
    /// A new resolver constructed from this resolver using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    protected override Resolver<TypeEntity> ConstructNew(TypeParameterMap typeParameterMap)
    {
      return new TypeNodeToTypeEntityResolver(this, typeParameterMap);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Implements the resolution logic.
    /// </summary>
    /// <param name="context">A semantic entity that is the context of the resolution.</param>
    /// <param name="errorHandler">The object used for error reporting.</param>
    /// <returns>The resolved entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    protected override TypeEntity GetResolvedEntity(ISemanticEntity context, ICompilationErrorHandler errorHandler)
    {
      if (IsGenericClone)
      {
        throw new ApplicationException("GetResolvedEntity called on generic clone object.");
      }

      return NamespaceOrTypeNameResolutionAlgorithm.ResolveTypeNode(SyntaxNode, context, errorHandler);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the target object. Null if not resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override TypeEntity Target
    {
      get
      {
        // If this resolver is a clone, then the target entity has to be type-parameter-mapped.
        if (IsGenericClone)
        {
          return (OriginalGenericTemplate as TypeNodeToTypeEntityResolver)._Target.GetMappedType(TypeParameterMap);
        }

        return _Target;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the state of the resolution.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override ResolutionState ResolutionState
    {
      get
      {
        // If this resolver is a clone, then it's resolution state is the same as the template's resolution state.
        if (IsGenericClone)
        {
          return (OriginalGenericTemplate as TypeNodeToTypeEntityResolver)._ResolutionState;
        }

        return _ResolutionState;
      }
    }
  }
}
