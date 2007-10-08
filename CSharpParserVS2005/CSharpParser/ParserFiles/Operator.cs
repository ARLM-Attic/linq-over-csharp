namespace CSharpParser.ParserFiles
{
  // ==================================================================================
  /// <summary>
  /// This enumeration enlists all operators that can be used in expressions and 
  /// declarations.
  /// </summary>
  /// <remarks>
  /// The enumeration also contains the possible combinations of unary and binary
  /// operators.
  /// </remarks>
  // ==================================================================================
  public enum Operator
  {
    Plus, 
    Minus, 
    Not, 
    BitwiseNot,
    Increment, 
    Decrement, 
    True, 
    False,
    Multiply, 
    Divide, 
    Modulus, 
    BitwiseAnd,
    BitwiseOr, 
    BitwiseXor, 
    LeftShift, 
    RightShift,
    Equal, 
    NotEqual, 
    GreaterThan, 
    LessThan,
    GreaterThanOrEqual, 
    LessThanOrEqual
  }
}
