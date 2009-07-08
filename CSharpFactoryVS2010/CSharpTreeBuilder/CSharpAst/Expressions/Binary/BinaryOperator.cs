// ================================================================================================
// BinaryOperator.cs
//
// Created: 2009.06.25, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Enumeration of binary operator types 
  /// (assignment operators not included, see AssignmentOperator).
  /// </summary>
  // ================================================================================================
  public enum BinaryOperator
  {
    /// <summary>* operator</summary>
    Multiplication,

    /// <summary>/ operator</summary>
    Division,

    /// <summary>% operator</summary>
    Modulo,

    /// <summary>binary + operator</summary>
    Addition,

    /// <summary>binary - operator</summary>
    Subtraction,

    /// <summary>&lt;&lt; operator</summary>
    LeftShift,

    /// <summary>&gt;&gt; operator</summary>
    RightShift,

    /// <summary>&lt; operator</summary>
    LessThan,

    /// <summary>&gt; operator</summary>
    GreaterThan,

    /// <summary>&lt;= operator</summary>
    LessThanOrEqual,

    /// <summary>&gt;= operator</summary>
    GreaterThanOrEqual,

    /// <summary>== operator</summary>
    Equals,

    /// <summary>!= operator</summary>
    NotEquals,

    /// <summary>&amp;= operator</summary>
    LogicalAnd,

    /// <summary>^ operator</summary>
    LogicalXor,

    /// <summary>| operator</summary>
    LogicalOr,

    /// <summary>&amp;&amp; operator</summary>
    ConditionalAnd,

    /// <summary>|| operator</summary>
    ConditionalOr,

    /// <summary>?? operator</summary>
    NullCoalescing
  }
}