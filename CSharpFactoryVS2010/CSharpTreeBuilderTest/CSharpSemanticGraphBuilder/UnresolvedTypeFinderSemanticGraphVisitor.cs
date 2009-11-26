using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Implements a semantic graph visitor that checks every type reference in the semantic tree
  /// and throws an exception for any reference that is not resolved. Can be used in unit tests.
  /// </summary>
  // ================================================================================================
  public sealed class UnresolvedTypeFinderSemanticGraphVisitor : SemanticGraphVisitor
  {
    public override void Visit(ConstantMemberEntity entity)
    {
      CheckReference(entity.TypeReference);
    }

    public override void Visit(DelegateEntity entity)
    {
      CheckReference(entity.ReturnTypeReference);
    }

    public override void Visit(EnumEntity entity)
    {
      CheckReference(entity.UnderlyingTypeReference);
    }

    public override void Visit(FieldEntity entity)
    {
      CheckReference(entity.TypeReference);
    }

    public override void Visit(FunctionMemberWithAccessorsEntity entity)
    {
      CheckReference(entity.TypeReference);
      CheckReference(entity.InterfaceReference);
    }

    public override void Visit(MethodEntity entity)
    {
      CheckReference(entity.ReturnTypeReference);
      CheckReference(entity.InterfaceReference);
    }

    public override void Visit(NonFieldVariableEntity entity)
    {
      CheckReference(entity.TypeReference);
    }

    public override void Visit(TypedLiteralExpressionEntity entity)
    {
      CheckReference(entity.TypeReference);
    }

    public override void Visit(TypeEntity entity)
    {
      foreach (var baseTypeReference in entity.BaseTypeReferences)
      {
        CheckReference(baseTypeReference);
      }
    }

    public override void Visit(TypeParameterEntity entity)
    {
      foreach (var typeReferenceConstraint in entity.TypeReferenceConstraints)
      {
        CheckReference(typeReferenceConstraint);
      }
    }

    public override void Visit(UsingAliasEntity entity)
    {
      CheckReference(entity.NamespaceOrTypeReference);
    }

    public override void Visit(UsingNamespaceEntity entity)
    {
      CheckReference(entity.NamespaceReference);
    }

    #region Private methods

    private static void CheckReference<T>(Resolver<T> reference)
      where T : SemanticEntity
    {
      if (reference != null)
      {
        reference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
      }
    }

    #endregion
  }
}
