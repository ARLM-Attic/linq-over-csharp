// ================================================================================================
// ModifierType.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Gets the type of the modifier.
  /// </summary>
  // ================================================================================================
  public enum ModifierType
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "new" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    New,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "public" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Public,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "protected" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Protected,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "internal" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Internal,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "private" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Private,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "unsafe" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Unsafe,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "static" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Static,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "readonly" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Readonly,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "volatile" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Volatile,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "virtual" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Virtual,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "sealed" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Sealed,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "override" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Override,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "abstract" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Abstract,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "extern" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Extern
  }
}