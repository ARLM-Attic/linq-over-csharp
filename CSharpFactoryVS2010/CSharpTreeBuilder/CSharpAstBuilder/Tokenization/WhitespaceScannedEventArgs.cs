// ================================================================================================
// WhitespaceScannedEventArgs.cs
//
// Created: 2009.06.20, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;

namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents the arguments of an event raised when a whitespace section is scanned.
  /// </summary>
  // ================================================================================================
  public class WhitespaceScannedEventArgs: EventArgs
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="WhitespaceScannedEventArgs"/> class.
    /// </summary>
    /// <param name="whitespace">The whitespace.</param>
    // ----------------------------------------------------------------------------------------------
    public WhitespaceScannedEventArgs(string whitespace)
    {
      Whitespace = whitespace;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the whitespace scanned
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Whitespace { get; private set; }
  }
}