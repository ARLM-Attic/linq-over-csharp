using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of resolvers where the source object is a NamespaceOrTypeNameNode.
  /// </summary>
  /// <typeparam name="TTargetType">The type of the target object. Any class.</typeparam>
  /// <remarks>
  /// This class implements the namespace and type name resolution logic described in the spec §3.8.
  /// The logic is implemented in static methods and parts of it can be called from other classes too
  /// (eg. from SimpleNameResolver).
  /// </remarks>
  // ================================================================================================
  public abstract class NamespaceOrTypeNameNodeResolver<TTargetType> : SyntaxNodeResolver<TTargetType, NamespaceOrTypeNameNode>
    where TTargetType: NamespaceOrTypeEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceOrTypeNameNodeResolver{TTargetType}"/> class.
    /// </summary>
    /// <param name="syntaxNode">The syntax node to resolve.</param>
    // ----------------------------------------------------------------------------------------------
    protected NamespaceOrTypeNameNodeResolver(NamespaceOrTypeNameNode syntaxNode)
      : base(syntaxNode)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceOrTypeNameNodeResolver{TTargetType}"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    protected NamespaceOrTypeNameNodeResolver(NamespaceOrTypeNameNodeResolver<TTargetType> template, TypeParameterMap typeParameterMap)
      :base(template, typeParameterMap)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Translates an exception thrown during namespace-or-type-name resolution to an error that
    /// can be reported to an ICompilationErrorHandler.
    /// </summary>
    /// <param name="e">A ResolverException object.</param>
    /// <param name="errorHandler">The object used for error reporting.</param>
    /// <remarks>If can't translate the exception then delegates it to its base.</remarks>
    // ----------------------------------------------------------------------------------------------
    protected override void TranslateExceptionToError(ResolverException e, ICompilationErrorHandler errorHandler)
    {
      var errorToken = SyntaxNode.StartToken;

      if (e is NamespaceOrTypeNameNotResolvedException)
      {
        errorHandler.Error("CS0246", errorToken,
                           "The type or namespace name '{0}' could not be found (are you missing a using directive or an assembly reference?)",
                           (e as NamespaceOrTypeNameNotResolvedException).NamespaceOrTypeName);
      }
      else if (e is AliasNameConflictException)
      {
        errorHandler.Error("CS0576", errorToken,
                           "Namespace '{0}' contains a definition conflicting with alias '{1}'",
                           (e as AliasNameConflictException).NamespaceName,
                           (e as AliasNameConflictException).AliasName);
      }
      else if (e is AmbigousReferenceException)
      {
        errorHandler.Error("CS0104", errorToken,
                           "'{0}' is an ambiguous reference between '{1}' and '{2}'",
                           (e as AmbigousReferenceException).NamespaceOrTypeName,
                           (e as AmbigousReferenceException).FullyQualifiedName1,
                           (e as AmbigousReferenceException).FullyQualifiedName2);
      }
      else if (e is QualifierRefersToType)
      {
        errorHandler.Error("CS0431", errorToken,
                           "Cannot use alias '{0}' with '::' since the alias references a type. Use '.' instead.",
                           (e as QualifierRefersToType).Qualifier);
      }
      else if (e is EntityIsInaccessibleException)
      {
        errorHandler.Error("CS0122", errorToken,
                           "'{0}' is inaccessible due to its protection level",
                           (e as EntityIsInaccessibleException).InaccessibleEntity.FullyQualifiedName);
      }
      else
      {
        base.TranslateExceptionToError(e, errorHandler);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the target object. Null if not resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override TTargetType Target
    {
      get
      {
        // If this resolver is a clone, and it's template is resolved to a TypeEntity then it has to be type-parameter-mapped.
        if (IsGenericClone)
        {
          var templateTarget = (OriginalGenericTemplate as NamespaceOrTypeNameNodeResolver<TTargetType>)._Target;

          if (templateTarget is TypeEntity)
          {
            return (templateTarget as TypeEntity).GetMappedType(TypeParameterMap) as TTargetType;
          }
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
          return (OriginalGenericTemplate as NamespaceOrTypeNameNodeResolver<TTargetType>)._ResolutionState;
        }
        
        return _ResolutionState;
      }
    }
  }
}
