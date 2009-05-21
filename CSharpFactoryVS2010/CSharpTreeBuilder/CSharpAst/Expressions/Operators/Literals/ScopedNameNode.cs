// ================================================================================================
// ScopedNameNode.cs
//
// Created: 2009.04.16, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents the common root class on named literals.
  /// </summary>
  // ================================================================================================
  public class ScopedNameNode : SimpleNameNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ScopedNameNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ScopedNameNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the qualifier token.
    /// </summary>
    /// <value>The qualifier token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token QualifierToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has qualifier.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has qualifier; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasQualifier { get { return QualifierToken != null; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the qualifier.
    /// </summary>
    /// <value>The qualifier.</value>
    // ----------------------------------------------------------------------------------------------
    public string Qualifier
    {
      get { return QualifierToken == null ? string.Empty : QualifierToken.Value; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the separator token.
    /// </summary>
    /// <value>The separator token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token QualifierSeparatorToken { get; internal set; }
  }
}