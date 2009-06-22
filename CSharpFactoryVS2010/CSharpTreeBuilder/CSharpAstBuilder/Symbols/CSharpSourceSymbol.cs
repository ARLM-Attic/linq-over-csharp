// ================================================================================================
// CSharpSourceSymbol.cs
//
// Created: 2009.06.20, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;

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
    /// <param name="kind">The kind.</param>
    /// <param name="value">The value.</param>
    // ----------------------------------------------------------------------------------------------
    protected CSharpSourceSymbol(int kind, string value)
      : base(kind, value)
    {
    }

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