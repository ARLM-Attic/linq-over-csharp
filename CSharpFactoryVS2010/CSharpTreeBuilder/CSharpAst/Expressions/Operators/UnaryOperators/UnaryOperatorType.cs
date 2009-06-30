namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Enumerates the unary operator types.
  /// </summary>
  // ================================================================================================
  public enum UnaryOperatorType
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