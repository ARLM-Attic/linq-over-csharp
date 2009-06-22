// ================================================================================================
// Symbol.cs
//
// Created: 2009.06.20, by Istvan Novak (DeepDiver)
// ================================================================================================

namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a symbol that is independent from any source file.
  /// </summary>
  // ================================================================================================
  public abstract class Symbol : ISymbol
  {
    #region Lifecycle methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="Symbol"/> class.
    /// </summary>
    /// <param name="kind">The kind.</param>
    /// <param name="value">The value.</param>
    // ----------------------------------------------------------------------------------------------
    protected Symbol(int kind, string value)
    {
      Kind = kind;
      Value = value;
    }

    #endregion

    #region ISymbol implementation

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string value of a symbol.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Value { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the kind of a symbol.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Kind { get; private set;}

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a flag indicating whether this instance is whitespace (including "end of line" symbol).
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is whitespace; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public abstract bool IsWhitespace { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is an "end of line" symbol.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is an "end of line" symbol; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public abstract bool IsEndOfLine { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is a keyword.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is keyword; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public abstract bool HasConstantValue { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is identifier.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is identifier; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public abstract bool IsIdentifier { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is a literal (integer, real, char, string or
    /// an identifier).
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is a literal; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public abstract bool IsLiteral { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is apragma.
    /// </summary>
    /// <value><c>true</c> if this instance is pragma; otherwise, <c>false</c>.</value>
    // ----------------------------------------------------------------------------------------------
    public abstract bool IsPragma { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is a comment.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is a comment; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public abstract bool IsComment { get; }

    #endregion

    #region System.Object overrides

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; 
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="T:System.NullReferenceException">
    /// The <paramref name="obj"/> parameter is null.
    /// </exception>
    // ----------------------------------------------------------------------------------------------
    public override bool Equals(object obj)
    {
      var otherSymbol = obj as Symbol;
      return otherSymbol != null && (Kind == otherSymbol.Kind && Value == otherSymbol.Value);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a <see cref="System.String"/> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public override string ToString()
    {
      return Value;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures 
    /// like a hash table. 
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public override int GetHashCode()
    {
      return Kind.GetHashCode() ^ Value.GetHashCode();
    }

    #endregion
  }
}