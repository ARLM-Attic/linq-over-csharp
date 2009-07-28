using System;
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
    private List<TypeEntityReference> _TypeArguments;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstructedGenericTypeEntity"/> class.
    /// </summary>
    /// <param name="embeddedType">An open generic type.</param>
    /// <param name="parent">The parent entity.</param>
    // ----------------------------------------------------------------------------------------------
    public ConstructedGenericTypeEntity(GenericCapableTypeEntity embeddedType, NamespaceOrTypeEntity parent)
      : base(embeddedType)
    {
      Parent = parent;

      BaseTypes = embeddedType.BaseTypes;
      _Members = (List<MemberEntity>)embeddedType.Members;

      _TypeArguments = new List<TypeEntityReference>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of type arguments of this type. 
    /// Empty list for non-generic types and open generic types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TypeEntityReference> TypeArguments
    {
      get { return _TypeArguments; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a type argument the this type.
    /// </summary>
    /// <param name="typeArgument">A type argument, which is a TypeEntityReference.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddTypeArgument(TypeEntityReference typeArgument)
    {
      _TypeArguments.Add(typeArgument);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the distinctive name of the entity, which is unique for all entities in a declaration space.
    /// The distinctive name for a constructed generic type is:
    /// EmbeddedType.DistinctiveName&lt;TypeArg1.TypeEntity.DistinctiveName,...&gt;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override string DistinctiveName
    {
      get
      {
        var distinctiveName = new StringBuilder(EmbeddedType.DistinctiveName);

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
                                   ? typeArgument.TypeEntity.FullyQualifiedName
                                   : "?");
        }
        distinctiveName.Append('>');

        return distinctiveName.ToString();
      }
    }

  }
}