// ================================================================================================
// CSharpSymbolReference.cs
//
// Created: 2009.06.27, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// This structure implements a C# symbol reference with storing the symbol position in the 
  /// physical symbol stream.
  /// </summary>
  // ================================================================================================
  public struct CSharpSymbolReference : ISymbolReference
  {
    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CSharpSymbolReference"/> struct.
    /// </summary>
    /// <param name="streamPosition">The position of the symbol in the source stream.</param>
    // --------------------------------------------------------------------------------------------
    public CSharpSymbolReference(int streamPosition)
      : this()
    {
      StreamPosition = streamPosition;
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the position of the symbol in the physical stream.
    /// </summary>
    /// <value>The stream position.</value>
    // --------------------------------------------------------------------------------------------
    public int StreamPosition { get; private set; }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a symbol from the specified source file.
    /// </summary>
    /// <param name="sourceNode">The source file node.</param>
    /// <returns>
    /// Symbol information obtained from the specified source file.
    /// </returns>
    // --------------------------------------------------------------------------------------------
    public ISymbol GetSymbol(SourceFileNode sourceNode)
    {
      var symbol = sourceNode.SymbolStream.ReadSymbol(StreamPosition);
      return new CSharpSymbol(symbol.Kind, symbol.Value);
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a positioned symbol from the specified source file.
    /// </summary>
    /// <param name="sourceNode">The source file node.</param>
    /// <returns>
    /// Positioned symbol information obtained from the specified source file.
    /// </returns>
    // --------------------------------------------------------------------------------------------
    public IPositionedSymbol GetPositionedSymbol(SourceFileNode sourceNode)
    {
      var symbol = sourceNode.SymbolStream.ReadSymbol(StreamPosition);
      int row;
      int column;
      sourceNode.SymbolStream.GetRowAndColumnInfo(StreamPosition, out row, out column);
      return new CSharpSymbol(symbol.Kind, symbol.Value, row, column);
    }
  }
}