using System;
using System.Collections.Generic;

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
    // ----------------------------------------------------------------------------------------------
    public ConstructedGenericTypeEntity(TypeEntity embeddedType)
      : base(embeddedType)
    {
      if (!(embeddedType is GenericCapableTypeEntity))
      {
        throw new ArgumentException(
          string.Format("GenericCapableTypeEntity expected, but received {0}", embeddedType.GetType()), 
          "embeddedType");
      }

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
  }
}