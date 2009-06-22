// ================================================================================================
// ISourceSymbol.cs
//
// Created: 2009.06.20, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// This interface represents the behavior of a symbol that is related to a source file.
  /// </summary>
  /// <remarks>
  /// An ISourceSymbol cannot be created by its own, it is created by the syntax parser and is bound
  /// to a source file (compilation unit). An ISourceSymbol provides access to the positions of the 
  /// symbol as well as to the preceeding and subsequent symbols.
  /// </remarks>
  // ================================================================================================
  public interface ISourceSymbol: ISymbol
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the previous symbol.
    /// </summary>
    /// <value>Previous symbol, or null, if this is the first symbol in the source file.</value>
    // ----------------------------------------------------------------------------------------------
    ISourceSymbol PreviousSymbol { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the next symbol.
    /// </summary>
    /// <value>Next symbol, or null, if this is the last symbol in the source file.</value>
    // ----------------------------------------------------------------------------------------------
    ISourceSymbol NextSymbol { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the line position of the symbol
    /// </summary>
    /// <remarks>
    /// Line numbering starts at 1.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    int Line { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the column position of the symbol
    /// </summary>
    /// <remarks>
    /// Column numbering starts at 1.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    int Column { get; }
  }
}