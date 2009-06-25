// ================================================================================================
// BinaryOperatorType.cs
//
// Created: 2009.06.25, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  public enum BinaryOperatorType
  {
    Multiplication,
    Division,
    Modulo,
    Addition,    
    Subtraction,
    LeftShift,
    RightShift,
    LessThan,
    GreaterThan,
    LessThanOrEqual,
    GreaterThanOrEqual,
    Equals,
    NotEquals,
    LogicalAnd,
    LogicalXor,
    LogicalOr,
    ConditionalAnd,
    ConditionalOr,
    NullCoalescing
  }
}