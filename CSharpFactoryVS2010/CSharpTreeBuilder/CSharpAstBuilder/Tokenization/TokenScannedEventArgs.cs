// ================================================================================================
// TokenScannedEventArgs.cs
//
// Created: 2009.06.20, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;

namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents the parameters of an event raised when the scanner reached a new token.
  /// </summary>
  /// <remarks>
  /// The only parameter of this event is the token scanned.
  /// </remarks>
  // ================================================================================================
  public class TokenScannedEventArgs : EventArgs 
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TokenScannedEventArgs"/> class.
    /// </summary>
    /// <param name="token">The token.</param>
    // ----------------------------------------------------------------------------------------------
    public TokenScannedEventArgs(Token token)
    {
      Token = token;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the token scanned
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token Token { get; private set; }
  }
}