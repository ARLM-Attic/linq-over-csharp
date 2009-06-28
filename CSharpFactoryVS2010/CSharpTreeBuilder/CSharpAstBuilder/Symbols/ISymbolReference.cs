// ================================================================================================
// ISymbolReference.cs
//
// Created: 2009.06.27, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// This interface represents the behavior of objects that store a reference to symbol and are 
  /// able to obtain the symbol information through a source file.
  /// </summary>
  // ================================================================================================
  public interface ISymbolReference
  {
    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a symbol from the specified source file.
    /// </summary>
    /// <param name="sourceNode">The source file node.</param>
    /// <returns>
    /// Symbol information obtained from the specified source file.
    /// </returns>
    // --------------------------------------------------------------------------------------------
    ISymbol GetSymbol(SourceFileNode sourceNode);

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a positioned symbol from the specified source file.
    /// </summary>
    /// <param name="sourceNode">The source file node.</param>
    /// <returns>
    /// Positioned symbol information obtained from the specified source file.
    /// </returns>
    // --------------------------------------------------------------------------------------------
    IPositionedSymbol GetPositionedSymbol(SourceFileNode sourceNode);
  }
}