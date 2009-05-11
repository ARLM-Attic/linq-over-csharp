// ================================================================================================
// InvocationOperatorNode.cs
//
// Created: 2009.04.26, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents an abstract invocation operator node.
  /// </summary>
  // ================================================================================================
  public abstract class InvocationOperatorNode : PrimaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="InvocationOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected InvocationOperatorNode(Token start)
      : base(start)
    {
      Arguments = new ArgumentNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the scope operand.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode ScopeOperand { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of arguments belonging to the invocation.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ArgumentNodeCollection Arguments { get; private set; }
  }
}