// ================================================================================================
// OutputItem.cs
//
// Created: 2009.03.22, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents an output item to be written at a specific position.
  /// </summary>
  // ================================================================================================
  public abstract class OutputItem : IComparable<OutputItem>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="OutputItem"/> class.
    /// </summary>
    /// <param name="row">The row position of the item.</param>
    /// <param name="column">The column position of the item.</param>
    // ----------------------------------------------------------------------------------------------
    protected OutputItem(int row, int column)
    {
      Row = row;
      Column = column;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the row position of the item.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Row { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the column position of the item.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Column { get; private set; }

    // ----------------------------------------------------------------------------------------------

    #region IComparable<OutputItem> Members

    /// <summary>
    /// Compares the current object with another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// A 32-bit signed integer that indicates the relative order of the objects being compared. 
    /// The return value has the following meanings: Value Meaning Less than zero This object is 
    /// less than the <paramref name="other"/> parameter. Zero This object is equal to
    /// <paramref name="other"/>. Greater than zero This object is greater than 
    /// <paramref name="other"/>.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public int CompareTo(OutputItem other)
    {
      return Row == other.Row
               ? Column - other.Column
               : Row - other.Row;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the text of this output item in the context of the specified options.
    /// </summary>
    /// <returns>
    /// Text of this output item in the context of the specified options.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public abstract string GetText(SyntaxTreeOutputOptions options);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the length of this output item in the context of the specified options.
    /// </summary>
    /// <param name="options">Output options.</param>
    /// <returns>
    /// Length of this output item in the context of the specified options.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public abstract int GetLength(SyntaxTreeOutputOptions options);
  }
}