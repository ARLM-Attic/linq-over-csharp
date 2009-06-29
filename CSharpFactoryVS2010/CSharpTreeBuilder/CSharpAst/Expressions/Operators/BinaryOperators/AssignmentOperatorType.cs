// ================================================================================================
// AssignmentOperatorType.cs
//
// Created: 2009.06.25, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Enumeration of assignment operator types.
  /// </summary>
  // ================================================================================================
  public enum AssignmentOperatorType
  {
    /// <summary>= operator</summary>
    SimpleAssignment,

    /// <summary>+= operator</summary>
    AdditionAssignment,

    /// <summary>-= operator</summary>
    SubtractionAssignment,

    /// <summary>*= operator</summary>
    MultiplicationAssignment,

    /// <summary>/= operator</summary>
    DivisionAssignment,

    /// <summary>%= operator</summary>
    ModuloAssignment,

    /// <summary>&amp;= operator</summary>
    LogicalAndAssignment,

    /// <summary>^= operator</summary>
    LogicalXorAssignment,

    /// <summary>|= operator</summary>
    LogicalOrAssignment,

    /// <summary>&lt;&lt;= operator</summary>
    LeftShiftAssignment,

    /// <summary>&gt;&gt;= operator</summary>
    RightShiftAssignment
  }
}