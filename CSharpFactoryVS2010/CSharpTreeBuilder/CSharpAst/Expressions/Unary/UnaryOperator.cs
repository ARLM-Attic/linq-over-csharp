namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Enumerates the unary operators.
  /// </summary>
  // ================================================================================================
  public enum UnaryOperator
  {
    /// <summary>Unary + operator</summary>
    Identity,

    /// <summary>Unary - operator</summary>
    Negation,
    
    /// <summary>Unary ! operator</summary>
    LogicalNegation,
    
    /// <summary>Unary ~ operator</summary>
    BitwiseNegation,
    
    /// <summary>Unary * operator</summary>
    PointerIndirection,
    
    /// <summary>Unary &amp; operator</summary>
    AddressOf
  }
}