﻿namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a struct entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class StructEntity : ChildTypeCapableTypeEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a value type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsValueType
    {
      get { return true; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a struct type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsStructType
    {
      get { return true; }
    }
  }
}