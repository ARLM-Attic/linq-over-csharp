// ================================================================================================
// NewLineReachedEventArgs.cs
//
// Created: 2009.06.20, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;

namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents the event arguments when a new line is reached by the scanner.
  /// </summary>
  /// <remarks>
  /// The event argument is the line number reached. When the scanner is initialized, an event is
  /// raised with line #1.
  /// </remarks>
  // ================================================================================================
  public class NewLineReachedEventArgs : EventArgs 
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NewLineReachedEventArgs"/> class.
    /// </summary>
    /// <param name="lineNumber">The line number reached.</param>
    // ----------------------------------------------------------------------------------------------
    public NewLineReachedEventArgs(int lineNumber)
    {
      LineNumber = lineNumber;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the line number reached.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int LineNumber { get; private set; }
  }
}