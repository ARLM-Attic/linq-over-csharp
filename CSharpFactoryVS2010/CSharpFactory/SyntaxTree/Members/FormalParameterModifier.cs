// ================================================================================================
// FormalParameterModifier.cs
//
// Created: 2009.05.12, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This enumeration contains values for formal parameter modifiers.
  /// </summary>
  // ================================================================================================
  public enum FormalParameterModifier
  {
    /// <summary>Input parameter</summary>
    In,
    /// <summary>Output parameter</summary>
    Out,
    /// <summary>Parameter passed by reference</summary>
    Ref,
    /// <summary>Extension method parameter</summary>
    This,
    /// <summary>"params" parameter modifier</summary>
    Params
  }
}