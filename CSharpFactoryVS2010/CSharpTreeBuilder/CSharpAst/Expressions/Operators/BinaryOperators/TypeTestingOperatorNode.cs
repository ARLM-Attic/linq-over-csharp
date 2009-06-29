// ================================================================================================
// TypeTestingOperatorNode.cs
//
// Created: 2009.06.25, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a type tesing operator node (as, is).
  /// </summary>
  // ================================================================================================
  public class TypeTestingOperatorNode : BinaryOperatorNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeTestingOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="opType">The type of the typetesting operator.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeTestingOperatorNode(Token start, TypeTestingOperatorType opType) : 
      base(start)
    {
      Operator = opType;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type of the type testing operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeTestingOperatorType Operator { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the right operand.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode RightOperand { get; internal set; }
  }
}