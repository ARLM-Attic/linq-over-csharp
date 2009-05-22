// ================================================================================================
// IAttributedDeclaration.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class declares the behavior of declarations with attributes.
  /// </summary>
  // ================================================================================================
  public interface IAttributedDeclaration
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the attribute decorations belonging to this declaration.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    AttributeDecorationNodeCollection AttributeDecorations { get; }
  }
}