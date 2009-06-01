// ================================================================================================
// AttributeDecorationNodeCollection.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a collection of attribute decoration nodes.
  /// </summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>{ <em>AttributeDecorationNode</em> }</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para><em>AttributeDecorationNode</em>: Collection item</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public sealed class AttributeDecorationNodeCollection :
    SyntaxNodeCollection<AttributeDecorationNode, ISyntaxNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Assigns this colection and all of its items to the specified parent syntax node.
    /// </summary>
    /// <param name="parent">The parent syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    internal void AssignToParent(ISyntaxNode parent)
    {
      ParentNode = parent;
      foreach (var item in this)
      {
        item.ParentNode = parent;
      }
    }
  }
}