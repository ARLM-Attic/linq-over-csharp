﻿namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface is implemented by types that are alias of other types.
  /// Eg. 'int' is alias of 'System.Int32', 'T?' is alias of System.Nullable&lt;T&gt;
  /// </summary>
  // ================================================================================================
  public interface IAliasType
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the alias type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    TypeEntity Alias { get; }
  }
}
