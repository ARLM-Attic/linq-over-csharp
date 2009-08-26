using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

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
    private readonly List<TypeEntity> _TypeArguments;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstructedGenericTypeEntity"/> class.
    /// </summary>
    /// <param name="underlyingType">An open generic type.</param>
    /// <param name="typeArguments">The list of type arguments.</param>
    // ----------------------------------------------------------------------------------------------
    public ConstructedGenericTypeEntity(GenericCapableTypeEntity underlyingType, List<TypeEntity> typeArguments)
      : base(underlyingType)
    {
      if (typeArguments.Count != underlyingType.AllTypeParameters.Count)
      {
        throw new ArgumentException(
          string.Format("Expected '{0}' type arguments for generic type definition '{1}', but received '{2}'.",
                        underlyingType.AllTypeParameters.Count, underlyingType.FullyQualifiedName, typeArguments.Count),
          "typeArguments");
      }
      _TypeArguments = typeArguments;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of base types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override IEnumerable<SemanticEntityReference<TypeEntity>> BaseTypeReferences
    {
      get { return UnderlyingType.BaseTypeReferences; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of member references.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override IEnumerable<MemberEntity> Members
    {
      get { return UnderlyingType.Members; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a reference type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsReferenceType
    {
      get { return UnderlyingType.IsReferenceType; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a value type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsValueType
    {
      get { return UnderlyingType.IsValueType; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a class type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsClassType
    {
      get { return UnderlyingType.IsClassType; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a struct type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsStructType
    {
      get { return UnderlyingType.IsStructType; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is an interface type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsInterfaceType
    {
      get { return UnderlyingType.IsInterfaceType; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a delegate type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsDelegateType
    {
      get { return UnderlyingType.IsDelegateType; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a read-only list of the type arguments of this type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ReadOnlyCollection<TypeEntity> TypeArguments
    {
      get
      {
        return _TypeArguments.AsReadOnly();
      }
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

        distinctiveName.Append('[');
        bool firstTypeArg = true;
        foreach (var typeArgument in TypeArguments)
        {
          if (firstTypeArg)
          {
            firstTypeArg = false;
          }
          else
          {
            distinctiveName.Append(',');
          }
          distinctiveName.Append(typeArgument.FullyQualifiedName);
        }
        distinctiveName.Append(']');

        return distinctiveName.ToString();
      }
    }

  }
}