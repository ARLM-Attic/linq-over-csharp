// ================================================================================================
// OperatorDeclarationNode.cs
//
// Created: 2009.05.18, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.AstFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// Declares an operator overload method.
  /// </summary>
  // ================================================================================================
  public class OperatorDeclarationNode : MethodDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="OperatorDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public OperatorDeclarationNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the token representing the operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token KindToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the second token (used only for right shift operator).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SecondToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the kind of the operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public OperatorKind Kind { get; internal set; }
  }
}