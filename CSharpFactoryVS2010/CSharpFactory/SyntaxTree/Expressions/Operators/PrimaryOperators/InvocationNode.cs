// ================================================================================================
// InvocationNode.cs
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
  public abstract class InvocationNode : PrimaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="InvocationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected InvocationNode(Token start)
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

  // ================================================================================================
  /// <summary>
  /// This class represents a method invocation operator node.
  /// </summary>
  // ================================================================================================
  public sealed class MethodInvocationNode : InvocationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodInvocationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public MethodInvocationNode(Token start)
      : base(start)
    {
    }
  }

  // ================================================================================================
  /// <summary>
  /// This class represents an array indexer invocation operator node.
  /// </summary>
  // ================================================================================================
  public sealed class ArrayIndexerInvocationNode : InvocationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ArrayIndexerInvocationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ArrayIndexerInvocationNode(Token start)
      : base(start)
    {
    }
  }
}