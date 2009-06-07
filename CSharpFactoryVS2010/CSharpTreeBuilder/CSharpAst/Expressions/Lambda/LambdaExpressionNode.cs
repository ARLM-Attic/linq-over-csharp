// ================================================================================================
// LambdaExpressionNode.cs
//
// Created: 2009.06.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a lambda expression.
  /// </summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>( <em>identifier</em> | "<strong>(</strong>" <em>FormalParameterNode</em>
  ///         { "<strong>,</strong>" <em>FormalParameterNode</em> } "<strong>)</strong>" )
  ///         "<strong>=&gt;</strong>" ( <em>ExpressionNode</em> |
  ///         <em>BlockStatementNode</em> )</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>"<strong>(</strong>"<br/>
  ///         { <em>FormalParameterNode</em> }<br/>
  ///         "<strong>)</strong>"<br/>
  ///         "<strong>=&gt;</strong>"<br/>
  /// 			<em>ExpressionNode</em><br/>
  /// 			<em>BlockStatementNode</em></para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class LambdaExpressionNode : ExpressionNode, IParentheses
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LambdaExpressionNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public LambdaExpressionNode(Token start)
      : base(start)
    {
      FormalParameters = new FormalParameterNodeCollection {ParentNode = this};
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the formal parameters of the lambda expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public FormalParameterNodeCollection FormalParameters { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the opening parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenParenthesis { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the lambda operator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token LambdaToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the closing parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token CloseParenthesis { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression of the lambda function.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Expression { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance represents a simple expression.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance represents a simple expression; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool IsSimpleExpression
    {
      get { return Expression != null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the block of statements belonging to the lambda expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BlockStatementNode Block { get; internal set; }
  }
}