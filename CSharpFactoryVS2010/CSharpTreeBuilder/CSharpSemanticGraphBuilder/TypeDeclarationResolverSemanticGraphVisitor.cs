using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements a semantic graph visitor that finds and resolves type references
  /// till the type declaration level (eg. base types).
  /// </summary>
  // ================================================================================================
  public sealed class TypeDeclarationResolverSemanticGraphVisitor : TypeResolverSemanticGraphVisitorBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeDeclarationResolverSemanticGraphVisitor"/> class.
    /// </summary>
    /// <param name="errorHandler">Error handler object for error and warning reporting.</param>
    /// <param name="semanticGraph">The semantic graph that this visitor is working on.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeDeclarationResolverSemanticGraphVisitor(ICompilationErrorHandler errorHandler, SemanticGraph semanticGraph)
      :base(errorHandler,semanticGraph)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves type references in a BuiltInTypeEntity node.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(BuiltInTypeEntity entity)
    {
      ResolveTypeEntityReference(entity.AliasedTypeReference, null);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves type references in a TypeEntity node.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(TypeEntity entity)
    {
      // Resolve base type references
      foreach (var typeEntityReference in entity.BaseTypeReferences)
      {
        ResolveTypeEntityReference(typeEntityReference, entity);
      }

      // Assign implicit base class, if necessary
      if (!entity.IsInterfaceType && entity.ReflectedMetadata != typeof(object) && entity.BaseType == null)
      {
        System.Type baseType = null;

        if (entity is EnumEntity)
        {
          baseType = typeof (System.Enum);
        }
        else if (entity is StructEntity)
        {
          baseType = typeof (System.ValueType);
        }
        else if (entity is DelegateEntity)
        {
          baseType = typeof (System.MulticastDelegate);
        }
        else if (entity is ClassEntity)
        {
          baseType = typeof (System.Object);
        }

        if (baseType != null)
        {
          var reference = new ReflectedTypeBasedTypeEntityReference(baseType);
          ResolveTypeEntityReference(reference, entity);
          entity.AddBaseTypeReference(reference);
        }
      }
    }
  }
}
