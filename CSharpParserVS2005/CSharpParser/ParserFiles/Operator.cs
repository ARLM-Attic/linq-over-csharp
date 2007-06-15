using System;

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
  [Flags]
  public enum Operator
  {
    plus = 0x00000001, 
    minus = 0x00000002, 
    not = 0x00000004, 
    tilde = 0x00000008,
    inc = 0x00000010, 
    dec = 0x00000020, 
    @true = 0x00000040, 
    @false = 0x00000080,
    times = 0x00000100, 
    div = 0x00000200, 
    mod = 0x00000400, 
    and = 0x00000800,
    or = 0x00001000, 
    xor = 0x00002000, 
    lshift = 0x00004000, 
    rshift = 0x00008000,
    eq = 0x00010000, 
    neq = 0x00020000, 
    gt = 0x00040000, 
    lt = 0x00080000,
    gte = 0x00100000, 
    lte = 0x00200000,

    // --- Set of unary operators
    unary = plus | minus | not | tilde | inc | dec | @true | @false,

    // --- Set of binary operators
    binary = plus | minus | times | div | mod | and | or | xor | lshift | 
      rshift | eq | neq | gt | lt | gte | lte
  }
}
