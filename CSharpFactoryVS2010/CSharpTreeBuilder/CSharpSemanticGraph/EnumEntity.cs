using System;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an enum entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public class EnumEntity : ValueTypeEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Overrides base method to disallow adding child types.
    /// </summary>
    /// <param name="typeEntity">A type entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void AddChildType(TypeEntity typeEntity)
    {
      throw new InvalidOperationException("Enums can not have embedded types.");
    }
  }
}