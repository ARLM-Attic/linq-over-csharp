// ================================================================================================
// CSharpSourceSymbol.cs
//
// Created: 2009.06.20, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a symbol that is bound to a source file.
  /// </summary>
  // ================================================================================================
  public class CSharpSourceSymbol : CSharpSymbol, ISourceSymbol
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CSharpSourceSymbol"/> class.
    /// </summary>
    /// <param name="compilationUnit">The compilation unit.</param>
    /// <param name="streamPosition">The stream position.</param>
    // ----------------------------------------------------------------------------------------------
    public CSharpSourceSymbol(SourceFileNode compilationUnit, int streamPosition)
    {
      CompilationUnit = compilationUnit;
      StreamPosition = streamPosition;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the compilation unit owning this symbol.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SourceFileNode CompilationUnit { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the stream position.
    /// </summary>
    /// <value>The stream position.</value>
    // ----------------------------------------------------------------------------------------------
    public int StreamPosition { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the previous symbol.
    /// </summary>
    /// <value>Previous symbol, or null, if this is the first symbol in the source file.</value>
    // ----------------------------------------------------------------------------------------------
    public ISourceSymbol PreviousSymbol
    {
      get { throw new NotImplementedException(); }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the next symbol.
    /// </summary>
    /// <value>Next symbol, or null, if this is the last symbol in the source file.</value>
    // ----------------------------------------------------------------------------------------------
    public ISourceSymbol NextSymbol
    {
      get { throw new NotImplementedException(); }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the line position of the symbol
    /// </summary>
    /// <remarks>
    /// Line numbering starts at 1.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public int Line
    {
      get { throw new NotImplementedException(); }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the column position of the symbol
    /// </summary>
    /// <remarks>
    /// Column numbering starts at 1.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public int Column
    {
      get { throw new NotImplementedException(); }
    }
  }
}