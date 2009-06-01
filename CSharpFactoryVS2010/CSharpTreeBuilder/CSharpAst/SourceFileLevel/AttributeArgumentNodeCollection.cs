// ================================================================================================
// AttributeArgumentNodeCollection.cs
//
// Created: 2009.05.29, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a collection of attribute arguments.
  /// </summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>{ <em>AttributeArgumentNode</em> }</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para><em>AttributeArgumentNode</em>: Collection item</para>
  /// 	</blockquote>
  /// 	<para></para>
  /// </remarks>
  // ================================================================================================
  public class AttributeArgumentNodeCollection : SyntaxNodeCollection<AttributeArgumentNode, ISyntaxNode>
  {
  }
}