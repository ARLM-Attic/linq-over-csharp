// ================================================================================================
// LabelNodeCollection.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Collections;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a collection of label nodes.
  /// </summary>
  // ================================================================================================
  public sealed class LabelNodeCollection : ImmutableCollection<LabelNode>
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