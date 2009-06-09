// ================================================================================================
// NewOperatorWithConstructorNode.cs
//
// Created: 2009.06.08, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// 
  /// </summary>
  // ================================================================================================
  public class NewOperatorWithConstructorNode : NewOperatorNode, IParentheses
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NewOperatorWithConstructorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public NewOperatorWithConstructorNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the opening parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenParenthesis { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the constructor arguments.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ArgumentNodeCollection Arguments { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the closing parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token CloseParenthesis { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the method call initializer expression belonging to the constructor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Initializer { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the optional initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ObjectOrCollectionInitializerNode ObjectInitializer { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is an implicit constructor call.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsImplicitCall
    {
      get { return OpenParenthesis == null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has an initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool HasObjectInitializer
    {
      get { return ObjectInitializer == null; }
    }
  }
}