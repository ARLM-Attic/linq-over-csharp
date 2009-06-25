// ================================================================================================
// TypeTestingOperatorNode.cs
//
// Created: 2009.06.25, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  public class TypeTestingOperatorNode: BinaryOperatorNodeBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeTestingOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    public TypeTestingOperatorNode(Token start, TypeTestingOperatorType opType) : 
      base(start)
    {
      Operator = opType;
    }

    public TypeTestingOperatorType Operator { get; internal set; }

    public TypeOrNamespaceNode RightOperand { get; internal set; }
  }
}