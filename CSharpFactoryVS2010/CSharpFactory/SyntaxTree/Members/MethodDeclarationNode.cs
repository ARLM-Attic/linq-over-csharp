// ================================================================================================
// MethodDeclarationNode.cs
//
// Created: 2009.04.19, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a method declaration.
  /// </summary>
  // ================================================================================================
  public class MethodDeclarationNode : MemberWithBodyDeclarationNode, IParentheses
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public MethodDeclarationNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the opening parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenParenthesis
    {
      get { return FormalParameters.StartToken; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the node representing formal parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public FormalParameterListNode FormalParameters { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the closing parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token CloseParenthesis 
    { 
      get { return FormalParameters.TerminatingToken; } 
    }
  }
}