// ================================================================================================
// MemberAccessOperatorNodeBase.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This type is intended to be the base class of all member access operators.
  /// </summary>
  // ================================================================================================
  public abstract class MemberAccessOperatorNodeBase : PrimaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberAccessOperatorNodeBase"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected MemberAccessOperatorNodeBase(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the scope operand.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode ScopeOperand { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the member name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameNode MemberName { get; internal set; }
  }
}