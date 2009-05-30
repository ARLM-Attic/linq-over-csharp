// ================================================================================================
// TypeParameterListNode.cs
//
// Created: 2009.05.30, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This syntax node represents a list of type parameters in type or member
  /// declarations.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The opening and closing angle brackets are represented by the start and terminating tokens, 
  /// respectively.
  /// </para>
  /// <para>Syntax:</para>
  /// <para>
  /// "<strong>&lt;</strong>" <em>TypeParameterNode</em> {
  /// "<strong>,</strong>" <em>TypeParameterNode</em> } "<strong>&gt;</strong>"
  /// </para>
  /// </remarks>
  // ================================================================================================
  public class TypeParameterListNode : SyntaxNodeCollection<TypeParameterNode, TypeOrMemberDeclarationNode>
  {
  }
}