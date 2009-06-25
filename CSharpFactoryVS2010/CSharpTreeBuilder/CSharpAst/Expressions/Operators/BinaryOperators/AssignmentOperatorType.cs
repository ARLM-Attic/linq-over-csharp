// ================================================================================================
// AssignmentOperatorType.cs
//
// Created: 2009.06.25, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  public enum AssignmentOperatorType
  {
    SimpleAssignment,
    AdditionAssignment,
    SubtractionAssignment,
    MultiplicationAssignment,
    DivisionAssignment,
    ModuloAssignment,
    LogicalAndAssignment,
    LogicalXorAssignment,
    LogicalOrAssignment,
    LeftShiftAssignment,
    RightShiftAssignment
  }
}