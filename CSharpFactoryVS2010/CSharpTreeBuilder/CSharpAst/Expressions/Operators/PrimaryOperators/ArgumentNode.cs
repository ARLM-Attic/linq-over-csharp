// ================================================================================================
// ArgumentNode.cs
//
// Created: 2009.04.26, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an argument in the actual parameter list.
  /// </summary>
  // ================================================================================================
  public class ArgumentNode : EmbeddedExpressionNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    // ----------------------------------------------------------------------------------------------
    public ArgumentNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the kind of parameter (in, out, ref)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token KindToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is "in" kind.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsIn
    {
      get { return KindToken == null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is "out" kind.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsOut
    {
      get { return KindToken != null && KindToken.Value == "out"; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is "ref" kind.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsRef
    {
      get { return KindToken != null && KindToken.Value == "ref"; }
    }
  }
}