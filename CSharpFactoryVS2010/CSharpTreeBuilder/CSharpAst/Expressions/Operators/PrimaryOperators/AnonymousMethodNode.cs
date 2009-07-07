// ================================================================================================
// AnonymousMethodNode.cs
//
// Created: 2009.05.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an anonymous method expression.
  /// </summary>
  // ================================================================================================
  public class AnonymousMethodNode : PrimaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AnonymousMethodNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public AnonymousMethodNode(Token start)
      : base(start)
    {
      ParameterList = new FormalParameterListNode(null);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the parameter list.
    /// </summary>
    /// <value>The parameter list.</value>
    // ----------------------------------------------------------------------------------------------
    public FormalParameterListNode ParameterList { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the body of the anonymous method.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BlockStatementNode Body { get; internal set; }
  }
}