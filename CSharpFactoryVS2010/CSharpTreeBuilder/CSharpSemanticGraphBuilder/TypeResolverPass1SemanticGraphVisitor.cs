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

      entity.RootNamespaceReference.Resolve(entity.Parent, _SemanticGraph, _ErrorHandler);
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

      entity.NamespaceReference.Resolve(entity.Parent, _SemanticGraph, _ErrorHandler);
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

      entity.NamespaceOrTypeReference.Resolve(entity.Parent, _SemanticGraph, _ErrorHandler);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves type references in a BuiltInTypeEntity node.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(BuiltInTypeEntity entity)
    {
      entity.AliasedTypeReference.Resolve(null, _SemanticGraph, _ErrorHandler);
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
        typeEntityReference.Resolve(entity, _SemanticGraph, _ErrorHandler);
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
          reference.Resolve(entity, _SemanticGraph, _ErrorHandler);
          entity.AddBaseTypeReference(reference);
        }
      }
    }
  }
}
