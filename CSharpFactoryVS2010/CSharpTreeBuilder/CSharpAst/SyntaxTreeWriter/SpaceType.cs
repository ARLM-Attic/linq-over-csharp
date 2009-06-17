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
    /// <summary>Assignment operator</summary>
    AssignmentOp,
    /// <summary>Before colon token in attributes</summary>
    BeforeColonInAttributes,
    /// <summary>After colon token in attributes</summary>
    AfterColonInAttributes,
    /// <summary>Before comma token</summary>
    BeforeComma,
    /// <summary>After comma token</summary>
    AfterComma,
    /// <summary>Before colon token preceding base type list</summary>
    BeforeBaseTypeColon,
    /// <summary>After colon token preceding base type list</summary>
    AfterBaseTypeColon,
    /// <summary>Before colon token preceding constraint tag list</summary>
    BeforeTypeConstraintColon,
    /// <summary>After colon token preceding constraint tag list</summary>
    AfterTypeConstraintColon
  }
}