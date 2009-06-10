// ================================================================================================
// OperatorKind.cs
//
// Created: 2009.05.18, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This enum defines the types of operators.
  /// </summary>
  // ================================================================================================
  public enum OperatorKind
  {
    /// <summary>
    /// Addition operator
    /// </summary>
    Addition,

    /// <summary>
    /// Substraction operator
    /// </summary>
    Subtraction,
    
    /// <summary>
    /// Logical not operator
    /// </summary>
    LogicalNot,
    
    /// <summary>
    /// Bitwise not operator
    /// </summary>
    BitwiseNot,
    
    /// <summary>
    /// Increment operator
    /// </summary>
    Increment,
    
    /// <summary>
    /// Decrement operator
    /// </summary>
    Decrement,
    
    /// <summary>
    /// True operator
    /// </summary>
    True,
    
    /// <summary>
    /// False operator
    /// </summary>
    False,
    
    /// <summary>
    /// Multiplication operator
    /// </summary>
    Multiplication,
    
    /// <summary>
    /// Division operator
    /// </summary>
    Division,
    
    /// <summary>
    /// Modulus operator
    /// </summary>
    Modulus,
    
    /// <summary>
    /// Bitwise and operator
    /// </summary>
    BitwiseAnd,
    
    /// <summary>
    /// Bitwise or operator
    /// </summary>
    BitwiseOr,
    
    /// <summary>
    /// Bitwise xor operator
    /// </summary>
    BitwiseXor,
    
    /// <summary>
    /// Left shift operator
    /// </summary>
    LeftShift,
    
    /// <summary>
    /// Right shift operator
    /// </summary>
    RightShift,
    
    /// <summary>
    /// Equal operator
    /// </summary>
    Equal,
    
    /// <summary>
    /// Not equal operator
    /// </summary>
    NotEqual,
    
    /// <summary>
    /// Less than operator
    /// </summary>
    LessThan,
    
    /// <summary>
    /// Greater than operator
    /// </summary>
    GreaterThan,
    
    /// <summary>
    /// Less than or equal operator
    /// </summary>
    LessThanOrEqual,
    
    /// <summary>
    /// Greater than or equal operator
    /// </summary>
    GreaterThanOrEqual
  }
}