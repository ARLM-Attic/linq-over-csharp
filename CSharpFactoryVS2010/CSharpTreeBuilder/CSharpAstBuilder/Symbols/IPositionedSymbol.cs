// ================================================================================================
// IPositionedSymbol.cs
//
// Created: 2009.06.27, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// This interface represents a symbol that can have row and column positions.
  /// </summary>
  // ================================================================================================
  public interface IPositionedSymbol: ISymbol
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the row position of the symbol.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    int Row { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the column position of the symbol.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    int Column { get; }
  }
}