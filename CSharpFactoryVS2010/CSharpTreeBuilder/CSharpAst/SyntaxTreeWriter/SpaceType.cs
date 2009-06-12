// ================================================================================================
// SpaceType.cs
//
// Created: 2009.05.29, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This enumeration defines the types of "space" segments.
  /// </summary>
  // ================================================================================================
  public enum SpaceType
  {
    AssignmentOp,
    BeforeColonInAttributes,
    AfterColonInAttributes,
    BeforeComma,
    AfterComma,
    BeforeBaseTypeColon,
    AfterBaseTypeColon,
    BeforeTypeConstraintColon,
    AfterTypeConstraintColon
  }
}