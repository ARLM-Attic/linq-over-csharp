// ================================================================================================
// ISymbol.cs
//
// Created: 2009.06.20, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// This interface defines the behavior of a single symbol.
  /// </summary>
  /// <remarks>
  /// A single symbol has meaning by itself, but has no direct relation to a source file. An ISymbol
  /// instance can be created by its own.
  /// </remarks>
  // ================================================================================================
  public interface ISymbol
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string value of a symbol.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    string Value { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the kind of a symbol.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    int Kind { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a flag indicating whether this instance is whitespace (including "end of line" symbol).
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is whitespace; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    bool IsWhitespace { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is an "end of line" symbol.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is an "end of line" symbol; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    bool IsEndOfLine { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has a constant value for all instances of the
    /// same kind.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has a const value; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    bool HasConstantValue { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is identifier.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is identifier; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    bool IsIdentifier { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is a literal (integer, real, char, string or
    /// an identifier).
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is a literal; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    bool IsLiteral { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is apragma.
    /// </summary>
    /// <value><c>true</c> if this instance is pragma; otherwise, <c>false</c>.</value>
    // ----------------------------------------------------------------------------------------------
    bool IsPragma { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is a comment.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is a comment; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    bool IsComment { get; }
  }
}