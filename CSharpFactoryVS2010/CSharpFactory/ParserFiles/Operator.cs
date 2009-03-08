namespace CSharpFactory.ParserFiles
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
    /// <summary>Unary or binary plus operator</summary>
    Plus,
    /// <summary>Unary or binary minus operator</summary>
    Minus,
    /// <summary>Logical not operator</summary>
    Not,
    /// <summary>Bitwise not operator</summary>
    BitwiseNot,
    /// <summary>Pre or post increment operator</summary>
    Increment,
    /// <summary>Pre or post decrement operator</summary>
    Decrement,
    /// <summary>True operator</summary>
    True,
    /// <summary>False operator</summary>
    False,
    /// <summary>Multiplication operator</summary>
    Multiply,
    /// <summary>Division operator</summary>
    Divide,
    /// <summary>Modulus operator</summary>
    Modulus,
    /// <summary>Bitwise 'and' operator</summary>
    BitwiseAnd,
    /// <summary>Bitwise 'or' operator</summary>
    BitwiseOr,
    /// <summary>Bitwise 'xor' operator</summary>
    BitwiseXor,
    /// <summary>Left shift operator</summary>
    LeftShift,
    /// <summary>Right shift operator</summary>
    RightShift,
    /// <summary>Equal operator</summary>
    Equal,
    /// <summary>Not equal operator</summary>
    NotEqual,
    /// <summary>Greater than operator</summary>
    GreaterThan,
    /// <summary>Less than operator</summary>
    LessThan,
    /// <summary>Greater than or equal operator</summary>
    GreaterThanOrEqual,
    /// <summary>Less than or equal operator</summary>
    LessThanOrEqual
  }
}
