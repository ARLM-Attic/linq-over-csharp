// ================================================================================================
// LabelNodeCollection.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a collection of label nodes.
  /// </summary>
  // ================================================================================================
  public sealed class LabelNodeCollection : SyntaxNodeCollection<LabelNode, StatementNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds the label to the first position of the collection..
    /// </summary>
    /// <param name="labelNode">The label node to add.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddLabel(LabelNode labelNode)
    {
      Insert(0, labelNode);
    }
  }
}