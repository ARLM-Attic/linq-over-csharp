// ================================================================================================
// OverloadableOperatorType.cs
//
// Created: 2009.06.14, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This enumeration describes the types of overloadable operators
  /// </summary>
  // ================================================================================================
  public enum OverloadableOperatorType
  {
#pragma warning disable 1591
    Addition,
    Subtraction,
    Not,
    BitwiseNot,
    Increment,
    Decrement,
    True,
    False,
    Multiplication,
    Division,
    Modulo,
    BitwiseAnd,
    BitwiseOr,
    BitwiseXor,
    LeftShift,
    RightShift,
    Equal,
    NotEqual,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual
#pragma warning restore 1591
  }
}