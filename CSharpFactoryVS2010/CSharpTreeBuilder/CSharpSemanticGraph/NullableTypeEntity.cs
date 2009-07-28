using System;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a nullable type entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class NullableTypeEntity : ConstructedTypeEntity, IValueType, IAliasType
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NullableTypeEntity"/> class.
    /// </summary>
    /// <param name="embeddedType">The underlying type.</param>
    // ----------------------------------------------------------------------------------------------
    public NullableTypeEntity(TypeEntity embeddedType)
      : base(embeddedType)
    {
      if ((embeddedType is NullableTypeEntity) || !(embeddedType is IValueType))
      {
        throw new ArgumentException(
          string.Format("Non-nullable value type expected, but received {0}", embeddedType.GetType()),
          "embeddedType");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the alias type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Alias
    {
      get
      {
        throw new NotImplementedException();
      }
    }
  }
}