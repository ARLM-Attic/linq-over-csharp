// ================================================================================================
// IIdentifierSupport.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This interface defines the behavior of supporting identifiers.
  /// </summary>
  // ================================================================================================
  public interface IIdentifierSupport
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the identifier token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Token IdentifierToken { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the identifier string.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    string Identifier { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has identifier.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has identifier; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    bool HasIdentifier { get; }
  }
}