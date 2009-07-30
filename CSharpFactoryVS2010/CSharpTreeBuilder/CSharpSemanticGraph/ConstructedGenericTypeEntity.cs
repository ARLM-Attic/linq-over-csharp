using System.Collections.Generic;
using System.Text;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a constructed generic type entity (that has concrete type arguments).
  /// </summary>
  // ================================================================================================
  public sealed class ConstructedGenericTypeEntity : ConstructedTypeEntity
  {
    /// <summary>Backing field for TypeArguments property to disallow direct adding or removing.</summary>
    private List<SemanticEntityReference<TypeEntity>> _TypeArguments;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstructedGenericTypeEntity"/> class.
    /// </summary>
    /// <param name="underlyingType">An open generic type.</param>
    /// <param name="parent">The parent entity.</param>
    // ----------------------------------------------------------------------------------------------
    public ConstructedGenericTypeEntity(GenericCapableTypeEntity underlyingType, NamespaceOrTypeEntity parent)
      : base(underlyingType)
    {
      Parent = parent;

      _BaseTypes = (List<SemanticEntityReference<TypeEntity>>)underlyingType.BaseTypes;
      _Members = (List<MemberEntity>)underlyingType.Members;

      _TypeArguments = new List<SemanticEntityReference<TypeEntity>>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a reference type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsReferenceType
    {
      get { return true; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of type arguments of this type. 
    /// Empty list for non-generic types and open generic types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<SemanticEntityReference<TypeEntity>> TypeArguments
    {
      get { return _TypeArguments; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a type argument the this type.
    /// </summary>
    /// <param name="typeArgument">A type argument, which is a TypeEntityReference.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddTypeArgument(SemanticEntityReference<TypeEntity> typeArgument)
    {
      _TypeArguments.Add(typeArgument);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the distinctive name of the entity, which is unique for all entities in a declaration space.
    /// The distinctive name for a constructed generic type is:
    /// UnderlyingType.DistinctiveName&lt;TypeArg1.TypeEntity.DistinctiveName,...&gt;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override string DistinctiveName
    {
      get
      {
        var distinctiveName = new StringBuilder(UnderlyingType.DistinctiveName);

        distinctiveName.Append('<');
        bool firstTypeArg = true;
        foreach (var typeArgument in TypeArguments)
        {
          if (firstTypeArg) { firstTypeArg = false; }
          else
          {
            distinctiveName.Append(',');
          }

          distinctiveName.Append(typeArgument.ResolutionState == ResolutionState.Resolved
                                   ? typeArgument.TargetEntity.FullyQualifiedName
                                   : "?");
        }
        distinctiveName.Append('>');

        return distinctiveName.ToString();
      }
    }

  }
}